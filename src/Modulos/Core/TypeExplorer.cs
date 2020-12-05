using System;
using System.Linq;
using System.Reflection;

namespace Modulos
{
    internal class TypeExplorer : ITypeExplorer
    {
        private readonly Type[] _classes;
        private readonly Type[] _interfaces;
        private readonly Type[] _types;

        public TypeExplorer(IAssemblyExplorer assemblyExplorer)
        {
            _types = assemblyExplorer.Assemblies
                .SelectMany(a => a.GetExportedTypes())
                .Where(type => type.IsPublic || type.IsNestedPublic)
                .Where(e => e.GetCustomAttribute<SuppressAutoAttribute>() == null)
                .ToArray();

            _classes = _types
                .Where(type => !type.IsAbstract && type.IsClass)
                .Where(e => e.GetCustomAttribute<SuppressAutoAttribute>() == null)
                .ToArray();

            _interfaces = _types
                .Where(type => type.IsInterface)
                .Where(e => e.GetCustomAttribute<SuppressAutoAttribute>() == null)
                .ToArray();
        }

        public Type[] GetSearchableClasses(Func<Type, bool> filter = null)
        {
            return filter != null
                ? _classes.Where(filter).ToArray()
                : _classes.ToArray();
        }

        public Type[] GetSearchableInterfaces(Func<Type, bool> filter = null)
        {
            return filter != null
                ? _interfaces.Where(filter).ToArray()
                : _interfaces.ToArray();
        }

        public Type[] GetSearchableTypes(Func<Type, bool> filter = null)
        {
            return filter != null
                ? _types.Where(filter).ToArray()
                : _types.ToArray();
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