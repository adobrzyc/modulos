using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;

namespace Modulos.Messaging.Protocol.Response.Definitions
{
    public interface IParseResponse
    {
        Task<IParsedResponseData<TResult>> Parse<TResult>([JetBrains.Annotations.NotNull] ITransferObject transferObject,
            [JetBrains.Annotations.NotNull] IMessageHeader requestHeader,
            [JetBrains.Annotations.NotNull] IMetricBag metricBag);
    }
}