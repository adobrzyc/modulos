using System;
using Modulos.Messaging.Diagnostics.Options;

namespace Modulos.Messaging.Diagnostics.Activity.FatalError
{
    public sealed class FatalErrorActivity : IActivity
    {
        public Exception Error { get; }

        public string Message { get; }

        public LogMark LogMark { get; }

        public string ErrorDescription { get;  }

        public Type Host { get; }

        public FatalErrorActivity(Exception error, string message, LogMark mark, string errorDescription, Type host)
        {
            Error = error;
            Message = message;
            LogMark = mark;
            ErrorDescription = errorDescription;
            Host = host;
        }
    }
}