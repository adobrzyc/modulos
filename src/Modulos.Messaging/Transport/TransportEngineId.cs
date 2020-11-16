using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Modulos.Messaging.Transport
{
    /// <summary>
    /// Defines unique identifier of transport.
    /// </summary>
    [DataContract]
    public struct TransportEngineId : IEquatable<TransportEngineId>
    {
        [DataMember]
        public string Value { get; private set; }

        public TransportEngineId(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public bool Equals(TransportEngineId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is TransportEngineId other && Equals(other);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            return Value != null ? Value.GetHashCode() : 0;
        }

        public static bool operator ==(TransportEngineId left, TransportEngineId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TransportEngineId left, TransportEngineId right)
        {
            return !left.Equals(right);
        }
    }
}