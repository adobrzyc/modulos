using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    [Obsolete]
    //todo: [optimization] "Check if it's a good place to use object pool to avoid allocation."
    [OptimizationRequired("object pooling?.")]
    public interface IPerformanceMonitor : IDisposable
    {
        void Add(IMetric metric);
        void AddRange(IEnumerable<IMetric> metrics);
        void Add(IMetric metric, decimal newValue);
       
        void Measure(Action operation, IMetric metric);
        T Measure<T>(Func<T> operation, IMetric metric);

        Task MeasureAsync(Func<Task> operation, IMetric metric);
        Task<T> MeasureAsync<T>(Func<Task<T>> operation, IMetric metric);
    }
}