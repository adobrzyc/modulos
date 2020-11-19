using System.Collections.Generic;

namespace Modulos.Pipes
{
    public class PipeResult
    {
        public static readonly PipeResult Continue = new PipeResult(PipeActionAfterExecute.Continue);
        public static readonly PipeResult Break = new PipeResult(PipeActionAfterExecute.Break);

        public PipeActionAfterExecute Action { get; }
        public IEnumerable<object> PublishedData { get; }

        public PipeResult(PipeActionAfterExecute action, params object[] data)
        {
            Action = action;
            PublishedData = data;
        }
    }
}