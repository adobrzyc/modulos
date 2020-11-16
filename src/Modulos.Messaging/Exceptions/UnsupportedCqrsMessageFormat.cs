using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace Modulos.Messaging
{
    
    [TypeMark("18f266a2-fc8f-4856-b689-83db5906cdfc")]
    public sealed class UnsupportedCqrsMessageFormat : ModulosException
    {
        public override string Code => "18f266a2-fc8f-4856-b689-83db5906cdfc";

        public UnsupportedCqrsMessageFormat(string message) : base(message)
        {
        }

        public UnsupportedCqrsMessageFormat(string message, Exception innerException) : base(message, innerException)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private UnsupportedCqrsMessageFormat(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}