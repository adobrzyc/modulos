using System;
using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Hosting.Options;
using Modulos.Messaging.Protocol.Response.Definitions;
using Modulos.Messaging.Protocol.Serializers.Definitions;

namespace Modulos.Messaging.Protocol.Response
{
    internal class CreateEmptyResponse : ICreateEmptyResponse
    {
        private readonly IHeaderSerializer headerSerializer;
        private readonly ITransferObjectFactory transferObjectFactory;
        private readonly IOperationModeOption operationMode;

        public CreateEmptyResponse(IHeaderSerializer headerSerializer, 
            ITransferObjectFactory transferObjectFactory, IOperationModeOption operationMode)
        {
            this.headerSerializer = headerSerializer;
            this.transferObjectFactory = transferObjectFactory;
            this.operationMode = operationMode;
        }

        public Task<IResponseData> Create(IMessageHeader requestHeader,
            IMetricBag metricBag,
            IInvocationContext invocationContext)
        {
            if (requestHeader == null) throw new ArgumentNullException(nameof(requestHeader));
            if (metricBag == null) throw new ArgumentNullException(nameof(metricBag));
            if (invocationContext == null) throw new ArgumentNullException(nameof(invocationContext));


            var transferObject = transferObjectFactory.CreateTransferObject();

            var header = new MessageHeader
            {
                Context = (InvocationContext) invocationContext,
                TypeInfo = TypeInfo.Empty,
                MessageKind = MessageKind.Empty,
                RefreshConnection = requestHeader.ConnectionMark != null && operationMode.Value != OperatingMode.Normal
            };

            transferObject.Header = headerSerializer.Serialize(header, InvocationPlace.Target, metricBag);
            transferObject.ByteContent = null;

            IResponseData result = new ResponseData(transferObject, header, error: null, invocationContext, null);

            return Task.FromResult(result);
        }
    }
}