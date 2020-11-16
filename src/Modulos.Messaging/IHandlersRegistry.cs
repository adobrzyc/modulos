using System;
using System.Collections.Generic;

namespace Modulos.Messaging
{
    public interface IHandlersRegistry
    {
        bool HasHandlerFor(IMessage message);
        bool HasHandlerFor(Type messageType);
        IEnumerable<IHandlerDescription> Descriptions { get; }
    }
}