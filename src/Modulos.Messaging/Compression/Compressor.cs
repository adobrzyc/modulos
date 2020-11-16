using System.Collections.Generic;
using System.Linq;

namespace Modulos.Messaging.Compression
{
    internal class Compressor : ICompressor
    {
        private readonly Dictionary<CompressionEngineId, ICompressionEngine> engines;
       
        public Compressor(IEnumerable<ICompressionEngine> engines)
        {
            this.engines = engines.ToDictionary(engine => engine.Engine, engine => engine);
        }

        public byte[] Wrap(byte[] data, CompressionEngineId engine)
        {
            return engine == CompressionEngines.None ? data : engines[engine].Wrap(data);
        }

        public byte[] Unwrap(byte[] data, CompressionEngineId engine)
        {        
            return engine == CompressionEngines.None ? data : engines[engine].Unwrap(data);
        }
    }
}