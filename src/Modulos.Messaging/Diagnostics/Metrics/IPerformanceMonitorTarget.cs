using System.Threading.Tasks;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    [OptimizationRequired("AddRange may boost performance.")]
    public interface IPerformanceMonitorTarget
    {
        int Count { get; }
        bool IsEnabled { get; set; }

        void Write(IMetric metric);
        Task WriteAsync(IMetric metric);
       
        void Clear();
        Task ClearAsync();
    }
}