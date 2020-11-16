using System;
using System.Threading.Tasks;

namespace Modulos.Messaging.Diagnostics.Activity.FatalError
{
    internal interface IFatalErrorActivityProcessor
    {
        Task Process(Exception error, string message, Type host, object relatedObject);
    }
}