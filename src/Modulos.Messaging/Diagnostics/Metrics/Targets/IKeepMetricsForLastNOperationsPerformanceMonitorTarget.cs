using System;

namespace Modulos.Messaging.Diagnostics.Metrics.Targets
{
    public interface IKeepMetricsForLastNOperationsPerformanceMonitorTarget : IPerformanceMonitorTarget, 
        IPerformanceMonitorReader, 
        IDisposable
    {
        int Capacity { get; }
    }
}