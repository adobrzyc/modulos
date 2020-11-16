using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Transport;
using Modulos.Messaging.Transport.Http;

namespace Modulos.Messaging.Tests.Tests.Integration
{
    public class FakeRemoteEndpointConfigSource : IEndpointConfigSource
    {
        private readonly IHttpEndpointConfiguration  hostConfig;
        public TransportEngineId Transport { get; } = HttpTransport.EngineId;

        public FakeRemoteEndpointConfigSource(IHttpEndpointConfiguration hostConfig)
        {
            this.hostConfig = hostConfig;
        }

        public Task<IEnumerable<IEndpointConfig>> GetConfiguration(EndpointConfigMark endpointConfigMark)
        {
            var config = new EndpointConfig
            {
                Info = new EndpointConfigInfo(endpointConfigMark: endpointConfigMark, endpointConfigStamp: new EndpointConfigStamp(Guid.Empty)),
                Address = $"http://localhost/{hostConfig.EndpointName}", // due to simulator behavior address domain will be overwritten
                Order = 1,
                IsDefault = true, 
                IsAvailable = true
            };

            var result = new[]
            {
                (IEndpointConfig) config
            };


            return Task.FromResult(result.AsEnumerable());
        }
    }
}