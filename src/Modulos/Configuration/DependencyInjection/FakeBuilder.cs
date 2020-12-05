using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Modulos
{
    /// <summary>
    /// 'Fake' builder used in pair with <see cref="ModulosServiceProviderFactory"/> to
    /// unify 'modulos' installation for .net core 3.1+
    /// </summary>
    public sealed class FakeBuilder 
    {
        private readonly IServiceCollection _collection;

        public FakeBuilder(IServiceCollection collection)
        {
            _collection = collection;
        }

        public void Populate(IServiceCollection source)
        {
            foreach (var serviceDescriptor in source.ToArray())
            {
                _collection.Add(serviceDescriptor);
            }
        }

        public IServiceProvider Build()
        {
            return _collection.BuildServiceProvider();
        }

    }
}