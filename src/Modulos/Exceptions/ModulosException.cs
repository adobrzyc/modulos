using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Modulos
{
    /// <summary>
    /// Base exception for all framework exceptions.
    /// </summary>
    /// <remarks>
    /// Important: <see cref="SerializableAttribute"/> attribute is NOT inherited from Exception, and MUST be specified 
    /// otherwise serialization will fail with a SerializationException stating that
    /// "Type X in Assembly Y is not marked as serializable."
    /// </remarks>
    public abstract class ModulosException : Exception
    {
        /// <summary>
        /// Unique exception code.
        /// </summary>
        public abstract string Code { get; }

        protected ModulosException() { }

        protected ModulosException(string message)
            : base(message)
        {

        }

        protected ModulosException(string message, Exception innerException)
            : base(message, innerException)
        {
     
        }

        /// <remarks>
        /// Constructor should be protected for unsealed classes, private for sealed classes.
        /// (The Serializer invokes this constructor through reflection, so it can be private)
        /// </remarks>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected ModulosException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            //Code = info.GetString("Code");
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue("Code", Code);

            // MUST call through to the base class to let it save its own state
            base.GetObjectData(info, context);
        }
    }
}





