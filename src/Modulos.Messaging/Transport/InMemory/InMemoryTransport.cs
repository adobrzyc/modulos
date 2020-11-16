using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Hosting;
using Modulos.Messaging.Protocol;
using Modulos.Messaging.Protocol.Request.Definitions;

namespace Modulos.Messaging.Transport.InMemory
{
     public class InMemoryTransport : ITransportEngine
     { 
        private readonly IServiceProvider serviceProvider;

        public static readonly TransportEngineId EngineId = new TransportEngineId("in-memory");

        TransportEngineId ITransportEngine.EngineId => EngineId;
        
        public bool IsLocal { get; } = true;

        public InMemoryTransport(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ITransferObject CreateTransferObject()
        {
            return new TransferObject();
        }

        public ITransferObject CreateTransferObject(ITransferObject source)
        {
            return new TransferObject
            {
                Header = source.Header,
                ByteContent = source.ByteContent,
                MediaType = source.MediaType,
                MediaTypeOfStream = source.MediaTypeOfStream,
                Stream = source.Stream,
                StringContent = source.StringContent
            };
        }

        public Task Validate(IMessage message, IMessageConfig messageConfig, IMetricBag metricBag,
            IInvocationContext invocationContext)
        {
            return Task.CompletedTask;
        }

        public async Task<ITransferObject> Send(ICreatedRequestData createdRequestData, IMetricBag metricBag, CancellationToken abortRequest)
        {
            var host = serviceProvider.GetRequiredService<IModulosHost>();
            //createdRequestData.MessageHeader.a
            return (await host.Execute(createdRequestData.TransferObject, metricBag, abortRequest)).TransferObject;
        }

        public bool IsTransportException(Exception exception)
        {
            return false;
        }

        public class EndpointConfigSource : IEndpointConfigSource
        {
            public TransportEngineId Transport { get; } = EngineId;

            public Task<IEnumerable<IEndpointConfig>> GetConfiguration(EndpointConfigMark endpointConfigMark)
            {
                IEnumerable<IEndpointConfig> configs = new IEndpointConfig[]
                {
                    new EndpointConfig
                    {
                        Address = "in-memory",
                        Order = 1,
                        IsAvailable = true,
                        IsDefault = true,
                        IsExpired = false,
                        Info = new EndpointConfigInfo
                        (
                            endpointConfigMark,
                            new EndpointConfigStamp()
                        )
                    }
                };

                return Task.FromResult(configs);
            }
        }
     }

}