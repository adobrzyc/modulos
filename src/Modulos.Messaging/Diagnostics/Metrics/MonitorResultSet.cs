using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    [Obsolete]
    
    [DataContract]
    public class MonitorResultSet
    {
        private Dictionary<Kind, TimeSpan> times;
        private Dictionary<Kind, TimeSpan> Times => times ?? (times = new Dictionary<Kind, TimeSpan>());


        private bool timesWasSet;

        public MonitorResultSet()
        {
            Metrics = new IMetric[] {};
        }

        public MonitorResultSet(IEnumerable<IMetric> metrics)
        {
            Metrics = metrics?.ToArray() ?? throw new ArgumentNullException(nameof(metrics));
        }
        
        [DataMember]
        public IMetric[] Metrics { get;  private set; }


        public TimeSpan GetTime(Kind kind)
        {
            SetTimes();
            return Times.ContainsKey(kind) ? Times[kind] : TimeSpan.Zero;
        }

        private bool TryGetTime(Kind kind, out TimeSpan time)
        {
            SetTimes();
            return Times.TryGetValue(kind, out time);
        }

        private void SetTimes()
        {
            if (timesWasSet) return;
            timesWasSet = true;

            if (Metrics == null)
                return;

            foreach (var value in KindsWithGroup.Where(e => e.Attributes.Group == Group.Time).Select(e => e.Kind))
            {
                var metrics = Metrics
                    .Where(e => e.Kind == value)
                    .ToArray();

                if (!metrics.Any()) continue;

                var time = TimeSpan.FromMilliseconds((long)metrics
                        .Sum(e => e.Value));
                Times.Add(value, time);
            }

            var requestTime = TimeSpan.Zero;
            var responseTime = TimeSpan.Zero;

            foreach (var call in Metrics.GroupBy(e=>e.ActionId))
            {
                var requestStartMetric = call.FirstOrDefault(e => e.Kind == Kind.RequestStart);
                var requestEndMetric = call.FirstOrDefault(e => e.Kind == Kind.RequestArrived);
                var responseStartMetric = call.FirstOrDefault(e => e.Kind == Kind.ResponseStart);
                var responseEndMetric = call.FirstOrDefault(e => e.Kind == Kind.ResponseArrived);

                if (requestStartMetric != null && requestEndMetric != null)
                {
                    var start = new DateTime((long)requestStartMetric.Value, DateTimeKind.Utc);
                    var end = new DateTime((long)requestEndMetric.Value, DateTimeKind.Utc);

                    requestTime = requestTime.Add(end - start);
                }

                if (responseStartMetric != null && responseEndMetric != null)
                {
                    var start = new DateTime((long)responseStartMetric.Value, DateTimeKind.Utc);
                    var end = new DateTime((long)responseEndMetric.Value, DateTimeKind.Utc);

                    responseTime = responseTime.Add(end - start);
                }
            }

            //todo: cleanup
            //if (Times.ContainsKey(Transport.TransferTo))
            //    Times[Transport.TransferTo] = Times[Transport.TransferTo] + requestTime;
            //else
            //{
            //    if(requestTime != TimeSpan.Zero)
            //        Times.Add(Transport.TransferTo,requestTime);
            //}

            //if (Times.ContainsKey(Transport.TransferFrom))
            //    Times[Transport.TransferFrom] = Times[Transport.TransferFrom] + requestTime;
            //else
            //{
            //    if (responseTime != TimeSpan.Zero)
            //        Times.Add(Transport.TransferFrom, responseTime);
            //}

            if (Times.ContainsKey(Kind.Transfer))
                Times[Kind.Transfer] = Times[Kind.Transfer] + requestTime + responseTime;
            else
            {
                if (requestTime != TimeSpan.Zero || responseTime != TimeSpan.Zero)
                    Times.Add(Kind.Transfer, requestTime + responseTime);
            }
        }

        private string stringRepresentation;

        public override string ToString()
        {
            if (stringRepresentation != null) return stringRepresentation;
            SetTimes();

            var sb = new StringBuilder();

            foreach (var value in KindsWithGroup.Where(e=>e.Attributes.Group == Group.Time))
            {
                TimeSpan time;
                if (!TryGetTime(value.Kind, out time)) continue;
                sb.AppendFormat("{0}: {1} ", value.Kind, GetFormattedTime(time));
                sb.AppendLine();
            }

            foreach (var value in KindsWithGroup.Where(e => e.Attributes.Group == Group.Memory))
            {
                var copyOfValue = value;
                foreach (var metric in Metrics.Where(e => e.Kind == copyOfValue.Kind))
                {
                    sb.AppendFormat("{0}: {1} ", value.Kind, GetFormattedMemory(metric.Value));
                    sb.AppendLine();
                }
            }
           

            stringRepresentation = sb.ToString();

            return stringRepresentation;
        }

        private static string GetFormattedMemory(decimal value)
        {
            if(value < 1024)
                return $"{value:##0} bytes";
           
            if (value < 1024 * 1024)
                return $"{value / 1024M:F} kB";

            return $"{value / 1024M / 1024M:F} MB";
        }

        private static string GetFormattedTime(TimeSpan time)
        {
            if (time == TimeSpan.Zero)
                return "0 ms";

            if (time.TotalMilliseconds < 1000)
                return $"{time.TotalMilliseconds:##0} ms";

            if(time.TotalSeconds < 60)
                return $"{time.TotalSeconds:F} sec";

            return $"{time.TotalMinutes:F} min";
        }
       
        private static IEnumerable<Kind> kinds;
        private static IEnumerable<KindInfo> kindsWithGroup;

        public static IEnumerable<KindInfo> KindsWithGroup
        {
            get
            {
                return kindsWithGroup ?? (kindsWithGroup = Enum.GetValues(typeof(Kind))
                    .OfType<Kind>()
                    .Select(e => new KindInfo(e))).ToArray();
            }
        }

        public static IEnumerable<Kind> Kinds => kinds ?? (kinds = Enum.GetValues(typeof(Kind))
                                                     .OfType<Kind>()).ToArray();
    }
}