using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Modulos
{
    public abstract class ModulosServiceProviderFactoryBase<TBuilder> : IServiceProviderFactory<TBuilder>
    {
        private readonly ModulosApp _modulos;
        private readonly Func<IServiceCollection, TBuilder> _builderFactory;
        private readonly Action<AutoRegistrationModule> _modifier;
        private readonly IDictionary<Type, object> _parameters;

        protected ModulosServiceProviderFactoryBase(ModulosApp modulos, 
            Func<IServiceCollection,TBuilder> builderFactory,
            Action<AutoRegistrationModule> modifier = null, 
            params object[] parameters)
        {
            _modulos = modulos;
            _builderFactory = builderFactory;
            _modifier = modifier;
            _parameters = parameters?.ToDictionary
            (
                e=>e.GetType(),
                e=>e
            ) ?? new Dictionary<Type, object>();

            if (!_parameters.ContainsKey(typeof(IAppInfo)))
            {
                _parameters.Add(typeof(IAppInfo), modulos.AppInfo);
            }

            if (!_parameters.ContainsKey(typeof(Assembly[])))
            {
                _parameters.Add(typeof(Assembly[]), modulos.Assemblies);
            }

            if (!_parameters.ContainsKey(typeof(IAssemblyExplorer)))
            {
                var assemblyExplorer = new AssemblyExplorer
                (
                    (Assembly[])_parameters[typeof(Assembly[])]
                );

                _parameters.Add(typeof(IAssemblyExplorer), assemblyExplorer);
            }

            if (!_parameters.ContainsKey(typeof(ITypeExplorer)))
            {
                var assemblyExplorer = (IAssemblyExplorer)_parameters[typeof(IAssemblyExplorer)];
                _parameters.Add(typeof(ITypeExplorer),new TypeExplorer(assemblyExplorer));
            }
        }

        protected abstract void Populate(TBuilder builder, IServiceCollection collection);

        protected abstract IServiceProvider Build(TBuilder builder);

        public TBuilder CreateBuilder(IServiceCollection services)
        {
            services.AddSingleton(_modulos);

            var builder = _builderFactory(services);

            var assemblies = _modulos.Assemblies.ToArray();

            var autoLoadModules = GetModules(assemblies)
                .Select(e =>
                {
                    _modifier?.Invoke(e);
                    return e;
                })
                .Where(e => e.AutoLoad)
                .OrderBy(e => e.Order.GetHashCode())
                .ToArray();


            var microsoftModules = autoLoadModules.Where(e => e.Instance is IModule<IServiceCollection>)
                .OrderBy(e => e.Order.GetHashCode());

            foreach (var microsoft in microsoftModules)
            {
                var module = (IModule<IServiceCollection>) microsoft.Instance;
                module.Load(services);
            }

            Populate(builder, services);

            var customModules = autoLoadModules.Where(e => e.Instance is IModule<TBuilder>)
                .OrderBy(e => e.Order.GetHashCode());

            foreach (var custom in customModules)
            {
                var module = (IModule<TBuilder>) custom.Instance;
                module.Load(builder);
            }

            /*
            
            This code is better for ordering, but it suffers from cutting possibility 
            to use methods like TryAdd.

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
                               
                                //todo: consider to remove support for both modules in same time 
                                //now it's impossible to use TryAdd... ;/ 

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
            */

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
                var keyFromAdditional = _parameters.Keys
                    .FirstOrDefault(e => paramInfo.ParameterType.IsAssignableFrom(e));

                if (keyFromAdditional != null)
                {
                    var paramFromAdditional = _parameters[keyFromAdditional];

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