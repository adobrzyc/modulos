using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Modulos.Messaging.Protocol;

namespace Modulos.Messaging.Transport.Http
{
    public class HttpContentCreator : IHttpContentCreator
    {
        public HttpContent CreateContent(IMessageHeader header, ITransferObject message, out IDictionary<string, string> headers)
        {
            var transferObject = message;
            
            headers = new Dictionary<string, string>
            {
                {Headers.ModulosHeader, transferObject.Header}
            };

            // stream must be first check
            if (transferObject.Stream != null)
            {
                headers.Add(Headers.ContentKind, ContentKinds.Stream);

                if (transferObject.StringContent != null)
                {
                    headers.Add(Headers.ObjectWhenStreaming, transferObject.StringContent);
                    headers.Add(Headers.ObjectWhenStreamingKind, ContentKinds.Text);
                }
                else if (transferObject.ByteContent != null)
                {
                    headers.Add(Headers.ObjectWhenStreaming, Convert.ToBase64String(transferObject.ByteContent));
                    headers.Add(Headers.ObjectWhenStreamingKind, ContentKinds.Binary);
                }

                var streamContent = new StreamContent(transferObject.Stream);
                streamContent.Headers.ContentType = MediaTypeHeaderValue.Parse(transferObject.MediaTypeOfStream ?? "application/octet-stream");
                
                PutProtocolHeader(header, headers);
                return streamContent;
            }

            if (!string.IsNullOrEmpty(transferObject.StringContent))
            {
                headers.Add(Headers.ContentKind, ContentKinds.Text);
               
                PutProtocolHeader(header, headers);
                return new StringContent(transferObject.StringContent, Encoding.UTF8, transferObject.MediaType ?? "text/plain");
            }

            if (transferObject.ByteContent != null)
            {
                headers.Add(Headers.ContentKind, ContentKinds.Binary);

                var byteArrayContent = new ByteArrayContent(transferObject.ByteContent);
                byteArrayContent.Headers.ContentType = MediaTypeHeaderValue.Parse(transferObject.MediaType ?? "application/octet-stream");
                
                PutProtocolHeader(header, headers);
                return byteArrayContent;
            }

            headers.Add(Headers.ContentKind, ContentKinds.Text);

            PutProtocolHeader(header, headers);
            return new StringContent(string.Empty, Encoding.UTF8);
        }

        private static void PutProtocolHeader(IMessageHeader header, IDictionary<string,string> headers)
        {
            foreach (var pair in header.ProtocolData)
            {
                if (headers.ContainsKey(pair.Key))
                    headers[pair.Key] = pair.Value;
                else headers.Add(pair.Key, pair.Value);
            }
        }
    }
}