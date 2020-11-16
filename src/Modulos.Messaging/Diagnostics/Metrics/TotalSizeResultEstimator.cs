using System.Collections.Generic;
using System.Linq;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    public class TotalSizeResultEstimator : IResultEstimator
    {
        public IEnumerable<Kind> Kinds { get; } = new[] {Kind.RequestSize, Kind.ResponseSize};
        public IEnumerable<ValueKind> ValueKinds { get; } = new[] {ValueKind.Sum};

        public IEnumerable<IResult> Calculate(IEnumerable<IMetric> metrics)
        {
            var results = new List<IResult>();
            var sum = metrics
                .Where(e => Kinds.Contains(e.Kind))
                .Select(e=>e.Value)
                .DefaultIfEmpty(0)
                .Sum();

            results.Add(new Result(Kind.TotalSize, ValueKind.Sum, sum));

            return results;
        }
    }
}