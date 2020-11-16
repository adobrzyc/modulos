using System;

namespace Modulos.Messaging.Serialization
{
    public interface ISupportStringSerialization
    {
        /// <summary>
        /// Serialize data.
        /// </summary>
        /// <param name="obj">Object to serialize.</param>
        /// <returns>Serialized object.</returns>
        string SerializeToString(object obj);

        /// <summary>
        /// Deserialize object represented by string.
        /// </summary>
        /// <param name="type">Data type of deserialized object.</param>
        /// <param name="serializedObject">String representation of serialized object.</param>
        /// <returns>Deserialized object.</returns>
        object DeserializeFromString(Type type, string serializedObject);

        /// <summary>
        /// Deserialize object represented by string.
        /// </summary>
        /// <param name="serializedObject">String representation of serialized object.</param>
        /// <returns>Deserialized object.</returns>
        object DeserializeFromString(string serializedObject);
    }
}