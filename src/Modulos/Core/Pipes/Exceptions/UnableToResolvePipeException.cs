// ReSharper disable UnusedMember.Global

namespace Modulos.Pipes
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public class UnableToResolvePipeException : Exception
    {
        public UnableToResolvePipeException()
        {
        }

        public UnableToResolvePipeException(string message) : base(message)
        {
        }

        public UnableToResolvePipeException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}