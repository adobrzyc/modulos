using System.Threading.Tasks;
using Modulos.Messaging.Protocol.Response.Definitions;

namespace Modulos.Messaging.Protocol.Response
{
    internal class ParseEmptyResponse : IParseEmptyResponse
    {
        public Task<IParsedResponseData<TResult>> Parse<TResult>(IMessageHeader responseHeader, ITransferObject transferObject)
        {
            var result = (IParsedResponseData<TResult>) new ParsedResponseData<TResult>
            (
                responseHeader, 
                default, 
                transferObject, 
                serializedObject: null
            );
            return Task.FromResult(result);
        }
    }
}