using System;
using System.Reflection;
using Modulos.Messaging.Operation;

namespace Modulos.Messaging
{
    internal class ActionInfoFactory : IActionInfoFactory
    {
        public ActionInfo Create(IMessage message, string hostName, OperationId? operationId = null, ActionId? actionId = null, bool start = true)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            operationId ??= new OperationId(KeyGenerator.NewSequentialGuid());

            actionId ??= new ActionId(KeyGenerator.NewSequentialGuid());

            var messageType = message.GetType();
            var mark = message.GetType().GetCustomAttribute<TypeMarkAttribute>()?.Mark;

            return new ActionInfo
            (
                operationId.Value, 
                actionId.Value, 
                messageType.FullName, 
                hostName, 
                messageType.AssemblyQualifiedName,  
                mark, start
            );
        }

        public ActionInfo Create(IMessage message, IActionHost actionHost, OperationId? operationId = null, ActionId? actionId = null, bool start = true)
        {
            if (actionHost == null) throw new ArgumentNullException(nameof(actionHost));
           
            return Create(message, actionHost.HostName, operationId, actionId, start);
        }
    }
}