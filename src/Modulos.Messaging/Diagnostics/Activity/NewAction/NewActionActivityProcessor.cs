using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Modulos.Messaging.Diagnostics.Activity.NewAction
{
    internal class NewActionActivityProcessor : INewActionActivityProcessor
    {
        private readonly IOneOrManyActivityHandlerInvoker<NewActionActivity> activityHandlerInvoker;
       
        public NewActionActivityProcessor(IOneOrManyActivityHandlerInvoker<NewActionActivity> activityHandlerInvoker)
        {
            this.activityHandlerInvoker = activityHandlerInvoker;
        }
      
        public async Task Process([NotNull] IActionInfo newAction, [CanBeNull] IActionInfo previousAction)
        {
            if (newAction == null) throw new ArgumentNullException(nameof(newAction));
            
            if (activityHandlerInvoker.Count == 0)
                return;

            var @event = new NewActionActivity(newAction, previousAction);
            await activityHandlerInvoker.Invoke(@event);
        }
    }
}