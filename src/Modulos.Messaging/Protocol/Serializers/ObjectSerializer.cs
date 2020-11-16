using System;
using System.Diagnostics;
using Modulos.Messaging.Compression;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Protocol.Serializers.Definitions;
using Modulos.Messaging.Serialization;

namespace Modulos.Messaging.Protocol.Serializers
{
    internal class ObjectSerializer : IObjectSerializer
    {
        private readonly ISerializationAndCompressionProvider serializationAndCompressionProvider;

        public ObjectSerializer(ISerializationAndCompressionProvider serializationAndCompressionProvider)
        {
            this.serializationAndCompressionProvider = serializationAndCompressionProvider;
        }

        public ISerializedObject Serialize(object data, 
            SerializerId serializerId,
            CompressionEngineId compressionEngineId, 
            ITransferObject transferObject, 
            string what, InvocationPlace where, 
            IMetricBag metricBag)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (transferObject == null) throw new ArgumentNullException(nameof(transferObject));
            if (what == null) throw new ArgumentNullException(nameof(what));
            if (metricBag == null) throw new ArgumentNullException(nameof(metricBag));

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                var result = serializationAndCompressionProvider.Serialize
                (
                    obj: data,
                    serializer: serializerId,
                    compressionEngine: compressionEngineId
                );

                switch (result.ContentKind)
                {
                    case ContentKind.String:
                        transferObject.StringContent = result.SerializedDataAsString;
                        break;
                    case ContentKind.Binary:
                        transferObject.ByteContent = result.SerializedDataAsByteArray;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                transferObject.MediaType = result.MediaType;

                return result;
            }
            finally
            {
                metricBag.Add(Kind.Serialization, what, @where, stopWatch.ElapsedMilliseconds);
            }
        }

        public TResult Deserialize<TResult>(ITransferObject transferObject, 
            TypeInfo typeInfo, 
            SerializerId serializerId, 
            CompressionEngineId compressionEngine,
            out ISerializedObject serializedObject,
            string what, InvocationPlace where,
            IMetricBag metricBag)
        {
            if (transferObject == null) throw new ArgumentNullException(nameof(transferObject));
            if (what == null) throw new ArgumentNullException(nameof(what));
            if (metricBag == null) throw new ArgumentNullException(nameof(metricBag));

            var sw = new Stopwatch();
            sw.Start();

            try
            {
                serializedObject = null;
                TResult deserialized = default;

                if ((transferObject.ByteContent == null || transferObject.ByteContent.Length == 0)
                    && string.IsNullOrEmpty(transferObject.StringContent))
                    return deserialized;

                if (!string.IsNullOrEmpty(transferObject.StringContent))
                {
                    serializedObject = new SerializedObject
                    (
                        typeInfo, transferObject.StringContent,
                        serializerId, CompressionEngines.None, transferObject.MediaType
                    );

                    deserialized = (TResult)serializationAndCompressionProvider.Deserialize(serializedObject);
                }
                else
                {
                    serializedObject = new SerializedObject
                    (
                        typeInfo, transferObject.ByteContent, 
                        serializerId, compressionEngine, transferObject.MediaType
                    );
                    deserialized = (TResult)serializationAndCompressionProvider.Deserialize(serializedObject);
                }

                if(deserialized is IContainDirectStream stream)
                {
                    stream.Stream = transferObject.Stream;
                    stream.MediaType = transferObject.MediaTypeOfStream;
                }

                return deserialized;

            }
            finally
            {
                metricBag.Add(Kind.Serialization, what, where, sw.ElapsedMilliseconds);
            }
        }
    }
}