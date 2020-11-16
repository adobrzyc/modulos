using System.Threading;
using System.Threading.Tasks;

// ReSharper disable UnusedType.Global

namespace Modulos.Messaging.Tests.Fixtures.Domain.Handlers
{
    public class PingQueryHandler : IQueryHandler<PingQuery, string>
    {
        public Task<string> Handle(PingQuery query, InvocationContext invocationContext, CancellationToken token)
        {
            return Task.FromResult(query.Data);
        }
    }
}