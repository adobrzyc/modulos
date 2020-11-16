using System;
using Newtonsoft.Json;

namespace Modulos.Messaging.Serialization.Engines
{
    internal class JsonNetSerializer : ISerializer, ISupportStringSerialization
    {
        public SerializerId Id => Serializers.JsonNet;
        public SerializationKind Kind => SerializationKind.String;
        public string MediaType => "application/json";

        public string SerializeToString(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }


        public object DeserializeFromString(Type type, string serializedObject)
        {
            return JsonConvert.DeserializeObject(serializedObject, type);
        }

        public object DeserializeFromString(string serializedObject)
        {
            return JsonConvert.DeserializeObject(serializedObject);
        }
    }
}