using System;
using System.Diagnostics.CodeAnalysis;

namespace Modulos.Messaging.Security
{
    public struct AuthenticationScheme : IEquatable<AuthenticationScheme>
    {
        public static readonly AuthenticationScheme None = new AuthenticationScheme("none");

        public string Name { get; }

        public AuthenticationScheme(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

        public bool Equals(AuthenticationScheme other)
        {
            return Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return obj is AuthenticationScheme other && Equals(other);
        }
        
        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }

        public static bool operator ==(AuthenticationScheme left, AuthenticationScheme right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AuthenticationScheme left, AuthenticationScheme right)
        {
            return !left.Equals(right);
        }
    }
}