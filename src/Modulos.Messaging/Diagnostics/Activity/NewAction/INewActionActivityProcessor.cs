using System.Threading.Tasks;

namespace Modulos.Messaging.Diagnostics.Activity.NewAction
{
    internal interface INewActionActivityProcessor
    {
        Task Process(IActionInfo newAction, IActionInfo previous);
    }
}