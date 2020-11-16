using System;
using System.Threading.Tasks;
using Modulos.Errors;
using Modulos.Messaging.Protocol.Exceptions;
using Modulos.Messaging.Protocol.Response.Definitions;
using Modulos.Messaging.Serialization;
using Modulos.Messaging.Serialization.Engines;

namespace Modulos.Messaging.Protocol.Response
{
    internal class ParseFaultResponse : IParseFaultResponse
    {
        private static readonly ISupportStringSerialization ErrorSerializer = new JsonNetSerializer();

        private readonly IExceptionTransformer exceptionTransformer;

        public ParseFaultResponse(IExceptionTransformer exceptionTransformer)
        {
            this.exceptionTransformer = exceptionTransformer;
        }

        public Task<Exception> Parse(IMessageHeader responseHeader, ITransferObject transferObject)
        {
            if (responseHeader == null) throw new ArgumentNullException(nameof(responseHeader));
            if (transferObject == null) throw new ArgumentNullException(nameof(transferObject));

            if(responseHeader.MessageKind != MessageKind.Error)
                throw new ProtocolException("Invalid message transportId.");

            if (transferObject.StringContent == null)
                throw new ProtocolException("Missing error data in response object.");

            var errorData = (ErrorData) ErrorSerializer.DeserializeFromString(typeof(ErrorData), transferObject.StringContent);
            exceptionTransformer.ToModulosException
            (
                code: errorData.ErrorCode,
                message: errorData.ErrorMessage,
                modulosException: out var modulosException
            );

            return Task.FromResult((Exception)modulosException);
        }
    }
}