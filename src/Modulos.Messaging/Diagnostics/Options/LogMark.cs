using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Modulos.Messaging.Diagnostics.Options
{
    [DataContract]
    public sealed class LogMark : IEquatable<LogMark>
    {
        [DataMember]
        public string Mark { get; private set; }

        [JsonConstructor]
        private LogMark()
        {
            
        }

        public LogMark(string mark)
        {
            Mark = mark;
        }

        public override string ToString()
        {
            return Mark;
        }

        public static LogMark Empty => new LogMark(null);

        public bool Equals(LogMark other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Mark, other.Mark, StringComparison.InvariantCulture);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is LogMark && Equals((LogMark) obj);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            return (Mark != null ? StringComparer.InvariantCulture.GetHashCode(Mark) : 0);
        }

        public static bool operator ==(LogMark left, LogMark right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(LogMark left, LogMark right)
        {
            return !Equals(left, right);
        }
    }
}