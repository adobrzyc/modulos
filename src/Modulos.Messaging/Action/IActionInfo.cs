using System;
using System.Diagnostics;
using Modulos.Messaging.Operation;

// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

namespace Modulos.Messaging
{
    /// <summary>
    /// Provides information of an action.
    /// </summary>
    public interface IActionInfo
    {
        /// <summary>
        /// Operation associated with action.
        /// </summary>
        OperationId OperationId { get; }

        ActionId Id { get; }
        string Name { get; }

        /// <summary>
        /// Name or description of action host.
        /// </summary>
        string HostName { get; }
        string Mark { get; }
        string AssemblyName { get; }

        DateTime StartTimeUtc { get; }
        long ElapsedMilliseconds { get; }
        void Start();
        void Restart();
        void Start(DateTime startTimeUtc, Stopwatch estimator);
    }
}