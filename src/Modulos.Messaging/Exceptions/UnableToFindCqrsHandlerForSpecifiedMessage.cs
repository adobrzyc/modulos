using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace Modulos.Messaging
{
    [TypeMark("82409a01-184c-4814-892b-6d8d9a5a3e4e")]
    public sealed class UnableToFindCqrsHandlerForSpecifiedMessage : ModulosException
    {
        public override string Code => "82409a01-184c-4814-892b-6d8d9a5a3e4e";

        public UnableToFindCqrsHandlerForSpecifiedMessage(IMessage message)
            : base($"Unable to determine handler for message: {message.GetType().FullName}")
        {
        }

        public UnableToFindCqrsHandlerForSpecifiedMessage(string message) : base(message)
        {
        }

        public UnableToFindCqrsHandlerForSpecifiedMessage(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private UnableToFindCqrsHandlerForSpecifiedMessage(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}	