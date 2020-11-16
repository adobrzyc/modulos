using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Modulos.Autofac.Tests
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Add_modulos_via_AutofacModulosServiceProviderFactory 
    {
        [Fact]
        public Task execute()
        {
            using var env = new BuildTestEnv<Startup>();
            using var client = env.CreateClient();
            return Task.CompletedTask;
        }

       
        private class Startup
        {
            public void Configure(IApplicationBuilder app)
            {
                var pipelineParameters = new object[]
                {
                    app
                };

                var modulosApp = app.ApplicationServices.GetRequiredService<ModulosApp>();
                modulosApp.Configure(app.ApplicationServices, pipelineParameters);
            }
        }


        private class BuildTestEnv<TStartup> : WebApplicationFactory<TStartup>
            where TStartup : class
        {
            protected override IHost CreateHost(IHostBuilder builder)
            {
                //it's needed cuz of System.IO.DirectoryNotFoundException 
                //don't know why - just found on stackoverflow  
                builder.UseContentRoot(Directory.GetCurrentDirectory());
                return base.CreateHost(builder);
            }

            protected override IHostBuilder CreateHostBuilder()
            {
                var modulosApp = new ModulosApp();
                modulosApp.Initialize<TStartup>();

                var builder = Host.CreateDefaultBuilder()
                    .UseServiceProviderFactory(context => new AutofacModulosServiceProviderFactory(modulosApp, context))
                    .ConfigureWebHostDefaults(x =>
                    {
                        x.UseStartup<TStartup>();
                        //x.UseTestServer();
                    });
                return builder;
            }
        }
    }
}