using System;
using System.Threading;
using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Protocol.Request.Definitions;
using Modulos.Pipes;

// ReSharper disable once UnusedType.Global

namespace Modulos.Messaging.Transport.Policy.Pipes
{
    internal class SendRequestPipe : IPipe
    {
        private readonly PipeData data;
        private readonly ICreatedRequestData requestData;
        private readonly IMetricBag metricBag;

        public SendRequestPipe(PipeData data, ICreatedRequestData requestData, IMetricBag metricBag)
        {
            this.data = data;
            this.requestData = requestData;
            this.metricBag = metricBag;
        }

        public async Task<PipeResult> Execute(CancellationToken cancellationToken)
        {
            var t1 = data.Context.Action.ElapsedMilliseconds;

            var response = await data.TransportEngine.Send(requestData, metricBag, cancellationToken)
                .ConfigureAwait(false);

            metricBag.Add
            (
                Kind.Transfer,
                "response", InvocationPlace.Caller,
                data.Context.Action.ElapsedMilliseconds - t1,
                data.Context.Action
            );

            metricBag.Add(Kind.ResponseArrived, InvocationPlace.Caller, DateTime.UtcNow.Ticks);
         
            return new PipeResult(PipeActionAfterExecute.Continue,response);
        }
    }
}