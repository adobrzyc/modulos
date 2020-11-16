namespace Modulos.Messaging.Transport
{
    internal interface ITransportEngineProvider
    {
        ITransportEngine GetTransportEngine(TransportEngineId transportEngineId);
    }
}