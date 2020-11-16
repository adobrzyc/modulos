using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Modulos.Tests.Unit
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AssemblyExploringTests
    {
        [Fact]
        public void RegisterAssembliesForNetCoreApp_register_affected_by_adito_hydra_core()
        {
            var hydra = new ModulosApp();
            hydra.Initialize<AssemblyExploringTests>();

            var suspectedAssemblies = new[]
            {
                "Modulos.Tests",
                "Modulos"
            };

            hydra.Assemblies.Select(e => e.GetName().Name)
                .Should()
                .Contain(suspectedAssemblies);
        }
    }
}
