using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Protocol.Request.Definitions;

namespace Modulos.Messaging.Diagnostics.Activity.NewRequest
{
    public interface INewRequestActivityProcessor
    {
        Task Process(IRequestData requestData, InvocationPlace where, IMetricBag metricBag);
    }
}