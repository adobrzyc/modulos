using System;

// ReSharper disable UnusedMethodReturnValue.Global

namespace Modulos.Errors
{
    internal interface IExceptionTransformer
    {
        bool ToModulosException(Exception error, out ModulosException modulosException);

        bool ToModulosException(string code, string message, out ModulosException modulosException);
    }
}