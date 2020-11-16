using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Modulos
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public abstract class ModulosServiceProviderFactory<TBuilder> : IServiceProviderFactory<TBuilder>
    {
        private readonly ModulosApp modulos;
        private readonly Func<TBuilder> builderFactory;
        private readonly Action<AutoRegistrationModule> modifier;
        private readonly IDictionary<Type, object> parameters;

        protected ModulosServiceProviderFactory(ModulosApp modulos, 
            Func<TBuilder> builderFactory,
            Action<AutoRegistrationModule> modifier = null, 
            params object[] parameters)
        {
            this.modulos = modulos;
            this.builderFactory = builderFactory ?? throw new ArgumentNullException(nameof(builderFactory));
            this.modifier = modifier;
            this.parameters = parameters?.ToDictionary
            (
                e=>e.GetType(),
                e=>e
            ) ?? new Dictionary<Type, object>();

            if (!this.parameters.ContainsKey(typeof(IAppInfo)))
            {
                this.parameters.Add(typeof(IAppInfo), modulos.AppInfo);
            }

            if (!this.parameters.ContainsKey(typeof(Assembly[])))
            {
                this.parameters.Add(typeof(Assembly[]), modulos.Assemblies);
            }

            if (!this.parameters.ContainsKey(typeof(IAssemblyExplorer)))
            {
                var assemblyExplorer = new AssemblyExplorer
                (
                    (Assembly[])this.parameters[typeof(Assembly[])]
                );

                this.parameters.Add(typeof(IAssemblyExplorer), assemblyExplorer);
            }

            if (!this.parameters.ContainsKey(typeof(ITypeExplorer)))
            {
                var assemblyExplorer = (IAssemblyExplorer)this.parameters[typeof(IAssemblyExplorer)];
                this.parameters.Add(typeof(ITypeExplorer),new TypeExplorer(assemblyExplorer));
            }
        }

        protected abstract void Populate(TBuilder builder, IServiceCollection collection);

        protected abstract IServiceProvider Build(TBuilder builder);

        public TBuilder CreateBuilder(IServiceCollection services)
        {
            var builder = builderFactory();

            services.AddSingleton(modulos);
            Populate(builder,services);

            var assemblies = modulos.Assemblies.ToArray();

            var autoLoadModules = GetModules(assemblies)
                .Select(e =>
                {
                    modifier?.Invoke(e);
                    return e;
                })
                .Where(e => e.AutoLoad)
                .OrderBy(e => e.Order.GetHashCode())
                .ToArray();

            foreach (var group in autoLoadModules.GroupBy(e => e.Order)
                .OrderBy(e => e.Key.GetHashCode()))
            {
                var orderedModules = group.GroupBy(e => e.Instance.ModuleOrder)
                    .OrderBy(e=>e.Key);
                    
                foreach (var modules in orderedModules)
                {
                    foreach (var module in modules)
                    {
                        switch (module.Instance)
                        {
                            case IModule<IServiceCollection> microsoft:
                          
                                var collection = new ServiceCollection();
                                microsoft.Load(collection);
                                Populate(builder, collection);
                                break;

                            case IModule<TBuilder> custom:
                                custom.Load(builder);
                                break;
                            default:
                                throw new NotSupportedException($"Module: {module.Instance.GetType().FullName} is not supported.");
                        }
                    }
                }
            }

            return builder;
        }

        public IServiceProvider CreateServiceProvider(TBuilder containerBuilder)
        {
            return Build(containerBuilder);
        }

        private IEnumerable<AutoRegistrationModule> GetModules(IEnumerable<Assembly> assemblies)
        {
            var assembliesArray = assemblies as Assembly[] ?? assemblies.ToArray();
            var typeExplorer = new TypeExplorer(new AssemblyExplorer(assembliesArray));

            var modules = typeExplorer.GetSearchableClasses<IModule>()
                .Select(CreateAutoLoadModule)
                .ToArray();

            return modules;
        }

        private AutoRegistrationModule CreateAutoLoadModule(Type moduleType)
        {
            var ctor = moduleType.GetConstructors()
                .Select(e => (ctor: e, count: e.GetParameters().Length))
                .OrderByDescending(e => e.count)
                .Select(e => e.ctor)
                .First();

            var args = new List<object>();
            foreach (var paramInfo in ctor.GetParameters())
            {
                var keyFromAdditional = parameters.Keys
                    .FirstOrDefault(e => paramInfo.ParameterType.IsAssignableFrom(e));

                if (keyFromAdditional != null)
                {
                    var paramFromAdditional = parameters[keyFromAdditional];

                    if (paramFromAdditional != null)
                    {
                        args.Add(paramFromAdditional);
                        continue;
                    }
                }
                if ((paramInfo.Attributes & ParameterAttributes.Optional) != 0)
                    args.Add(paramInfo.ParameterType.GetDefault());
                else
                {
                    throw new ApplicationException(
                        $"Unable to create parameter: {paramInfo.ParameterType.FullName} for {moduleType.FullName} ctor.");
                }
              
            }
            var instance = (IModule)Activator.CreateInstance(moduleType, args.ToArray());

            return new AutoRegistrationModule(instance);
        }

    }
}