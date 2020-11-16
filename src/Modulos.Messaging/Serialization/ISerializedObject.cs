using System.Runtime.Serialization;
using Modulos.Messaging.Compression;
using Newtonsoft.Json;

namespace Modulos.Messaging.Serialization
{
    /// <summary>
    /// Defines serialized object.
    /// </summary>
    public interface ISerializedObject
    {
        /// <summary>
        /// Type info, used to determine type of serialized object.
        /// </summary>
        TypeInfo TypeInfo { get; }

        /// <summary>
        /// Serialized identity.
        /// </summary>
        SerializerId SerializerId { get; }

        /// <summary>
        /// Compression engine.
        /// </summary>
        CompressionEngineId CompressionEngineId { get; }

        /// <summary>
        /// Determines content transportId.
        /// </summary>
        ContentKind ContentKind { get; }
       
        /// <summary>
        /// Determines media type of serialized data.
        /// </summary>
        string MediaType { get; }

        /// <summary>
        /// Serialized data as byte array.
        /// </summary>
        byte[] SerializedDataAsByteArray { get; }

        /// <summary>
        /// Serialized data as string.
        /// </summary>
        string SerializedDataAsString { get; }

        SerializedObjectProperty[] Properties { get; }
    }

    
    [DataContract]
    public sealed class SerializedObjectProperty 
    {
        [DataMember]
        public string Key { get; private set; }

        [DataMember]
        public string Value { get; private set; }

        [JsonConstructor]
        private SerializedObjectProperty()
        {
            
        }

        public SerializedObjectProperty(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}