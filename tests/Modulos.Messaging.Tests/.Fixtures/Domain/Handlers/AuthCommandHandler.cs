using System.Threading;
using System.Threading.Tasks;
using Modulos.Messaging.Security;
using Modulos.Messaging.Security.Jwt;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedVariable

namespace Modulos.Messaging.Tests.Fixtures.Domain.Handlers
{
    public class AuthCommandHandler : ICommandHandler<AuthCommand>
    {
        private readonly ICurrentPrincipal currentPrincipal;

        public AuthCommandHandler(ICurrentPrincipal currentPrincipal)
        {
            this.currentPrincipal = currentPrincipal;
        }

        public async Task Handle(AuthCommand command, InvocationContext invocationContext, CancellationToken token)
        {
            // ReSharper disable once UnusedVariable
            var defaultPrincipal = await currentPrincipal.Get();
            var jwt = await currentPrincipal.Get<JwtUser>();
        }
    }
}