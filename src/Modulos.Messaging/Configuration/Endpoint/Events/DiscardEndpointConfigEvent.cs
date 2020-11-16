using Modulos.Messaging.EventBus;

namespace Modulos.Messaging.Configuration
{
    public class DiscardEndpointConfigEvent : IEvent
    {
        public DiscardEndpointConfigEvent(EndpointConfigMark endpointConfigMark)
        {
            EndpointConfigMark = endpointConfigMark;
        }

        public EndpointConfigMark EndpointConfigMark { get; }
    }
}