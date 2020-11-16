using System;
using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Protocol.Response.Definitions;
using Modulos.Messaging.Protocol.Serializers.Definitions;

namespace Modulos.Messaging.Protocol.Response
{
    internal class ParseObjectResponse : IParseObjectResponse
    {
        private readonly IObjectSerializer objectSerializer;

        public ParseObjectResponse(IObjectSerializer objectSerializer)
        {
            this.objectSerializer = objectSerializer;
        }

        public Task<IParsedResponseData<TResult>> Parse<TResult>(IMessageHeader responseHeader, ITransferObject transferObject, IMetricBag metricBag)
        {
            if (responseHeader == null) throw new ArgumentNullException(nameof(responseHeader));
            if (transferObject == null) throw new ArgumentNullException(nameof(transferObject));

            var response = objectSerializer.Deserialize<TResult>
            (
                transferObject: transferObject,
                typeInfo: responseHeader.TypeInfo,
                serializerId: responseHeader.ResponseSerializer,
                compressionEngine: responseHeader.ResponseCompressor,
                //deserialized: out response,
                serializedObject: out var serializedObject,
                what: "deserialize response", 
                @where: InvocationPlace.Caller, metricBag
            );

            var result = (IParsedResponseData<TResult>)new ParsedResponseData<TResult>
            (
                responseHeader, response, 
                transferObject, serializedObject: serializedObject
            );

            return Task.FromResult(result);
        }
    }
}