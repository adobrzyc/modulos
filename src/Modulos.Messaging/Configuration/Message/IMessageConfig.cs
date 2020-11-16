using System;
using System.Collections.Generic;
using Modulos.Messaging.Compression;
using Modulos.Messaging.Protocol;
using Modulos.Messaging.Security;
using Modulos.Messaging.Serialization;
using Modulos.Messaging.Transport;

// ReSharper disable UnusedMember.Global

namespace Modulos.Messaging.Configuration
{
    /// <summary>
    /// Defines message configuration. 
    /// </summary>
    public interface IMessageConfig : IFreezable
    {
        /// <summary>
        /// Defines transport layer used to communicate.
        /// </summary>
        TransportEngineId TransportEngine { get; set; }

        /// <summary>
        /// Defines way to authenticate communication.
        /// </summary>
        AuthenticationMode AuthenticationMode { get; set; }

        /// <summary>
        /// It's used as identifier to obtain endpoint configuration(s) via transport
        /// layer (<see cref="ITransportEngine"/>).
        /// </summary>
        EndpointConfigMark EndpointConfigMark { get; set; }

        /// <summary>
        /// Defines compression engine used to compress request.
        /// </summary>
        CompressionEngineId RequestCompressor { get; set; }

        /// <summary>
        /// Defines compression engine used to compress response.
        /// </summary>
        CompressionEngineId ResponseCompressor { get; set; }

        /// <summary>
        /// Defines serialization engine used to serialize request.
        /// </summary>
        SerializerId RequestSerializer { get; set; }

        /// <summary>
        /// Defines serialization engine used to serialize response.
        /// </summary>
        SerializerId ResponseSerializer { get; set; }

        /// <summary>
        /// Defines timeout for each call. 
        /// </summary>
        /// <remarks>
        /// For most of the implementations it's propagated into transport layer.
        /// By default timeout is not used for local execution (InMemoryTransport).
        /// </remarks>
        TimeSpan Timeout { get; set; }

        /// <summary>
        /// Defines maximum retries for operation.
        /// </summary>
        int MaxInvokeAttempts { get; set; }

        /// <summary>
        /// Determines whether <see cref="IMessageHeader.ConnectionMark"/> is supported.
        /// Marking connection is mostly used to optimize environments using load balancers.
        /// </summary>
        bool SupportConnectionMark { get; set; }

        /// <summary>
        /// Additional information associated with configuration.
        /// </summary>
        IDictionary<string, string> Properties { get; }

        /// <summary>
        /// Simplified Access to <see cref="Properties"/>. 
        /// </summary>
        /// <param name="key">
        /// Key of property.
        /// </param>
        /// <returns>
        /// Value of property.
        /// </returns>
        string this[string key] { get; set; }
        
    }
}