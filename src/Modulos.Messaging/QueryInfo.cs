using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Modulos.Messaging
{
    [DebuggerDisplay("{QueryType.Name,nq}")]
    [OptimizationRequired]
    public sealed class QueryInfo
    {
        public readonly Type QueryType;
        public readonly Type ResultType;

        public QueryInfo(Type queryType)
        {
            QueryType = queryType;
            ResultType = DetermineResultTypes(queryType).Single();
        }

        public static bool IsQuery(Type type)
        {
            return DetermineResultTypes(type).Any();
        }

        private static IEnumerable<Type> DetermineResultTypes(Type type)
        {
            return from interfaceType in type.GetInterfaces()
            where interfaceType.IsGenericType
            where interfaceType.GetGenericTypeDefinition() == typeof (IQuery<>)
            select interfaceType.GetGenericArguments()[0];
        }
    }
}