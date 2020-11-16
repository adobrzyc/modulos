using System.Threading;
using System.Threading.Tasks;

// ReSharper disable UnusedType.Global

namespace Modulos.Messaging.Tests.Fixtures.Domain.Handlers
{
    public class ObjectQueryHandler : IQueryHandler<ObjectQuery, object>
    {
        public Task<object> Handle(ObjectQuery query, InvocationContext invocationContext, CancellationToken token)
        {
            dynamic obj = query.Object;


            object result = new
            {
                obj.Data
            };

            return Task.FromResult(result);
        }
    }
}