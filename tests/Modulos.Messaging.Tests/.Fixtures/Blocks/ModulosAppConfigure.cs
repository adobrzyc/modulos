using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Modulos.Testing;

namespace Modulos.Messaging.Tests.Fixtures.Blocks
{
    [UsedImplicitly]
    public class ModulosAppConfigure : IBlock
    {
        private readonly ModulosApp modulos;
        private readonly IServiceProvider serviceProvider;

        public ModulosAppConfigure(ModulosApp modulos, IServiceProvider serviceProvider)
        {
            this.modulos = modulos;
            this.serviceProvider = serviceProvider;
        }

        public Task<BlockExecutionResult> Execute(ITestEnvironment testEnv)
        {  
            var builder = new ApplicationBuilder(testEnv.GetServiceProvider());
            modulos.Configure(serviceProvider,builder);
            return Task.FromResult(BlockExecutionResult.EmptyContinue);
        }

        public Task ExecuteAtTheEnd(ITestEnvironment testEnv)
        {
            return Task.CompletedTask;
        }
    }
}