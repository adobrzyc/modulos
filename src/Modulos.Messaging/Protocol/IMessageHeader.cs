using System.Collections.Generic;
using Modulos.Messaging.Compression;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Security;
using Modulos.Messaging.Serialization;
using Modulos.Messaging.Transport;

namespace Modulos.Messaging.Protocol
{
    public interface IMessageHeader
    {
        TransportEngineId TransportEngine { get; }

        ConnectionMark? ConnectionMark { get; }

        EndpointConfig EndpointConfig { get; }

        TypeInfo TypeInfo { get; }

        AppInfo AppInfo { get; }

        InvocationContext Context { get; }

        SecurityContext SecurityContext { get; }

        MessageKind MessageKind { get; }

        CompressionEngineId RequestCompressor { get; }

        CompressionEngineId ResponseCompressor { get; }

        SerializerId RequestSerializer { get; }

        SerializerId ResponseSerializer { get; }

        bool RefreshConnection { get; }

        /// <summary>
        /// Additional data set to use for some custom implementations. 
        /// </summary>
        IDictionary<string, string> Properties { get; }

        /// <summary>
        /// Defines headers/properties of specified protocol, e.q: by default http transport engine
        /// exposes this property into http headers. By default protocol data may overwrite
        /// protocol values previous by modulos components.
        /// 
        /// This member should not be serialized.
        /// </summary>
        IDictionary<string, string> ProtocolData{ get; }
    }
}