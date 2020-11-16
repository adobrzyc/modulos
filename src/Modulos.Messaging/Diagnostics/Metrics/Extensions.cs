using System;
using System.Collections.Generic;
using Modulos.Messaging.Operation;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    internal static class Extensions
    {
        public static void Sign(this IEnumerable<IMetric> metrics, OperationId? operationId = null, ActionId? actionId = null, bool replaceExisting = false)
        {
            if (metrics == null) throw new ArgumentNullException(nameof(metrics));
            
            foreach (var metric in metrics)
            {
                if (!replaceExisting && metric.IsSigned) continue;

                if (operationId != null)
                    metric.OperationId = operationId.Value;
                if (actionId != null)
                    metric.ActionId = actionId.Value;
            }
        }

        public static void Sign(this IEnumerable<IMetric> metrics, IActionInfo actionInfo, bool replaceExisting = false)
        {
            metrics.Sign(actionInfo.OperationId, actionInfo.Id, replaceExisting);
        }

        public static void Sign(this IMetric metric, IActionInfo actionInfo)
        {
            new[] {metric}.Sign(actionInfo);

        }
    }
}