namespace Modulos.Pipes
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IPipeWithExceptionHandling : IPipe
    {
        ValueTask WhenError(Exception error, CancellationToken token);
    }
}