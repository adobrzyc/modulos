using System.Threading;
using System.Threading.Tasks;

// ReSharper disable UnusedType.Global

namespace Modulos.Messaging.Tests.Fixtures.Domain.Handlers
{
    public class EndChainQueryHandler : IQueryHandler<EndChainQuery, string>
    {
        public Task<string> Handle(EndChainQuery query, InvocationContext invocationContext, CancellationToken token)
        {
            return Task.FromResult("2");
        }
    }
}