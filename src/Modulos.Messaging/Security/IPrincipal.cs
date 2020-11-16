namespace Modulos.Messaging.Security
{
    public interface IPrincipal 
    {
        bool IsAnonymous { get; }
    }
}