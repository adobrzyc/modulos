using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Modulos
{
    /// <summary>
    /// 'Fake' builder used in pair with <see cref="DefaultModulosServiceProviderFactory"/> to
    /// unify 'modulos' installation for .net core 3.1+
    /// </summary>
    public sealed class FakeBuilder 
    {
        public readonly IServiceCollection Collection;

        public FakeBuilder(IServiceCollection collection)
        {
            Collection = collection;
        }

        public void Populate(IServiceCollection source)
        {
            foreach (var serviceDescriptor in source.ToArray())
            {
                Collection.Add(serviceDescriptor);
            }
        }

        public IServiceProvider Build()
        {
            return Collection.BuildServiceProvider();
        }

    }
}