// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Local

namespace Examples.Modulos
{
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using global::Modulos.Pipes;
    using Xunit;

    public class Pipeline_consume_data_examples
    {
        [Fact]
        public async Task passing_data_between_pipes()
        {
            var pipeline = new Pipeline();
            pipeline.Add<Pipe1>();
            pipeline.Add<Pipe2>();
            await pipeline.Execute(CancellationToken.None);
        }

        private class Pipe1 : IPipe
        {
            public string PipeData => "Pipe1 data";

            public ValueTask<PipeResult> Execute(CancellationToken cancellationToken)
            {
                var result = PipeResult.NewContinue("some extra data");
                return new ValueTask<PipeResult>(result);
            }
        }

        private class Pipe2 : IPipe
        {
            public Pipe2(Pipe1 pipe1, string someExtraData)
            {
                pipe1.PipeData.Should().Be("Pipe1 data");
                someExtraData.Should().Be("some extra data");
            }

            public ValueTask<PipeResult> Execute(CancellationToken cancellationToken)
            {
                return new ValueTask<PipeResult>(PipeResult.Continue);
            }
        }
    }
}