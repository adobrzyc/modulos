using System;

namespace Modulos
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    internal class VerifiedAttribute : Attribute
    {
        public string Description { get; }

        public VerifiedAttribute(string description)
        {
            Description = description;
        }

        public VerifiedAttribute()
        {
            
        }
    }
}