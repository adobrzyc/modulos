using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Messaging.EventBus;
using Modulos.Messaging.Tests.Fixtures;
using Modulos.Messaging.Tests.Fixtures.Collections;
using Modulos.Testing;
using Xunit;

namespace Modulos.Messaging.Tests.Tests.Functional.DependencyInjection
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [Collection(nameof(InMemoryCollection))]
    public class RegisterEventsModule_Tests : IDisposable
    {
        private readonly ITest test;

        public RegisterEventsModule_Tests(ImMemoryEnv env)
        {
            test = env.CreateTest().GetAwaiter().GetResult();
        }

        public void Dispose()
        {
            test?.Dispose();
        }

        [Fact]
        public void check_auto_registration()
        {
            var handler1 = test.GetService<IHandleEvent<SampleEvent>>();
            var handler2 = test.GetService<IHandleEvent<SampleEvent>>();
            handler1.Should().NotBeNull();
            handler2.Should().NotBeNull();
            handler1.Should().NotBeSameAs(handler2);
        }

        [Fact]
        public void check_suppress_of_auto_registration()
        {
            var handler1 = test.GetService<IHandleEvent<SampleEvent2>>();
            handler1.Should().BeNull();
        }

        [Fact]
        public void check_auto_registration_with_options_singleton()
        {
            var handler1 = test.GetService<IHandleEvent<SampleEvent3>>();
            var handler2 = test.GetService<IHandleEvent<SampleEvent3>>();
            handler1.Should().NotBeNull();
            handler2.Should().NotBeNull();
            handler1.Should().BeSameAs(handler2);
        }
       
        [Fact]
        public void check_auto_registration_with_options_transient()
        {
            var handler1 = test.GetService<IHandleEvent<SampleEvent4>>();
            var handler2 = test.GetService<IHandleEvent<SampleEvent4>>();
            handler1.Should().NotBeNull();
            handler2.Should().NotBeNull();
            handler1.Should().NotBeSameAs(handler2);
        }

        [Fact]
        [SuppressMessage("ReSharper", "TooWideLocalVariableScope")]
        public void check_auto_registration_with_options_scoped()
        {
            IHandleEvent<SampleEvent5> handler1;
            IHandleEvent<SampleEvent5> handler2;

            using (var scope = test.CreateScope())
            {
                handler1 = scope.ServiceProvider.GetService<IHandleEvent<SampleEvent5>>();
                handler2 = scope.ServiceProvider.GetService<IHandleEvent<SampleEvent5>>();
                handler1.Should().NotBeNull();
                handler2.Should().NotBeNull();
                handler1.Should().BeSameAs(handler2);
            }

            var handler3 = test.ServiceProvider.GetService<IHandleEvent<SampleEvent5>>();

            handler2.Should().NotBeSameAs(handler3);
        }


        public class SampleEvent : IEvent
        {
        }

        public class SampleEvent2 : IEvent
        {
        }

        public class SampleEvent3 : IEvent
        {
        }
        
        public class SampleEvent4 : IEvent
        {
        }

        public class SampleEvent5 : IEvent
        {
        }

        public class SampleEventHandler : IHandleEvent<SampleEvent>
        {
            public Task Handle(SampleEvent @event)
            {
                return Task.CompletedTask;
            }
        }

        [SuppressAuto]
        public class NotAutoRegisteredEventHandler : IHandleEvent<SampleEvent2>
        {
            public Task Handle(SampleEvent2 @event)
            {
                return Task.CompletedTask;
            }
        }

        [AutoRegistrationOptions(ServiceLifetime.Singleton)]
        public class AutoRegisteredWithOptionsAsSingletonEventHandler : IHandleEvent<SampleEvent3>
        {
            public Task Handle(SampleEvent3 @event)
            {
                return Task.CompletedTask;
            }
        }
      
        [AutoRegistrationOptions(ServiceLifetime.Transient)]
        public class AutoRegisteredWithOptionsAsTransientEventHandler : IHandleEvent<SampleEvent4>
        {
            public Task Handle(SampleEvent4 @event)
            {
                return Task.CompletedTask;
            }
        }

        [AutoRegistrationOptions(ServiceLifetime.Scoped)]
        public class AutoRegisteredWithOptionsAsScopedEventHandler : IHandleEvent<SampleEvent5>
        {
            public Task Handle(SampleEvent5 @event)
            {
                return Task.CompletedTask;
            }
        }

    
    }

    
}