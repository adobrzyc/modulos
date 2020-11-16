using System.Threading;
using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Pipes;

// ReSharper disable once UnusedType.Global

namespace Modulos.Messaging.Transport.Policy.Pipes
{
    internal class FinishInitializationPipe : IPipe
    {
        private readonly PipeData data;
        private readonly IMetricBag metricBag;

        public FinishInitializationPipe(PipeData data, IMetricBag metricBag)
        {
            this.data = data;
            this.metricBag = metricBag;
        }

        public Task<PipeResult> Execute(CancellationToken cancellationToken)
        {
            metricBag.Add(Kind.Initialization, InvocationPlace.Caller, data.Context.Action.ElapsedMilliseconds);

            return Task.FromResult(new PipeResult(PipeActionAfterExecute.Continue));
        }
    }
}