using System.Threading.Tasks;

namespace Modulos.Messaging.Security
{
    public interface IAuthenticationHandler
    {
        AuthenticationScheme Scheme { get; }

        ValueTask<IAuthenticationData> Handle(ISecurityContext securityContext);
    }
}