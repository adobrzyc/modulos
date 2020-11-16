using System.Net.Http;
using Modulos.Messaging.Configuration;

namespace Modulos.Messaging.Transport.Http
{
    public interface IClientFactory
    {
        LoadOrder Order { get; }
        bool IsMatch(IMessage message, IMessageConfig config);
        HttpClient CreateClient(IMessage message, IMessageConfig config);
    }
}