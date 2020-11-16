using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Tests.Fixtures;
using Modulos.Messaging.Tests.Fixtures.Domain;
using Modulos.Messaging.Transport.Http;
using Modulos.Testing;
using Xunit;

namespace Modulos.Messaging.Tests.Tests.Integration
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Simple_scenarios_tests
    {
        [Fact]
        public async Task execute_query_and_query_will_execute_another_query()
        {
            var env = await new ImMemoryEnv().UpdateIoc(services =>
            {
                services.AddScoped<IClientFactory, FakeHttpClientFactory<Startup>>();
                services.AddSingleton<IEndpointConfigSource, FakeRemoteEndpointConfigSource>();

            }).Build();

            await using var test = await env.CreateTest<Test>();

            var bus = test.Resolve<IMessageInvoker>();
            var result = await bus.Send(new EnterChainQuery
            {
                Data = "1"
            });

            result.Should().Be("1->2");
        }

        public class Startup
        {
            private readonly ModulosApp modulos;

            public Startup()
            {
                modulos = new ModulosApp();
                modulos.Initialize<Startup>();
            }

            public void ConfigureServices(IServiceCollection services)
            {
                services.AddModulos(modulos);

                // it's very important to mark IClientFactory as Scoped, instead test is going
                // to run infinitely
                services.AddScoped<IClientFactory, FakeHttpClientFactory<Startup>>();
                services.AddSingleton<IEndpointConfigSource, FakeRemoteEndpointConfigSource>();
            }

            public void Configure(IApplicationBuilder app)
            {
                modulos.Configure(app.ApplicationServices, app);
            }
        }
    }
}