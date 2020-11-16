using System;
using System.Threading.Tasks;
//using Hydra.Framework;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Protocol.Request.Definitions;
using Modulos.Messaging.Protocol.Serializers.Definitions;
using Modulos.Messaging.Security;

namespace Modulos.Messaging.Protocol.Request
{
    internal class CreateRequest : ICreateRequest
    {
        private readonly IObjectSerializer objectSerializer;
        private readonly IHeaderSerializer headerSerializer;
        private readonly IConnectionMarkProvider connectionMarkProvider;
        private readonly ITransferObjectFactory transferObjectFactory;
        private readonly IAppInfo appInfo;

        public CreateRequest(IObjectSerializer objectSerializer, IHeaderSerializer headerSerializer,
            IConnectionMarkProvider connectionMarkProvider, ITransferObjectFactory transferObjectFactory, 
            IAppInfo appInfo)
        {
            this.objectSerializer = objectSerializer;
            this.headerSerializer = headerSerializer;
            this.connectionMarkProvider = connectionMarkProvider;
            this.transferObjectFactory = transferObjectFactory;
            this.appInfo = appInfo;
        }
       
        public ValueTask<ICreatedRequestData> Create(IMessage message,
            IMessageConfig messageConfig,
            IEndpointConfig endpointConfig,
            IMetricBag metricBag,
            IInvocationContext invocationContext,
            IAuthenticationData authenticationData)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (messageConfig == null) throw new ArgumentNullException(nameof(messageConfig));
            if (endpointConfig == null) throw new ArgumentNullException(nameof(endpointConfig));
            if (metricBag == null) throw new ArgumentNullException(nameof(metricBag));
            if (invocationContext == null) throw new ArgumentNullException(nameof(invocationContext));

            var transferObject = transferObjectFactory.CreateTransferObject();

            var serializedRequestMessage = objectSerializer.Serialize
            (
                data: message,
                serializerId: messageConfig.RequestSerializer,
                compressionEngineId: messageConfig.RequestCompressor, 
                transferObject: transferObject,
                what: "serialize request", @where: InvocationPlace.Caller, metricBag: metricBag
            );

            var header = new MessageHeader
            {
                Context = (InvocationContext) invocationContext,
                SecurityContext = new SecurityContext(authenticationData),

                TypeInfo = serializedRequestMessage.TypeInfo,

                EndpointConfig = GetEndpointConfigInstanceFromIEndpointConfig(endpointConfig),

                ConnectionMark = messageConfig.SupportConnectionMark ? connectionMarkProvider.Get(endpointConfig.Info.EndpointConfigMark) : (ConnectionMark?) null,

                AppInfo = new AppInfo(appInfo),

                MessageKind = MessageKind.Message,

                RequestSerializer = messageConfig.RequestSerializer,
                ResponseSerializer = messageConfig.ResponseSerializer,

                RequestCompressor = messageConfig.RequestCompressor,
                ResponseCompressor = messageConfig.ResponseCompressor,

                TransportEngine = messageConfig.TransportEngine
            };

            // ReSharper disable once SuspiciousTypeConversion.Global
            if (message is IContainDirectStream directStream)
            {
                transferObject.Stream = directStream.Stream;
                transferObject.MediaTypeOfStream = directStream.MediaType;
            }

            transferObject.Header = headerSerializer.Serialize(header, InvocationPlace.Caller, metricBag);

            var requestData = new CreatedRequestData
            (
                message, header, serializedRequestMessage,
                transferObject, messageConfig, endpointConfig
            );

            return new ValueTask<ICreatedRequestData>(requestData);
        }

        private static EndpointConfig GetEndpointConfigInstanceFromIEndpointConfig(IEndpointConfig endpointConfig)
        {
            if (endpointConfig is EndpointConfig cast)
                return cast;

            var created = new EndpointConfig(endpointConfig.Properties)
            {
                Address = endpointConfig.Address,
                Info = endpointConfig.Info,
                ExpirationTimeUtc = endpointConfig.ExpirationTimeUtc,
                IsAvailable = endpointConfig.IsAvailable,
                IsDefault = endpointConfig.IsDefault,
                IsExpired = endpointConfig.IsExpired,
                Order = endpointConfig.Order
            };

            return created;
        }
    }
}