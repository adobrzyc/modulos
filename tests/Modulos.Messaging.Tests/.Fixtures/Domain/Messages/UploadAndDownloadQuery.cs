using System.IO;
using System.Runtime.Serialization;
using Jil;
using Newtonsoft.Json;

namespace Modulos.Messaging.Tests.Fixtures.Domain
{
    [DataContract]
    public class UploadAndDownloadQuery : IQuery<FileResult>, IContainDirectStream
    {
        [DataMember]
        public string FileName { get; set; }
       
        [IgnoreDataMember]
        [JilDirective(true)]
        [JsonIgnore]
        public string MediaType { get; set; }
        
        [IgnoreDataMember]
        [JilDirective(true)]
        [JsonIgnore]
        public Stream Stream { get; set; }
    }
}