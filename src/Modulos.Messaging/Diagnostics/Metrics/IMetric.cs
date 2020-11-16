using System;
using Modulos.Messaging.Operation;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    //todo: lets think about adding 
    public interface IMetric
    {
        /// <summary>
        /// Unique identifier of counter.
        /// </summary>
        Guid MetricId { get; set; }

        //TypeInfo TypeInfo { get; set; } //todo: [optimization] check it out if it's really important at all
        OperationId OperationId { get; set; }
        ActionId ActionId { get; set; }
      
        string What { get; set; }
        DateTime TimeUtc { get; set; }
        decimal Value { get; set; }

        Kind Kind { get; set; }
        InvocationPlace Where { get; set; }


        bool IsSigned { get; }

        //void Sign(IHydraContext context);
        //void Sign(OperationInfo operationInfo, CallInfo callInfo);
    }
}