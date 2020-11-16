// ReSharper disable UnusedTypeParameter

namespace Modulos.Messaging
{
    /// <summary>
    /// Defines CQRS query.
    /// </summary>
    /// <typeparam name="TResult">
    /// Result type.
    /// </typeparam>
    public interface IQuery<out TResult> : IQueryBase
    {    
    }
}