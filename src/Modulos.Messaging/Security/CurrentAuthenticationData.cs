using System.Threading.Tasks;

namespace Modulos.Messaging.Security
{
    public class CurrentAuthenticationData : ICurrentAuthenticationData
    {
        private readonly object locker = new object();
        private IAuthenticationData current;

        public ValueTask Set(IAuthenticationData data)
        {
            lock (locker)
                current = data;

            return default;
        }

        public ValueTask<IAuthenticationData> Get()
        {
            lock (locker)
            {
                return new ValueTask<IAuthenticationData>(current ?? AnonymousAuthenticationData.Instance);
            }
        }
    }
}