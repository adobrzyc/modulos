using System;
using Microsoft.Extensions.DependencyInjection;

namespace Modulos
{
    /// <summary>
    /// Allows overwrite default service lifetime for auto registration mechanisms.
    /// Use this carefully and with understanding of captive dependency.
    /// </summary>
    /// <remarks>
    /// It's not supported by all of the auto registration mechanisms. 
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class AutoRegistrationOptionsAttribute : Attribute
    {
        public AutoRegistrationOptionsAttribute(ServiceLifetime serviceLifetime)
        {
            ServiceLifetime = serviceLifetime;
        }

        public ServiceLifetime ServiceLifetime { get; }
    }
}