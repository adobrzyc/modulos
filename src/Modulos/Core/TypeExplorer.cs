using System;
using System.Linq;
using System.Reflection;

namespace Modulos
{
    internal class TypeExplorer : ITypeExplorer
    {
        private readonly Type[] classes;
        private readonly Type[] interfaces;
        private readonly Type[] types;

        public TypeExplorer(IAssemblyExplorer assemblyExplorer)
        {
            types = assemblyExplorer.Assemblies
                .SelectMany(a => a.GetExportedTypes())
                .Where(type => type.IsPublic || type.IsNestedPublic)
                .Where(e => e.GetCustomAttribute<SuppressAutoAttribute>() == null)
                .ToArray();

            classes = types
                .Where(type => !type.IsAbstract && type.IsClass)
                .Where(e => e.GetCustomAttribute<SuppressAutoAttribute>() == null)
                .ToArray();

            interfaces = types
                .Where(type => type.IsInterface)
                .Where(e => e.GetCustomAttribute<SuppressAutoAttribute>() == null)
                .ToArray();
        }

        public Type[] GetSearchableClasses(Func<Type, bool> filter = null)
        {
            return filter != null
                ? classes.Where(filter).ToArray()
                : classes.ToArray();
        }

        public Type[] GetSearchableInterfaces(Func<Type, bool> filter = null)
        {
            return filter != null
                ? interfaces.Where(filter).ToArray()
                : interfaces.ToArray();
        }

        public Type[] GetSearchableTypes(Func<Type, bool> filter = null)
        {
            return filter != null
                ? types.Where(filter).ToArray()
                : types.ToArray();
        }


        public Type[] GetSearchableClasses<TAssignableTo>(Func<Type, bool> filter = null)
        {
            return GetSearchableClasses(filter)
                .Where(e => typeof(TAssignableTo).IsAssignableFrom(e))
                .ToArray();
        }

        public Type[] GetSearchableInterfaces<TAssignableTo>(Func<Type, bool> filter = null)
        {
            return GetSearchableInterfaces(filter)
                .Where(e => typeof(TAssignableTo).IsAssignableFrom(e))
                .ToArray();
        }

        public Type[] GetSearchableTypes<TAssignableTo>(Func<Type, bool> filter = null)
        {
            return GetSearchableTypes(filter)
                .Where(e => typeof(TAssignableTo).IsAssignableFrom(e))
                .ToArray();
        }
    }
}