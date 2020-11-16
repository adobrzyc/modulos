using System.Threading.Tasks;

namespace Modulos.Messaging.Configuration
{
    /// <summary>
    /// Checks if config is up to date.
    /// </summary>
    public interface IEndpointConfigValidator
    {
        /// <summary>
        /// Determines whether passed <see cref="EndpointConfigInfo"/> is up to date.
        /// </summary>
        /// <param name="endpointConfigInfo">
        /// Configuration info to check.
        /// </param>
        /// <returns>
        /// Configuration state.
        /// </returns>
        Task<EndpointConfigState> CheckConfig(EndpointConfigInfo endpointConfigInfo);
    }
}