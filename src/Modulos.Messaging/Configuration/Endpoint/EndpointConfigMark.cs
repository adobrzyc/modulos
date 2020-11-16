using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Modulos.Messaging.Transport;

namespace Modulos.Messaging.Configuration
{
    [DataContract]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    public struct EndpointConfigMark : IEquatable<EndpointConfigMark>
    {
        public EndpointConfigMark(string mark, TransportEngineId transport)
        {
            if (mark == null) throw new ArgumentNullException(nameof(mark));
            if (string.IsNullOrWhiteSpace(mark)) 
                throw new ArgumentException("Mark can not be empty or null", nameof(mark));
            
            Mark = mark;
            Transport = transport;
            Placeholder = string.Empty;
        }

        /// <summary>
        /// Set <see cref="Mark"/> based on <see cref="Type.FullName"/> property.
        /// </summary>
        /// <param name="type">Type used to create <see cref="Mark"/>.</param>
        /// <param name="transport">Config transport.</param>
        public EndpointConfigMark(Type type, TransportEngineId transport) 
            : this(type.FullName, transport)
        {

        }

        [DataMember]
        public string Mark { get; private set; }

        [DataMember]
        public TransportEngineId Transport { get; private set; }

        [DataMember]
        public string Placeholder { get; set; }

        public bool Equals(EndpointConfigMark other)
        {
            return string.Equals(Mark, other.Mark, StringComparison.Ordinal)
                   && Transport == other.Transport;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is EndpointConfigMark mark && Equals(mark);
        }

        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Mark != null ? StringComparer.Ordinal.GetHashCode(Mark) : 0)*397) ^ Transport.GetHashCode();
            }
        }

        public static bool operator ==(EndpointConfigMark left, EndpointConfigMark right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(EndpointConfigMark left, EndpointConfigMark right)
        {
            return left.Equals(right);
        }

        public override string ToString()
        {
            return $"{Mark} ({Transport.Value})";
        }
    }
}