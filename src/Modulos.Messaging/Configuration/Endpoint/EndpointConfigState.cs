using System.Runtime.Serialization;

namespace Modulos.Messaging.Configuration
{
    
    [DataContract]
    public enum EndpointConfigState
    {
        [EnumMember] Valid,
        [EnumMember] Obsolete,
    }
}