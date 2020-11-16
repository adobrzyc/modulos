using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Modulos.Messaging.Security
{
    public class CurrentPrincipal : ICurrentPrincipal
    {
        private readonly ICurrentAuthenticationData currentAuthData;
        private readonly IEnumerable<IPrincipalMapper> mappers;

        public CurrentPrincipal(ICurrentAuthenticationData currentAuthData,
            IEnumerable<IPrincipalMapper> mappers)
        {
            this.currentAuthData = currentAuthData;
            this.mappers = mappers;
        }

        public async ValueTask<IPrincipal> Get()
        {
            var authData = await currentAuthData.Get().ConfigureAwait(false);
            return new DefaultPrincipal(authData);
        }

        public async ValueTask<TPrincipal> Get<TPrincipal>()
            where TPrincipal : IPrincipal, new()
        {
            var authData = await currentAuthData.Get().ConfigureAwait(false);
            if (authData.Scheme == AnonymousAuthenticationData.Scheme)
                return new TPrincipal();

            var mapper = mappers
                .LastOrDefault(e => e.IsMatch(authData, typeof(TPrincipal)));

            if (mapper == null)
                return new TPrincipal();

            var principal = await mapper.Map(authData).ConfigureAwait(false);
            return (TPrincipal) principal;
        }
    }
}