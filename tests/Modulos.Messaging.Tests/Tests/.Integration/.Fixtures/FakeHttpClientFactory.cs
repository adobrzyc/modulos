using System;
using System.Net.Http;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Transport.Http;

namespace Modulos.Messaging.Tests.Tests.Integration
{
    public class FakeHttpClientFactory<TStartup> : IClientFactory, IDisposable
        where TStartup: class
    {
        private readonly NetCoreFakeServer<TStartup> server;
            
        public FakeHttpClientFactory()
        {
            server = new NetCoreFakeServer<TStartup>();
        }

        public LoadOrder Order { get; } = LoadOrder.Test;
      
        public bool IsMatch(IMessage message, IMessageConfig config)
        {
            return true;
        }

        public HttpClient CreateClient(IMessage message, IMessageConfig config)
        {
            return server.CreateClient();
        }

        public void Dispose()
        {
            server.Dispose();
        }
    }
}