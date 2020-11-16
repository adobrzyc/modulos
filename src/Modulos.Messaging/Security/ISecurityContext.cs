namespace Modulos.Messaging.Security
{
    public interface ISecurityContext
    {
        AuthenticationScheme Scheme { get; }
        string Payload { get; }
    }
}