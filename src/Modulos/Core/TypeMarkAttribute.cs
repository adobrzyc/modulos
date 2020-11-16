using System;

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
        public string Mark { get; set; }

        public TypeMarkAttribute(string mark)
        {
            Mark = mark;
        }
    }
}