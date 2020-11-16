using System.Threading.Tasks;

namespace Modulos.Messaging.Diagnostics.Activity
{
    public interface IActivityHandler<in TActivity> where TActivity : IActivity
    {
        Task Handle(TActivity @event);
    }
}