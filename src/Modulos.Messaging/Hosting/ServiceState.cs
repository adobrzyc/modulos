using System;
using System.Runtime.Serialization;
using Modulos.Messaging.Diagnostics.Options;
using Modulos.Messaging.Hosting.Options;

namespace Modulos.Messaging.Hosting
{
    [DataContract]
    public class ServiceState : IServiceState
    {
        public ServiceState()
        {
            LastOperationTimeUtc = DateTime.MinValue.ToUniversalTime();
        }

        [DataMember]
        public int ActiveOperationCount { get; internal set; }

        [DataMember]
        public int TotalOperationCount { get; internal set; }

        [DataMember]
        public DateTime LastOperationTimeUtc { get; internal set; }

        [DataMember]
        public OperatingMode OperatingMode { get; internal set; }
       
        [DataMember]
        public LogMark LogMark { get; internal set; }
    }
}