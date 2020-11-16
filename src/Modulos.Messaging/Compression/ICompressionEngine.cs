namespace Modulos.Messaging.Compression
{
    public interface ICompressionEngine
    {
        /// <summary>
        /// Compression engine identifier.
        /// </summary>
        CompressionEngineId Engine { get; }

        byte[] Wrap(byte[] dataToCompress);

        byte[] Unwrap(byte[] compressedData);
    }
}