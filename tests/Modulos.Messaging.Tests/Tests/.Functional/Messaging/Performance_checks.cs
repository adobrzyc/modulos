using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Messaging.Tests.Fixtures;
using Modulos.Messaging.Tests.Fixtures.Collections;
using Modulos.Messaging.Tests.Fixtures.Domain;
using Modulos.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Modulos.Messaging.Tests.Tests.Functional.Messaging
{
    [Collection(nameof(InMemoryCollection))]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Performance_checks
    {
        private readonly ImMemoryEnv env;
        private readonly ITestOutputHelper console;

        public Performance_checks(ImMemoryEnv env, ITestOutputHelper console)
        {
            this.env = env;
            this.console = console;
        }

 
        [Fact(Skip = "Only for manual execution.")]
        //[Fact]
        public async void MassiveSend_PingCommand()
        {
            await using var test = await env.CreateTest<Test>();
            {
                var sw = new Stopwatch();
                sw.Start();
                var count = 10000;
                var taskCount = 8;

                // ReSharper disable once ConvertToLocalFunction
                Func<Task> createTask = () =>
                {
                    return Task.Run(async () =>
                    {
                        for (var i = 0; i < count; i++)
                        {
                            // ReSharper disable once AccessToDisposedClosure
                            var bus = test.Resolve<IMessageInvoker>();
                            await bus.Send(new PingCommand());
                        }
                    });
                };
                var tasks = new List<Task>();

                for (var i = 0; i < taskCount; i++)
                {
                    tasks.Add(createTask());
                }

                await Task.WhenAll(tasks);

                sw.Stop();
                console.WriteLine((tasks.Count * count / (sw.ElapsedMilliseconds / 1000M)).ToString("n0") + " / sec");
            }
        }

        [Fact(Skip = "Only for manual execution.")]
        //[Fact]
        [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
        public async void MassiveSend_PingQuery()
        {
            await using var test = await env.CreateTest<Test>();
            {
                var sw = new Stopwatch();
                sw.Start();
                var count = 10000;
                var taskCount = 8;

                Task CreateTask()
                {
                    return Task.Run(async () =>
                    {
                        test.CreateScope();
                        for (var i = 0; i < count; i++)
                        {
                            var bus = test.Resolve<IMessageInvoker>();
                            await bus.Send(new PingQuery {Data = DateTime.Now.ToShortDateString()});
                        }
                    });
                }

                var tasks = new List<Task>();

                for (var i = 0; i < taskCount; i++)
                {
                    tasks.Add(CreateTask());
                }

                await Task.WhenAll(tasks);

                sw.Stop();
                console.WriteLine((tasks.Count * count / (sw.ElapsedMilliseconds / 1000M)).ToString("n0") + " / sec");
            }
        }

    }
}
