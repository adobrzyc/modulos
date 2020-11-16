using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace Modulos
{
    public abstract class Freezable : IFreezable
    {
        protected void ThrowIfFrozen()
        {
            if(IsFrozen)
                throw new ObjectIsFrozenException("Object is frozen. Changes are not allowed.");
        }

        public virtual void Freeze()
        {
            IsFrozen = true;
        }
        
        public bool IsFrozen { get; private set; }

        
        public sealed class ObjectIsFrozenException : ModulosException
        {
            public override string Code => "54d5faff-4f88-410a-8ac3-509e2269b5de";

            public ObjectIsFrozenException(string message) : base(message)
            {
            }

            public ObjectIsFrozenException(string message, Exception innerException) : base(message, innerException)
            {
            }

        
            [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
            private ObjectIsFrozenException(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
            }
        }
    }
}