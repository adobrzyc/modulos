using System.Collections.Generic;

namespace Modulos.Pipes
{
    public class PipeResult
    {
        public PipeActionAfterExecute Action { get; }
        public IEnumerable<object> PublishedData { get; }

        public PipeResult(PipeActionAfterExecute action, params object[] data)
        {
            Action = action;
            PublishedData = data;
        }
    }
}