using System.Diagnostics;
using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Diagnostics.Options;
using Modulos.Messaging.Protocol.Request.Definitions;

namespace Modulos.Messaging.Diagnostics.Activity.NewRequest
{
    public class NewRequestActivityProcessor : INewRequestActivityProcessor
    {
        private readonly IOneOrManyActivityHandlerInvoker<NewRequestActivity> activityHandlerInvoker;
        private readonly ILogMarkOption markOption;

        public NewRequestActivityProcessor(IOneOrManyActivityHandlerInvoker<NewRequestActivity> activityHandlerInvoker, ILogMarkOption markOption)
        {
            this.markOption = markOption;
            this.activityHandlerInvoker = activityHandlerInvoker;
        }

        public async Task Process(IRequestData requestData, InvocationPlace where, IMetricBag metricBag)
        {
            if (activityHandlerInvoker.Count == 0) return;

            var sw = new Stopwatch();
            sw.Start();
            try
            {
                var @event = new NewRequestActivity(requestData, markOption.Value);
                await activityHandlerInvoker.Invoke(@event);
            }
            finally
            {
                metricBag.Add(Kind.Monitoring, "log request", where, sw.ElapsedMilliseconds,requestData.MessageHeader.Context.Action);
                sw.Stop();
            }
        }
    }
}