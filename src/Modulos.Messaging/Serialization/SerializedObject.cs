using System.Runtime.Serialization;
using Modulos.Messaging.Compression;
using Newtonsoft.Json;

namespace Modulos.Messaging.Serialization
{
    [DataContract]
    public class SerializedObject : ISerializedObject
    {
        [DataMember]
        public TypeInfo TypeInfo { get; private set; }

        [DataMember]
        public SerializerId SerializerId { get; private set; }
        
        [DataMember]
        public CompressionEngineId CompressionEngineId { get; private set; }

        [DataMember]
        public ContentKind ContentKind { get; private set; }

        [DataMember]
        public string MediaType { get; private set; }

        [DataMember]
        public byte[] SerializedDataAsByteArray { get; private set; }

        [DataMember]
        public string SerializedDataAsString { get; private set; }

        [DataMember]
        public SerializedObjectProperty[] Properties { get; private set; }


        [JsonConstructor]
        private SerializedObject()
        {
            
        }

        public SerializedObject(TypeInfo typeInfo,
            byte[] serializedData,
            SerializerId serializerId,
            CompressionEngineId compressionEngine, string mediaType, SerializedObjectProperty[] properties = null)
        {
            TypeInfo = typeInfo;

            SerializerId = serializerId;

            CompressionEngineId = compressionEngine;

            MediaType = mediaType;

            SerializedDataAsByteArray = serializedData;

            ContentKind = ContentKind.Binary;
            
            Properties = properties;
        }

        public SerializedObject(TypeInfo typeInfo,
            string serializedData,
            SerializerId serializerId,
            CompressionEngineId compressionEngine, string mediaType, SerializedObjectProperty[] properties = null)
        {
            TypeInfo = typeInfo;

            SerializerId = serializerId;

            CompressionEngineId = compressionEngine;

            MediaType = mediaType;

            SerializedDataAsString = serializedData;

            ContentKind = ContentKind.String;

            Properties = properties;
        }

    }
}