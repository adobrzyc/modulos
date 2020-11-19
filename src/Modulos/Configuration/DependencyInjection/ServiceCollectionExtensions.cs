using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Modulos
{
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add modulos to current solution.
        /// For ASP.NET Core 3.1+ it's recommended to use <see cref="DefaultModulosServiceProviderFactory"/>.
        /// </summary>
        /// <param name="services">IServiceCollection instance.</param>
        /// <param name="modulos">Instance of modulos application</param>
        /// <param name="modifier">
        /// Enables to redefine detected modules. For example, it may be used to
        /// load only particular modules in test environments. 
        /// </param>
        /// <param name="additionalData">Additional data available for modules.</param>
        public static IServiceCollection AddModulos(this IServiceCollection services, 
            ModulosApp modulos, 
            Action<AutoRegistrationModule> modifier = null, 
            params object[] additionalData)
        {
            var factory = new DefaultModulosServiceProviderFactory(modulos, services, modifier, additionalData);
            return factory.CreateBuilder(services).Collection;
        }

        /// <summary>
        /// Add modulos to current solution.
        /// For ASP.NET Core 3.1+ it's recommended to use <see cref="DefaultModulosServiceProviderFactory"/>.
        /// </summary>
        /// <param name="services">IServiceCollection instance.</param>
        /// <param name="modulos">Instance of modulos application</param>
        /// <param name="additionalData">Additional data available for modules.</param>
        public static IServiceCollection AddModulos(this IServiceCollection services, 
            ModulosApp modulos,
            params object[] additionalData)
        {
            var factory = new DefaultModulosServiceProviderFactory(modulos, services, null, additionalData);
            return factory.CreateBuilder(services).Collection;
        }




        public static IServiceCollection AddSingletonAsImplementedInterfacesAndSelf<T>(this IServiceCollection collection) where T:class
        {
            collection.AddSingleton<T,T>();
            foreach (var interfaceType in typeof(T).GetInterfaces())
            {
                collection.AddSingleton(interfaceType,provider => provider.GetRequiredService<T>());
            }

            return collection;
        }
        
        public static IServiceCollection AddSingletonAsImplementedInterfacesAndSelf(this IServiceCollection collection, object instance)
        {
            var type = instance.GetType();
            collection.AddSingleton(type,instance);
            foreach (var interfaceType in type.GetInterfaces())
            {
                collection.AddSingleton(interfaceType,provider => provider.GetRequiredService(type));
            }

            return collection;
        }
        
        public static IServiceCollection AddScopedAsImplementedInterfacesAndSelf<T>(this IServiceCollection collection) 
            where T:class
        {
            collection.AddScoped<T,T>();
            foreach (var interfaceType in typeof(T).GetInterfaces())
            {
                collection.AddScoped(interfaceType,provider => provider.GetRequiredService<T>());
            }

            return collection;
        }
        
        public static IServiceCollection AddScopedAsImplementedInterfacesAndSelf(this IServiceCollection collection, Type serviceType)
        {
            collection.AddScoped(serviceType);

            foreach (var interfaceType in serviceType.GetInterfaces())
            {
                collection.AddScoped(interfaceType, provider => provider.GetRequiredService(serviceType));
            }

            return collection;
        }
    }
}