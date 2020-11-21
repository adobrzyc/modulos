using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Modulos.Pipes;
using Xunit;

// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Local

namespace Examples.Modulos
{
    public class Pipeline_break_pipeline_examples
    {
        [Fact]
        public async Task break_pipeline()
        {
            var pipeline = new Pipeline();
            pipeline.Add<Pipe1>();
            pipeline.Add<BreakPipe>();
            pipeline.Add<Pipe2>();
            var result = await pipeline.Execute(CancellationToken.None);
            result.GetOptional<Pipe2>().Should().BeNull();
        }

        private class BreakPipe : IPipe
        {
            public ValueTask<PipeResult> Execute(CancellationToken cancellationToken)
            {
                return new ValueTask<PipeResult>(PipeResult.Break);
            }
        }

        private class Pipe1 : IPipe
        {
            public ValueTask<PipeResult> Execute(CancellationToken cancellationToken)
            {
                return new ValueTask<PipeResult>(PipeResult.Continue);
            }
        }

        private class Pipe2 : IPipe
        {
            public ValueTask<PipeResult> Execute(CancellationToken cancellationToken)
            {
                return new ValueTask<PipeResult>(PipeResult.Continue);
            }
        }
    }
}