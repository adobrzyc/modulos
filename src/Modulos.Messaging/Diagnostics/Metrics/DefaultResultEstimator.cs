using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Modulos.Messaging.Diagnostics.Metrics.Utils;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    [UsedImplicitly]
    public class DefaultResultEstimator : IResultEstimator
    {
        public IEnumerable<Kind> Kinds { get; } = KindUtils.KindInformation
            .Where(e => e.Attributes.IsColumn && !e.Attributes.IsSpecial)
            .Select(e => e.Kind)
            .ToArray();

        public IEnumerable<ValueKind> ValueKinds { get; } = new[] {ValueKind.Avg, ValueKind.Md, ValueKind.Min, ValueKind.Max, ValueKind.Sum};

        public IEnumerable<IResult> Calculate(IEnumerable<IMetric> metrics)
        {
            var results = new List<IResult>();

            foreach (var metricKinds in metrics.Where(e => Kinds.Contains(e.Kind)).GroupBy(x => x.Kind))
            {
                var attribute = metricKinds.Key.GetAttributeFromMember<KindInfoAttribute>();
                if (!attribute.IsColumn || attribute.IsSpecial)
                    continue;

                var tmpCounters = new List<IMetric>();

                //todo: adito
                //todo: L0 - replace with side effect * is it still actually ?
                if (metricKinds.Key == Kind.Execution || metricKinds.Key == Kind.Total)
                {
                    foreach (var operation in metricKinds.GroupBy(i => i.OperationId.Value))
                    {
                        //var minLevel = operation.Min(e => e.CallInfo.Level);
                        //todo: tutututuut
                        //tmpCounters.AddRange(operation.Where(e => Equals(e.ActionId.PrevCallId, null)));
                    }

                    //var grouped = countersKind.GroupBy(i => i.InvocationPlace).ToArray();

                    //var callers = grouped.Where(i => i.Key == InvocationPlace.Caller).ToArray();
                    //if (callers.Any())
                    //    foreach (var caller in callers)
                    //        tmpCounters.AddRange(caller);
                    //else
                    //    foreach (var target in grouped.Where(i => i.Key == InvocationPlace.Target))
                    //        tmpCounters.AddRange(target);
                }
                else
                    tmpCounters = metricKinds.ToList();

                var listOfValues = new List<decimal>();
                foreach (var counter in tmpCounters.GroupBy(i => i.ActionId.Value))
                    listOfValues.Add(counter.Select(i => i.Value).Sum());

                results.Add(new Result(metricKinds.Key, ValueKind.Avg, listOfValues.Any() ? listOfValues.Average() : 0));

                results.Add(new Result(metricKinds.Key, ValueKind.Min, listOfValues.Any() ? listOfValues.Min() : 0));

                results.Add(new Result(metricKinds.Key, ValueKind.Max, listOfValues.Any() ? listOfValues.Max() : 0));

                results.Add(new Result(metricKinds.Key, ValueKind.Sum, listOfValues.Any() ? listOfValues.Sum() : 0));

                results.Add(new Result(metricKinds.Key, ValueKind.Md, listOfValues.Any() ? MathUtils.CalculateMedian(listOfValues) : 0));
            }
            return results;
        }
    }
}