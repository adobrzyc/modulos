using System;
using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;
using Kind = Modulos.Messaging.Diagnostics.Metrics.Kind;

namespace Modulos.Messaging.Diagnostics.Activity.FinishAction
{
    public class FinishActionActivityProcessor : IFinishActionActivityProcessor
    {
        private readonly IOneOrManyActivityHandlerInvoker<FinishActionActivity> activityHandlerInvoker;

        public FinishActionActivityProcessor(IOneOrManyActivityHandlerInvoker<FinishActionActivity> activityHandlerInvoker)
        {
            this.activityHandlerInvoker = activityHandlerInvoker;
        }

        public async Task Process(IActionInfo action, [JetBrains.Annotations.NotNull] IMetricBag metricBag, string reason, InvocationPlace where, 
            object relatedObject, object host, Exception error)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (metricBag == null) throw new ArgumentNullException(nameof(metricBag));

            if (activityHandlerInvoker.Count == 0)
                return;

            metricBag.Add
            (
                Kind.Execution, where,
                action.ElapsedMilliseconds,
                action
            );

            //if (error != null)
            //{
            //    //todo: #check usage: of text monitor
            //    textMonitor.Add(new TextData(action)
            //    {
            //        InvocationPlace = InvocationPlace.Caller,
            //        Value = error.GetMessage(),
            //        Transport = Logging.Text.Transport.Error,
            //        CustomInfo = error.GetType().FullName
            //    });
            //}
            
            var @event = new FinishActionActivity(action, metricBag, reason, where, relatedObject, host, error);
            await activityHandlerInvoker.Invoke(@event);
        }
    }
}