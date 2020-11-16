using System.Collections.Generic;
using System.Threading.Tasks;

namespace Modulos.Messaging.Configuration
{
    public interface IEndpointConfigProvider
    {
        Task<bool> Exists(EndpointConfigInfo endpointConfigInfo);

        Task<IEndpointConfig> GetPreferred(EndpointConfigMark endpointConfigMark);

        Task Discard(EndpointConfigMark endpointConfigMark);
        
        Task<IEnumerable<IEndpointConfig>> GetAll(EndpointConfigMark endpointConfigMark);

        Task DiscardAll();
    }
}