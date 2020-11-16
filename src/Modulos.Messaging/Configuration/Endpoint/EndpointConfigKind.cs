using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Modulos.Messaging.Configuration
{
    [DataContract]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    public struct EndpointConfigKind : IEquatable<EndpointConfigKind>
    {
        public EndpointConfigKind(string kind)
        {
            Kind = kind;
        }

        [DataMember]
        public string Kind { get; private set; }

        public bool Equals(EndpointConfigKind other)
        {
            return string.Equals(Kind, other.Kind, StringComparison.Ordinal);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is EndpointConfigKind kind && Equals(kind);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            return Kind != null ? StringComparer.Ordinal.GetHashCode(Kind) : 0;
        }

        public static bool operator ==(EndpointConfigKind left, EndpointConfigKind right)
        {
            return left.Kind == right.Kind;
        }

        public static bool operator !=(EndpointConfigKind left, EndpointConfigKind right)
        {
            return left.Kind != right.Kind;
        }

        public override string ToString()
        {
            return $"{Kind}";
        }
    }
}