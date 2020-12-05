using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modulos.Pipes
{
    public class PipelineResult : IPipelineResult
    {
        private readonly IDictionary<Type, object> _availableData = new Dictionary<Type, object>();

        public PipelineResult(IEnumerable<object> publishedData)
        {
            foreach (var data in publishedData)
            {
                _availableData.Add(data.GetType(), data);
            }
        }

        public T Get<T>()
        {
            var key = _availableData.Keys
                .SingleOrDefault(e => typeof(T).IsAssignableFrom(e));

            if (key == null)
               throw new MissingDataFromPipelineResultException($"Unable to resolve: {typeof(T).FullName} for pipe: {GetType().FullName}");

            return (T) _availableData[key];
        }

        public T GetOptional<T>()
        {
            var key = _availableData.Keys
                .SingleOrDefault(e => typeof(T).IsAssignableFrom(e));
       
            if (key == null)
                return default;
            return (T) _availableData[key];
        }

        public object[] GetAll()
        {
            return _availableData.Values.ToArray();
        }

        public async ValueTask DisposeAsync()
        {
            foreach (var pair in _availableData)
            {
                if (pair.Value is IAsyncDisposable asyncDisposable)
                    await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                else if(pair.Value is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            _availableData.Clear();
        }
    }
}