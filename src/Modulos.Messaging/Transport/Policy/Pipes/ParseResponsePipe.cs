using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Protocol;
using Modulos.Messaging.Protocol.Request.Definitions;
using Modulos.Messaging.Protocol.Response.Definitions;
using Modulos.Pipes;

// ReSharper disable once UnusedType.Global

namespace Modulos.Messaging.Transport.Policy.Pipes
{
    //todo: check if there is a chance to convert IParseResponse.Parse<T> into non-generic method
    internal class ParseResponsePipe : IPipe
    {
        private readonly PipeData data;
        private readonly ICreatedRequestData request;
        private readonly IParseResponse parseResponse;
        private readonly ITransferObject response;
        private readonly IMetricBag metricBag;

        public ParseResponsePipe(PipeData data, ICreatedRequestData request, IParseResponse parseResponse,
            ITransferObject response, IMetricBag metricBag)
        {
            this.data = data;
            this.request = request;
            this.parseResponse = parseResponse;
            this.response = response;
            this.metricBag = metricBag;
        }

        public async Task<PipeResult> Execute( CancellationToken cancellationToken)
        {
            // ReSharper disable once PossibleNullReferenceException
            var method = parseResponse.GetType()
                .GetMethod(nameof(IParseResponse.Parse))
                .MakeGenericMethod(data.ResultType);

            var parsedResponse = await InvokeAsync(method, parseResponse, response, request.MessageHeader, metricBag);

            //var parsedResponse = await parseResponse.Parse<TResult>
            //(
            //    transferObject: response,
            //    requestHeader: request.MessageHeader,
            //    metricBag: metricBag
            //);

            return new PipeResult(PipeActionAfterExecute.Continue, parsedResponse);
        }

        private static async Task<object> InvokeAsync(MethodInfo @this, object obj, params object[] parameters)
        {
            dynamic awaitable = @this.Invoke(obj, parameters);
            await awaitable.ConfigureAwait(false);
            return awaitable.GetAwaiter().GetResult();
        }
    }
}