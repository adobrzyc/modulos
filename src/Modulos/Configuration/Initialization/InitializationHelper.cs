using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

// ReSharper disable UnusedMember.Global

namespace Modulos
{
    internal static class InitializationHelper
    {
        private const string Selector = "Modulos";

        public static bool IsModulosAssembly(AssemblyName asm)
        {
            return asm.FullName.StartsWith(Selector);
        }

        public static bool IsModulosAssembly(Assembly asm)
        {
            return asm.FullName.StartsWith(Selector) 
                   || asm.GetReferencedAssemblies().Any(e => e.FullName.StartsWith(Selector));
        }

        public static bool IsModulosAssembly(Library compilationLibrary)
        {
            return compilationLibrary.Name == Selector
                   || compilationLibrary.Dependencies.Any(d => d.Name.StartsWith(Selector));
        }

        public static AppInfo GetAppInfoFromAssembly(Assembly assembly)
        {
            return new AppInfo(new Guid(assembly.GetType().GUID.ToString()),
                assembly.GetName().Name,
                assembly.GetName().Version.ToString());
        }
    }
}