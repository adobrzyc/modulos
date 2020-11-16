using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Modulos.Messaging.Configuration
{
    [DataContract]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    public struct EndpointConfigStamp : IEquatable<EndpointConfigStamp>
    {
        public EndpointConfigStamp(Guid stamp)
        {
            Stamp = stamp;
        }

        [DataMember]
        public Guid Stamp { get; private set; }

        public bool Equals(EndpointConfigStamp other)
        {
            return Stamp.Equals(other.Stamp);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is EndpointConfigStamp stamp && Equals(stamp);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return Stamp.GetHashCode();
        }

        public static bool operator ==(EndpointConfigStamp left, EndpointConfigStamp right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EndpointConfigStamp left, EndpointConfigStamp right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return Stamp.ToString();
        }
    }
}