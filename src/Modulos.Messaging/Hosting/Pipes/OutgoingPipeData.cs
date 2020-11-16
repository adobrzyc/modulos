using System;
using Modulos.Messaging.Protocol.Response.Definitions;

namespace Modulos.Messaging.Hosting.Pipes
{
    public sealed class OutgoingPipeData
    {
        public IResponseData Response { get; }
        public Exception Error { get; }
        public IMessage Message { get; }
        public bool IsLocalInvocation { get; }

        public OutgoingPipeData(IResponseData response, Exception error, IMessage message, bool isLocalInvocation)
        {
            Error = error;
            Message = message;
            IsLocalInvocation = isLocalInvocation;
            Response = response;
        }
    }
}