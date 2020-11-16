namespace Modulos.Messaging
{
    /// <summary>
    /// Provides additional information about host of action.
    /// </summary>
    public interface IActionHost
    {
        string HostName { get; }
    }

}