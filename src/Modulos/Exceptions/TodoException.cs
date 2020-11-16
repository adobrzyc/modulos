using System;
using System.Runtime.Serialization;

// ReSharper disable UnusedMember.Global

namespace Modulos
{
    public class TodoException : Exception
    {
        public TodoException()
        {
        }

        public TodoException(string message) : base(message)
        {
        }

        public TodoException(string message, Exception inner) : base(message, inner)
        {
        }

        protected TodoException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}