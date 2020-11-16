using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

// ReSharper disable MemberCanBeProtected.Global

namespace Modulos.Messaging.Security.Exceptions
{
    public class SecurityException : ModulosException
    {
        public override string Code => "255ece09-738a-4c66-811e-571d1584d5ba";

        public SecurityException(string message)
            : base(message)
        {
        }

        public SecurityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected SecurityException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}