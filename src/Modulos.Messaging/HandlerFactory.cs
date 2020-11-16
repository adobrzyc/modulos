using System;

namespace Modulos.Messaging
{
    public class HandlerFactory : IHandlerFactory
    {
        private readonly IServiceProvider serviceProvider;
      
        public HandlerFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public object GetHandler(IMessage message)
        {
            object handler = null;
            var messageType = message.GetType();

            if (message is IQueryBase)
            {
                var handlerType = typeof(IQueryHandler<,>).MakeGenericType(messageType, new QueryInfo(messageType).ResultType);
                handler = serviceProvider.GetService(handlerType);
            }

            if (message is ICommandBase)
            {
                var handlerType = typeof(ICommandHandler<>).MakeGenericType(messageType);
                handler = serviceProvider.GetService(handlerType);
            }

            if(handler == null)
                throw new UnableToFindCqrsHandlerForSpecifiedMessage(message);

            return handler;
        }
    }
}