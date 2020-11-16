using Modulos.Messaging.Diagnostics.Options;
using Modulos.Messaging.Protocol.Response.Definitions;

namespace Modulos.Messaging.Diagnostics.Activity.NewResponse
{
    public sealed class NewResponseActivity : IActivity
    {
        public IResponseData ResponseData { get; }

        public LogMark LogMark { get; }

        public NewResponseActivity(IResponseData responseData, LogMark mark)
        {
            ResponseData = responseData;
            LogMark = mark;
        }
    }
}