using System;

namespace Modulos.Messaging.Security
{
    public class DefaultPrincipal : IPrincipal, IContainsAuthenticationData
    {
        public bool IsAnonymous => AuthData.Scheme == AnonymousAuthenticationData.Scheme;
   
        public IAuthenticationData AuthData { get; }

        public DefaultPrincipal([JetBrains.Annotations.NotNull] IAuthenticationData authData)
        {
            AuthData = authData ?? throw new ArgumentNullException(nameof(authData));
        }
    }
}