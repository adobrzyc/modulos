using System;
using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Protocol.Request.Definitions;
using Modulos.Messaging.Protocol.Serializers.Definitions;

namespace Modulos.Messaging.Protocol.Request
{
    internal class ParseRequest : IParseRequest
    {
        private readonly IHeaderSerializer headerSerializer;
        private readonly IObjectSerializer objectSerializer;

        public ParseRequest(IHeaderSerializer headerSerializer, IObjectSerializer objectSerializer)
        {
            this.headerSerializer = headerSerializer;
            this.objectSerializer = objectSerializer;
        }

        public ValueTask<IRequestData> Parse(ITransferObject transferObject, IMetricBag metricBag)
        {
            if (transferObject == null) throw new ArgumentNullException(nameof(transferObject));
            if (metricBag == null) throw new ArgumentNullException(nameof(metricBag));

            var header = headerSerializer.Deserialize(transferObject, metricBag, InvocationPlace.Target);

            if (header.MessageKind != MessageKind.Message)
                throw new TodoException("Invalid data. Transfer object does not represents message.");

            var deserializedRequest = objectSerializer.Deserialize<object>
            (
                transferObject,
                header.TypeInfo,
                header.RequestSerializer,
                header.RequestCompressor,
                //deserialized: out object deserializedRequest,
                out var serializedRequest,
                "deserialize request", InvocationPlace.Target, metricBag
            );

            IRequestData result = new RequestData((IMessage) deserializedRequest, header, serializedRequest, transferObject);

            return new ValueTask<IRequestData>(result);
        }

    }
}