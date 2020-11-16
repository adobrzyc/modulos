using System.IO;
using System.Runtime.Serialization;

namespace Modulos.Messaging.Protocol
{
    public class TransferObject : ITransferObject
    {
        public string MediaType { get; set; }
        public string MediaTypeOfStream { get; set; }

        public string Header { get; set; }
        public string StringContent { get; set; }
        public byte[] ByteContent { get; set; }

        [IgnoreDataMember]
        public Stream Stream { get; set; }
    }
}