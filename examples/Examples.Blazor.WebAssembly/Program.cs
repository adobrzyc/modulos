using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Modulos;

namespace Examples.Blazor.WebAssembly
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var modulosApp = new ModulosApp();
            modulosApp.Initialize<Program>();
            
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            });
            
            builder.ConfigureContainer(new FactoryForBlazor(modulosApp), collection => {});

            await builder.Build().RunAsync();
        }

        private class FactoryForBlazor : ModulosServiceProviderFactory
        {
            private readonly ModulosApp _modulos;

            public FactoryForBlazor(ModulosApp modulos, Action<AutoRegistrationModule> modifier = null,
                params object[] parameters) : base(modulos, modifier, parameters)
            {
                _modulos = modulos;
            }

            protected override IServiceProvider Build(FakeBuilder builder)
            {
                var sp = base.Build(builder);
                _modulos.Configure(sp);
                return sp;
            }
        }
    }
}
