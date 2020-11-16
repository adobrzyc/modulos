using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Transport.Policy;

/*
 * #design-concept: All measurements should be placed into lower layers. So MessageInvoker must not take
 *                  any action (itself) longer than almost nothing.
 *
 * #design-concept: MessageInvoker is just a proxy.
 *
 *
 * #abandoned-concept: MessageInvoker was something much bigger than now (just a proxy). It wasn't a good idea, mostly
 *                     because of problem with controlling measurements.
 *
 * #abandoned-concept: MessageInvoker should pass ICounterBag and at the end use IPerformanceMonitor to save counters.
 *                     #why: IActionInfo may be replaced in lower layer e.q. due to retry policy.
 */
namespace Modulos.Messaging
{
    [Verified]
    public sealed class MessageInvoker : IMessageInvoker, IActionHost
    {
        #region Fields
        private readonly ITransportPolicy transportPolicy;
        public string HostName { get; } = nameof(MessageInvoker);
        #endregion

        public MessageInvoker(ITransportPolicy transportPolicy)
        {
            this.transportPolicy = transportPolicy;
        }


        private async Task<TResult> Execute<TResult>(IMessage message,
            Action<IMessageConfig> configModification,
            [CanBeNull] InvocationContext invocationContext,
            CancellationToken token = default)
        {
            var result = default(TResult);

            if (message is IQueryBase query)
                result = await transportPolicy.ExecuteQuery<TResult>(query, configModification, token, invocationContext).ConfigureAwait(false);
            else await transportPolicy.ExecuteCommand((ICommandBase) message, configModification, token, invocationContext).ConfigureAwait(false);

            return result;
        }

        public Task<object> Send(IQueryBase query, Action<IMessageConfig> configModification, CancellationToken token, InvocationContext context)
        {
            return Execute<object>(query, configModification, invocationContext: context, token: token);
        }

        public Task<TResult> Send<TResult>(IQuery<TResult> query, Action<IMessageConfig> configModification, CancellationToken token, InvocationContext context)
        {
            return Execute<TResult>(query, configModification, invocationContext: context, token: token);
        }

        public Task<object> Send(IQueryBase query, Action<IMessageConfig> configModification, CancellationToken token)
        {
            return Execute<object>(query, configModification, invocationContext: null, token: token);
        }

        public Task<TResult> Send<TResult>(IQuery<TResult> query, Action<IMessageConfig> configModification, CancellationToken token)
        {
            return Execute<TResult>(query, configModification, invocationContext: null, token: token);
        }

        public Task<TResult> Send<TResult>(IQuery<TResult> query, Action<IMessageConfig> configModification, InvocationContext context)
        {
            return Execute<TResult>(query, configModification, invocationContext: context);
        }

        public Task<TResult> Send<TResult>(IQuery<TResult> query, Action<IMessageConfig> configModification)
        {
            return Execute<TResult>(query, configModification, invocationContext: null);
        }

        public Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken token, InvocationContext context)
        {
            return Execute<TResult>(query, configModification: null, invocationContext: context, token: token);
        }

        public Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken token)
        {
            return Execute<TResult>(query, configModification: null, invocationContext: null, token: token);
        }

        public Task<TResult> Send<TResult>(IQuery<TResult> query, InvocationContext context)
        {
            return Execute<TResult>(query, null, invocationContext: context);
        }

        public Task<TResult> Send<TResult>(IQuery<TResult> query)
        {
            return Execute<TResult>(query, configModification: null, invocationContext: null);
        }


        public Task Send(ICommandBase command, Action<IMessageConfig> configModification, CancellationToken token, InvocationContext context)
        {
            return Execute<object>(command, configModification, invocationContext: context, token: token);
        }

        public Task Send(ICommandBase command, Action<IMessageConfig> configModification, CancellationToken token)
        {
            return Execute<object>(command, configModification, invocationContext: null, token: token);
        }

        public Task Send(ICommandBase command, Action<IMessageConfig> configModification, InvocationContext context)
        {
            return Execute<object>(command, configModification, invocationContext: context);
        }

        public Task Send(ICommandBase command, Action<IMessageConfig> configModification)
        {
            return Execute<object>(command, configModification, invocationContext: null);
        }

        public Task Send(ICommandBase command, CancellationToken token, InvocationContext context)
        {
            return Execute<object>(command, configModification: null, invocationContext: context, token: token);
        }

        public Task Send(ICommandBase command, CancellationToken token)
        {
            return Execute<object>(command, configModification: null, invocationContext: null, token: token);
        }

        public Task Send(ICommandBase command, InvocationContext context)
        {
            return Execute<object>(command, configModification: null, invocationContext: context);
        }

        public Task Send(ICommandBase command)
        {
            return Execute<object>(command, configModification: null, invocationContext: null);
        }
    }
}
