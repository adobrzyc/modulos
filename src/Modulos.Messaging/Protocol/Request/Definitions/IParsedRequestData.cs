using Modulos.Messaging.Transport;

namespace Modulos.Messaging.Protocol.Request.Definitions
{
    public interface IParsedRequestData : IRequestData
    {   
        ITransportEngine TransportEngine { get; }
    }
}