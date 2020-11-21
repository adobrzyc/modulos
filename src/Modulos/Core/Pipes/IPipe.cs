using System.Threading;
using System.Threading.Tasks;

namespace Modulos.Pipes
{
    public interface IPipe
    {
        ValueTask<PipeResult> Execute(CancellationToken cancellationToken);
    }
}