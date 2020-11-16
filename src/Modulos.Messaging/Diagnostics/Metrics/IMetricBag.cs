using System.Collections.Generic;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    /*
     * #rename: todo: change name to metric/s
     *
     * #design-concept: Avoid usage of Add(ICounter) because another methods may use object pools (implementation detail).
     *
     * #abandoned-concept: maybe there should NOT be a way to add metric without action info ?
     *      #why-not: metrics must be available not only from boundaries (e.q.: ICqrsBus, HttpMiddleware)
     *      #why-not: there are situations where IActionInfo is not available. Avoiding passing IActionInfo is good practice.
     */

    /// <summary>
    /// Provides bag for metrics collected during operation lifetime.
    /// </summary>
    public interface IMetricBag
    {
        IEnumerable<IMetric> GetAll();

        IMetric Add(Kind kind, string what, InvocationPlace where, decimal value, IActionInfo actionInfo);
        IMetric Add(Kind kind, string what, InvocationPlace where, decimal value);
        IMetric Add(Kind kind, InvocationPlace where, decimal value);
        IMetric Add(Kind kind, InvocationPlace where, decimal value, IActionInfo actionInfo);
    }
}