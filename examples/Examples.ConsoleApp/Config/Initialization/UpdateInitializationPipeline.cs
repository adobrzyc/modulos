// ReSharper disable UnusedType.Global

namespace Examples.ConsoleApp.Config
{
    using Modulos;
    using Modulos.Pipes;

    public class UpdateInitializationPipeline : ModulosApp.IUpdateInitializationPipeline
    {
        public void Update(IPipeline pipeline)
        {
            pipeline.Add<PrepareConfiguration>();
            pipeline.Add<MakeSomeActionBaseOnConfiguration>();
        }
    }
}