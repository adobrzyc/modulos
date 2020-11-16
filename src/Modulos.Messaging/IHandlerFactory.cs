namespace Modulos.Messaging
{
    public interface IHandlerFactory
    {
        object GetHandler(IMessage message);
    }
}