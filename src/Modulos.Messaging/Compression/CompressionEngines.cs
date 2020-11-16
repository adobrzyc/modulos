using System.Diagnostics.CodeAnalysis;

namespace Modulos.Messaging.Compression
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class CompressionEngines
    {
        public static readonly CompressionEngineId None = new CompressionEngineId("none");
        public static readonly CompressionEngineId LZ4 = new CompressionEngineId("lz4");
    }
}