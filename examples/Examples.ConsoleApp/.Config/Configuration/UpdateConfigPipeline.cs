using Modulos;
using Modulos.Pipes;

// ReSharper disable UnusedType.Global

namespace Examples.ConsoleApp.Config
{
    public class UpdateConfigPipeline : ModulosApp.IUpdateConfigPipeline
    {
        public void Update(IPipeline pipeline)
        {
            pipeline.Add<ConfigureAppWhenInMemoryStorage>();
            pipeline.Add<ConfigureAppWhenFileStorage>();
        }
    }
}