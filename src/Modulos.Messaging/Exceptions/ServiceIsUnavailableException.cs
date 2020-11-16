using System;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Modulos.Messaging.Configuration;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace Modulos.Messaging
{
    [TypeMark("bd4b47b5-212f-4441-84bc-4f1f19521514")]
    public sealed class ServiceIsUnavailableException : ModulosException
    {
        public override string Code => "bd4b47b5-212f-4441-84bc-4f1f19521514";

        public ServiceIsUnavailableException(EndpointConfigMark endpointConfigMark)
            : base($"Service is unavailable: {endpointConfigMark}")
        {
        }

        public ServiceIsUnavailableException(EndpointConfigMark endpointConfigMark, string message)
            : base($"Service is unavailable: {endpointConfigMark}, for particular reason: {message}.")
        {
        }


       
        public ServiceIsUnavailableException(string message) : base(message)
        {
        }

        public ServiceIsUnavailableException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        /// <remarks>
        /// Constructor should be protected for unsealed classes, private for sealed classes.
        /// (The Serializer invokes this constructor through reflection, so it can be private)
        /// </remarks>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private ServiceIsUnavailableException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}	