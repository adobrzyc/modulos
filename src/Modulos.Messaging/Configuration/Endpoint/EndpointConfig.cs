using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Modulos.Messaging.Configuration
{
    public sealed class EndpointConfig : IEndpointConfig
    {
        public EndpointConfig()
            : this(Enumerable.Empty<KeyValuePair<string, object>>())
        {

        }

        public EndpointConfig(IEnumerable<KeyValuePair<string, object>> additionalProperties)
        {

            var dictionary = new Dictionary<string, object>();
            foreach (var keyValuePair in additionalProperties)
                dictionary.Add(keyValuePair.Key,keyValuePair.Value);

            Properties = new ReadOnlyDictionary<string, object>(dictionary);
        }

        public EndpointConfigInfo Info { get; set; }

        public string Address { get; set; }
        public int Order { get; set; }
        public bool IsExpired { get; set; }
        public bool IsDefault { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime? ExpirationTimeUtc { get; set; } //todo: change it for some transportId of delta
        //public DateTime? GracePeriodUntilTimeUtc { get; set; }

        public IReadOnlyDictionary<string, object> Properties { get; }
    }
}