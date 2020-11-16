using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modulos.Messaging.OneOrManyExecutionRoutine
{
    internal sealed class OneOrManyExecutionRoutine<T> : IOneOrManyExecutionRoutine<T> where T : class
    {
        private readonly T[] executors;
        private readonly T notNullExecutorWhenOne;

        public OneOrManyExecutionRoutine(IEnumerable<T> processors)
        {
            var array = processors as T[] ?? processors.ToArray();
            Count = array.Length;
            executors = array;
            notNullExecutorWhenOne = Count == 1 ? array[0] : null;
        }

        public int Count { get; }
   
        public T Get()
        {
            return notNullExecutorWhenOne ?? throw new TodoException("Unable to retrieve one consumer.");
        }

        public IEnumerable<T> GetAll()
        {
            return executors;
        }

        public Task Execute(Func<T,Task> consume)
        {
            if (Count == 1)
                return consume(notNullExecutorWhenOne);

            var tasks = new List<Task>();

            foreach (var consumer in executors)
            {
                tasks.Add(consume(consumer));
            }
        
            return Task.WhenAll(tasks);
        }
    }
}