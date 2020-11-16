using Modulos.Messaging.Diagnostics.Options;
using Modulos.Messaging.Protocol.Request.Definitions;

namespace Modulos.Messaging.Diagnostics.Activity.NewRequest
{
    public sealed class NewRequestActivity : IActivity
    {
        public IRequestData RequestData { get; }

        public LogMark LogMark { get; }

        public NewRequestActivity(IRequestData requestData, LogMark mark)
        {
            RequestData = requestData;
            LogMark = mark;
        }
    }
}