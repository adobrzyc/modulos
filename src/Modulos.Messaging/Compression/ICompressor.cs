namespace Modulos.Messaging.Compression
{
    internal interface ICompressor
    {
        byte[] Wrap(byte[] data, CompressionEngineId engine);
       
        byte[] Unwrap(byte[] data, CompressionEngineId engine);
    }
}