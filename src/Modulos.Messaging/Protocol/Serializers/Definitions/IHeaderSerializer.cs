using Modulos.Messaging.Diagnostics.Metrics;

namespace Modulos.Messaging.Protocol.Serializers.Definitions
{
    internal interface IHeaderSerializer
    {
        string Serialize([JetBrains.Annotations.NotNull] IMessageHeader headerToSerialize, InvocationPlace where, [JetBrains.Annotations.NotNull] IMetricBag metricBag);

        IMessageHeader Deserialize([JetBrains.Annotations.NotNull] ITransferObject transferObject, [JetBrains.Annotations.NotNull] IMetricBag metricBag, InvocationPlace where);
    }
}