using System.Runtime.Serialization;

namespace Modulos.Messaging.Hosting.Options
{
    [DataContract]
    public enum OperatingMode
    {
        [EnumMember] Normal = 0,
        [EnumMember] Maintenance = 1,
    }
}