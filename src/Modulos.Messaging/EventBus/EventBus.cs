using System;
using Microsoft.Extensions.DependencyInjection;

namespace Modulos.Messaging.EventBus
{
    public class EventBus : IEventBus
    {
        private readonly IServiceProvider serviceProvider;

        public EventBus(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var handlers = serviceProvider.GetServices<IHandleEvent<TEvent>>();

            foreach (var handler in handlers)
            {
                handler.Handle(@event);
            }

            //var handlerType = typeof(IHandleEvent<>).MakeGenericType(@event.GetType());
            //var type = typeof(IEnumerable<>).MakeGenericType(handlerType);
            //var handlers = (IEnumerable<IHandleEvent<TEvent>>)cc.Resolve(type);
        }
    }
}