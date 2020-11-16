using System.Threading;
using System.Threading.Tasks;

namespace Modulos.Pipes
{
    public interface IPipe
    {
        Task<PipeResult> Execute(CancellationToken cancellationToken);
    }
}