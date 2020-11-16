using System.Threading.Tasks;
using Modulos.Messaging.Config;
using Modulos.Messaging.Tests.Fixtures.Blocks;
using Modulos.Testing;
using Xunit;

namespace Modulos.Messaging.Tests.Fixtures
{
    public class ImMemoryEnv : TestEnvironment, IAsyncLifetime
    {
        public ImMemoryEnv()
        {
            Add<ModulosAppInitialize>();

            Add<InitializeIoc>((block, env) =>
            {
                block.RegisterServices = (modulos, services) =>
                {
                    services.AddModulos(modulos);
                    services.AddInMemoryTransport();
                };
            });

            Add<ModulosAppConfigure>();
        }

        public async Task InitializeAsync()
        {
            await Build();
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await DisposeAsync();
        }
    }
}