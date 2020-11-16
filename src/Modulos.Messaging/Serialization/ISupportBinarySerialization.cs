using System;

namespace Modulos.Messaging.Serialization
{
    public interface ISupportBinarySerialization
    {
        /// <summary>
        /// Serialize data.
        /// </summary>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>Serialized object.</returns>
        byte[] SerializeToByteArray(object obj);

        /// <summary>
        /// Deserialize object represented by byte array.
        /// </summary>
        /// <param name="type">Data type of deserialized object.</param>
        /// <param name="serializedObject">Byte array with serialized object.</param>
        /// <returns>Deserialized object.</returns>
        object DeserializeFromByteArray(Type type, byte[] serializedObject);

        /// <summary>
        /// Deserialize object represented by byte array.
        /// </summary>
        /// <param name="serializedObject">Byte array with serialized object.</param>
        /// <returns>Deserialized object.</returns>
        object DeserializeFromByteArray(byte[] serializedObject);
    }
}