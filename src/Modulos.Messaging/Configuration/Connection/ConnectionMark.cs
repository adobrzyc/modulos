using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Modulos.Messaging.Configuration
{
    [DataContract]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    public struct ConnectionMark 
    {
        [DataMember]
        public Guid ConnMark { get; private set; }


        public ConnectionMark(Guid connMark)
        {
            ConnMark = connMark;
        }

        public override string ToString()
        {
            return ConnMark.ToString();
        }
    }
}