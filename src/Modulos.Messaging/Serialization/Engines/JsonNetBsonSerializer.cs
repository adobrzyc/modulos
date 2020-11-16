using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Modulos.Messaging.Serialization.Engines
{
    internal class JsonNetBsonSerializer : ISerializer, ISupportBinarySerialization
    {
        public SerializerId Id => Serializers.JsonNetBson;
        public SerializationKind Kind => SerializationKind.Binary;
        public string MediaType => "application/octet-stream";

        public byte[] SerializeToByteArray(object obj)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BsonDataWriter(stream))
                {
                    //writer.CloseOutput = false;
                    var serializer = JsonSerializer.CreateDefault();
                    serializer.Serialize(writer, obj);
                }
                return stream.ToArray();
            }
        }

        public object DeserializeFromByteArray(Type type, byte[] serializedObject)
        {
            using (var stream = new MemoryStream(serializedObject))
            {
                using (var reader = new BsonDataReader(stream))
                {
                    //reader.CloseInput = false;
                    var serializer = JsonSerializer.CreateDefault();
                    
                    return serializer.Deserialize(reader, type);
                }
            }
           
        }

        public object DeserializeFromByteArray(byte[] serializedObject)
        {
            using (var stream = new MemoryStream(serializedObject))
            {
                using (var reader = new BsonDataReader(stream))
                {
                    //reader.CloseInput = false;
                    var serializer = JsonSerializer.CreateDefault();
                    
                    return serializer.Deserialize(reader);
                }
            }
        }
    }
}