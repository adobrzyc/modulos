using System;
using System.IdentityModel.Tokens.Jwt;

namespace Modulos.Messaging.Security.Jwt
{
    public class JwtAuthenticationData : IAuthenticationData
    {
        public static readonly AuthenticationScheme Schema = new AuthenticationScheme("jwt");

        AuthenticationScheme IAuthenticationData.Scheme => Schema;

        public string Payload
        {
            get
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                return tokenHandler.WriteToken(Token);
            }
        }

        public JwtSecurityToken Token { get; }

        public JwtAuthenticationData([JetBrains.Annotations.NotNull] JwtSecurityToken token)
        {
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }
    }
}