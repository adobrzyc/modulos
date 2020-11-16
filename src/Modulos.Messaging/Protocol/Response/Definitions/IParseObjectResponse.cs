using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;

namespace Modulos.Messaging.Protocol.Response.Definitions
{
    internal interface IParseObjectResponse
    {
        Task<IParsedResponseData<TResult>> Parse<TResult>(IMessageHeader responseHeader, ITransferObject transferObject, IMetricBag metricBag);
    }
}