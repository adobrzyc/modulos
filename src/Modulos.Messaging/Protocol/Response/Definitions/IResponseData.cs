using System;
using JetBrains.Annotations;
using Modulos.Messaging.Serialization;

namespace Modulos.Messaging.Protocol.Response.Definitions
{
    public interface IResponseData
    {
        [NotNull]
        IMessageHeader MessageHeader { get; }

        [NotNull]
        ITransferObject TransferObject { get; }
       
        [CanBeNull]
        ISerializedObject SerializedResponse { get; }

        [CanBeNull]
        IInvocationContext InvocationContext { get; }

        [CanBeNull]
        Exception Error { get;}
    }
}