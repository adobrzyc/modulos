using System.Net.Http;
using Modulos.Messaging.Configuration;

namespace Modulos.Messaging.Transport.Http
{
    public class DefaultClientFactory : IClientFactory
    {
        private readonly IHttpClientFactory httpClientFactory;

        public DefaultClientFactory(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public LoadOrder Order { get; } = LoadOrder.Internal;

        public bool IsMatch(IMessage message, IMessageConfig config)
        {
            return true;
        }

        public HttpClient CreateClient(IMessage message, IMessageConfig config)
        {
            var client = httpClientFactory.CreateClient();
            client.Timeout = config.Timeout;

            return client;
        }
    }
}