using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;

namespace Modulos.Messaging.Protocol.Response.Definitions
{
    internal interface ICreateObjectResponse
    {
        Task<IResponseData> Create([JetBrains.Annotations.NotNull] object response,
            [JetBrains.Annotations.NotNull] IMessageHeader requestHeader,
            [JetBrains.Annotations.NotNull] IMetricBag metricBag,
            [JetBrains.Annotations.NotNull] IInvocationContext invocationContext);
    }
}