using System.Threading;
using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Options;

// ReSharper disable UnusedType.Global

namespace Modulos.Messaging.Maintenance.Commands
{
    public class SetLogMarkCommandHandler : ICommandHandler<SetLogMarkCommand>
    {
        private readonly ILogMarkOption markOption;

        public SetLogMarkCommandHandler(ILogMarkOption markOption)
        {
            this.markOption = markOption;
        }

        public Task Handle(SetLogMarkCommand command, InvocationContext invocationContext, CancellationToken token)
        {
            markOption.Value = command.LogMark;
            return Task.CompletedTask;
        }
    }
}