using System;
using System.Diagnostics.CodeAnalysis;
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
    public class CheckExceptions_tests : IDisposable
    {
        private readonly ITest test;

        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        public CheckExceptions_tests(ImMemoryEnv env)
        {
            test = env.CreateTest().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            test.Dispose();
        }

        [Fact]
        public async void check_exceptions_UnknownHydraException()
        {
            try
            {
                var bus = test.GetRequiredService<IMessageInvoker>();
                var query = new SampleQuery
                {
                    Data = "",
                    ThrowException = true
                };

                await bus.Send(query);
            }
            catch (UnknownModulosException)
            {
                return;
            }

            throw new Exception($"Missing: {nameof(UnknownModulosException)}.");
        }
    }
}