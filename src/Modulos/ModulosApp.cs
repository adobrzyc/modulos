using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    public partial class ModulosApp
    {
        public Assembly[] Assemblies
        {
            get 
            {
                lock (locker)
                {
                    if (!initialized)
                        throw new InvalidOperationException
                        (
                            $"Property: {nameof(Assemblies)}" +
                            " is available after initialization process."
                        );

                    return assemblies;
                }
            }
            private set
            {
                lock (locker)
                {
                    if (initialized)
                        throw new InvalidOperationException
                        (
                            $"Property: {nameof(Assemblies)}" +
                            " can not be set after initialization is performed."
                        );

                    assemblies = value;
                }
            }
        }

        public AppInfo AppInfo
        {
            get
            {
                lock (locker)
                {
                    if (!initialized)
                        throw new InvalidOperationException
                        (
                            $"Property: {nameof(AppInfo)}" +
                            " is available after initialization process."
                        );

                    return appInfo;
                }
            }
        }

        private readonly object locker = new object();
        private AppInfo appInfo;
        private Assembly[] assemblies;
        private bool initialized;
        private IPipeline iniPipeline;
        private IPipeline configPipeline;

        public IPipelineResult Initialize<TClassInProject>(params object[] additionalParameters) where TClassInProject : class
        {
            return Initialize<TClassInProject>(pipeline => { }, additionalParameters);
        }

        public IPipelineResult Initialize<TClassInProject>(Action<IPipeline> updatePipeline, 
            params object[] additionalParameters) where TClassInProject : class
        {
            if (updatePipeline == null) throw new ArgumentNullException(nameof(updatePipeline));
            Assembly[] localAssemblies;
            lock (locker)
            {
                if (assemblies == null)
                    UseNetCore();

                localAssemblies = assemblies;
            }

            appInfo = InitializationHelper.GetAppInfoFromAssembly(typeof(TClassInProject).Assembly);

            var typeExplorer = new TypeExplorer(new AssemblyExplorer(localAssemblies));
            var updaters = typeExplorer.GetSearchableClasses<IUpdateInitializationPipeline>()
                .Select(Activator.CreateInstance)
                .Cast<IUpdateInitializationPipeline>().ToArray();

            foreach (var updater in updaters)
            {
                updater.Update(iniPipeline);
            }

            updatePipeline.Invoke(iniPipeline);

            using var serviceProvider = new ServiceCollection().BuildServiceProvider();
            {    
                iniPipeline = new Pipeline(serviceProvider);
                iniPipeline.Add<Initialization.Begin>();

                var parameters = additionalParameters.ToList();
                parameters.Add(localAssemblies);
                parameters.Add(typeExplorer);
                parameters.Add(new AssemblyExplorer(localAssemblies));

                var result = iniPipeline.Execute(CancellationToken.None, parameters.ToArray())
                    .GetAwaiter().GetResult();

                lock (locker)
                {
                    initialized = true;
                }

                return result;
            }
        }


        public IPipelineResult Configure(IServiceProvider serviceProvider,
            params object[] additionalParameters)
        {
            return Configure(serviceProvider, pipeline => { }, additionalParameters);
        }

        public IPipelineResult Configure(IServiceProvider serviceProvider,
            Action<IPipeline> updatePipeline, params object[] additionalParameters)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            if (updatePipeline == null) throw new ArgumentNullException(nameof(updatePipeline));

            Assembly[] localAssemblies;
            lock (locker)
            {
                if (assemblies == null)
                    UseNetCore();

                localAssemblies = assemblies;
            }

            using var scope = serviceProvider.CreateScope();
            {
                configPipeline = new Pipeline(scope.ServiceProvider);
                configPipeline.Add<Configuration.Begin>();

                var typeExplorer = new TypeExplorer(new AssemblyExplorer(localAssemblies));
                var updaters = typeExplorer.GetSearchableClasses<IUpdateConfigPipeline>()
                    .Select(Activator.CreateInstance)
                    .Cast<IUpdateConfigPipeline>().ToArray();

                foreach (var updater in updaters)
                {
                    updater.Update(configPipeline);
                }

                var parameters = additionalParameters.ToList();
                parameters.Add(localAssemblies);
                parameters.Add(typeExplorer);
                parameters.Add(new AssemblyExplorer(localAssemblies));

                return configPipeline.Execute(CancellationToken.None, parameters.ToArray())
                    .GetAwaiter().GetResult();
            }
        }

        public void UseNetCore(Predicate<RuntimeLibrary> predicate = null)
        {
            lock (locker)
            {
                if (initialized)
                    throw new InvalidOperationException("Can not call this method after initialization.");
            }

            var watch = Stopwatch.StartNew();
            var result = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries
                .Where(e => predicate == null || predicate(e));

            foreach (var library in dependencies)
            {
                if (!InitializationHelper.IsModulosAssembly(library)) continue;

                foreach (var assemblyName in library.GetDefaultAssemblyNames(DependencyContext.Default))
                {
                    var assembly = Assembly.Load(assemblyName);
                    result.Add(assembly);
                }
            }

            watch.Stop();
            Debug.WriteLine($"Assemblies with modulos loaded in {watch.ElapsedMilliseconds} ms");

            lock (locker)
            {
                assemblies = result.ToArray();
            }
        }
    }
}