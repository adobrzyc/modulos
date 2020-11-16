using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

// ReSharper disable UnusedMember.Global

namespace Modulos.Messaging.Protocol.Exceptions
{
    [TypeMark("3617d63c-f5fe-4a66-b434-d65b7c1143e4")]
    public class ProtocolException : ModulosException
    {
        public override string Code => "3617d63c-f5fe-4a66-b434-d65b7c1143e4";

        public ProtocolException()
        {
            
        }

        public ProtocolException(string message) : base(message)
        {
        }

        public ProtocolException(string message, Exception innerException) : base(message, innerException)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected ProtocolException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}