using System.IO;
using JetBrains.Annotations;

namespace Modulos.Messaging.Protocol
{
    /// <summary>
    /// Represents message with configuration in communication layer.
    /// </summary>
    public interface ITransferObject
    {
        [CanBeNull]
        string MediaType { get; set; }

        [CanBeNull]
        string MediaTypeOfStream { get; set; }

        string Header { get; set; }

        string StringContent { get; set; }

        byte[] ByteContent { get; set; }

        Stream Stream { get; set; }
    }
}