using LZ4;

// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming

namespace Modulos.Messaging.Compression.Engines
{
    internal class LZ4CompressionEngine : ICompressionEngine
    {
        public CompressionEngineId Engine => CompressionEngines.LZ4;

        public byte[] Wrap(byte[] dataToCompress)
        {
            return LZ4Codec.Wrap(dataToCompress);
        }

        public byte[] Unwrap(byte[] compressedData)
        {
            return LZ4Codec.Unwrap(compressedData);
        }
    }
}