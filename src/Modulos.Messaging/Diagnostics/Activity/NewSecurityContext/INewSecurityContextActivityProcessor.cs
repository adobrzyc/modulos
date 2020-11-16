using System.Threading.Tasks;

namespace Modulos.Messaging.Diagnostics.Activity.NewSecurityContext
{
    public interface INewSecurityContextActivityProcessor
    {
        Task Process(IInvocationContext context);
    }
}