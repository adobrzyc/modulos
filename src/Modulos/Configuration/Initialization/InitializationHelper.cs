using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;

// ReSharper disable UnusedMember.Global

namespace Modulos
{
    internal static class InitializationHelper
    {
        private const string AditoSelector = "Modulos";

        public static bool IsModulosAssembly(AssemblyName asm)
        {
            return asm.FullName.StartsWith(AditoSelector);
        }

        public static bool IsModulosAssembly(Assembly asm)
        {
            return asm.FullName.StartsWith(AditoSelector) 
                   || asm.GetReferencedAssemblies().Any(e => e.FullName.StartsWith(AditoSelector));
        }

        public static bool IsModulosAssembly(Library compilationLibrary)
        {
            return compilationLibrary.Name == AditoSelector
                   || compilationLibrary.Dependencies.Any(d => d.Name.StartsWith(AditoSelector));
        }

        public static AppInfo GetAppInfoFromAssembly(Assembly assembly)
        {
            return new AppInfo(new Guid(assembly.GetType().GUID.ToString()),
                assembly.GetName().Name,
                assembly.GetName().Version.ToString());
        }
    }
}