using System;
using System.Threading.Tasks;

namespace Modulos.Messaging.Protocol.Response.Definitions
{
    internal interface IParseFaultResponse
    {
        Task<Exception> Parse([JetBrains.Annotations.NotNull] IMessageHeader responseHeader, [JetBrains.Annotations.NotNull] ITransferObject transferObject);
    }
}