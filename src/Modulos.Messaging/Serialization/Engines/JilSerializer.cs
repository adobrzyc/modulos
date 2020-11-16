using System;
using Jil;

namespace Modulos.Messaging.Serialization.Engines
{
    internal class JilSerializer : ISerializer, ISupportStringSerialization
    {
        public SerializerId Id => Serializers.Jil;
        public SerializationKind Kind => SerializationKind.String;
        public string MediaType => "application/json";

        public string SerializeToString(object obj)
        {
            return JSON.Serialize(obj);
        }

        public object DeserializeFromString(Type type, string serializedObject)
        {
            return JSON.Deserialize(serializedObject, type);
        }

        public object DeserializeFromString(string serializedObject)
        {
            return JSON.DeserializeDynamic(serializedObject);
        }
    }
}