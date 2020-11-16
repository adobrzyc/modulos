using Modulos.Messaging.Configuration;

namespace Modulos.Messaging.Protocol.Request.Definitions
{
    public interface ICreatedRequestData : IRequestData
    {
        IMessageConfig MessageConfig { get; }
        IEndpointConfig EndpointConfig { get; }
    }
}