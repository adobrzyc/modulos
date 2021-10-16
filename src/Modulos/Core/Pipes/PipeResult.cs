namespace Modulos.Pipes
{
    using System.Collections.Generic;

    public class PipeResult
    {
        public static readonly PipeResult Continue = new(PipeActionAfterExecute.Continue);
        public static readonly PipeResult Break = new(PipeActionAfterExecute.Break);

        public PipeResult(PipeActionAfterExecute action, params object[] data)
        {
            Action = action;
            PublishedData = data;
        }

        public PipeActionAfterExecute Action { get; }
        public IEnumerable<object> PublishedData { get; }

        public static PipeResult NewContinue(params object[] data)
        {
            return new PipeResult(PipeActionAfterExecute.Continue, data);
        }

        public static PipeResult NewBreak(params object[] data)
        {
            return new PipeResult(PipeActionAfterExecute.Break, data);
        }
    }
}