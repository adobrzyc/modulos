using Microsoft.AspNetCore.Builder;

namespace Modulos.Messaging.Transport.Http.Config
{
    public static class UseModulos
    {
        public static void UserModulos(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<HttpMiddleware>();
        }
    }
}