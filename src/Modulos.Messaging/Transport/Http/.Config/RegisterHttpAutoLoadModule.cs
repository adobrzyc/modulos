using Microsoft.Extensions.DependencyInjection;

namespace Modulos.Messaging.Transport.Http.Config
{
    public class RegisterHttpAutoLoadModule : MicrosoftDiModule
    {
        public override LoadOrder Order { get; } = LoadOrder.Internal;
        public override bool AutoLoad { get;} = true;

        public override void Load(IServiceCollection collection)
        {
            collection.AddHttpClient();

            collection.AddTransient<ITransportEngine, HttpTransport>();
            collection.AddTransient<IHttpContentReader, HttpContentReader>();
            collection.AddTransient<IHttpContentCreator, HttpContentCreator>();
            collection.AddTransient<IClientFactory, DefaultClientFactory>();
            collection.AddSingleton<IHttpEndpointConfiguration>(provider => new HttpEndpointConfiguration("modulos"));
        }
    }
}