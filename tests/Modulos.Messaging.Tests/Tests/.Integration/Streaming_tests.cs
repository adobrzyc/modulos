using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Tests.Fixtures;
using Modulos.Messaging.Tests.Fixtures.Domain;
using Modulos.Messaging.Transport.Http;
using Modulos.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Modulos.Messaging.Tests.Tests.Integration
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Streaming_tests
    {
        private readonly ITestOutputHelper console;
        public Streaming_tests(ITestOutputHelper console)
        {
            this.console = console;
        }

        [Fact]
        public async Task ExecuteUploadAndDownloadQuery()
        {
            var env = await new ImMemoryEnv().UpdateIoc(services =>
            {
                services.AddScoped<IClientFactory, FakeHttpClientFactory<Startup>>();
                services.AddSingleton<IEndpointConfigSource, FakeRemoteEndpointConfigSource>();

            }).Build();

            await using var test = await env.CreateTest<Test>();

            var file = Utils.EvilPngPath;

            var bus = test.GetRequiredService<IMessageInvoker>();

            await using var stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
            {
                var query = new UploadAndDownloadQuery
                {
                    FileName = Path.GetFileName(file),
                    Stream = stream
                };

                var result = await bus.Send(query, config =>
                {
                    config.TransportEngine = HttpTransport.EngineId;
                    config.EndpointConfigMark = new EndpointConfigMark("test", HttpTransport.EngineId);
                });

                await using (result.Stream)
                {
                    var counter = 0;
                    var total = 0;
                    int bytesRead;
                    var buffer = new byte[1024];
                    while ((bytesRead = await result.Stream.ReadAsync(buffer)) > 0)
                    {
                        total += bytesRead;
                        counter++;
                    }
                    console.WriteLine($"file name: {result.FileName} with {total} bytes in {counter} steps.");
                }

            }
        }

        [Fact]
        public async Task ExecuteDownloadDataQuery()
        {
            var env = await new ImMemoryEnv().UpdateIoc(services =>
            {
                services.AddScoped<IClientFactory, FakeHttpClientFactory<Startup>>();
                services.AddSingleton<IEndpointConfigSource, FakeRemoteEndpointConfigSource>();

            }).Build();

            await using var test = await env.CreateTest<Test>();
            var bus = test.GetRequiredService<IMessageInvoker>();

            var query = new DownloadQuery
            {
                FileName = "file to download"
            };

            var result = await bus.Send(query, config =>
            {
                config.TransportEngine = HttpTransport.EngineId;
                config.EndpointConfigMark = new EndpointConfigMark("test", HttpTransport.EngineId);
            });

            await using (result.Stream)
            {
                var total = 0;
                int bytesRead;
                var buffer = new byte[1024];
                while ((bytesRead = await result.Stream.ReadAsync(buffer)) > 0)
                {
                    total += bytesRead;
                }

                console.WriteLine($"file name: {result.FileName} with {total} bytes");
            }
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

                // it's very important to mark IClientFactory as Scoped, instead test is going to run infinitely
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