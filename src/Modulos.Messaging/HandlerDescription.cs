using System;

namespace Modulos.Messaging
{
    public class HandlerDescription : IHandlerDescription
    {
        public HandlerDescription(Type handlerType, Type messageType)
        {
            HandlerType = handlerType;
            MessageType = messageType;
        }

        public Type HandlerType { get; }
        public Type MessageType { get; }
    }
}