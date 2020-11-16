using System;

namespace Modulos.Messaging
{
    /// <summary>
    /// Encapsulate information about operation, current action and security. 
    /// </summary>
    public interface IInvocationContext : ICloneable
    {
        ActionInfo Action { get; }
    }
}