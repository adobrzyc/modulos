// ReSharper disable UnusedType.Global

namespace Examples.ConsoleApp.Config
{
    using Modulos;
    using Modulos.Pipes;

    public class UpdateConfigPipeline : ModulosApp.IUpdateConfigPipeline
    {
        public void Update(IPipeline pipeline)
        {
            pipeline.Add<ConfigureAppWhenInMemoryStorage>();
            pipeline.Add<ConfigureAppWhenFileStorage>();
        }
    }
}