using System;
using System.Runtime.Serialization;

namespace Modulos.Messaging.Compression
{
    [DataContract]
    public struct CompressionEngineId : IEquatable<CompressionEngineId>
    {
        [DataMember]
        public string Id { get; private set; }

        public CompressionEngineId(string id)
        {
            Id = id;
        }

        public bool Equals(CompressionEngineId other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CompressionEngineId id && Equals(id);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return Id.GetHashCode();
        }

        public static bool operator ==(CompressionEngineId left, CompressionEngineId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CompressionEngineId left, CompressionEngineId right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return Id;
        }

        public static implicit operator CompressionEngineId(string id)
        {
            return new CompressionEngineId(id);
        }
    }
}