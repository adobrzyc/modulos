namespace Modulos.Messaging.Configuration
{
    public interface IMessageConfigDefiner
    {
        LoadOrder Order { get; }
        bool IsForThisMessage(IMessage message);
        void GetConfig(IMessage message, ref IMessageConfig config);
    }
}