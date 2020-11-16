using System.Threading;
using System.Threading.Tasks;
using Modulos.Messaging.Hosting;

// ReSharper disable UnusedType.Global

namespace Modulos.Messaging.Maintenance.Queries
{
    public class GetServiceStateQueryHandler : IQueryHandler<GetServiceStateQuery, IServiceState>
    {
        private readonly IServiceStateProvider serviceStateProvider;

        public GetServiceStateQueryHandler(IServiceStateProvider serviceStateProvider)
        {
            this.serviceStateProvider = serviceStateProvider;
        }

        public Task<IServiceState> Handle(GetServiceStateQuery query, InvocationContext invocationContext, CancellationToken token)
        {
            var result =  serviceStateProvider.GetServiceState();
            return Task.FromResult(result);
        }
    }
}