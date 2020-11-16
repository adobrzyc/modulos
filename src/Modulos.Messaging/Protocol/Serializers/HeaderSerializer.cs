using System;
using System.Diagnostics;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Protocol.Serializers.Definitions;
using Modulos.Messaging.Serialization;
using Modulos.Messaging.Serialization.Engines;

namespace Modulos.Messaging.Protocol.Serializers
{
    internal class HeaderSerializer : IHeaderSerializer
    {
        private static readonly ISupportStringSerialization serializer = new JsonNetSerializer();

        public string Serialize(IMessageHeader headerToSerialize, InvocationPlace where, IMetricBag metricBag)
        {
            if (headerToSerialize == null) throw new ArgumentNullException(nameof(headerToSerialize));
            if (metricBag == null) throw new ArgumentNullException(nameof(metricBag));

            var sw = new Stopwatch();
            sw.Start();

         
            try
            {
                return serializer.SerializeToString(headerToSerialize);
            }
            finally
            {
                metricBag.Add(Kind.Serialization, "serialize header", @where, sw.ElapsedMilliseconds);
            }
        }

        public IMessageHeader Deserialize(ITransferObject transferObject, IMetricBag metricBag, InvocationPlace where)
        {
            if (transferObject == null) throw new ArgumentNullException(nameof(transferObject));
            if (metricBag == null) throw new ArgumentNullException(nameof(metricBag));

            var estimator = new Stopwatch();
            estimator.Start();

            try
            {
                return (IMessageHeader) serializer.DeserializeFromString(typeof(MessageHeader), transferObject.Header);
            }
            finally
            {
                metricBag.Add(Kind.Serialization, "deserialize header", @where, estimator.ElapsedMilliseconds);
            }
        }

    }
}