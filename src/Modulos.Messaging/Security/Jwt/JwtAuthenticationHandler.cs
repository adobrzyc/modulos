using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Modulos.Messaging.Security.Exceptions;

namespace Modulos.Messaging.Security.Jwt
{
    public class JwtAuthenticationHandler : IAuthenticationHandler
    {
        private readonly JwtOptions options;
        public AuthenticationScheme Scheme { get; } = JwtAuthenticationData.Schema;

        public JwtAuthenticationHandler(JwtOptions options)
        {
            this.options = options;
        }
        public ValueTask<IAuthenticationData> Handle(ISecurityContext securityContext)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(securityContext.Payload, options.ValidationParameters, out var validatedToken);

                var jwtToken = (JwtSecurityToken) validatedToken;

                return new ValueTask<IAuthenticationData>
                (
                    new JwtAuthenticationData(jwtToken)
                );
            }
            catch (Exception e)
            {
                throw new SecurityException(e.Message,e);
            }
        }
    }

}