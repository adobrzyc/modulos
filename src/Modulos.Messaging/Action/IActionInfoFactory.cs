using Modulos.Messaging.Operation;

// ReSharper disable UnusedMemberInSuper.Global

namespace Modulos.Messaging
{
    internal interface IActionInfoFactory
    {
        ActionInfo Create(IMessage message, string hostName, OperationId? operationId = null, ActionId? actionId = null, bool start = true);
        ActionInfo Create(IMessage message, IActionHost actionHost, OperationId? operationId = null, ActionId? actionId = null, bool start = true);
    }
}