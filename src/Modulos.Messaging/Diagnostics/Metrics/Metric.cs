using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Modulos.Messaging.Operation;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    
    [DataContract]
    public sealed class Metric : IMetric, IEquatable<Metric>, IEquatable<IMetric>, IComparable<IMetric>
    {
        private string what = "";

        [DataMember]
        public Guid MetricId { get; set; } = KeyGenerator.NewSequentialGuid();

        [DataMember]
        public OperationId OperationId { get; set; }

        [DataMember]
        public ActionId ActionId { get; set; }
       
        [DataMember]
        public Kind Kind { get; set; }

        [DataMember]
        public InvocationPlace Where { get; set; }

        [DataMember]
        public string What
        {
            get => what;
            set => what = value ?? "";
        }

        //todo: #design-concept #performance: consider to use float, due to performance improvement 
        [DataMember]
        public decimal Value { get; set; }

        [DataMember]
        private long timeUtc = DateTime.UtcNow.Ticks;

        [IgnoreDataMember]
        public DateTime TimeUtc
        {
            get => new DateTime(timeUtc, DateTimeKind.Utc);
            set => timeUtc = value.Ticks;
        }

        // todo: check if counter may be signed only to operation info ... for now I think not....
        public bool IsSigned => OperationId != OperationId.Empty
                                || ActionId != ActionId.Empty;

        public Metric()
        {

        }

        public Metric(IActionInfo action)
        {
            OperationId = action.OperationId;
            ActionId = action.Id;
        }

        public int CompareTo(IMetric other)
        {
            if (Kind != other.Kind)
                return ((int) Kind).CompareTo((int) other.Kind);
            return Value.CompareTo(other.Value);
        }

        public override string ToString()
        {
            return $"Transport: {Kind}, " + $"CallInfo: {ActionId}, " 
                + $"OperationInfo: {OperationId}, " 
                + $"Info: {What}, " 
                + $"Value: {Value}, " 
                + $"InvocationPlace: {Where}";
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(Metric other)
        {
            return Equals((IMetric) other);
        }

        [ExcludeFromCodeCoverage]
        public bool Equals(IMetric other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return MetricId.Equals(other.MetricId);
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Metric counter && Equals(counter);
        }

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode
            return MetricId.GetHashCode();
        }

        [ExcludeFromCodeCoverage]
        public static bool operator ==(Metric left, Metric right)
        {
            return Equals(left, right);
        }

        [ExcludeFromCodeCoverage]
        public static bool operator !=(Metric left, Metric right)
        {
            return !Equals(left, right);
        }
    }
}