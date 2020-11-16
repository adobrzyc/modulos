using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Messaging.Tests.Fixtures;
using Modulos.Messaging.Tests.Fixtures.Collections;
using Modulos.Messaging.Tests.Fixtures.Domain;
using Xunit;
using ITest = Modulos.Testing.ITest;

namespace Modulos.Messaging.Tests.Tests.Functional.Messaging
{
    [Collection(nameof(InMemoryCollection))]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AnonymousTypes_tests : IDisposable
    {
        private readonly ITest test;

        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        public AnonymousTypes_tests(ImMemoryEnv env)
        {
            test = env.CreateTest().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            test.Dispose();
        }

        [Fact]
        public async Task anonymous_type_in_message_and_result()
        {
            var bus = test.GetRequiredService<IMessageInvoker>();
            var data = Utils.RandomString(10);
            var query = new ObjectQuery
            {
                Object = new
                {
                    Data = data
                }
            };
            dynamic result = await bus.Send(query);
            ((string) result.Data).Should().Be(data);
        }
    }
}