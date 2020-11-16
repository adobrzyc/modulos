using System.Collections.Generic;
using System.Linq;

namespace Modulos.Messaging.Transport
{
    internal class TransportEngineProvider : ITransportEngineProvider
    {
        private readonly Dictionary<string, ITransportEngine> engines;

        public TransportEngineProvider(IEnumerable<ITransportEngine> engines)
        {
            this.engines = engines.ToDictionary
            (
                e => e.EngineId.Value, 
                layer => layer
            );
        }

        public ITransportEngine GetTransportEngine(TransportEngineId transportEngineId)
        {
            if(!engines.TryGetValue(transportEngineId.Value, out var layer))
                throw new TodoException($"Unable to define transport layer {transportEngineId.Value}");
            return layer;
        }
    }
}