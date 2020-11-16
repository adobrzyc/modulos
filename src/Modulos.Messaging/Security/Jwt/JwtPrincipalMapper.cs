using System;
using System.Threading.Tasks;

namespace Modulos.Messaging.Security.Jwt
{
    public class JwtPrincipalMapper : IPrincipalMapper
    {
        public bool IsMatch(IAuthenticationData authData, Type userType)
        {
            if (authData == null) throw new ArgumentNullException(nameof(authData));
            return authData is JwtAuthenticationData && userType == typeof(JwtUser);
        }

        public ValueTask<IPrincipal> Map(IAuthenticationData authData)
        {
            if (authData == null) throw new ArgumentNullException(nameof(authData));
            return new ValueTask<IPrincipal>(new JwtUser((JwtAuthenticationData)authData));
        }
    }
}