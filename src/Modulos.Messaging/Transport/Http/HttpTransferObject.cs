using System.IO;
using System.Runtime.Serialization;
using Modulos.Messaging.Protocol;

namespace Modulos.Messaging.Transport.Http
{
    public class HttpTransferObject : ITransferObject
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