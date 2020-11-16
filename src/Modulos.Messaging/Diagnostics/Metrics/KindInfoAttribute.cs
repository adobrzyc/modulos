using System;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class KindInfoAttribute : Attribute
    {
        public KindInfoAttribute(Group group, bool isSpecial, bool isColumn)
        {
            Group = group;
            IsSpecial = isSpecial;
            IsColumn = isColumn;
        }

        public Group Group { get; }
        public bool IsSpecial { get; }
        public bool IsColumn { get; }
        public string DisplayName { get; set; }
    }
}