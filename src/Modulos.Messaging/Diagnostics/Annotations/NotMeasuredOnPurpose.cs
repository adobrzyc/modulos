using System;

namespace Modulos.Messaging.Diagnostics.Annotations
{
    [AttributeUsage(AttributeTargets.All, Inherited = false)]
    internal class NotMeasuredOnPurpose : Attribute
    {
        public string Description { get;  }

        public NotMeasuredOnPurpose(string description)
        {
            Description = description;
        }

        public NotMeasuredOnPurpose()
        {
            
        }
    }
}