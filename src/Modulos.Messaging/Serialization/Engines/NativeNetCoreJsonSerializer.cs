using System;

namespace Modulos.Messaging.Serialization.Engines
{
    internal class NativeNetCoreJsonSerializer : ISerializer, ISupportStringSerialization
    {
        public SerializerId Id { get; } = Serializers.NetCoreJson;
        public SerializationKind Kind { get; } = SerializationKind.String;
        public string MediaType { get; } = "application/json";
        public string SerializeToString(object obj)
        {
            return System.Text.Json.JsonSerializer.Serialize(obj);
        }

        public object DeserializeFromString(Type type, string serializedObject)
        {
            return System.Text.Json.JsonSerializer.Deserialize(serializedObject, type);
        }

        public object DeserializeFromString(string serializedObject)
        {
            return System.Text.Json.JsonSerializer.Deserialize<object>(serializedObject);
        }
    }
}