using System.Threading.Tasks;
using JetBrains.Annotations;
using Modulos.Testing;

namespace Modulos.Messaging.Tests.Fixtures.Blocks
{
    [UsedImplicitly]
    public sealed class ModulosAppInitialize : IBlock
    {
        public Task<BlockExecutionResult> Execute(ITestEnvironment testEnv)
        {
            var modulosApp= new ModulosApp();
            modulosApp.Initialize<ModulosAppInitialize>();

            return Task.FromResult(new BlockExecutionResult(ActionAfterBlock.Continue, modulosApp));
        }

        public Task ExecuteAtTheEnd(ITestEnvironment testEnv)
        {
            return Task.CompletedTask;
        }
    }
}