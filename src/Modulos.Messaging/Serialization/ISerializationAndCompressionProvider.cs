using Modulos.Messaging.Compression;

namespace Modulos.Messaging.Serialization
{
    /// <summary>
    /// Defines serializer used by framework to serialize and deserialized objects.
    /// </summary>
    internal interface ISerializationAndCompressionProvider
    {
        ISerializer this[SerializerId id] { get; }

        ISerializedObject Serialize(object obj, SerializerId serializer, CompressionEngineId compressionEngine);

        object Deserialize(ISerializedObject obj);
    }
}