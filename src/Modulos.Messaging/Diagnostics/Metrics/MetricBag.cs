using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    public class MetricBag : IMetricBag
    {
        private readonly ConcurrentBag<IMetric> counters = new ConcurrentBag<IMetric>();

        public IEnumerable<IMetric> GetAll()
        {
            return counters;
        }

        public void AddCounter(IMetric metric)
        {
            counters.Add(metric);
        }

        public IMetric Add(Kind kind, string what, InvocationPlace where, decimal value)
        {
            var counter = new Metric
            {
                What = what,
                Where = where,
                Kind = kind,
                Value = value
            };

            counters.Add(counter);

            return counter;
        }

        public IMetric Add(Kind kind, string what, InvocationPlace where, decimal value, IActionInfo actionInfo)
        {
            var counter = new Metric(actionInfo)
            {
                What = what,
                Where = where,
                Kind = kind,
                Value = value
            };

            counters.Add(counter);

            return counter;
        }

        public IMetric Add(Kind kind, InvocationPlace where, decimal value)
        {
            var counter = new Metric
            {
                Where = where,
                Kind = kind,
                Value = value
            };

            counters.Add(counter);

            return counter;
        }

        public IMetric Add(Kind kind, InvocationPlace where, decimal value, IActionInfo actionInfo)
        {
            var counter = new Metric(actionInfo)
            {
                Where = where,
                Kind = kind,
                Value = value
            };

            counters.Add(counter);

            return counter;
        }
    }
}