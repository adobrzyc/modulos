using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Modulos.Messaging.Diagnostics.Activity;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Hosting;
using Modulos.Messaging.Protocol;
using Modulos.Messaging.Protocol.Response.Definitions;
using Modulos.Messaging.Security.Exceptions;

namespace Modulos.Messaging.Transport.Http
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class HttpMiddleware : IActionHost
    {
        private readonly RequestDelegate next;
        private readonly IModulosHost host;
        private readonly IHttpEndpointConfiguration configuration;
        private readonly IHttpContentCreator contentCreator;
        private readonly IHttpContentReader contentReader;
        private readonly IActivityPublisher activityPublisher;
        private readonly IAppInfo appInfo;

        public HttpMiddleware(RequestDelegate next, IModulosHost host, IHttpEndpointConfiguration configuration,
            IHttpContentCreator contentCreator, IHttpContentReader contentReader, 
            IActivityPublisher activityPublisher,IAppInfo appInfo)
        {
            this.next = next;
            this.host = host;
            this.configuration = configuration;
            this.contentCreator = contentCreator;
            this.contentReader = contentReader;
            this.activityPublisher = activityPublisher;
            this.appInfo = appInfo;
            //this.credentialsReaders = credentialsReaders;
            //this.currentAuthenticationData = currentAuthenticationData;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.Value.Contains(configuration.EndpointName))
            {
                await next(context);
                return;
            }

            if (context.Request.Path.Value.EndsWith("/info"))
            {
                var result = appInfo.ToString();
                await context.Response.WriteAsync(result, Encoding.UTF8, context.RequestAborted);
                return;
            }

            //if (context.Request.Path.Value.EndsWith("performance/last"))
            //{
            //    var counters = await performanceMonitorTarget.ReadAllAsync();
            //    var result = serializationAndCompressionProvider.Serialize(counters, Serializers.JsonNet, CompressionEngines.Anonymous);

            //    await context.Response.WriteAsync(result.SerializedDataAsString, Encoding.UTF8, context.RequestAborted);
            //    return;
            //}

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var counterBag = new MetricBag();
           
            counterBag.Add(Kind.RequestArrived, InvocationPlace.Target, DateTime.UtcNow.Ticks);

            //todo: tutututut
            //// start from last registered
            //foreach (var credentialsReader in credentialsReaders.Reverse())
            //{
            //    var result = await credentialsReader.ReadFromTransportLayer(context);
            //    // first match won 
            //    if (result.HasCredentials)
            //    {
            //        await currentAuthenticationData.Set(result.Data);
            //        break;
            //    }
            //}

            //counterBag.Add(Kind.Security, InvocationPlace.Target, stopwatch.ElapsedMilliseconds);


            ITransferObject requestTransferObject = null; // = new HttpTransferObject();
            ITransferObject responseTransferObject = null;
            IResponseData hostResult = null;
            Exception error = null;

            try
            {
                // request
                try
                {
                    requestTransferObject = await contentReader.ReadContent
                    (
                        headers: context.Request.Headers.ToDictionary
                        (
                            e=>e.Key,
                            e=>e.Value.ToString()
                        ),
                        content: context.Request.Body,
                        token: context.RequestAborted
                    );
                }
                finally
                {
                    //todo: check if measuring ReadContent is not statistically negligible 
                    counterBag.Add(Kind.ReadContent, InvocationPlace.Target, stopwatch.ElapsedMilliseconds);
                }

                hostResult = await host.Execute(requestTransferObject, counterBag, context.RequestAborted);

                try
                {
                    var length = context.Request.ContentLength ?? context.Request.Body.Length;
                    counterBag.Add(Kind.RequestSize, InvocationPlace.Target, length);
                }
                catch
                {
                    //ignore
                }

                responseTransferObject = hostResult.TransferObject;



                //todo: #temporary solution, #dirty coding
                if (hostResult.Error is SecurityException)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                }

                using (var responseContent = contentCreator.CreateContent(hostResult.MessageHeader, responseTransferObject, out var headers))
                {
                    //todo: check if there should be a mechanism to override MediaType from message handler layer 
                    //todo: check if should not be propagated directly into Response.ContentType 
                    //context.Response.ContentType = responseContent.Headers.ContentType.ToString();
                    //context.Response.ContentType = responseContent.Headers.ContentType.MediaType;
                    counterBag.Add(Kind.ResponseStart, InvocationPlace.Target, DateTime.UtcNow.Ticks);

                    try
                    {
                        foreach (var keyValuePair in headers)
                            context.Response.Headers.Add(keyValuePair.Key, keyValuePair.Value);

                        using (var contentStream = await responseContent.ReadAsStreamAsync())
                        {
                            await StreamCopyOperation.CopyToAsync
                            (
                                source: contentStream,
                                destination: context.Response.Body,
                                count: null,
                                cancel: context.RequestAborted
                            );

                            try
                            {
                                counterBag.Add(Kind.ResponseSize, InvocationPlace.Target, contentStream.Length);
                            }
                            catch
                            {
                                // ignore
                            }
                        }
                    }
                    finally
                    {
                        counterBag.Add(Kind.ResponseArrived, InvocationPlace.Target, DateTime.UtcNow.Ticks);
                    }
                }


                //using (var multipartContent = HttpContentUtils.CreateMultipartContent(responseTransferObject))
                //{
                //    context.Response.ContentType = multipartContentType;

                //    using (var contentStream = await multipartContent.ReadAsStreamAsync().ConfigureAwait(false))
                //    {
                //        await StreamCopyOperation.CopyToAsync
                //        (
                //            source: contentStream,
                //            destination: context.Response.Body,
                //            count: null,
                //            cancel: context.RequestAborted
                //        ).ConfigureAwait(false);
                //    }
                //}
            }
            catch (Exception e)
            {
                error = e;
                //await hydraProcess.FinishWithFatalError(e, $"error in {nameof(HttpMiddleware)}", requestTransferObject, this);
                //HydraInternalLogger.Error(e, e.Message);
                //Console.Error.Write(e);
                //logger.Error(e, e.Message);
                throw;
            }
            finally
            {
                // #design-concept: it's designed/supposed behaviour to dispose stream inside handler logic.
                // yup... there is possibility to call Dispose few time - it's ok ;)
                requestTransferObject?.Stream?.Dispose();
                responseTransferObject?.Stream?.Dispose();

                if (error != null && hostResult?.InvocationContext == null)
                {
                    await activityPublisher.PublishFinishWithFatalError(error, $"error in {nameof(HttpMiddleware)}", requestTransferObject, this);
                }
               
                if (hostResult?.InvocationContext != null)
                {
                    counterBag.Add(Kind.Total, InvocationPlace.Target, stopwatch.ElapsedMilliseconds, hostResult.InvocationContext.Action);

                    counterBag.GetAll().Sign(hostResult.InvocationContext.Action);

                    await activityPublisher.PublishActionFinished
                    (
                        action: hostResult.InvocationContext.Action,
                        metricBag: counterBag,
                        reason: "processed request via .net core",
                        @where: InvocationPlace.Target,
                        relatedObject: null,
                        host: this,
                        error: error ?? hostResult.Error
                    );
                }
               
            }
        }

        public string HostName { get; } = nameof(HttpMiddleware);
    }
}