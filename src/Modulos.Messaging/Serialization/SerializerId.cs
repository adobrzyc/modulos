using System;
using System.Runtime.Serialization;

namespace Modulos.Messaging.Serialization
{
    [DataContract]
    public struct SerializerId : IEquatable<SerializerId>
    {
        [DataMember]
        public string Value { get; private set; }

        public SerializerId(string value)
        {
            Value = value;
        }

        public bool Equals(SerializerId other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is SerializerId id && Equals(id);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return Value.GetHashCode();
        }

        public static bool operator ==(SerializerId left, SerializerId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(SerializerId left, SerializerId right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return Value;
        }


        public static implicit operator SerializerId(string id)
        {
            return new SerializerId(id);
        }
    }
}