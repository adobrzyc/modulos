namespace Modulos.Messaging.Configuration
{
    public interface IMessageConfigProvider
    {
        IMessageConfig GetConfig(IMessage message);
    }
}