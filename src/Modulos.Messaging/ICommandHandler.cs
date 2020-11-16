using System.Threading;
using System.Threading.Tasks;

namespace Modulos.Messaging
{
    public interface ICommandHandler<in TCommand> : IMessageHandler
        where TCommand : ICommand
    {
        Task Handle(TCommand command, InvocationContext invocationContext, CancellationToken token);
    }
}