using System.Collections.Generic;

// ReSharper disable UnusedMember.Global

namespace Modulos.Messaging.Maintenance
{
    public interface IInfrastructureProvider
    {
        IEnumerable<IInstanceInfo> GetInstanceInformation();
    }
}