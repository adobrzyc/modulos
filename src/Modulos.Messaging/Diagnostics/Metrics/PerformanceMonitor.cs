using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Modulos.Messaging.Operation;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    [DebuggerStepThrough]
    public class PerformanceMonitor : IPerformanceMonitor
    {
        private readonly IEnumerable<IPerformanceMonitorTarget> targets;

        public PerformanceMonitor(IEnumerable<IPerformanceMonitorTarget> targets)
        {
            this.targets = targets;
        }

        public void Add(IMetric metric)
        {
            if(!IsValid(metric)) 
                throw new TodoException("Invalid metric.");

            foreach (var target in targets.Where(e=>e.IsEnabled))
            {
                target.Write(metric);
            }
        }

        [OptimizationRequired]
        //todo: optimization required
        public void AddRange(IEnumerable<IMetric> metrics)
        {
            foreach (var c in metrics)
            {
                Add(c);
            }
        }

        public void Add(IMetric metric, decimal newValue)
        {
            metric.Value = newValue;
            Add(metric);
        }

        public void Measure(Action operation, IMetric metric)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            if (metric == null) throw new ArgumentNullException(nameof(metric));

            var sw = new Stopwatch();
            sw.Start();
            try
            {
                operation();
            }
            finally
            {
                sw.Stop();
                metric.Value = sw.ElapsedMilliseconds;
                Add(metric);
            }
        }
         
        public T Measure<T>(Func<T> operation, IMetric metric)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            if (metric == null) throw new ArgumentNullException(nameof(metric));

            var sw = new Stopwatch();
            sw.Start();
            try
            {
                var result = operation();
                return result;
            }
            finally
            {
                sw.Stop();
                metric.Value = sw.ElapsedMilliseconds;
                Add(metric);
            }
        }
         
        public async Task MeasureAsync(Func<Task> operation, IMetric metric)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            if (metric == null) throw new ArgumentNullException(nameof(metric));

            var sw = new Stopwatch();
            sw.Start();
            try
            {
                await operation();
            }
            finally
            {
                sw.Stop();
                metric.Value = sw.ElapsedMilliseconds;
                Add(metric);
            }
        }
       
        public async Task<T> MeasureAsync<T>(Func<Task<T>> operation, IMetric metric)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            if (metric == null) throw new ArgumentNullException(nameof(metric));

            var sw = new Stopwatch();
            sw.Start();
            try
            {
                var result = await operation();
                return result;
            }
            finally
            {
                sw.Stop();
                metric.Value = sw.ElapsedMilliseconds;
                Add(metric);
            }
        }

        public void Dispose()
        {
            foreach (var target in targets.OfType<IDisposable>())
            {
                target.Dispose();
            }
        }

        private static bool IsValid(IMetric metric)
        {
            return metric.ActionId != ActionId.Empty || metric.OperationId != OperationId.Empty;
        }
    }
}