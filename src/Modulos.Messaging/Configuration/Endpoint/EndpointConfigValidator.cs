using System.Threading.Tasks;

namespace Modulos.Messaging.Configuration
{
    public class EndpointConfigValidator : IEndpointConfigValidator
    {
        private readonly IEndpointConfigProvider endpointConfigProvider;

        public EndpointConfigValidator(IEndpointConfigProvider endpointConfigProvider)
        {
            this.endpointConfigProvider = endpointConfigProvider;
        }

        public async Task<EndpointConfigState> CheckConfig(EndpointConfigInfo endpointConfigInfo)
        {
            return await endpointConfigProvider.Exists(endpointConfigInfo) ? EndpointConfigState.Valid : EndpointConfigState.Obsolete;
        }
    }
}