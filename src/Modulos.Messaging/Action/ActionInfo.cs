using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Modulos.Messaging.Operation;
using Newtonsoft.Json;

/*
 * #design-concept: ActionInfo should provide time measurement mechanism.
 * #design-concept: Object pool should be used to obtain new ActionInfo objects.
 * #considerations: 
 *   - #performance cost is acceptable for now, but review and tests are required.
 *     Check if keeping a lot of living Stopwatch objects does not kill performance.
 * 
 *   - There is no good alternative to measure time instead of ActionInfo. 
 *
 * #design-concept: use object pool
 */

namespace Modulos.Messaging
{
    [DataContract]
    public class ActionInfo : IActionInfo
    {
        private Stopwatch estimator;
        private long elapsedMilliseconds = -1;

        [DataMember]
        public string Mark { get; private set; }

        [DataMember]
        public string AssemblyName { get; private set; }

        [DataMember]
        public OperationId OperationId { get; private set;  }

        [DataMember]
        public ActionId Id { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public string HostName { get;private set;  }

        [DataMember]
        public DateTime StartTimeUtc { get; private set; }

        [DataMember]
        public long ElapsedMilliseconds
        {
            get => estimator?.ElapsedMilliseconds ?? elapsedMilliseconds;
            private set => elapsedMilliseconds = value;
        }

        [JsonConstructor]
        private ActionInfo()
        {

        }

        public ActionInfo(OperationId operationId, ActionId actionId, 
            string actionName, string actionClass, string assemblyName, 
            string actionMark, bool start)
        {
            OperationId = operationId;
            Id = actionId;
            Name = actionName;
            HostName = actionClass;
            Mark = actionMark;
            AssemblyName = assemblyName;

            if(start)
                Start();
        }


        public void Start()
        {
            StartTimeUtc = DateTime.UtcNow;
            estimator = new Stopwatch();
            estimator.Start();
        }

        [SuppressMessage("ReSharper", "ParameterHidesMember")]
        public void Start(DateTime startTimeUtc, Stopwatch estimator)
        {
            StartTimeUtc = startTimeUtc;
            this.estimator = estimator;
            if(!this.estimator.IsRunning)
                this.estimator.Start();
        }

        public void Restart()
        {
            StartTimeUtc = DateTime.UtcNow;
            estimator ??= new Stopwatch();
            estimator.Restart();
        }
    }
}