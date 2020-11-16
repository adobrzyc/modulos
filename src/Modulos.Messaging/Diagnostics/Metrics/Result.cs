using System;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    public sealed class Result : IResult, IEquatable<Result>
    {
        public Kind Kind { get; }
        public ValueKind ValueKind { get; }
        public decimal Value { get; }

        public Result(Kind kind, ValueKind valueKind, decimal value)
        {
            Kind = kind;
            ValueKind = valueKind;
            Value = value;
        }

        private string stringRepresentation;

        private static string GetFormattedMemory(decimal value)
        {
            if (value < 1024)
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

            if (time.TotalSeconds < 60)
                return $"{time.TotalSeconds:F} sec";

            return $"{time.TotalMinutes:F} min";
        }

        public int CompareTo(IResult other)
        {
            if (other.Kind == Kind && other.ValueKind == ValueKind)
                return decimal.Compare(Value, other.Value);

            return 0;
        }

        public int CompareTo(object obj)
        {
            if (obj is IResult) 
                return CompareTo((IResult) obj);
            return 0;
        }

        public override string ToString()
        {
            if (stringRepresentation != null) 
                return stringRepresentation;
            
            var kindWithGroup = new KindInfo(Kind);

            switch (kindWithGroup.Attributes.Group)
            {
                case Group.Time:
                    stringRepresentation = GetFormattedTime(TimeSpan.FromMilliseconds(Convert.ToDouble(Value)));
                    break;
                case Group.Date:
                    stringRepresentation = new DateTime((long)Value, DateTimeKind.Utc).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case Group.Memory:
                    stringRepresentation = GetFormattedMemory(Value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return stringRepresentation;
        }


        public bool Equals(Result other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Kind == other.Kind && ValueKind == other.ValueKind && Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Result && Equals((Result) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Kind;
                hashCode = (hashCode * 397) ^ (int) ValueKind;
                hashCode = (hashCode * 397) ^ Value.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Result left, Result right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Result left, Result right)
        {
            return !Equals(left, right);
        }
    }


  
}