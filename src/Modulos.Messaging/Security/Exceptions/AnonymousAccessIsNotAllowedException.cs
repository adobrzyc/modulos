using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace Modulos.Messaging.Security.Exceptions
{
    public sealed class AnonymousAccessIsNotAllowedException : SecurityException
    {
        public override string Code => "ca95033d-1877-4b85-b4e2-d6e23267baef";

        public AnonymousAccessIsNotAllowedException() :base("Anonymous access is not allowed.")
        {
            
        }

        public AnonymousAccessIsNotAllowedException(string message) : base(message)
        {
        }

        public AnonymousAccessIsNotAllowedException(string message, Exception innerException) : base(message, innerException)
        {
        }   

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private AnonymousAccessIsNotAllowedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}	