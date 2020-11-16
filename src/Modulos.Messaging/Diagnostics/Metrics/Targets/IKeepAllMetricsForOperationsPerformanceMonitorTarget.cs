using System;

namespace Modulos.Messaging.Diagnostics.Metrics.Targets
{
    public interface IKeepAllMetricsForOperationsPerformanceMonitorTarget : IPerformanceMonitorTarget, 
        IPerformanceMonitorReader, 
        IDisposable
    {
    }
}