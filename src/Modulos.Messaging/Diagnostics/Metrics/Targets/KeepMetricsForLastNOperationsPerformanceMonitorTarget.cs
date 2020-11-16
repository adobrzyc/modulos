using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Modulos.Messaging.Diagnostics.Metrics.Targets
{
    //todo: [optimization] check it out 
    public class KeepMetricsForLastNOperationsPerformanceMonitorTarget : IKeepMetricsForLastNOperationsPerformanceMonitorTarget
    {
        private readonly OrderedDictionary data = new OrderedDictionary();
        private readonly ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
        private readonly IEnumerable<IMetric> empty = new IMetric[] { };
        private bool isDisposed;
        private readonly object disposeGuard = new object();

        public int Count
        {
            get
            {
                locker.EnterReadLock();
                try
                {
                    return data.Count;
                }
                finally
                {
                    locker.ExitReadLock();
                }
            }
        }

        /// <summary>
        /// Not thread safe, so it may propagate with delay. Default true.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        public int Capacity { get; }

        public KeepMetricsForLastNOperationsPerformanceMonitorTarget()
        {
            Capacity = 5000;
        }

        public KeepMetricsForLastNOperationsPerformanceMonitorTarget(int capacity)
        {
            if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be greater than 0.");

            Capacity = capacity;
        }

        private IEnumerable<IMetric> Values
        {
            get
            {
                return data.Count == 0 ? empty : data.Cast<DictionaryEntry>()
                    .SelectMany(e => (List<IMetric>)e.Value);
            }
        }

        public IEnumerable<IMetric> Read(Func<IMetric, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            locker.EnterReadLock();
            try
            {
                return Values.Where(predicate);
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        //public IEnumerable<ICounter> Read(OperationInfo operationInfo)
        //{
        //    return Read(counter => counter.OperationInfo == operationInfo);
        //}

        public IEnumerable<IMetric> Read(ActionId actionId)
        {
            if (actionId == null) throw new ArgumentNullException(nameof(actionId));
            locker.EnterReadLock();
            try
            {
                if (!data.Contains(actionId))
                    return empty;

                return (List<IMetric>)data[actionId];
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        public IEnumerable<IMetric> ReadAll()
        {
            locker.EnterReadLock();
            try
            {
                return Values;
            }
            finally
            {
                locker.ExitReadLock();
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
            var output = Read(counter => counter.ActionId == actionId);
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

            locker.EnterWriteLock();
            try
            {
                if (data.Count == Capacity)
                    data.RemoveAt(0);

                if (!data.Contains(metric.ActionId))
                    data.Add(metric.ActionId, new List<IMetric> { metric });
                else ((List<IMetric>)data[metric.ActionId]).Add(metric);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public Task WriteAsync(IMetric metric)
        {
            Write(metric);
            return Task.CompletedTask;
        }

        public void Clear()
        {
            locker.EnterWriteLock();
            try
            {
                data.Clear();
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }

        public Task ClearAsync()
        {
            Clear();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            if (isDisposed) return;

            lock (disposeGuard)
            {
                if (isDisposed) return;
                isDisposed = true;
            }

            Clear();
            locker.Dispose();
        }
    }
}