using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Testing;

// ReSharper disable ClassNeverInstantiated.Global

namespace Modulos.Messaging.Tests.Fixtures.Blocks
{
    public sealed class InitializeIoc : IBlock
    {
        private readonly ModulosApp modulos;

        public InitializeIoc(ModulosApp modulos)
        {
            this.modulos = modulos;
        }

        public Action<ModulosApp, IServiceCollection> RegisterServices { get; set; }

        public Task<BlockExecutionResult> Execute(ITestEnvironment testEnv)
        {
            var sc = new ServiceCollection();
            
            RegisterServices?.Invoke(modulos, sc);
            var sp = sc.BuildServiceProvider();
            
            testEnv.SetServiceProvider(sp);

            return Task.FromResult(BlockExecutionResult.EmptyContinue);
        }

        public Task ExecuteAtTheEnd(ITestEnvironment testEnv)
        {
            return Task.CompletedTask;
        }
    }
}