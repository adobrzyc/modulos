using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace Modulos.Messaging
{
    public sealed class ReCallableExecutionException : BusinessLogicException
    {
        public override string Code => "3C22CDD9-6FFA-4539-BCCE-B054F1E4FA79";

        public ReCallableExecutionException(Exception innerException)
            : base(innerException.Message, innerException)
        {
        }

        public ReCallableExecutionException(string message) : base(message)
        {
        }

        public ReCallableExecutionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        private ReCallableExecutionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}