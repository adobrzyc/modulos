namespace Modulos.Messaging.Serialization
{
    public static class Serializers
    {
        public static readonly SerializerId JsonNet = new SerializerId("JsonNet");
        public static readonly SerializerId JsonNetBson = new SerializerId("JsonNetBson");
        public static readonly SerializerId NetCoreJson = new SerializerId("NetCoreJson");
        public static readonly SerializerId Jil = new SerializerId("Jil");
    }
}