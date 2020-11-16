using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Protocol.Response.Definitions;

namespace Modulos.Messaging.Diagnostics.Activity.NewResponse
{
    public interface INewResponseActivityProcessor
    {
        Task Process(IResponseData response, IMetricBag metricBag);
    }
}