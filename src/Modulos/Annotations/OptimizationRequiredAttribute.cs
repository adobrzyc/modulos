using System;
using System.Diagnostics.CodeAnalysis;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Modulos
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    internal sealed class OptimizationRequiredAttribute : Attribute
    {
        public string Message { get; }

        public OptimizationRequiredAttribute()
        {
            
        }
        // ReSharper disable once UnusedParameter.Local
        public OptimizationRequiredAttribute(string message)
        {
            Message = message;
        }
    }
}