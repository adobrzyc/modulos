using System;
using System.Threading;
using System.Threading.Tasks;

namespace Modulos.Pipes
{
    public interface IOptionalPipeWithExceptionHandling : IOptionalPipe
    {
        ValueTask WhenError(Exception error, CancellationToken token);
    }
}