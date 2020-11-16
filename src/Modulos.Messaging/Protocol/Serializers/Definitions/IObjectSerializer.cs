using Modulos.Messaging.Compression;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Serialization;

namespace Modulos.Messaging.Protocol.Serializers.Definitions
{
    internal interface IObjectSerializer
    {
        ISerializedObject Serialize([JetBrains.Annotations.NotNull] object data, 
            SerializerId serializerId, 
            CompressionEngineId compressionEngineId, 
            [JetBrains.Annotations.NotNull] ITransferObject transferObject, 
            [JetBrains.Annotations.NotNull] string what, InvocationPlace where, 
            [JetBrains.Annotations.NotNull] IMetricBag metricBag);

        TResult Deserialize<TResult>([JetBrains.Annotations.NotNull] ITransferObject transferObject, 
            TypeInfo typeInfo, 
            SerializerId serializerId, 
            CompressionEngineId compressionEngine,
            out ISerializedObject serializedObject,
            [JetBrains.Annotations.NotNull] string what, InvocationPlace where,
            [JetBrains.Annotations.NotNull] IMetricBag metricBag);
    }
}