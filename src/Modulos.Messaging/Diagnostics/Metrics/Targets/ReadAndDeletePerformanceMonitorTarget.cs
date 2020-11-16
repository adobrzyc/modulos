using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modulos.Messaging.Diagnostics.Metrics.Targets
{
    public class ReadAndDeletePerformanceMonitorTarget : IReadAndDeletePerformanceMonitorTarget
    {
        private readonly object locker = new object();
        private readonly Dictionary<ActionId, List<IMetric>> data = new Dictionary<ActionId, List<IMetric>>();
        private readonly IEnumerable<IMetric> empty = new IMetric[] { };

        public int Count
        {
            get
            {
                lock (locker)
                {
                    return data.Count;
                }

            }
        }

        /// <summary>
        /// Not thread safe, so it may propagate with little delay. Default true.
        /// </summary>
        public bool IsEnabled { get; set; } = true;


        public IEnumerable<IMetric> Read(Func<IMetric, bool> predicate)
        {
            throw new NotSupportedException("This operation is not supported by this target.");
        }

        //public IEnumerable<ICounter> Read(OperationInfo operationInfo)
        //{
        //    return Read(counter => counter.OperationInfo == operationInfo);
        //}

        public IEnumerable<IMetric> Read(ActionId actionId)
        {
            if (actionId == null) throw new ArgumentNullException(nameof(actionId));

            lock (locker)
            {
                if (!data.ContainsKey(actionId))
                    return empty;

                var output = data[actionId];
                data.Remove(actionId);
                return output;
            }
        }

        public IEnumerable<IMetric> ReadAll()
        {
            lock (locker)
            {
                var output = data.Count == 0 ? empty : data.Values.SelectMany(e => e);
                data.Clear();
                return output;
            }   
        }

        public Task<IEnumerable<IMetric>> ReadAsync(Func<IMetric, bool> predicate)
        {
            var output = Read(predicate);
            return Task.FromResult(output);
        }

        //public Task<IEnumerable<ICounter>> ReadAsync(OperationInfo operationInfo)
        //{
        //    var output = Read(counter => counter.OperationInfo == operationInfo);
        //    return Task.FromResult(output);
        //}

        public Task<IEnumerable<IMetric>> ReadAsync(ActionId actionId)
        {
            var output = Read(actionId);
            return Task.FromResult(output);
        }

        public Task<IEnumerable<IMetric>> ReadAllAsync()
        {
            var output = ReadAll();
            return Task.FromResult(output);
        }

        public void Write(IMetric metric)
        {
            if (metric == null) throw new ArgumentNullException(nameof(metric));

            lock (locker)
            {
                if (!data.ContainsKey(metric.ActionId))
                    data.Add(metric.ActionId, new List<IMetric> { metric });
                else data[metric.ActionId].Add(metric);
            }
        }

        public Task WriteAsync(IMetric metric)
        {
            Write(metric);
            return Task.CompletedTask;
        }

        public void Clear()
        {
            lock (locker)
            {
                data.Clear();
            }
        }

        public Task ClearAsync()
        {
            Clear();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Clear();
        }
    }
}