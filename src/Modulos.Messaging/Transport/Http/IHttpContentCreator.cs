using System.Collections.Generic;
using System.Net.Http;
using Modulos.Messaging.Protocol;

namespace Modulos.Messaging.Transport.Http
{
    public interface IHttpContentCreator
    {
        HttpContent CreateContent(IMessageHeader header, ITransferObject message, out IDictionary<string,string> headers);
    }
}