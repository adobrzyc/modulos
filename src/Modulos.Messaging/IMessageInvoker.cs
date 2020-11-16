using System;
using System.Threading;
using System.Threading.Tasks;
using Modulos.Messaging.Configuration;

namespace Modulos.Messaging
{
    public interface IMessageInvoker
    {
        Task<object> Send(IQueryBase query, Action<IMessageConfig> configModification, CancellationToken token, InvocationContext context);

        Task<TResult> Send<TResult>(IQuery<TResult> query, Action<IMessageConfig> configModification, CancellationToken token, InvocationContext context);

        Task<TResult> Send<TResult>(IQuery<TResult> query, Action<IMessageConfig> configModification, InvocationContext context);

        Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken token, InvocationContext context);

        Task<TResult> Send<TResult>(IQuery<TResult> query, InvocationContext context);


        Task Send(ICommandBase command, Action<IMessageConfig> configModification, CancellationToken token, InvocationContext context);

        Task Send(ICommandBase command, Action<IMessageConfig> configModification, InvocationContext context);

        Task Send(ICommandBase command, CancellationToken token, InvocationContext context);

        Task Send(ICommandBase command, InvocationContext context);


        Task<object> Send(IQueryBase query, Action<IMessageConfig> configModification, CancellationToken token);

        Task<TResult> Send<TResult>(IQuery<TResult> query, Action<IMessageConfig> configModification, CancellationToken token);

        Task<TResult> Send<TResult>(IQuery<TResult> query, Action<IMessageConfig> configModification);

        Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken token);

        Task<TResult> Send<TResult>(IQuery<TResult> query);
        

        Task Send(ICommandBase command, Action<IMessageConfig> configModification, CancellationToken token);

        Task Send(ICommandBase command, Action<IMessageConfig> configModification);

        Task Send(ICommandBase command, CancellationToken token);

        Task Send(ICommandBase command);
    }
}