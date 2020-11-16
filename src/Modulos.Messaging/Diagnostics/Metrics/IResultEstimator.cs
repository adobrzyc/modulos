using System.Collections.Generic;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    public interface IResultEstimator
    {
        IEnumerable<Kind> Kinds { get; }
        IEnumerable<ValueKind> ValueKinds { get; } 
        IEnumerable<IResult> Calculate(IEnumerable<IMetric> metrics);
    }
}