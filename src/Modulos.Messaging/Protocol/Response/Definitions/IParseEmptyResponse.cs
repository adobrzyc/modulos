using System.Threading.Tasks;

namespace Modulos.Messaging.Protocol.Response.Definitions
{
    internal interface IParseEmptyResponse
    {
        Task<IParsedResponseData<TResult>> Parse<TResult>([JetBrains.Annotations.NotNull] IMessageHeader responseHeader, [JetBrains.Annotations.NotNull] ITransferObject transferObject);
    }
}