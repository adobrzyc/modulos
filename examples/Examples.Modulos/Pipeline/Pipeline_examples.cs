// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Local

namespace Examples.Modulos
{
    using System.Threading;
    using System.Threading.Tasks;
    using global::Modulos.Pipes;
    using Xunit;

    public class Pipeline_examples
    {
        [Fact]
        public async Task simple_execution()
        {
            var pipeline = new Pipeline();
            pipeline.Add<Pipe1>();
            pipeline.Add<Pipe2>();
            await pipeline.Execute(CancellationToken.None);
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