using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Modulos;
using Newtonsoft.Json;

// ReSharper disable PossibleNullReferenceException

namespace Examples.Blazor.WebAssembly
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            using (var http = new HttpClient
            {
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
            })
            {
                var json = await http.GetStringAsync("_framework/blazor.boot.json");
                dynamic doc = JsonConvert.DeserializeObject(json);

                foreach (Newtonsoft.Json.Linq.JProperty prop in doc.resources.assembly)
                {
                    var name = prop.Name.Replace(".dll", "");
                    if (ModulosApp.IsExcludedAssemblyName(name))
                        continue;

                    Assembly.Load(name);

                    Console.WriteLine("Assembly loaded = {0}", name);
                }
            }

            var modulosApp = new ModulosApp();
            modulosApp.Initialize<Program>();
            
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

            protected override IServiceProvider Build(IServiceCollection builder)
            {
                var sp = base.Build(builder);
                _modulos.Configure(sp);
                return sp;
            }
        }
    }
}
