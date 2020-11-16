using System;

namespace Modulos.Messaging.Diagnostics.Metrics.Targets
{
    public interface IReadAndDeletePerformanceMonitorTarget : IPerformanceMonitorTarget,
        IPerformanceMonitorReader,
        IDisposable
    {
    }
}