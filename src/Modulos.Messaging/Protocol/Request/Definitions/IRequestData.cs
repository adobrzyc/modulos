using Modulos.Messaging.Serialization;

namespace Modulos.Messaging.Protocol.Request.Definitions
{
    public interface IRequestData
    {
        IMessage Message { get; }
        IMessageHeader MessageHeader { get; }
        ISerializedObject SerializedMessage { get; }
        ITransferObject TransferObject { get;}
    }
}