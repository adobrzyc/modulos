using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modulos.Messaging.Diagnostics.Activity
{
    public sealed class OneOrManyActivityHandlerInvoker<T> : IOneOrManyActivityHandlerInvoker<T> where T:IActivity
    {
        private readonly IActivityHandler<T>[] handlers;
        private readonly IActivityHandler<T> notNullHandlerWhenOne;

        public OneOrManyActivityHandlerInvoker(IEnumerable<IActivityHandler<T>> handlers)
        {
            var array = handlers as IActivityHandler<T>[] ?? handlers.ToArray();
            Count = array.Length;
            this.handlers = array;
            notNullHandlerWhenOne = Count == 1 ? array[0] : null;
        }

        public int Count { get; }
   
        public IActivityHandler<T> GetOne()
        {
            return notNullHandlerWhenOne ?? throw new TodoException("Unable to retrieve one consumer.");
        }

        public IEnumerable<IActivityHandler<T>> GetAll()
        {
            return handlers;
        }

        public Task Invoke(T @event)
        {
            if (Count == 1)
                return notNullHandlerWhenOne.Handle(@event);

            var tasks = new List<Task>();

            foreach (var handler in handlers)
            {
                tasks.Add(handler.Handle(@event));
            }
        
            return Task.WhenAll(tasks);
        }
    }
}