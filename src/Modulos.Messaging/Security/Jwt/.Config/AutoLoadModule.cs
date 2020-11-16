using Microsoft.Extensions.DependencyInjection;

namespace Modulos.Messaging.Security.Jwt.Config
{
    public class AutoLoadModule : MicrosoftDiModule
    {
        public override LoadOrder Order { get; } = LoadOrder.Internal;
        public override bool AutoLoad { get;} = true;

        public override void Load(IServiceCollection collection)
        {
            collection.AddTransient<IPrincipalMapper, JwtPrincipalMapper>();

            collection.AddTransient<IAuthenticationHandler, JwtAuthenticationHandler>();
            collection.AddSingleton<JwtOptions, JwtOptions>();
        }
    }
}