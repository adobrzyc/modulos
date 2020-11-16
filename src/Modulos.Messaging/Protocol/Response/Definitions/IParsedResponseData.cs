using Modulos.Messaging.Serialization;

namespace Modulos.Messaging.Protocol.Response.Definitions
{
    public interface IParsedResponseData<out TResult>
    {
        IMessageHeader MessageHeader { get; }
        TResult Result { get; }
        ITransferObject TransferObject { get;}
        ISerializedObject SerializedObject { get; }
    }
}