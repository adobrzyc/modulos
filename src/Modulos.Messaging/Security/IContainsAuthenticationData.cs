namespace Modulos.Messaging.Security
{
    public interface IContainsAuthenticationData
    {
        IAuthenticationData AuthData { get; }
    }
}