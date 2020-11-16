using Microsoft.Extensions.DependencyInjection;

// ReSharper disable UnusedType.Global

namespace Modulos.Pipes.Config
{
    public class PipesRegistrationModule  : MicrosoftDiModule
    {
        public override LoadOrder Order { get; } = LoadOrder.Internal;
        public override bool AutoLoad { get;} = true;

        public override void Load(IServiceCollection collection)
        {
            collection.AddTransient<IPipeline, Pipeline>();
        }
    }
}