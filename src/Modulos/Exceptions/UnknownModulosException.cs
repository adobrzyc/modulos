using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

// ReSharper disable UnusedMember.Local

namespace Modulos
{
    public sealed class UnknownModulosException : ModulosException
    {
        public override string Code => "176B8149-3E88-44C7-B5A1-036FBE0016CE";

        public UnknownModulosException(string message) : base(message)
        {
        }

        public UnknownModulosException(string message, Exception innerException) : base(message, innerException)
        {
        }

        
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private UnknownModulosException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}	