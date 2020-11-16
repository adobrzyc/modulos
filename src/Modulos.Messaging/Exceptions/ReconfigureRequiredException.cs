using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace Modulos.Messaging
{
    
    [TypeMark("0e489a1b-858e-42c4-913c-c1b58cc92387")]
    public sealed class ReconfigureRequiredException : ModulosException
    {
        public override string Code => "0e489a1b-858e-42c4-913c-c1b58cc92387";

        public ReconfigureRequiredException()
        {

        }

        public ReconfigureRequiredException(string message) : base(message)
        {
        }

        public ReconfigureRequiredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private ReconfigureRequiredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}