using System.Linq;
using FluentAssertions;
using Xunit;

namespace Modulos.Tests.Unit
{
    public class AssemblyExploringTests
    {
        [Fact]
        public void RegisterAssembliesForNetCoreApp_register_affected_by_adito_hydra_core()
        {
            var modulos = new ModulosApp();
            modulos.Initialize<AssemblyExploringTests>();

            var suspectedAssemblies = new[]
            {
                "Modulos.Tests",
                "Modulos"
            };

            modulos.Assemblies.Select(e => e.GetName().Name)
                .Should()
                .Contain(suspectedAssemblies);
        }
    }
}
