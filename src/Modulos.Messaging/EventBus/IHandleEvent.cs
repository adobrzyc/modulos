using System.Threading.Tasks;

namespace Modulos.Messaging.EventBus
{
    public interface IHandleEvent<in TEvent>  where TEvent : IEvent 
    {
        Task Handle(TEvent @event);
    }
}