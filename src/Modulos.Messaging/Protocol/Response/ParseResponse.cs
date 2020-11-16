using System;
using System.Threading.Tasks;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Protocol.Exceptions;
using Modulos.Messaging.Protocol.Response.Definitions;
using Modulos.Messaging.Protocol.Serializers.Definitions;

namespace Modulos.Messaging.Protocol.Response
{
    internal class ParseResponse : IParseResponse
    {
        private readonly IHeaderSerializer headerSerializer;
        private readonly IConnectionMarkProvider connectionMarkProvider;
        private readonly IParseFaultResponse parseFaultResponse;
        private readonly IParseEmptyResponse parseEmptyResponse;
        private readonly IParseObjectResponse parseObjectResponse;

        public ParseResponse(IHeaderSerializer headerSerializer,
            IConnectionMarkProvider connectionMarkProvider,
            IParseFaultResponse parseFaultResponse, 
            IParseEmptyResponse parseEmptyResponse, 
            IParseObjectResponse parseObjectResponse)
        {
            this.headerSerializer = headerSerializer;
            this.connectionMarkProvider = connectionMarkProvider;
            this.parseFaultResponse = parseFaultResponse;
            this.parseEmptyResponse = parseEmptyResponse;
            this.parseObjectResponse = parseObjectResponse;
        }

        public async Task<IParsedResponseData<TResult>> Parse<TResult>(ITransferObject transferObject,
            IMessageHeader requestHeader,
            IMetricBag metricBag)
        {
            if (transferObject == null) throw new ArgumentNullException(nameof(transferObject));
            if (requestHeader == null) throw new ArgumentNullException(nameof(requestHeader));
            if (metricBag == null) throw new ArgumentNullException(nameof(metricBag));

            var responseHeader = headerSerializer.Deserialize(transferObject, metricBag, InvocationPlace.Caller);

            if (requestHeader.ConnectionMark != null)
            {
                if (responseHeader.RefreshConnection && requestHeader.EndpointConfig != null)
                {
                    connectionMarkProvider.Refresh(requestHeader.EndpointConfig.Info.EndpointConfigMark);
                }
            }

            switch (responseHeader.MessageKind)
            {
                case MessageKind.Error:
                    throw await parseFaultResponse.Parse(responseHeader, transferObject);
                case MessageKind.Empty:
                    return await parseEmptyResponse.Parse<TResult>(responseHeader, transferObject);
                case MessageKind.Object:
                    return await parseObjectResponse.Parse<TResult>(responseHeader, transferObject, metricBag);
                default:
                    throw new ProtocolException($"Invalid response data. Not supported response {nameof(MessageKind)}: {responseHeader.MessageKind}.");
            }
        }

    }
}