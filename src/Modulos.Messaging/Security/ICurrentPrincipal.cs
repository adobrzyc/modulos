using System.Threading.Tasks;

namespace Modulos.Messaging.Security
{
    public interface ICurrentPrincipal
    {
        ValueTask<IPrincipal> Get();
        ValueTask<TPrincipal> Get<TPrincipal>() where TPrincipal : IPrincipal, new();
    }
}