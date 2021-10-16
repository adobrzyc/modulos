namespace Modulos.Pipes
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IPipe
    {
        ValueTask<PipeResult> Execute(CancellationToken token);
    }
}