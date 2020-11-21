using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Modulos.Pipes;
using Xunit;

// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Local

namespace Examples.Modulos
{
    public class Pipeline_optional_pipe_examples
    {
        [Fact]
        public async Task optional_pipe()
        {
            var pipeline = new Pipeline();
            pipeline.Add<OptionalPipe>();
            pipeline.Add<Pipe1>();
            pipeline.Add<Pipe2>();
            pipeline.Add<Pipe3>();
            var result = await pipeline.Execute(CancellationToken.None);
            result.GetOptional<OptionalPipe>().Should().NotBeNull();
            //that's because OptionalPipe breaks pipeline 
            result.GetOptional<Pipe3>().Should().BeNull();
        }

        private class OptionalPipe : IOptionalPipe
        {
            // until pipe2 will not show up OptionalPipe won't be created and executed
            public OptionalPipe(Pipe2 pipe2)
            {
                pipe2.Should().NotBeNull();
            }

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

        private class Pipe3 : IPipe
        {
            public ValueTask<PipeResult> Execute(CancellationToken cancellationToken)
            {
                return new ValueTask<PipeResult>(PipeResult.Continue);
            }
        }
    }
}