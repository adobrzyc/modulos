using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

// ReSharper disable MemberCanBeProtected.Global

namespace Modulos.Messaging
{
    public class BusinessLogicException : ModulosException
    {
        public override string Code => "8867ce90-2b16-4845-a548-c1d22dd55d82";

        public BusinessLogicException(string message)
            : base(message)
        {
        }

        public BusinessLogicException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected BusinessLogicException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}