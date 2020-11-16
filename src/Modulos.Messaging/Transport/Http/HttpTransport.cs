using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Protocol;
using Modulos.Messaging.Protocol.Request.Definitions;

namespace Modulos.Messaging.Transport.Http
{
    [Verified("a bit cursorily")]
    public class HttpTransport : ITransportEngine
    {
        #region Fields

        private readonly IEnumerable<IClientFactory> clientFactories;
        private readonly IClientFactory clientFactory;
        private readonly IHttpContentCreator contentCreator;
        private readonly IHttpContentReader contentReader;
        public static readonly TransportEngineId EngineId = new TransportEngineId("http");
       
        TransportEngineId ITransportEngine.EngineId => EngineId;
        public bool IsLocal { get; } = false;

        #endregion

        public HttpTransport(IEnumerable<IClientFactory> clientFactories,
            IHttpContentCreator contentCreator, 
            IHttpContentReader contentReader)
        {
            this.clientFactories = clientFactories.ToArray();
            var array = (IClientFactory[])this.clientFactories;
            clientFactory = array.Length == 1 ? array[0] : null;

            this.contentCreator = contentCreator;
            this.contentReader = contentReader;
        }

        public ITransferObject CreateTransferObject()
        {
            return new HttpTransferObject();
        }

        public ITransferObject CreateTransferObject(ITransferObject source)
        {
            return new HttpTransferObject
            {
                Header = source.Header,
                ByteContent = source.ByteContent,
                MediaType = source.MediaType,
                MediaTypeOfStream = source.MediaTypeOfStream,
                Stream = source.Stream,
                StringContent = source.StringContent
            };
        }

        public Task Validate(IMessage message, IMessageConfig messageConfig, IMetricBag metricBag, IInvocationContext invocationContext)
        {
            return Task.CompletedTask;
        }

        public async Task<ITransferObject> Send([JetBrains.Annotations.NotNull] ICreatedRequestData createdRequestData, IMetricBag metricBag, CancellationToken abortRequest)
        {
            if (createdRequestData == null) throw new ArgumentNullException(nameof(createdRequestData));
            
            var action = createdRequestData.MessageHeader.Context.Action;

            var factory = GetFactory(createdRequestData.Message, createdRequestData.MessageConfig);

            var client = factory.CreateClient(createdRequestData.Message, createdRequestData.MessageConfig);

            var disposables = new Stack<IDisposable>();
            // ReSharper disable once SuspiciousTypeConversion.Global
            if(factory is IDisposable disposableFactory)
                disposables.Push(disposableFactory);
            disposables.Push(client);

            ITransferObject responseTransferObject = null;
            var disposing = false;

            try
            {
                HttpResponseMessage response;

                using (var request = new HttpRequestMessage(HttpMethod.Post, createdRequestData.EndpointConfig.Address))
                {
                    // ReSharper disable once UseAwaitUsing
                    // async disposables streams are supported in .net core 2.1+
                    using (createdRequestData.TransferObject.Stream)
                    {
                        request.Content = contentCreator.CreateContent
                        (
                            createdRequestData.MessageHeader,
                            createdRequestData.TransferObject, 
                            out var headersToAdd
                        );
                     
                        foreach (var pair in headersToAdd)
                            request.Headers.Add(pair.Key, pair.Value);
                        
                        //await destination.Content.LoadIntoBufferAsync(); todo: [knowledge]

                        metricBag.Add(Kind.RequestStart, InvocationPlace.Caller, DateTime.UtcNow.Ticks, action);

                        var t1 = action.ElapsedMilliseconds;
                        try
                        {
                            response = await client.SendAsync
                            (
                                request: request,
                                completionOption: HttpCompletionOption.ResponseHeadersRead,
                                cancellationToken: abortRequest
                            ).ConfigureAwait(false);
                        }
                        finally
                        {
                            metricBag.Add(Kind.Transfer, InvocationPlace.Caller, action.ElapsedMilliseconds - t1, action);
                        }

                        metricBag.Add(Kind.ResponseArrived, InvocationPlace.Caller, DateTime.UtcNow.Ticks, action);

                        disposables.Push(response);
                    }
                }

                //todo: decision made at: 05.11.2020
                //response.EnsureSuccessStatusCode();

                // handle response 
                var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                disposables.Push(responseStream);
                
                //var headers = new HeaderDictionary
                //(
                //    response.Headers.ToDictionary(e => e.Key, e => new StringValues(e.Value.ToArray()))
                //);
                var headers = response.Headers.ToDictionary
                (
                    e => e.Key,
                    e => new StringValues(e.Value.ToArray()).ToString()
                );
                
                responseTransferObject = await contentReader.ReadContent(headers, responseStream, abortRequest);
                
                // if it's wrapped stream then responsibility of releasing all resources is moved to end-user 
                // by calling Dispose() method of stream object (end-user controls reading from stream).
                if (responseTransferObject.Stream is StreamWithAdditionalReleaseOfResource wrappedStream)
                {
                    foreach (var disposable in disposables)
                        wrappedStream.RegisterForDispose(disposable);

                    disposables = null;
                }
                else // if stream is null then inline 'Dispose'
                {
                    disposing = true;
                    Release(disposables);
                }

                return responseTransferObject;
            }
            catch
            {
                //todo: net3.1+ changes required
                //todo: 02.11.2020: check if it's possible to dispose twice due to using (createdRequestData.TransferObject.Stream)
                createdRequestData.TransferObject?.Stream?.Dispose();
                responseTransferObject?.Stream?.Dispose();

                if (!disposing) // only if error is not react of disposal
                    Release(disposables);

                throw;
            }
        }

        public bool IsTransportException(Exception exception)
        {
            return exception is HttpRequestException;
        }

        private static void Release(Stack<IDisposable> disposables)
        {
            if (disposables == null) return;

            var disposable = disposables.Pop();
            while (disposable != null)
            {
                disposable.Dispose();
                disposable = disposables.Count > 0 ? disposables.Pop() : null;
            }
        }

        private IClientFactory GetFactory(IMessage message, IMessageConfig config)
        {
            if (clientFactory != null && clientFactory.IsMatch(message,config)) 
                return clientFactory;

            var factory =  clientFactories
                .Where(e => e.IsMatch(message, config))
                .OrderByDescending(e => e.Order)
                .FirstOrDefault();

            if (factory == null)
                throw new TodoException($"Unable to retrieve client factory for:" +
                                               $"\r\nmessage: {message.GetType().FullName}" +
                                               $"\r\nconfig: {config}");

            return factory;
        }
    }
}