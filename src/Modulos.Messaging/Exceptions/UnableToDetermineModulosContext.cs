using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace Modulos.Messaging
{
    [TypeMark("6396102a-358b-4749-aec7-1d96d62dbd4f")]
    public sealed class UnableToDetermineModulosContext : ModulosException
    {
        public override string Code => "6396102a-358b-4749-aec7-1d96d62dbd4f";


        public UnableToDetermineModulosContext(IMessage message)
            : base($"Unable to determine modulos context for message: {message.GetType().FullName}")
        {
        }

        public UnableToDetermineModulosContext(IMessage message, Exception innerException)
            : base($"Unable to determine modulos context for message: {message.GetType().FullName}", innerException)
        {
        }

        public UnableToDetermineModulosContext(string message) : base(message)
        {
        }

        public UnableToDetermineModulosContext(string message, Exception innerException) : base(message, innerException)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private UnableToDetermineModulosContext(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}