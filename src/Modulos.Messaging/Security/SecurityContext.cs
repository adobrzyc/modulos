using System.Runtime.Serialization;

namespace Modulos.Messaging.Security
{
    [DataContract]
    public class SecurityContext : ISecurityContext
    {
        [DataMember]
        public AuthenticationScheme Scheme { get; private set; }
        
        [DataMember]
        public string Payload { get; private set; }

        public SecurityContext()
        {
            
        }

        public SecurityContext(AuthenticationScheme scheme, string payload)
        {
            Scheme = scheme;
            Payload = payload;
        }

        public SecurityContext(IAuthenticationData authData)
        {
            Scheme = authData?.Scheme ?? new AuthenticationScheme();
            Payload = authData?.Payload;
        }
    }
}