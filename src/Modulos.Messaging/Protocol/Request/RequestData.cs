using Modulos.Messaging.Protocol.Request.Definitions;
using Modulos.Messaging.Serialization;

namespace Modulos.Messaging.Protocol.Request
{
    public class RequestData : IRequestData
    {
        public IMessage Message { get; }
        public IMessageHeader MessageHeader { get; }
        public ISerializedObject SerializedMessage { get; }
        public ITransferObject TransferObject { get; }

        public RequestData(IMessage message, IMessageHeader messageHeader, ISerializedObject serializedMessage, ITransferObject transferObject)
        {
            Message = message;
            MessageHeader = messageHeader;
            SerializedMessage = serializedMessage;
            TransferObject = transferObject;
        }
    }
}