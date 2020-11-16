using Microsoft.Extensions.DependencyInjection;
using Modulos.Messaging.Compression.Engines;

// ReSharper disable UnusedType.Global

namespace Modulos.Messaging.Compression.Config
{
    public class CompressionDiModule : MicrosoftDiModule
    {
        public override LoadOrder Order { get; } = LoadOrder.Internal;
        public override bool AutoLoad { get; } = true;

        public override void Load(IServiceCollection collection)
        {
            collection.AddSingleton<ICompressor, Compressor>();
            collection.AddSingleton<ICompressionEngine, LZ4CompressionEngine>();
        }
    }
}