using System;

namespace Modulos
{
    internal static class TypeExtensions
    {
        public static object GetDefault(this Type t)
        {
            Func<object> f = GetDefault<object>;
            return f.Method.GetGenericMethodDefinition().MakeGenericMethod(t).Invoke(null, null);
        }

        private static T GetDefault<T>()
        {
            return default;
        }

        public static string FullNameWithAssemblyNameWithoutVersion(this Type type)
        {
            return $"{type.FullName}, {type.Assembly.GetName().Name}";
        }

        public static T GetAttributeFromMember<T>(this object memberInstance) where T : Attribute
        {
            var type = memberInstance.GetType();
            var memInfo = type.GetMember(memberInstance.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0 ? (T) attributes[0] : null;
        }
    }
}