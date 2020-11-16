using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modulos.Messaging.Diagnostics.Metrics.Targets
{
    public class KeepAllMetricsForOperationsPerformanceMonitorTarget : IKeepAllMetricsForOperationsPerformanceMonitorTarget
    {
        private ConcurrentBag<IMetric> data = new ConcurrentBag<IMetric>();

        public int Count => data.Count;

        /// <summary>
        /// Not thread safe, so it may propagate with delay. Default false.
        /// </summary>
        public bool IsEnabled { get; set; }

        public void Write(IMetric metric)
        {
            data.Add(metric);
        }


        public Task WriteAsync(IMetric metric)
        {
            Write(metric);
            return Task.CompletedTask; 
        }

        public void Clear()
        {
            data = new ConcurrentBag<IMetric>();
        }

        public Task ClearAsync()
        {
            Clear();
            return Task.CompletedTask;
        }

        public IEnumerable<IMetric> Read(Func<IMetric, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return data.Where(predicate);
        }

        //public IEnumerable<ICounter> Read(OperationInfo operationInfo)
        //{
        //    if (operationInfo == null) throw new ArgumentNullException(nameof(operationInfo));
        //    return data.Where(e => Equals(e.OperationInfo, operationInfo));
        //}

        public IEnumerable<IMetric> Read(ActionId actionId)
        {
            return Read(counter => counter.ActionId == actionId);
        }

        public IEnumerable<IMetric> ReadAll()
        {
            return data;
        }

        public Task<IEnumerable<IMetric>> ReadAsync(Func<IMetric, bool> predicate)
        {
            var output = Read(predicate);
            return Task.FromResult(output);
        }

        //public Task<IEnumerable<ICounter>> ReadAsync(OperationInfo operationInfo)
        //{
        //    var output = Read(operationInfo);
        //    return Task.FromResult(output);
        //}

        public Task<IEnumerable<IMetric>> ReadAsync(ActionId actionId)
        {
            var output = Read(counter => counter.ActionId == actionId);
            return Task.FromResult(output);
        }

        public Task<IEnumerable<IMetric>> ReadAllAsync()
        {
            var output = ReadAll();
            return Task.FromResult(output);
        }

        public void Dispose()
        {
            Clear();
        }
    }
}