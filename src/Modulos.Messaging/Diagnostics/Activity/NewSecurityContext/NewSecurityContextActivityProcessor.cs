using System;
using System.Threading.Tasks;

namespace Modulos.Messaging.Diagnostics.Activity.NewSecurityContext
{
    //todo: redesign required (important)
    public class NewSecurityContextActivityProcessor : INewSecurityContextActivityProcessor
    {
        private readonly IOneOrManyActivityHandlerInvoker<NewSecurityContextActivity> activityInvoker;

        public NewSecurityContextActivityProcessor(IOneOrManyActivityHandlerInvoker<NewSecurityContextActivity> activityInvoker)
        {
            this.activityInvoker = activityInvoker;
        }

        public async Task Process([JetBrains.Annotations.NotNull] IInvocationContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (activityInvoker.Count == 0) return;
            await activityInvoker.Invoke(new NewSecurityContextActivity(context));
        }
    }
}