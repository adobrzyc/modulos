// ReSharper disable UnusedMember.Global

namespace Modulos.Pipes
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IOptionalPipeWithExceptionHandling : IOptionalPipe
    {
        ValueTask WhenError(Exception error, CancellationToken token);
    }
}