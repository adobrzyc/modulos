using System;
using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;

namespace Modulos.Messaging.Protocol.Response.Definitions
{
    internal interface ICreateFaultResponse
    {
        Task<IResponseData> CreateWithoutContext([JetBrains.Annotations.NotNull] Exception error, [JetBrains.Annotations.NotNull] IMetricBag metricBag);

        Task<IResponseData> Create([JetBrains.Annotations.NotNull] Exception error, [JetBrains.Annotations.NotNull] IMessageHeader requestHeader,
            [JetBrains.Annotations.NotNull] IMetricBag metricBag, [JetBrains.Annotations.NotNull] IInvocationContext invocationContext);
    }
}