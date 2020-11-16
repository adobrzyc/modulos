using System.Threading;
using System.Threading.Tasks;

// ReSharper disable UnusedType.Global

namespace Modulos.Messaging.Tests.Fixtures.Domain.Handlers
{
    public class EnterChainQueryHandler : IQueryHandler<EnterChainQuery, string>
    {
        private readonly IMessageInvoker bus;

        public EnterChainQueryHandler(IMessageInvoker bus)
        {
            this.bus = bus;
        }

        public async Task<string> Handle(EnterChainQuery query, InvocationContext invocationContext, CancellationToken token)
        {
            var result = await bus.Send(new EndChainQuery
            {
                Data = query.Data
            }, token, invocationContext);

            return query.Data + "->" + result;
        }
    }
}