using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Modulos.Pipes;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable ParameterOnlyUsedForPreconditionCheck.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Modulos
{
    public sealed partial class ModulosApp : IDisposable
    {
        public Assembly[] Assemblies
        {
            get 
            {
                lock (_locker)
                {
                    if (!_initialized)
                        throw new InvalidOperationException
                        (
                            $"Property: {nameof(Assemblies)}" +
                            " is available after initialization process."
                        );

                    return _assemblies;
                }
            }
            private set
            {
                lock (_locker)
                {
                    if (_initialized)
                        throw new InvalidOperationException
                        (
                            $"Property: {nameof(Assemblies)}" +
                            " can not be set after initialization is performed."
                        );

                    _assemblies = value;
                }
            }
        }

        public AppInfo AppInfo
        {
            get
            {
                lock (_locker)
                {
                    if (!_initialized)
                        throw new InvalidOperationException
                        (
                            $"Property: {nameof(AppInfo)}" +
                            " is available after initialization process."
                        );

                    return _appInfo;
                }
            }
        }

        private readonly object _locker = new object();
        private AppInfo _appInfo;
        private Assembly[] _assemblies;
        private Assembly[] _additionalAssemblies;
        private bool _initialized;
        private IPipeline _iniPipeline;
        private IPipeline _configPipeline;
        private IPipelineResult _iniResult;
        private IPipelineResult _configResult;

        public IPipelineResult Initialize<TClassInProject>(params object[] additionalParameters) where TClassInProject : class
        {
            return Initialize<TClassInProject>(pipeline => { }, additionalParameters);
        }

        public IPipelineResult Initialize<TClassInProject>(Action<IPipeline> updatePipeline, 
            params object[] additionalParameters) where TClassInProject : class
        {
            if (updatePipeline == null) throw new ArgumentNullException(nameof(updatePipeline));
            Assembly[] localAssemblies;
            lock (_locker)
            {
                if (_assemblies == null)
                    ExploreAssemblies();

                if (_additionalAssemblies != null)
                {
                    var list = _assemblies.ToList();
                    list.AddRange(_additionalAssemblies);
                    _assemblies = list.Distinct().ToArray();
                }
                    
                localAssemblies = _assemblies;
            }

            _appInfo = InitializationHelper.GetAppInfoFromAssembly(typeof(TClassInProject).Assembly);

            var typeExplorer = new TypeExplorer(new AssemblyExplorer(localAssemblies));
            
            using var serviceProvider = new ServiceCollection().BuildServiceProvider();
            {
                _iniPipeline = new Pipeline(serviceProvider);
                _iniPipeline.Add<Initialization.Begin>();

                var updaters = typeExplorer.GetSearchableClasses<IUpdateInitializationPipeline>()
                    .Select(Activator.CreateInstance)
                    .Cast<IUpdateInitializationPipeline>().ToArray();

                foreach (var updater in updaters)
                {
                    updater.Update(_iniPipeline);
                }

                updatePipeline.Invoke(_iniPipeline);


                var parameters = additionalParameters.ToList();
                if(!parameters.Any(e=>e is IAppInfo))
                    parameters.Add(_appInfo);
                if(!parameters.Any(e=>e is Assembly[]))
                    parameters.Add(localAssemblies);
                if(!parameters.Any(e=>e is ITypeExplorer))
                    parameters.Add(typeExplorer);
                if(!parameters.Any(e=>e is IAssemblyExplorer))
                    parameters.Add(new AssemblyExplorer(localAssemblies));

                _iniResult = _iniPipeline.Execute(CancellationToken.None, parameters.ToArray())
                    .GetAwaiter().GetResult();

                lock (_locker)
                {
                    _initialized = true;
                }

                return _iniResult;
            }
        }


        public IPipelineResult Configure(IServiceProvider serviceProvider,
            params object[] additionalParameters)
        {
            return Configure(serviceProvider, pipeline => { }, additionalParameters);
        }

        public IPipelineResult Configure(IServiceProvider serviceProvider,
            Action<IPipeline> updatePipeline, 
            params object[] additionalParameters)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (updatePipeline == null) throw new ArgumentNullException(nameof(updatePipeline));

            lock (_locker)
            {
                if (!_initialized)
                    throw new InvalidOperationException("Can not call this method before initialization.");
            }

            using var scope = serviceProvider.CreateScope();
            {
                _configPipeline = new Pipeline(scope.ServiceProvider);
                _configPipeline.Add<Configuration.Begin>();


                var typeExplorer = serviceProvider.GetRequiredService<ITypeExplorer>();
                var updaters = typeExplorer.GetSearchableClasses<IUpdateConfigPipeline>()
                    .Select(Activator.CreateInstance)
                    .Cast<IUpdateConfigPipeline>().ToArray();

                foreach (var updater in updaters)
                {
                    updater.Update(_configPipeline);
                }

                var parameters = additionalParameters.ToList();

                return _configResult = _configPipeline.Execute(CancellationToken.None, 
                        parameters.ToArray())
                    .GetAwaiter().GetResult();
            }
        }


        private void ExploreAssemblies(Predicate<Assembly> predicate = null)
        {
            lock (_locker)
            {
                if (_initialized)
                    throw new InvalidOperationException("Can not call this method after initialization.");
            }

            var result = new List<Assembly>();
            var dependencies = DependencyContext.Default?.RuntimeLibraries;

            if (dependencies != null)
            {
                foreach (var library in dependencies)
                {
                    if (!InitializationHelper.IsModulosAssembly(library)) continue;
                    foreach (var assemblyName in library.GetDefaultAssemblyNames(DependencyContext.Default))
                    {
                        var assembly = Assembly.Load(assemblyName);
                        if(predicate == null || predicate(assembly))
                            result.Add(assembly);
                    }
                }

                lock (_locker)
                {
                    _assemblies = result.ToArray();
                }
            }
            else
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(e => predicate == null || predicate(e)).ToArray();

                foreach (var library in assemblies)
                {
                    if (!InitializationHelper.IsModulosAssembly(library)) continue;

                    result.Add(library);
                }

                lock (_locker)
                {
                    _assemblies = result.ToArray();
                }
            }
        }

        /// <summary>
        /// Prevent Modulos from exploring assemblies by setting it manually.
        /// </summary>
        /// <param name="assemblies">Assemblies to set.</param>
        public void SetAssemblies(Assembly[] assemblies)
        {
            lock (_locker)
            {
                if (_initialized)
                    throw new InvalidOperationException("Can not call this method after initialization.");
            }

            lock (_locker)
            {
                _assemblies = assemblies;
            }
        }

        /// <summary>
        /// Adds additional assemblies to explored via modulos.
        /// </summary>
        /// <param name="assemblies">Assemblies to add.</param>
        public void AddAssemblies(Assembly[] assemblies)
        {
            lock (_locker)
            {
                if (_initialized)
                    throw new InvalidOperationException("Can not call this method after initialization.");
            }

            lock (_locker)
            {
                _additionalAssemblies = assemblies.ToArray();
            }
        }

        public void Dispose()
        {
            _iniResult?.DisposeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            _configResult?.DisposeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}