using System.Threading;
using System.Threading.Tasks;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Protocol.Request.Definitions;
using Modulos.Pipes;

// ReSharper disable once UnusedType.Global

namespace Modulos.Messaging.Transport.Policy.Pipes
{
    internal class CreateRequestPipe : IPipe
    {
        private readonly PipeData data;
        private readonly ICreateRequest createRequest;
        private readonly IMetricBag metricBag;
        private readonly IEndpointConfig endpointConfig;

        public CreateRequestPipe(PipeData data, ICreateRequest createRequest, IMetricBag metricBag, 
            IEndpointConfig endpointConfig)
        {
            this.data = data;
            this.createRequest = createRequest;
            this.metricBag = metricBag;
            this.endpointConfig = endpointConfig;
        }

        public async Task<PipeResult> Execute(CancellationToken cancellationToken)
        {
            var request = await createRequest.Create
            (
                message: data.Message,
                messageConfig: data.Config,
                endpointConfig: endpointConfig,
                metricBag: metricBag,
                invocationContext: data.Context,
                null //todo: [not work] to fill - it wont work  
            ).ConfigureAwait(false);

            return new PipeResult(PipeActionAfterExecute.Continue, request);
        }
    }
}