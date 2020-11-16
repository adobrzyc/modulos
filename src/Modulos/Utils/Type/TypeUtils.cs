using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Modulos
{
    internal static class TypeUtils
    {
        public static bool CheckIfAnonymousType(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            // HACK: The only way to detect anonymous types right now.
            return Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false)
                   && type.IsGenericType && type.Name.Contains("AnonymousType")
                   && (type.Name.StartsWith("<>") || type.Name.StartsWith("VB$"))
                   && type.Attributes.HasFlag(TypeAttributes.NotPublic);
        }

        public static bool IsAssignableToGenericType(Type givenType, Type genericType, 
            out Type concreteGeneric,  
            out IEnumerable<Type> genericTypeArguments)
        {
           // givenType.IsClosedTypeOf();
            while (true)
            {
                var interfaceTypes = givenType.GetInterfaces();

                foreach (var type in interfaceTypes)
                {
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                    {
                        concreteGeneric = type;
                        genericTypeArguments = type.GenericTypeArguments;
                        return true;
                    }
                }

                if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                {
                    concreteGeneric = givenType;
                    genericTypeArguments = givenType.GenericTypeArguments;
                    return true;
                }

                var baseType = givenType.BaseType;
                if (baseType == null)
                {
                    concreteGeneric = null;
                    genericTypeArguments = Enumerable.Empty<Type>();
                    return false;
                }

                givenType = baseType;
            }
        }
    }
}
