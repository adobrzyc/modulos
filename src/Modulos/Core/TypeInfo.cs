using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Serialization;

namespace Modulos
{
    [DataContract]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    public struct TypeInfo : IEquatable<TypeInfo>
    {
        public static readonly TypeInfo Empty = new TypeInfo
        {
            IsAnonymous = false,
            TypeMark = Guid.Empty.ToString(),
            TypeDefinition =  null,
            TypeName = null
        };


        /// <summary>
        /// Defines mark of a type, unique at application level. Used to find type based on
        /// unique (e.q. guid) identifier. Use <see cref="TypeMark"/> to define mark on type.
        /// </summary>
        [DataMember]
        public string TypeMark { get; private set; }

        /// <summary>
        /// Human readable name of a type.
        /// </summary>
        [DataMember]
        public string TypeName { get; private set; }

        /// <summary>
        /// Type definition used to resolve type when <see cref="TypeMark"/> is not defined.
        /// </summary>
        [DataMember]
        public string TypeDefinition { get; private set; }

        /// <summary>
        /// Defines if specified type is anonymous type.
        /// </summary>
        [DataMember]
        public bool IsAnonymous { get; private set; }

        /// <summary>
        /// Create new instance of class.
        /// </summary>
        /// <param name="type">Type to describe.</param>
        public TypeInfo(Type type)
            : this()
        {
            Create(type);
        }

        public TypeInfo(object obj)
            : this()
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            switch (obj)
            {
                case TypeInfo typeInfo:
                    TypeMark = typeInfo.TypeMark;
                    IsAnonymous = typeInfo.IsAnonymous;
                    TypeName = typeInfo.TypeName;
                    TypeDefinition = typeInfo.TypeDefinition;
                    break;
                default:
                    Create(obj.GetType());
                    break;
            }
        }

        private void Create(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            TypeMark = type.GetCustomAttribute<TypeMarkAttribute>()?.Mark;

            if (string.IsNullOrEmpty(TypeMark))
            {
                if (TypeUtils.CheckIfAnonymousType(type))
                {
                    IsAnonymous = true;
                    TypeName = "anonymous";
                }
                else
                {
                    IsAnonymous = false;
                    TypeName = type.FullName;
                    TypeDefinition = type.FullNameWithAssemblyNameWithoutVersion();
                }
            }
            else
            {
                // marked type cannot be anonymous
                IsAnonymous = false;
                TypeName = type.FullName;
                TypeDefinition = type.FullNameWithAssemblyNameWithoutVersion();
            }
        }


        public bool Equals(TypeInfo other)
        {
            if (!string.IsNullOrEmpty(TypeMark) || !string.IsNullOrEmpty(other.TypeMark))
                return string.Equals(TypeMark, other.TypeMark, StringComparison.Ordinal);
           
            return string.Equals(TypeDefinition, other.TypeDefinition)
                   && IsAnonymous == other.IsAnonymous;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TypeInfo other && Equals(other);
        }

        // ReSharper disable NonReadonlyMemberInGetHashCode
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (TypeMark != null ? TypeMark.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (TypeDefinition != null ? TypeDefinition.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsAnonymous.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(TypeInfo left, TypeInfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TypeInfo left, TypeInfo right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return TypeName ?? TypeDefinition ?? TypeMark ?? "unknown";
        }
    }
}
