using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Messaging.OneOrManyExecutionRoutine;
using Modulos.Messaging.Tests.Fixtures;
using Modulos.Testing;
using Xunit;

namespace Modulos.Messaging.Tests.Tests.Functional.DependencyInjection
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class IOneOrManyExecutionRoutine_Registration_Tests 
    {
        [Fact]
        public async Task resolve_when_single_consumer()
        {
            await using var env = await new ImMemoryEnv().UpdateIoc(services =>
            {
                services.AddTransient<IConsumer, Consumer>();

            }).Build();

            await using var test = await env.CreateTest();

            var consumer = test.Resolve<IOneOrManyExecutionRoutine<IConsumer>>();
            consumer.Count.Should().Be(1);
            consumer.Get().Should().NotBeNull();
            consumer.GetAll().Should().HaveCount(1);
        }

        [Fact]
        public async Task resolve_when_multiple_consumers()
        {
            await using var env = await new ImMemoryEnv().UpdateIoc(services =>
            {
                services.AddTransient<IConsumer, Consumer>();
                services.AddTransient<IConsumer, Consumer>();
            }).Build();

            await using var test = await env.CreateTest();
            var consumer = test.Resolve<IOneOrManyExecutionRoutine<IConsumer>>();

            consumer.Count.Should().Be(2);
            consumer.GetAll().Should().HaveCount(2);
        }



        private interface IConsumer
        {
        }

        private class Consumer : IConsumer
        {

        }
    }
}