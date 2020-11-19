using System;

// ReSharper disable UnusedMember.Global

namespace Modulos.Pipes
{
    public interface IPipelineResult: IAsyncDisposable
    {
        T Get<T>();
        T GetOptional<T>();
        object[] GetAll();
    }
}