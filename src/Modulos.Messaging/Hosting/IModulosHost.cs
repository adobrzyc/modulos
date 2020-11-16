using System.Threading;
using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Protocol;
using Modulos.Messaging.Protocol.Response.Definitions;

namespace Modulos.Messaging.Hosting
{
    public interface IModulosHost
    {
        Task<IResponseData> Execute(ITransferObject transferObject, IMetricBag metricBag,  CancellationToken token);
    }
}