using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Modulos.Messaging.Configuration
{
    [DataContract]
    public struct EndpointConfigInfo : IEquatable<EndpointConfigInfo>
    {
        public EndpointConfigInfo(EndpointConfigMark endpointConfigMark, EndpointConfigStamp endpointConfigStamp)
        {
            EndpointConfigMark = endpointConfigMark;
            EndpointConfigStamp = endpointConfigStamp;
        }
        
        [DataMember]
        public EndpointConfigMark EndpointConfigMark { get; private set; }

        [DataMember]
        public EndpointConfigStamp EndpointConfigStamp { get; private set; }


        public bool Equals(EndpointConfigInfo other)
        {
            return EndpointConfigMark.Equals(other.EndpointConfigMark) && EndpointConfigStamp.Equals(other.EndpointConfigStamp);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is EndpointConfigInfo info && Equals(info);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                return (EndpointConfigMark.GetHashCode() * 397) ^ EndpointConfigStamp.GetHashCode();
            }
        }

        public static bool operator ==(EndpointConfigInfo left, EndpointConfigInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EndpointConfigInfo left, EndpointConfigInfo right)
        {
            return !left.Equals(right);
        }
    }
}