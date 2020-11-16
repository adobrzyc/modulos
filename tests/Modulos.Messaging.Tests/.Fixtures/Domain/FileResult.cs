using System.IO;
using System.Runtime.Serialization;
using Jil;
using Newtonsoft.Json;

namespace Modulos.Messaging.Tests.Fixtures.Domain
{
    [DataContract]
    public class FileResult : IContainDirectStream
    {
        [DataMember]
        public string FileName { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        [JilDirective(true)]
        public string MediaType { get; set; }

        [IgnoreDataMember]
        [JsonIgnore]
        [JilDirective(true)]
        public Stream Stream { get; set; }
    }
}