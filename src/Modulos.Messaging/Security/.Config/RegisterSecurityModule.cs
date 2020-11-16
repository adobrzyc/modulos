using Microsoft.Extensions.DependencyInjection;

// ReSharper disable UnusedType.Global

namespace Modulos.Messaging.Security.Config
{
    public class RegisterSecurityModule : MicrosoftDiModule
    {
        public override LoadOrder Order { get; } = LoadOrder.Internal;
        public override bool AutoLoad { get;} = true;

        public override void Load(IServiceCollection collection)
        {
            collection.AddScoped<ICurrentAuthenticationData, CurrentAuthenticationData>();
            collection.AddTransient<ICurrentPrincipal, CurrentPrincipal>();
        }
    }
}