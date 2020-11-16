using System;
using System.Net.Http;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

namespace Modulos.Messaging.Tests.Tests.Integration
{
    public class NetCoreFakeServer<TStartup> : IDisposable  
        where TStartup: class 
    {
        private HttpClient client;

        public readonly BuildTestEnv<TStartup> Server;
        public HttpClient Client => client ??= Server.CreateClient();
        public HttpClient CreateClient() => Server.CreateClient();

        public NetCoreFakeServer()
        {
            Server = new BuildTestEnv<TStartup>();
        }

        public void Dispose()
        {
            client?.Dispose();
            Server?.Dispose();
        }
    }
}