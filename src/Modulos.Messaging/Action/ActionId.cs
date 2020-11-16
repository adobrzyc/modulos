using System;
using System.Runtime.Serialization;

// ReSharper disable UnusedMember.Global

namespace Modulos.Messaging
{
    [DataContract]
    public struct ActionId : IEquatable<ActionId>
    {
        public static readonly ActionId Empty = new ActionId(Guid.Empty);
        
        public static ActionId Create()
        {
            return new ActionId(KeyGenerator.NewSequentialGuid());
        }

        [DataMember]
        public Guid Value { get; private set; }

        public ActionId(Guid value)
        {
            Value = value;
        }

        public bool Equals(ActionId other)
        {
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ActionId other && Equals(other);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return Value.GetHashCode();
        }

        public static bool operator ==(ActionId left, ActionId right)
        {
            return left.Value == right.Value;
        }

        public static bool operator !=(ActionId left, ActionId right)
        {
            return left.Value != right.Value;
        }
    }
}