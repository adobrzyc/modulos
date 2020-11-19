using System;
using System.Diagnostics.CodeAnalysis;

namespace Modulos.Pipes
{
    [ExcludeFromCodeCoverage]
    public sealed class MissingDataFromPipelineResultException : Exception
    {
        public MissingDataFromPipelineResultException(string message) : base(message)
        {
        }

        public MissingDataFromPipelineResultException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}