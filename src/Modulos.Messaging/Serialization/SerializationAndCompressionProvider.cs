using System;
using System.Collections.Generic;
using System.Text;
using Modulos.Messaging.Compression;

namespace Modulos.Messaging.Serialization
{
    internal class SerializationAndCompressionProvider : ISerializationAndCompressionProvider
    {
        private readonly ICompressor compressor;
        private readonly IMessageTypeProvider messageTypeProvider;

        private readonly Dictionary<string, ISerializer> registeredSerializers = new Dictionary<string, ISerializer>();

        public SerializationAndCompressionProvider(IEnumerable<ISerializer> serializers, ICompressor compressor, 
            IMessageTypeProvider messageTypeProvider)
        {
            this.compressor = compressor;
            this.messageTypeProvider = messageTypeProvider;

            foreach (var serializer in serializers)
            {
                registeredSerializers.Add(serializer.Id.Value, serializer);
            }
        }

        public ISerializer this[SerializerId id]
        {
            get
            {
                if (!registeredSerializers.TryGetValue(id.Value, out var serializer))
                    throw new TodoException($"Unable to determine serializer for specified parameter: {id}");

                return serializer;
            }
        }


        public ISerializedObject Serialize(object obj, SerializerId serializerId, CompressionEngineId compressionEngine)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var serializer = this[serializerId];

            string stringResult = null;
            byte[] byteResult = null;

            switch (serializer.Kind)
            {
                case SerializationKind.String:
                    stringResult = CastToStringSerializer(serializer).SerializeToString(obj);
                    break;
                case SerializationKind.Binary:
                    byteResult = CastToBinarySerializer(serializer).SerializeToByteArray(obj);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(serializerId), serializerId, null);
            }

            //todo: [important] check it, was: if (compressionEngine == CompressionEngines.LZ4)
            if (compressionEngine != CompressionEngines.None)
            {
                if (stringResult != null)
                {
                    //todo: check scenario 
                    byteResult = Encoding.UTF8.GetBytes(stringResult);
                    stringResult = null;
                }
                byteResult = compressor.Wrap(byteResult, compressionEngine);
            }

            return stringResult != null
                ? new SerializedObject(new TypeInfo(obj), stringResult, serializerId, CompressionEngines.None, serializer.MediaType)
                : new SerializedObject(new TypeInfo(obj), byteResult, serializerId, compressionEngine, serializer.MediaType);
        }

        public object Deserialize(ISerializedObject obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            var serializer = this[obj.SerializerId];

            var type = messageTypeProvider.FindType(obj.TypeInfo, true);

            if (obj.CompressionEngineId == CompressionEngines.None)
            {
                switch (serializer.Kind)
                {
                    case SerializationKind.String:
                        return CastToStringSerializer(serializer).DeserializeFromString(type, obj.SerializedDataAsString);
                    case SerializationKind.Binary:
                        return CastToBinarySerializer(serializer).DeserializeFromByteArray(type, obj.SerializedDataAsByteArray);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else if (obj.CompressionEngineId == CompressionEngines.LZ4)
            {
                switch (serializer.Kind)
                {
                    case SerializationKind.String:
                        var bytes = compressor.Unwrap(obj.SerializedDataAsByteArray, obj.CompressionEngineId);
                        var unwrappedStringContent = Encoding.UTF8.GetString(bytes); //todo: L-100000000: check it out 
                        return CastToStringSerializer(serializer).DeserializeFromString(type, unwrappedStringContent);

                    case SerializationKind.Binary:
                        var unwrappedObject = compressor.Unwrap(obj.SerializedDataAsByteArray, obj.CompressionEngineId);
                        return CastToBinarySerializer(serializer)
                            .DeserializeFromByteArray(type, unwrappedObject);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        private ISupportStringSerialization CastToStringSerializer(ISerializer serializer)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            if (!(serializer is ISupportStringSerialization stringSerializer))
                throw new TodoException($"Serializer: {serializer.Id} does not support string serialization.");
            return stringSerializer;
        }

        private ISupportBinarySerialization CastToBinarySerializer(ISerializer serializer)
        {
            if (serializer == null) throw new ArgumentNullException(nameof(serializer));

            if (!(serializer is ISupportBinarySerialization byteSerializer))
                throw new TodoException($"Serializer: {serializer.Id} does not support byte serialization.");
            return byteSerializer;
        }
    }
}