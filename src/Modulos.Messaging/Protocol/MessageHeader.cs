using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Modulos.Messaging.Compression;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Security;
using Modulos.Messaging.Serialization;
using Modulos.Messaging.Transport;

namespace Modulos.Messaging.Protocol
{
    [DataContract]
    public sealed class MessageHeader : IMessageHeader
    {
        [DataMember]
        public TypeInfo TypeInfo { get; set; }

        [DataMember]
        public EndpointConfig EndpointConfig { get; set; }

        [DataMember]
        public AppInfo AppInfo { get; set; }

        [DataMember]
        public ConnectionMark? ConnectionMark { get; set; }

        [DataMember]
        public InvocationContext Context { get; set; }
        
        [DataMember]
        public SecurityContext SecurityContext { get; set; }

        [DataMember]
        public MessageKind MessageKind { get; set; }

        [DataMember]
        public SerializerId RequestSerializer { get; set; }
            
        [DataMember]
        public SerializerId ResponseSerializer { get; set; }

        [DataMember]
        public CompressionEngineId RequestCompressor { get; set; }

        [DataMember]
        public CompressionEngineId ResponseCompressor { get; set; }

        [DataMember]
        public TransportEngineId TransportEngine { get; set; }

        [DataMember]
        public bool RefreshConnection { get; set; }
       
        [DataMember]
        public IDictionary<string, string> Properties { get; private set; } = new ConcurrentDictionary<string, string>();

        [IgnoreDataMember]
        [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
        public IDictionary<string, string> ProtocolData { get; private set;} = new ConcurrentDictionary<string, string>();
    }
}