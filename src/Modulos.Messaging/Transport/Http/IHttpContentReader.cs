using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Modulos.Messaging.Protocol;

namespace Modulos.Messaging.Transport.Http
{
    public interface IHttpContentReader
    {
        Task<ITransferObject> ReadContent(IDictionary<string, string> headers, Stream content, CancellationToken token);
        //Task<ITransferObject> ReadContent(HttpRequest request);
        //Task<ITransferObject> ReadContent(HttpResponseMessage response);
    }
}