using System.Threading;
using System.Threading.Tasks;
using Modulos.Messaging.Configuration;
using Modulos.Pipes;

// ReSharper disable once UnusedType.Global

namespace Modulos.Messaging.Transport.Policy.Pipes
{
    internal class DetermineEndpointConfigPipe : IPipe
    {
        private readonly PipeData data;
        private readonly IEndpointConfigProvider endpointConfigProvider;

        public DetermineEndpointConfigPipe(PipeData data, IEndpointConfigProvider endpointConfigProvider)
        {
            this.data = data;
            this.endpointConfigProvider = endpointConfigProvider;
        }

        public async Task<PipeResult> Execute(CancellationToken cancellationToken)
        {
            var endpointConfig = await endpointConfigProvider.GetPreferred(data.Config.EndpointConfigMark) 
                .ConfigureAwait(false);

            return new PipeResult(PipeActionAfterExecute.Continue, endpointConfig);
        }
    }
}