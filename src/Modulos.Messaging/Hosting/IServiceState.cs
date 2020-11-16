using System;
using Modulos.Messaging.Diagnostics.Options;
using Modulos.Messaging.Hosting.Options;

namespace Modulos.Messaging.Hosting
{
    public interface IServiceState
    {
        int ActiveOperationCount { get; }
        int TotalOperationCount { get; }
        DateTime LastOperationTimeUtc { get; }
        OperatingMode OperatingMode { get; }
        LogMark LogMark { get; }
    }
}