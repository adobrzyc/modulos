using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Modulos.Messaging.Protocol;

namespace Modulos.Messaging.Transport.Http
{
    public class HttpContentReader : IHttpContentReader
    {
        public async Task<ITransferObject> ReadContent(IDictionary<string,string> headers, Stream body, CancellationToken token)
        {
            var transferObject = new HttpTransferObject();

            var header = headers[Headers.ModulosHeader];
            transferObject.Header = header;

            var contentKind =  headers[Headers.ContentKind];

            switch (contentKind)
            {
                case ContentKinds.Text:
                    using (var reader = new StreamReader(body, Encoding.UTF8))
                    {
                        transferObject.StringContent = await reader.ReadToEndAsync();
                    }
                    break;
                case ContentKinds.Binary:
                    transferObject.ByteContent = await body.ToByteArrayAsync();
                    break;
                case ContentKinds.Stream:
                    var obj = headers[Headers.ObjectWhenStreaming];
                    var streamingContentKind = headers[Headers.ObjectWhenStreamingKind];
                    switch (streamingContentKind)
                    {
                        case ContentKinds.Text:
                            transferObject.StringContent = obj;
                            break;
                        case ContentKinds.Binary:
                            transferObject.ByteContent = Convert.FromBase64String(obj);
                            break;
                        default :
                            throw new TodoException("Invalid content transportId.");
                    }

                    transferObject.Stream = new StreamWithAdditionalReleaseOfResource(body);
                    break;

                default:
                    throw new TodoException("Invalid content transportId.");
            }

            return transferObject;
        }
    }
}