using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Modulos.Messaging;

namespace Modulos.Benchmarks.Common
{
    [UsedImplicitly]
    public class PingCommandHandler : ICommandHandler<PingCommand>
    {
        public Task Handle(PingCommand command, InvocationContext invocationContext, CancellationToken token)
        {
            return Task.CompletedTask;
        }
    }
}