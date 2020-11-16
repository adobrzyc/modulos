using System;
using Modulos.Messaging.Configuration;

// ReSharper disable UnusedMember.Global

namespace Modulos.Messaging.Maintenance
{
    /// <summary>
    /// Defines instance info of the service.
    /// </summary>
    public interface IInstanceInfo
    {
        /// <summary>
        /// Unique instance identifier.
        /// </summary>
        Guid InstanceId { get; }

        /// <summary>
        /// Friendly name of the instance.
        /// </summary>
        string FriendlyName { get; }

        /// <summary>
        /// Indicates the service to which it belongs.
        /// </summary>
        EndpointConfigMark Mark { get; }

        /// <summary>
        /// Determines group of this info e.q.: "price-group"
        /// </summary>
        string Group { get; }

        /// <summary>
        /// Used to execute maintenance operation in specific order. 
        /// </summary>
        int MaintenanceOrder { get; }

        /// <summary>
        /// Direct address of the service.
        /// </summary>
        string Address { get; }

        /// <summary>
        /// Machine name of the service instance.
        /// </summary>
        string MachineName { get; }
        
        /// <summary>
        /// Ip address of the service.
        /// </summary>
        string IpAddress { get; }

        /// <summary>
        /// External info of the instance.
        /// </summary>
        string Info { get; }
        
        /// <summary>
        /// Insert date.
        /// </summary>
        DateTime InsertDate { get; }
    }
}