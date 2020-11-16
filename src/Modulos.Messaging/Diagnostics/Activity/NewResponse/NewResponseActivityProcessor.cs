using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Diagnostics.Options;
using Modulos.Messaging.Protocol.Response.Definitions;

namespace Modulos.Messaging.Diagnostics.Activity.NewResponse
{
    public class NewResponseActivityProcessor : INewResponseActivityProcessor
    {
        private readonly IOneOrManyActivityHandlerInvoker<NewResponseActivity> activityInvoker;
        private readonly ILogMarkOption markOption;

        public NewResponseActivityProcessor(IOneOrManyActivityHandlerInvoker<NewResponseActivity> activityInvoker, ILogMarkOption markOption)
        {
            this.markOption = markOption;
            this.activityInvoker = activityInvoker;
        }

        public async Task Process([JetBrains.Annotations.NotNull] IResponseData responseData, [JetBrains.Annotations.NotNull] IMetricBag metricBag)
        {
            if (responseData == null) throw new ArgumentNullException(nameof(responseData));
            if (metricBag == null) throw new ArgumentNullException(nameof(metricBag));

            if (activityInvoker.Count == 0) return;

            var sw = new Stopwatch();
            sw.Start();
            try
            {
                var @event = new NewResponseActivity(responseData, markOption.Value);
                await activityInvoker.Invoke(@event);
            }
            finally
            {
                metricBag.Add
                (
                    Kind.Monitoring, 
                    "log response", 
                    InvocationPlace.Target, 
                    sw.ElapsedMilliseconds, 
                    responseData.MessageHeader.Context.Action
                );

                sw.Stop();
            }
        }
    }
}