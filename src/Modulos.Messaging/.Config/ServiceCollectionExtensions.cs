using Microsoft.Extensions.DependencyInjection;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Transport;
using Modulos.Messaging.Transport.InMemory;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace Modulos.Messaging.Config
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds in memory transport to modulos messaging.
        /// </summary>
        public static IServiceCollection AddInMemoryTransport(this IServiceCollection services)
        {
            services.AddTransient<ITransportEngine, InMemoryTransport>();
            services.AddTransient<IMessageConfigDefiner, InMemoryMessageConfigDefiner>();
            services.AddSingleton<IEndpointConfigSource, InMemoryTransport.EndpointConfigSource>();
            return services;
        }
    }
}