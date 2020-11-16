using System;
using System.Threading;
using System.Threading.Tasks;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Protocol;
using Modulos.Messaging.Protocol.Request.Definitions;
using Modulos.Messaging.Transport.Policy;

namespace Modulos.Messaging.Transport
{
    /// <summary>
    /// Transport layer to provide communication for specific protocol/technology e.q: http.
    /// </summary>
    public interface ITransportEngine 
    {
        /// <summary>
        /// Unique identifier of transport engine.
        /// </summary>
        TransportEngineId EngineId { get; }

        /// <summary>
        /// Determines whether engine is a local engine. Local engines means execute locally.
        /// </summary>
        bool IsLocal { get; }

        /// <summary>
        /// Creates new instance of and empty transfer object.
        /// </summary>
        /// <returns>
        /// New an empty instance of transfer object.
        /// </returns>
        ITransferObject CreateTransferObject();

        /// <summary>
        /// Creates new instance of transfer object filled with <param name="source"></param> values.
        /// </summary>
        /// <returns>
        /// New instance of transfer object filled with <param name="source"></param> values.
        /// </returns>
        ITransferObject CreateTransferObject(ITransferObject source);

        /// <summary>
        /// Used by <see cref="ITransportPolicy"/> to validate passed parameters.
        /// </summary>
        /// <param name="message">
        /// Message for validation.
        /// </param>
        /// <param name="messageConfig">
        /// Message configuration.
        /// </param>
        /// <param name="metricBag">
        /// Counter bag.
        /// </param>
        /// <param name="invocationContext">
        /// Hydra context.
        /// </param>
        Task Validate(IMessage message, IMessageConfig messageConfig, IMetricBag metricBag, IInvocationContext invocationContext);

        /// <summary>
        /// Sends request.
        /// </summary>
        /// <param name="createdRequestData">Request data.</param>
        /// <param name="metricBag">Counter bag.</param>
        /// <param name="abortRequest">Token used to break sending.</param>
        /// <returns> Response as a transfer object.</returns>
        Task<ITransferObject> Send(ICreatedRequestData createdRequestData,  IMetricBag metricBag, CancellationToken abortRequest);
        
        /// <summary>
        /// Defines whether specified exception is transport engine exception. 
        /// </summary>
        /// <param name="exception">
        /// Examined exception.
        /// </param>
        /// <returns>
        /// True if exception is transport engine exception, otherwise false.
        /// </returns>
        bool IsTransportException(Exception exception);
    }
}