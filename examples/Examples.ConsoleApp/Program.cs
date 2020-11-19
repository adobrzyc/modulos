using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Modulos;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Pipes;
using Console = System.Console;

// ReSharper disable UnusedType.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Examples.ConsoleApp
{
    class Program
    {
        static void Main()
        {
            // 1. initialize
            var modulosApp = new ModulosApp();
       
            modulosApp.UseNetCore();
            var iniResult = modulosApp.Initialize<Program>();


            // 2. organize dependency injection 
            var sc = new ServiceCollection();
            sc.AddModulos
            (
                modulosApp, 
                // data from initialization pipeline, will be available for DI containers
                iniResult.GetAll() 
            );
            var sp = sc.BuildServiceProvider();

            // 3. configure after dependency injection 
            modulosApp.Configure(sp);
        }
    }


    //
    // Initialization: it can be delivered event from external package
    // 
    public class UpdateInitializationPipeline : ModulosApp.IUpdateInitializationPipeline
    {
        public void Update(IPipeline pipeline)
        {
            pipeline.Add<PrepareConfiguration>();
            pipeline.Add<MakeSomeActionBaseOnConfiguration>();
        }
    }

    public class PrepareConfiguration : IPipe
    {
        public Task<PipeResult> Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine("PrepareConfiguration...");

            var builder = new ConfigurationBuilder();
            builder.Add(new MemoryConfigurationSource
            {
                InitialData = new []
                {
                    new KeyValuePair<string, string>("AppVersion","1.0.0"),
                    new KeyValuePair<string, string>("Storage","InMemory")
                }
            });
            var config = builder.Build();

            var result = new PipeResult(PipeActionAfterExecute.Continue, config);

            
            return Task.FromResult(result);
        }
    }

    public class MakeSomeActionBaseOnConfiguration : IPipe
    {
        // pipes can use previous pipes data 
        private readonly IConfiguration config;

        public MakeSomeActionBaseOnConfiguration(IConfiguration config)
        {
            this.config = config;
        }

        public Task<PipeResult> Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine("MakeSomeActionBaseOnConfiguration...");
            foreach (var pair in config.AsEnumerable())
            {
                Console.WriteLine(pair);
            }
            
            return Task.FromResult(PipeResult.Continue);
        }
    }

    //
    // DI modules
    // 
    public class RegisterStorageModule : MicrosoftDiModule
    {
        public override LoadOrder Order { get; } = LoadOrder.Project;
        public override bool AutoLoad { get; } = true;

        // config is available from initialization pipeline 
        private readonly IConfiguration config;

        public RegisterStorageModule(IConfiguration config)
        {
            this.config = config;
        }

        public override void Load(IServiceCollection services)
        {
            if (config["Storage"] == "InMemory")
            {
                services.AddTransient<IStorage, InMemoryStorage>();
                services.AddTransient<InMemoryStorage, InMemoryStorage>();
            }
            else
            {
                services.AddTransient<IStorage, FileStorage>();
                services.AddTransient<FileStorage, FileStorage>();
            }
        }
    }

    public interface IStorage {}

    public class InMemoryStorage : IStorage
    {
        public override string ToString()
        {
            return GetType().Name;
        }
    }

    public class FileStorage : IStorage
    {
        public override string ToString()
        {
            return GetType().Name;
        }
    }

    //
    // Configuration
    // 
    public class UpdateConfigPipeline : ModulosApp.IUpdateConfigPipeline
    {
        public void Update(IPipeline pipeline)
        {
            pipeline.Add<ConfigureAppWhenInMemoryStorage>();
            pipeline.Add<ConfigureAppWhenFileStorage>();
        }
    }

    // pipes can be optional, created and executed only if all params in ctor are available 
    public class ConfigureAppWhenInMemoryStorage : IOptionalPipe
    {
        private readonly InMemoryStorage storage;

        public ConfigureAppWhenInMemoryStorage(InMemoryStorage storage)
        {
            this.storage = storage;
        }

        public Task<PipeResult> Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{GetType().Name}...");
            Console.WriteLine(storage.ToString());
            return Task.FromResult(PipeResult.Continue);
        }
    }

    // this pipe will not be created either executed, because FileStorage is not available
    public class ConfigureAppWhenFileStorage : IOptionalPipe
    {
        private readonly FileStorage storage;

        public ConfigureAppWhenFileStorage(FileStorage storage)
        {
            this.storage = storage;
        }

        public Task<PipeResult> Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{GetType().Name}...");
            Console.WriteLine(storage.ToString());
            return Task.FromResult(PipeResult.Continue);
        }
    }




}
