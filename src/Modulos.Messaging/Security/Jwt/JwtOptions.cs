using Microsoft.IdentityModel.Tokens;

namespace Modulos.Messaging.Security.Jwt
{
    public class JwtOptions
    {
        public TokenValidationParameters ValidationParameters { get; set; }
    }
}