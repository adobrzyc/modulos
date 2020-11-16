using System;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Security;
using Modulos.Messaging.Security.Exceptions;
using Modulos.Messaging.Security.Jwt;
using Modulos.Messaging.Tests.Fixtures;
using Modulos.Messaging.Tests.Fixtures.Domain;
using Modulos.Messaging.Transport.Http;
using Modulos.Testing;
using Xunit;

namespace Modulos.Messaging.Tests.Tests.Integration
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Authentication_tests
    {
        [Fact]
        public async Task execute_AuthCommand_with_jwt_token_env()
        {
            var env = await new ImMemoryEnv().UpdateIoc(services =>
            {
                services.AddScoped<IClientFactory, FakeHttpClientFactory<Startup>>();
                services.AddSingleton<IEndpointConfigSource, FakeRemoteEndpointConfigSource>();
                services.AddTransient<IMessageConfigDefiner, ConfigDefinerForAuthenticatedMessages>();

            }).Build();
            
            await using var test = await env.CreateTest<Test>();
            {
                var token = CreateToken();
                var authData =  new JwtAuthenticationData(token);
                var currentAuthData = test.Resolve<ICurrentAuthenticationData>();
                await currentAuthData.Set(authData);

                var bus = test.Resolve<IMessageInvoker>();
                var message = new AuthCommand();

                await bus.Send(message);
            }
        }

        [Fact]
        public async Task execute_AuthCommand_with_not_set_authentication_data()
        {
            var env = await new ImMemoryEnv().UpdateIoc(services =>
            {
                services.AddScoped<IClientFactory, FakeHttpClientFactory<Startup>>();
                services.AddSingleton<IEndpointConfigSource, FakeRemoteEndpointConfigSource>();
                services.AddTransient<IMessageConfigDefiner, ConfigDefinerForAuthenticatedMessages>();

            }).Build();
            
            
            await using var test = await env.CreateTest<Test>();
            {
                try
                {
                    var bus = test.Resolve<IMessageInvoker>();
                    var message = new AuthCommand();

                    await bus.Send(message);
                }
                catch (AnonymousAccessIsNotAllowedException)
                {
                    return;
                }

                throw new Exception("Missing: AnonymousAccessIsNotAllowedException.");
            }
        }

        private static JwtSecurityToken CreateToken()
        {
            var expiration = DateTime.UtcNow.AddDays(1);
            var issuer = "test.issuer";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("test.key.12987312873198273"));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, "test.UniqueName"),
                new Claim(JwtRegisteredClaimNames.GivenName, "test.name"),
                new Claim(JwtRegisteredClaimNames.Email, "test.email")
            };

            var token = new JwtSecurityToken(issuer,
                issuer,
                claims,
                expires: expiration, signingCredentials: signingCredentials
                );

            return token;
        }

        private class Startup
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
                
                services.AddSingleton(new JwtOptions
                {
                    ValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("test.key.12987312873198273"))
                    }
                });

                // it's very important to mark IClientFactory as Scoped, instead test is going
                // to run infinitely
                services.AddScoped<IClientFactory, FakeHttpClientFactory<Startup>>();
                services.AddSingleton<IEndpointConfigSource, FakeRemoteEndpointConfigSource>();
                services.AddTransient<IMessageConfigDefiner, ConfigDefinerForAuthenticatedMessages>();
            }

            public void Configure(IApplicationBuilder app)
            {
                modulos.Configure(app.ApplicationServices, app);
            }
        }

        private class ConfigDefinerForAuthenticatedMessages : IMessageConfigDefiner
        {
            public LoadOrder Order { get; } = LoadOrder.Test;

            public bool IsForThisMessage(IMessage message)
            {
                return message is AuthCommand;
            }

            public void GetConfig(IMessage message, ref IMessageConfig config)
            {
                config.AuthenticationMode = AuthenticationMode.Required;
                config.TransportEngine = HttpTransport.EngineId;
                config.EndpointConfigMark = new EndpointConfigMark("test", HttpTransport.EngineId);
            }
        }
    }
}