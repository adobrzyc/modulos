using System.Runtime.Serialization;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    
    [DataContract]
    public enum InvocationPlace
    {
        [EnumMember]
        Caller, 
        [EnumMember]
        Target
    }
}