using System;
using System.Threading;
using System.Threading.Tasks;
using Modulos.Messaging.Configuration;

namespace Modulos.Messaging.Transport.Policy
{
    /// <summary>
    /// Defines policy (tactic/routine) for remote transport communication.
    /// </summary>
    public interface ITransportPolicy 
    {
        Task ExecuteCommand(ICommandBase command, Action<IMessageConfig> configModification, CancellationToken token, InvocationContext context);
        Task<TResult> ExecuteQuery<TResult>(IQueryBase query,  Action<IMessageConfig> configModification, CancellationToken token, InvocationContext context);
    }
}