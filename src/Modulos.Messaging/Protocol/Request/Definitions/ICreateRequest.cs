using System.Threading.Tasks;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Security;

namespace Modulos.Messaging.Protocol.Request.Definitions
{
    public interface ICreateRequest
    {
        ValueTask<ICreatedRequestData> Create([JetBrains.Annotations.NotNull] IMessage message,
            [JetBrains.Annotations.NotNull] IMessageConfig messageConfig,
            [JetBrains.Annotations.NotNull] IEndpointConfig endpointConfig,
            [JetBrains.Annotations.NotNull] IMetricBag metricBag,
            [JetBrains.Annotations.NotNull] IInvocationContext invocationContext,
            [JetBrains.Annotations.NotNull] IAuthenticationData authenticationData);
    }
}