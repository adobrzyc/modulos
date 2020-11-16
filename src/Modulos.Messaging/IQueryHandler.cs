using System.Threading;
using System.Threading.Tasks;

namespace Modulos.Messaging
{
    /// <summary>
    /// Defines CQRS query handler.
    /// </summary>
    /// <typeparam name="TQuery">
    /// Query type.
    /// </typeparam>
    /// <typeparam name="TResult">
    /// Result type.
    /// </typeparam>
    public interface IQueryHandler<in TQuery, TResult> : IMessageHandler
        where TQuery : IQuery<TResult>
    {
        /// <summary>
        /// Handles query.
        /// </summary>
        /// <param name="query">
        /// Query to handle.
        /// </param>
        /// <param name="invocationContext">
        /// Hydra context.
        /// </param>
        /// <param name="token">
        /// Token used to eventually cancel operation.
        /// </param>
        /// <returns>
        /// Handle result.
        /// </returns>
        Task<TResult> Handle(TQuery query, InvocationContext invocationContext, CancellationToken token);
    }
}