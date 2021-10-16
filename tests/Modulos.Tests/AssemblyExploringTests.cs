namespace Modulos.Tests
{
    using System.Linq;
    using FluentAssertions;
    using Xunit;

    public class AssemblyExploringTests
    {
        [Fact]
        public void RegisterAssembliesForNetCoreApp_register_affected_by_adito_hydra_core()
        {
            var modulos = new ModulosApp();
            modulos.Initialize();

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