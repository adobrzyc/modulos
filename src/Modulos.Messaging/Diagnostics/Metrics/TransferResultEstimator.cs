using System;
using System.Collections.Generic;
using System.Linq;
using Modulos.Messaging.Diagnostics.Metrics.Utils;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    //todo: learn and delete it 22.01.2019 
    //todo: candidate to be obsolete ?
    //#design-concept: it's generally bad idea to measure/estimate anything based on times collected in two separated machines
    public class TransferResultEstimator : IResultEstimator
    {
        private static readonly Kind[] KindsCache = {Kind.RequestArrived, Kind.ResponseArrived, Kind.RequestStart, Kind.ResponseStart};
        private static readonly ValueKind[] ValuesKindsCache = {ValueKind.Avg, ValueKind.Md, ValueKind.Min, ValueKind.Max, ValueKind.Sum};

        public IEnumerable<Kind> Kinds => KindsCache;
        public IEnumerable<ValueKind> ValueKinds => ValuesKindsCache;

        public IEnumerable<IResult> Calculate(IEnumerable<IMetric> metrics)
        {
            var results = new List<IResult>();
            var times = new List<KeyValuePair<Kind, TimeSpan>>();

            var calls = metrics.Where(e => Kinds.Contains(e.Kind))
                .GroupBy(e => e.ActionId)
                .ToArray();

            foreach (var call in calls)
            {
                var requestTime = TimeSpan.Zero;
                var responseTime = TimeSpan.Zero;

                var requestStartCounter = call.FirstOrDefault(e => e.Kind == Kind.RequestStart);
                var requestEndCounter = call.FirstOrDefault(e => e.Kind == Kind.RequestArrived);
                var responseStartCounter = call.FirstOrDefault(e => e.Kind == Kind.ResponseStart);
                var responseEndCounter = call.FirstOrDefault(e => e.Kind == Kind.ResponseArrived);

                if (requestStartCounter != null && requestEndCounter != null)
                {
                    var start = new DateTime((long) requestStartCounter.Value, DateTimeKind.Utc);
                    var end = new DateTime((long) requestEndCounter.Value, DateTimeKind.Utc);
                    requestTime = requestTime.Add(end - start);
                }

                if (responseStartCounter != null && responseEndCounter != null)
                {
                    var start = new DateTime((long) responseStartCounter.Value, DateTimeKind.Utc);
                    var end = new DateTime((long) responseEndCounter.Value, DateTimeKind.Utc);
                    responseTime = responseTime.Add(end - start);
                }

                //22.01.2019
                //times.Add(new KeyValuePair<Transport, TimeSpan>(Transport.TransferTo, requestTime));
                //times.Add(new KeyValuePair<Transport, TimeSpan>(Transport.TransferFrom, responseTime));
            }

            //var transferKinds = new[] {Transport.TransferFrom, Transport.TransferTo};

            //foreach (var transferKind in transferKinds)
            //{
            //    var kindValues = times.Where(x => x.Key == transferKind)
            //        .Select(x => x.Value.TotalMilliseconds)
            //        .ToArray();
            //    var anyValue = kindValues.Any();

            //    results.Add(new Result(transferKind, ValueKind.Avg, anyValue ? new decimal(kindValues.Average()) : 0));
            //    results.Add(new Result(transferKind, ValueKind.Md, anyValue ? MathUtils.CalculateMedian(kindValues.Select(Convert.ToDecimal)) : 0));
            //    results.Add(new Result(transferKind, ValueKind.Min, anyValue ? new decimal(kindValues.Min()) : 0));
            //    results.Add(new Result(transferKind, ValueKind.Max, anyValue ? new decimal(kindValues.Max()) : 0));
            //    results.Add(new Result(transferKind, ValueKind.Sum, anyValue ? new decimal(kindValues.Sum()) : 0));
            //}

            var anyTimesValue = times.Any();

            results.Add(new Result(Kind.Transfer, ValueKind.Avg, anyTimesValue
                ? new decimal(times.Average(x => x.Value.TotalMilliseconds))
                : 0));

            results.Add(new Result(Kind.Transfer, ValueKind.Md, anyTimesValue
                ? MathUtils.CalculateMedian(times.Select(x => Convert.ToDecimal(x.Value.TotalMilliseconds)))
                : 0));

            results.Add(new Result(Kind.Transfer, ValueKind.Min, anyTimesValue
                ? new decimal(times.Min(x => x.Value.TotalMilliseconds))
                : 0));

            results.Add(new Result(Kind.Transfer, ValueKind.Max, anyTimesValue
                ? new decimal(times.Max(x => x.Value.TotalMilliseconds))
                : 0));

            results.Add(new Result(Kind.Transfer, ValueKind.Sum, anyTimesValue
                ? new decimal(times.Sum(x => x.Value.TotalMilliseconds))
                : 0));
            return results;
        }
    }
}