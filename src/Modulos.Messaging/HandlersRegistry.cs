using System;
using System.Collections.Generic;

namespace Modulos.Messaging
{
    public class HandlersRegistry : IHandlersRegistry
    {
        private readonly IDictionary<Type, bool> registered = new Dictionary<Type, bool>();

        public IEnumerable<IHandlerDescription> Descriptions { get; }

        public HandlersRegistry(IEnumerable<IHandlerDescription> handlers)
        {
            Descriptions = handlers;

            foreach (var desc in Descriptions)
            {
                if(!registered.ContainsKey(desc.MessageType))
                    registered.Add(desc.MessageType, true);
            }
        }

       
        public bool HasHandlerFor(IMessage message)
        {
            return registered.ContainsKey(message.GetType());
        }

        public bool HasHandlerFor(Type messageType)
        {
            return registered.ContainsKey(messageType);
        }

       
    }
}