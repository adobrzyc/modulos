using System;
using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;

namespace Modulos.Messaging.Diagnostics.Activity.FinishAction
{
    public interface IFinishActionActivityProcessor
    {
        Task Process(IActionInfo action, IMetricBag metricBag, string reason, InvocationPlace where, object relatedObject, object host, Exception error);
    }
}