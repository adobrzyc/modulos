using System.Collections.Generic;
using System.Linq;

// ReSharper disable UnusedType.Global

namespace Modulos.Messaging.Maintenance
{
    public class InfrastructureProvider : IInfrastructureProvider
    {
        private readonly IEnumerable<IInstanceInfoSource> sources;

        public InfrastructureProvider(IEnumerable<IInstanceInfoSource> sources)
        {
            this.sources = sources;
        }

        public IEnumerable<IInstanceInfo> GetInstanceInformation()
        {
            return sources.SelectMany(e => e.Read());
        }
    }
}
