namespace Modulos.Messaging.Security
{
    public class AuthenticationResult
    {
        public bool Handled { get; }
        public IAuthenticationData AuthData { get; }

        public AuthenticationResult(bool handled, IAuthenticationData authData)
        {
            Handled = handled;
            AuthData = authData;
        }
    }
}