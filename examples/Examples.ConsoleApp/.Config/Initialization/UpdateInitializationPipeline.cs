using Modulos;
using Modulos.Pipes;

// ReSharper disable UnusedType.Global

namespace Examples.ConsoleApp.Config
{
    public class UpdateInitializationPipeline : ModulosApp.IUpdateInitializationPipeline
    {
        public void Update(IPipeline pipeline)
        {
            pipeline.Add<PrepareConfiguration>();
            pipeline.Add<MakeSomeActionBaseOnConfiguration>();
        }
    }
}