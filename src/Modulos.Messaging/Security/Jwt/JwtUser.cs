using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Modulos.Messaging.Security.Jwt
{
    public class JwtUser : IPrincipal
    {
        private readonly JwtAuthenticationData authData;

        public string Subject => authData.Token.Subject ?? string.Empty;

        public bool IsAnonymous => authData == null;
       
        public IEnumerable<Claim> Claims => authData?.Token.Claims ?? Enumerable.Empty<Claim>();

        public JwtUser(JwtAuthenticationData authData)
        {
            this.authData = authData;
        }

        public JwtUser(){}
    }
}