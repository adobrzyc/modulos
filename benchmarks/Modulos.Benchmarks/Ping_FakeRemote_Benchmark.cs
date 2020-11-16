using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Benchmarks.Common;
using Modulos.Messaging;
using Modulos.Messaging.Config;
using Xunit;
using Xunit.Abstractions;

namespace Modulos.Benchmarks
{
    // ReSharper disable once InconsistentNaming
    public class Ping_FakeRemote_Benchmark
    {
        private readonly ITestOutputHelper console;

        public Ping_FakeRemote_Benchmark(ITestOutputHelper console)
        {
            this.console = console;
        }

        [Fact(Skip = "Only manually.")]
        public void Execute()
        {
            BenchmarkUtils.RunBenchmarkAndLogIntoXUnitConsole<Benchmark>(console);
        }

        [MemoryDiagnoser]
        public class Benchmark
        {
            private IServiceProvider serviceProvider;

            [GlobalSetup]
            public void Setup()
            {
                //0. prepare DI container
                IServiceCollection serviceCollection = new ServiceCollection();

                //1. explore assemblies 
                var modulosApp = new ModulosApp();
                modulosApp.Initialize<Benchmark>();

                //2. configure DI container 
                serviceCollection.AddModulos(modulosApp);
                serviceCollection.AddInMemoryTransport();

                serviceProvider = serviceCollection.BuildServiceProvider();
                var app = new ApplicationBuilder(serviceProvider);

                modulosApp.Configure(serviceProvider,app);
            }

            [Benchmark]
            public async Task SendPingCommand()
            {
                var bus = serviceProvider.GetRequiredService<IMessageInvoker>();
                await bus.Send(new PingCommand());
            }
        }
    }
}