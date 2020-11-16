using System.Threading.Tasks;

namespace Modulos.Messaging.Security
{
    public interface ICurrentAuthenticationData
    {
        ValueTask Set(IAuthenticationData data);
        ValueTask<IAuthenticationData> Get();
    }
}