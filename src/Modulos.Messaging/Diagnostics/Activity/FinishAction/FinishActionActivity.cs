using System;
using Modulos.Messaging.Diagnostics.Metrics;

namespace Modulos.Messaging.Diagnostics.Activity.FinishAction
{
    public sealed class FinishActionActivity : IActivity
    {
        public IActionInfo ActionInfo { get; }
        public IMetricBag MetricBag { get; }
        public string Reason { get; }
        public InvocationPlace Where { get;  }
        public object RelatedObject { get; }
        public object Host { get; }
        public Exception Error { get; }

        public FinishActionActivity(IActionInfo actionInfo, IMetricBag metricBag, string reason, InvocationPlace where, 
            object relatedObject, object host, Exception error)
        {
            ActionInfo = actionInfo;
            Where = where;
            Host = host;
            Error = error;
            MetricBag = metricBag;
            RelatedObject = relatedObject;
            Reason = reason;
        }
    }
}