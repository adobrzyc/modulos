using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    public interface IPerformanceMonitorReader
    {
        IEnumerable<IMetric> Read(Func<IMetric, bool> predicate);
        IEnumerable<IMetric> Read(ActionId actionId);
        IEnumerable<IMetric> ReadAll();
        
        Task<IEnumerable<IMetric>> ReadAsync(Func<IMetric, bool> predicate);
        Task<IEnumerable<IMetric>> ReadAsync(ActionId actionId);
        Task<IEnumerable<IMetric>> ReadAllAsync();
    }
}