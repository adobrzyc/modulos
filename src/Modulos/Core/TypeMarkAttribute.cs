using System;
using System.Linq;

namespace Modulos
{
    /// <summary>
    /// Defines mark for a type. Mark must be unique at application level.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    public sealed class TypeMarkAttribute : Attribute
    {
        /// <summary>
        /// Mark of a type. It's preferable to use <see cref="Guid"/> to generate this member.
        /// </summary>
        public string Mark { get; }

        public TypeMarkAttribute(string mark)
        {
            Mark = mark;
        }

        public TypeMarkAttribute(string prefix, Type type, MarkKind kind)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            string fromType;

            switch (kind)
            {
                case MarkKind.FullName:
                    fromType = type.FullName;
                    break;
                case MarkKind.RevertFullName:
                    fromType = string.Join(".", type.FullName.Split('.').Reverse());
                    break;
                case MarkKind.FullNameWithAssembly:
                    fromType = type.FullNameWithAssemblyNameWithoutVersion();
                    break;
                case MarkKind.ClassName:
                    fromType = type.Name;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }

            if (!string.IsNullOrEmpty(prefix))
                prefix += ".";

            Mark = prefix ?? "" + fromType;
        }

        public TypeMarkAttribute(Type type, MarkKind kind) : this(null,type,kind)
        {
        }

        public TypeMarkAttribute(Type type) : this(null,type, MarkKind.RevertFullName)
        {
        }

        public TypeMarkAttribute(string prefix, Type type) : this(prefix, type, MarkKind.ClassName)
        {
        }
    }

    public enum MarkKind
    {
        FullName,
        RevertFullName,
        FullNameWithAssembly,
        ClassName
    }
}