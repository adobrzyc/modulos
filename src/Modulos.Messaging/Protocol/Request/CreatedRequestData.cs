using Modulos.Messaging.Configuration;
using Modulos.Messaging.Protocol.Request.Definitions;
using Modulos.Messaging.Serialization;

namespace Modulos.Messaging.Protocol.Request
{
    public sealed class CreatedRequestData : RequestData, ICreatedRequestData
    {
        public IMessageConfig MessageConfig { get; }
        public IEndpointConfig EndpointConfig { get; }

        public CreatedRequestData(IMessage message, IMessageHeader messageHeader, ISerializedObject serializedMessage,
            ITransferObject transferObject, IMessageConfig messageConfig,
            IEndpointConfig endpointConfig
        ) : base(message, messageHeader, serializedMessage, transferObject)
        {
            MessageConfig = messageConfig;
            EndpointConfig = endpointConfig;

        }
    }
}