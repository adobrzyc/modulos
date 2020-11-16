using System;
using Modulos.Messaging.Compression;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Security;
using Modulos.Messaging.Serialization;

namespace Modulos.Messaging.Transport.InMemory
{
    public class InMemoryMessageConfigDefiner : IMessageConfigDefiner
    {
        private readonly IHandlersRegistry registry;

        public InMemoryMessageConfigDefiner(IHandlersRegistry registry)
        {
            this.registry = registry;
        }

        public LoadOrder Order { get; } = LoadOrder.Internal;

        public bool IsForThisMessage(IMessage message)
        {
            return registry.HasHandlerFor(message);
        }

        public void GetConfig(IMessage message, ref IMessageConfig config)
        {
            config.TransportEngine = InMemoryTransport.EngineId;
            config.EndpointConfigMark = new EndpointConfigMark(message.GetType(), InMemoryTransport.EngineId);

            config.AuthenticationMode = AuthenticationMode.None;
            config.RequestSerializer = Serializers.JsonNet;
            config.ResponseSerializer = Serializers.JsonNet;
            config.RequestCompressor = CompressionEngines.None;
            config.ResponseCompressor = CompressionEngines.None;
            config.Timeout = TimeSpan.FromSeconds(30);
            config.MaxInvokeAttempts = 1;
        }
    }
}