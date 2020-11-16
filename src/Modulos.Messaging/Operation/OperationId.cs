using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Modulos.Messaging.Operation
{
    [DataContract]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    public struct OperationId : IEquatable<OperationId>
    {
        public static readonly OperationId Empty = new OperationId(Guid.Empty);

        public static OperationId Create()
        {
            return new OperationId(KeyGenerator.NewSequentialGuid());
        }

        [DataMember]
        public Guid Value { get; private set; }


        public OperationId(Guid value)
        {
            Value = value;
        }

        public OperationId(OperationId source)
            :this(source.Value)
        {
        }


        public bool Equals(OperationId other)
        {
            return  Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is OperationId info && Equals(info);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return Value.GetHashCode();
        }

        public static bool operator ==(OperationId left, OperationId right)
        {
            return left.Value == right.Value;
        }

        public static bool operator !=(OperationId left, OperationId right)
        {
            return left.Value != right.Value;
        }


        public override string ToString()
        {
            return Value.ToString();
        }
    }
}