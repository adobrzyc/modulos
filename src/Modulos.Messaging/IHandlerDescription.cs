using System;

namespace Modulos.Messaging
{
    public interface IHandlerDescription
    {
        Type HandlerType { get; }
        Type MessageType { get; }
    }
}