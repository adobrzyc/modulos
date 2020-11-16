using System;

namespace Modulos.Messaging.Security
{
    public sealed class AnonymousAuthenticationData : IAuthenticationData
    {
        public static readonly AuthenticationScheme Scheme = new AuthenticationScheme(String.Empty);
        public static readonly IAuthenticationData Instance = new AnonymousAuthenticationData();
      
        AuthenticationScheme IAuthenticationData.Scheme => Scheme;
        
        public string Payload { get; } = String.Empty;

        private AnonymousAuthenticationData()
        {
        }
    }
}