using System;
using System.Linq;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    //todo: create KindInfo with info of: group and transportId attribute properties such as order, IsColumn , IsSpecial ect.
    public class KindInfo
    {
        public Kind Kind { get; }
        public KindInfoAttribute Attributes { get; }

        public KindInfo(Kind kind)
        {
            Kind = kind;
            Attributes = GetAttribute<KindInfoAttribute>(kind);
        }

        private static T GetAttribute<T>(Enum enumValue) where T : Attribute
        {
            var memberInfo = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault();

            if (memberInfo == null) return null;
            var attribute = (T)memberInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
            return attribute;
        }
    }
}