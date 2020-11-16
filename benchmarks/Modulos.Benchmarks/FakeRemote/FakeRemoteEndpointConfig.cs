using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Transport.Http;

namespace Modulos.Benchmarks.FakeRemote
{
    public class FakeRemoteEndpointConfig :   IEndpointConfig
    {
        public FakeRemoteEndpointConfig()
        {
            var dictionary = new Dictionary<string, object>();
            Properties = new ReadOnlyDictionary<string, object>(dictionary);
        }

        public EndpointConfigInfo Info { get; set; } = new EndpointConfigInfo(
            new EndpointConfigMark
            (
                "fake-mark", HttpTransport.EngineId
            ),
            new EndpointConfigStamp()
        );

        public string Address { get; set; }
        public int Order { get; set; }
        public bool IsExpired { get; set; }
        public bool IsDefault { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime? ExpirationTimeUtc { get; set; }
        public IReadOnlyDictionary<string, object> Properties { get; }
    }
}