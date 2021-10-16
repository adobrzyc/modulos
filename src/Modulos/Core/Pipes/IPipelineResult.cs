// ReSharper disable UnusedMember.Global

namespace Modulos.Pipes
{
    using System;

    public interface IPipelineResult : IAsyncDisposable
    {
        T Get<T>();
        T GetOptional<T>();
        object[] GetAll();
    }
}