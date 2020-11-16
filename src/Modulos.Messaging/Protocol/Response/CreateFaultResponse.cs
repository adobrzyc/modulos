using System;
using System.Threading.Tasks;
using Modulos.Errors;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Hosting.Options;
using Modulos.Messaging.Protocol.Response.Definitions;
using Modulos.Messaging.Protocol.Serializers.Definitions;
using Modulos.Messaging.Serialization;
using Modulos.Messaging.Serialization.Engines;

namespace Modulos.Messaging.Protocol.Response
{
    internal class CreateFaultResponse : ICreateFaultResponse
    {
        private static readonly ISupportStringSerialization HeaderSerializer = new JsonNetSerializer();

        private readonly IHeaderSerializer headerSerializer;
        private readonly IExceptionTransformer exceptionTransformer;
        private readonly IOperationModeOption operationMode;
        private readonly ITransferObjectFactory transferObjectFactory;

        public CreateFaultResponse(IHeaderSerializer headerSerializer, 
            IExceptionTransformer exceptionTransformer,
            IOperationModeOption operationMode, 
            ITransferObjectFactory transferObjectFactory)
        {
            this.headerSerializer = headerSerializer;
            this.exceptionTransformer = exceptionTransformer;
            this.operationMode = operationMode;
            this.transferObjectFactory = transferObjectFactory;
        }

        public Task<IResponseData> CreateWithoutContext(Exception error, IMetricBag metricBag)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));
            if (metricBag == null) throw new ArgumentNullException(nameof(metricBag));

            var transferObject = transferObjectFactory.CreateTransferObject();

            exceptionTransformer.ToModulosException(error, out var exception);

            var header = new MessageHeader
            { 
                TypeInfo = new TypeInfo(exception),
                MessageKind = MessageKind.Error,
                RefreshConnection = operationMode.Value != OperatingMode.Normal
            };

            transferObject.Header = headerSerializer.Serialize(header, InvocationPlace.Target, metricBag);
            transferObject.ByteContent = null;
            var errorData = new ErrorData(exception.Code, exception.Message);
            transferObject.StringContent = HeaderSerializer.SerializeToString(errorData); //modulosException.Message; //todo: redundant code 
                
            IResponseData result = new ResponseData(transferObject, header, error, invocationContext: null, serializedResponse: null);

            return Task.FromResult(result);
        }

        public Task<IResponseData> Create(Exception error, IMessageHeader requestHeader,
            IMetricBag metricBag, IInvocationContext invocationContext)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));
            if (requestHeader == null) throw new ArgumentNullException(nameof(requestHeader));
            if (metricBag == null) throw new ArgumentNullException(nameof(metricBag));
            if (invocationContext == null) throw new ArgumentNullException(nameof(invocationContext));


            var transferObject = transferObjectFactory.CreateTransferObject();

            exceptionTransformer.ToModulosException(error, out var exception);

            var header = new MessageHeader
            {
                Context = (InvocationContext) invocationContext,
                TypeInfo = new TypeInfo(exception),
                MessageKind = MessageKind.Error,
                RefreshConnection = requestHeader.ConnectionMark != null && operationMode.Value != OperatingMode.Normal
            };

            transferObject.Header = headerSerializer.Serialize(header, InvocationPlace.Target, metricBag);
            transferObject.ByteContent = null;
            var errorData = new ErrorData(exception.Code, exception.Message);
            transferObject.StringContent = HeaderSerializer.SerializeToString(errorData);

            IResponseData result = new ResponseData(transferObject, header, error, invocationContext, null);

            return Task.FromResult(result);
        }

        
    }
}