using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;

namespace Modulos.Messaging.Protocol.Request.Definitions
{
    internal interface IParseRequest
    {
        ValueTask<IRequestData> Parse([JetBrains.Annotations.NotNull] ITransferObject transferObject, [JetBrains.Annotations.NotNull] IMetricBag metricBag);
    }
}