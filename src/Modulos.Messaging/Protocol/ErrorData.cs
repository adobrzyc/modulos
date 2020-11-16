using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Modulos.Messaging.Protocol
{
    [DataContract]
    [SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Local")]
    public struct ErrorData
    {
        public ErrorData(string errorCode)
        {
            ErrorCode = errorCode;
            ErrorMessage = string.Empty;
        }

        public ErrorData(string errorCode, string errorMessage)
        {
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }

        [DataMember]
        public string ErrorCode { get; private set; }
        
        [DataMember]
        public string ErrorMessage { get; private set; }

        public override string ToString()
        {
            return $"[{ErrorCode}] - message : {ErrorMessage}";
        }
    }
}