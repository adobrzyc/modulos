using System;
using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Hosting.Options;
using Modulos.Messaging.Protocol.Response.Definitions;
using Modulos.Messaging.Protocol.Serializers.Definitions;

namespace Modulos.Messaging.Protocol.Response
{
    internal class CreateObjectResponse : ICreateObjectResponse
    {
        private readonly IHeaderSerializer headerSerializer;
        private readonly IObjectSerializer objectSerializer;
        private readonly IOperationModeOption operationMode;
        private readonly ITransferObjectFactory transferObjectFactory;

        public CreateObjectResponse(IHeaderSerializer headerSerializer, 
            IObjectSerializer objectSerializer,
            IOperationModeOption operationMode, ITransferObjectFactory transferObjectFactory)
        {
            this.headerSerializer = headerSerializer;
            this.objectSerializer = objectSerializer;
            this.operationMode = operationMode;
            this.transferObjectFactory = transferObjectFactory;
        }

        public Task<IResponseData> Create(object response,
            IMessageHeader requestHeader,
            IMetricBag metricBag,
            IInvocationContext invocationContext)
        {
            if (response == null) throw new ArgumentNullException(nameof(response));
            if (requestHeader == null) throw new ArgumentNullException(nameof(requestHeader));
            if (metricBag == null) throw new ArgumentNullException(nameof(metricBag));
            if (invocationContext == null) throw new ArgumentNullException(nameof(invocationContext));


            var transferObject = transferObjectFactory.CreateTransferObject();

            var serializedResponse = objectSerializer.Serialize
            (
                response,
                requestHeader.ResponseSerializer,
                requestHeader.ResponseCompressor, transferObject, "serialize response", InvocationPlace.Target, metricBag
            );

            var header = new MessageHeader
            {
                Context = (InvocationContext) invocationContext,

                TypeInfo = serializedResponse.TypeInfo,

                MessageKind = MessageKind.Object,

                ResponseSerializer = requestHeader.ResponseSerializer,
                ResponseCompressor = requestHeader.ResponseCompressor,

                RefreshConnection = requestHeader.ConnectionMark != null && operationMode.Value != OperatingMode.Normal
            };

            if (response is IContainDirectStream directStream)
            {
                transferObject.Stream = directStream.Stream;
                transferObject.MediaTypeOfStream = directStream.MediaType;
            }
            
            transferObject.Header = headerSerializer.Serialize(header, InvocationPlace.Target, metricBag);

            IResponseData result = new ResponseData(transferObject, header, error: null, invocationContext, serializedResponse);

            return Task.FromResult(result);
        }

    }
}