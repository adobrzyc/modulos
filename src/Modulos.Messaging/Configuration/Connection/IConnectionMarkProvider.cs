namespace Modulos.Messaging.Configuration
{
    public interface IConnectionMarkProvider
    {
        ConnectionMark Get(EndpointConfigMark endpointConfigMark);
        void Refresh(EndpointConfigMark endpointConfigMark);
    }
}