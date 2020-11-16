using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Builder.Internal;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Benchmarks.Common;
using Modulos.Benchmarks.FakeRemote;
using Modulos.Messaging;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Protocol.Request.Definitions;
using Modulos.Messaging.Security;
using Xunit;
using Xunit.Abstractions;

namespace Modulos.Benchmarks
{
    // ReSharper disable once InconsistentNaming
    public class Protocol_CreateRequest_Benchmark
    {
        private readonly ITestOutputHelper console;
 
        public Protocol_CreateRequest_Benchmark(ITestOutputHelper console)
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
            private readonly PingCommand message = new PingCommand();
            private readonly FakeRemoteEndpointConfig fakeRemoteEndpointConfig = new FakeRemoteEndpointConfig();

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

                serviceProvider = serviceCollection.BuildServiceProvider();
                var app = new ApplicationBuilder(serviceProvider);

                //3. initialize hydra app
                modulosApp.Configure(serviceProvider,app);
            }

           
            [Benchmark]
            public async Task CreateRequest()
            {
                var counterBag = new MetricBag();

                //var message = new PingCommand();
                //var objInfo = new ObjectInfo(message);
                var messageConfigProvider = serviceProvider.GetRequiredService<IMessageConfigProvider>();
                
                //129.9 ns | 1.141 ns | 1.067 ns |       0 B |
                var messageConfig = messageConfigProvider.GetConfig(message);
                // 457.0 ns | 5.855 ns | 5.477 ns | 0.0505 |     160 B |

                var actionInfoFactory = serviceProvider.GetRequiredService<IActionInfoFactory>();
                //var hydraContextProvider = _serviceProvider.GetService<IHydraContextProvider>();
                //var hydraContext = hydraContextProvider.New().Result;
                var newAction = actionInfoFactory.Create(message, "test");
                var hydraContext = new InvocationContext(newAction);
                // 5.785 us | 0.0570 us | 0.0505 us | 0.5035 |   1.56 KB 

                var protocol = serviceProvider.GetRequiredService<ICreateRequest>();

                await protocol.Create(message: message,
                    messageConfig: messageConfig,
                    endpointConfig: fakeRemoteEndpointConfig, 
                    metricBag: counterBag,
                    invocationContext: hydraContext,
                    AnonymousAuthenticationData.Instance );

            }

        }
    }
}