using System;
using System.Collections.Generic;

namespace Modulos.Messaging.Configuration
{
    /// <summary>
    /// Defines config used to establish remote call. 
    /// </summary>
    public interface IEndpointConfig 
    {
        /// <summary>
        /// Configuration info. It's used to verify currency and identity config.
        /// </summary>
        EndpointConfigInfo Info { get; }

        /// <summary>
        /// It's usually endpoint address of the service (depends on implementation). 
        /// </summary>
        string Address { get; }

        /// <summary>
        /// Config order, used during set or switch configuration.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Defines if config is expired.
        /// </summary>
        bool IsExpired { get; }

        /// <summary>
        /// True if this config is default service configuration, otherwise false.
        /// </summary>
        bool IsDefault { get; }

        /// <summary>
        /// True if config is available. Should aggregate checks from all limiters like <see cref="ExpirationTimeUtc"/>.
        /// </summary>
        bool IsAvailable { get; }

        /// <summary>
        /// Determines expiration date of the configuration.
        /// </summary>
        DateTime? ExpirationTimeUtc { get; }

        /// <summary>
        /// Defines additional properties associated with configuration.
        /// </summary>
        /// <remarks>
        /// It's good place to put transport layer dependent values.
        /// </remarks>
        IReadOnlyDictionary<string, object> Properties { get; }
    }
}