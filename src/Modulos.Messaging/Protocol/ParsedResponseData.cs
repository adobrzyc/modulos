using Modulos.Messaging.Protocol.Response.Definitions;
using Modulos.Messaging.Serialization;

namespace Modulos.Messaging.Protocol
{
    public class ParsedResponseData<TResult> : IParsedResponseData<TResult>
    {
        public IMessageHeader MessageHeader { get; }
        public TResult Result { get; }
        public ITransferObject TransferObject { get; }
        public ISerializedObject SerializedObject { get; }

        public ParsedResponseData(IMessageHeader messageHeader, TResult result, ITransferObject transferObject, ISerializedObject serializedObject)
        {
            MessageHeader = messageHeader;
            Result = result;
            TransferObject = transferObject;
            SerializedObject = serializedObject;
        }
    }
}