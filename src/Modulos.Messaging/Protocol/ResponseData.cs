using System;
using Modulos.Messaging.Protocol.Response.Definitions;
using Modulos.Messaging.Serialization;

namespace Modulos.Messaging.Protocol
{
    public sealed class ResponseData : IResponseData
    {
        public ITransferObject TransferObject { get; set; }
        public ISerializedObject SerializedResponse { get; }
        public IMessageHeader MessageHeader { get; set; }
        public IInvocationContext InvocationContext { get; set; }
        public Exception Error { get; set; }

        public ResponseData(ITransferObject transferObject, IMessageHeader messageHeader, Exception error, 
            IInvocationContext invocationContext, ISerializedObject serializedResponse)
        {
            TransferObject = transferObject;
            MessageHeader = messageHeader;
            InvocationContext = invocationContext; 
            SerializedResponse = serializedResponse;
            Error = error;
        }
    }
}