using System.Threading;
using System.Threading.Tasks;

// ReSharper disable UnusedType.Global

namespace Modulos.Messaging.Tests.Fixtures.Domain.Handlers
{
    public class PingCommandHandler : ICommandHandler<PingCommand>
    {
        public Task Handle(PingCommand command, InvocationContext invocationContext, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}