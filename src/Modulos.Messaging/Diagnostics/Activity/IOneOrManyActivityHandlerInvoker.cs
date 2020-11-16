using System.Collections.Generic;
using System.Threading.Tasks;

namespace Modulos.Messaging.Diagnostics.Activity
{
    public interface IOneOrManyActivityHandlerInvoker<in T> where T : IActivity
    {
        int Count { get; }
        IActivityHandler<T> GetOne();
        IEnumerable<IActivityHandler<T>> GetAll();
        Task Invoke(T @event);
    }
}