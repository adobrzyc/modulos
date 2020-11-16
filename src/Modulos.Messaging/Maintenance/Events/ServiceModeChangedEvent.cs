using Modulos.Messaging.EventBus;
using Modulos.Messaging.Hosting.Options;

namespace Modulos.Messaging.Maintenance.Events
{
    public class ServiceModeChangedEvent : IEvent
    {
        public ServiceModeChangedEvent(OperatingMode mode)
        {
            Mode = mode;
        }

        public OperatingMode Mode { get; private set; }
    }
}