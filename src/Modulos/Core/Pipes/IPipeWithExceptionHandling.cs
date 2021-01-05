using System;
using System.Threading;
using System.Threading.Tasks;

namespace Modulos.Pipes
{
    public interface IPipeWithExceptionHandling : IPipe
    {
        ValueTask WhenError(Exception error, CancellationToken token);
    }
}