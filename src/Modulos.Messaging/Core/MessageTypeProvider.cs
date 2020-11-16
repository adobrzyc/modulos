using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Modulos.Messaging
{
    internal class MessageTypeProvider : IMessageTypeProvider
    {
        private readonly Dictionary<string, Type> typesDefinedByMark = new Dictionary<string, Type>();
        private readonly Type anonymousType = typeof(object);
        private readonly Type queryBaseType = typeof(IQueryBase);

        public MessageTypeProvider(ITypeExplorer typeExplorer)
        {
            var types = typeExplorer.GetSearchableClasses();

            var messageType = typeof(IMessage);
            var messages = types
                .Where(e => (messageType.IsAssignableFrom(e) || e.GetCustomAttribute<TypeMarkAttribute>() != null)
                            && (e.IsPublic || e.IsNestedPublic)
                            && (e.IsClass || e.IsValueType && !e.IsEnum && !e.IsPrimitive)
                            && !e.IsAbstract);

            foreach (var type in messages)
            {
                AddDefinition(type);
             
                if(queryBaseType.IsAssignableFrom(type))
                {
                    var queryInfo = new QueryInfo(type);
                    AddDefinition(queryInfo.ResultType);
                }
            }
        }

        private void AddDefinition(Type type)
        {
            var key = type.GetCustomAttribute<TypeMarkAttribute>()?.Mark ?? type.GUID.ToString();
            if(!typesDefinedByMark.ContainsKey(key))
                typesDefinedByMark.Add(key, type);
        }

        
        public Type FindType(TypeInfo typeInfo, bool throwExceptionIfNotFound)
        {
            if (typeInfo.IsAnonymous) return anonymousType;

            if (!string.IsNullOrEmpty(typeInfo.TypeMark))
                return typesDefinedByMark[typeInfo.TypeMark];

            if (!string.IsNullOrEmpty(typeInfo.TypeDefinition))
                return Type.GetType(typeInfo.TypeDefinition, throwExceptionIfNotFound);
            
            if (throwExceptionIfNotFound)
                throw new TodoException($"Unable to find specified type: {typeInfo}");

            return null;
        }
    }
}