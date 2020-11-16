namespace Modulos.Messaging.Security
{
    public interface IAuthenticationData
    {
        AuthenticationScheme Scheme { get; }
        string Payload { get; }
    }
}