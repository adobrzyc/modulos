using System.Text;
using BenchmarkDotNet.Loggers;

namespace Modulos.Benchmarks.Common
{
    public class XUnitLoggerForBenchmarkNet : ILogger
    {
        private readonly StringBuilder builder = new StringBuilder();

        public string Id => "Logger";

        public int Priority => 0;

        public void Write(LogKind kind, string text)
        {
            builder.Append($"{text}");
        }

        public void WriteLine()
        {
            builder.AppendLine();
        }

        public void WriteLine(LogKind kind, string text)
        {
            builder.AppendLine($"{text}");
        }

        public void Flush()
        {
            
        }

        public override string ToString()
        {
            return builder.ToString();
        }
    }
}