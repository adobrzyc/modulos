using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Modulos.Messaging.Tests.Fixtures;
using Modulos.Messaging.Tests.Fixtures.Collections;
using Modulos.Messaging.Tests.Fixtures.Domain;
using Modulos.Testing;
using Xunit;

namespace Modulos.Messaging.Tests.Tests.Functional.Messaging
{
    [Collection(nameof(InMemoryCollection))]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Simple_scenarios_tests
    {
        private readonly ImMemoryEnv env;

        public Simple_scenarios_tests(ImMemoryEnv env)
        {
            this.env = env;
        }

        [Fact]
        public async Task execute_PingCommand()
        {
            await using var test = await env.CreateTest<Test>();
            
            var bus = test.Resolve<IMessageInvoker>();
            await bus.Send(new PingCommand());
        }

        [Fact]
        public async Task execute_PingQuery()
        {
            await using var test = await env.CreateTest<Test>();
            var bus = test.Resolve<IMessageInvoker>();
            await bus.Send(new SampleQuery());
        }

        [Fact]
        public async Task execute_query_and_query_will_execute_another_query()
        {
            await using var test = await env.CreateTest<Test>();
            var bus = test.Resolve<IMessageInvoker>();
            var result = await bus.Send(new EnterChainQuery
            {
                Data = "1"
            });

            result.Should().Be("1->2");
        }
    }
}
