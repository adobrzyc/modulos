using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Modulos.Pipes;
using Moq;
using Xunit;

// ReSharper disable InconsistentNaming
// ReSharper disable ClassNeverInstantiated.Local

namespace Modulos.Tests.Unit.Pipes
{
    public class Pipeline_execution_tests
    {
        [Fact]
        public async Task empty_pipe()
        {
            var sp = new Mock<IServiceProvider>();
            var pipeline = new Pipeline(sp.Object);

            var result = await pipeline.Execute(CancellationToken.None);
            result.GetAll().Should().HaveCount(0);
        }

        [Fact]
        public async Task break_pipe()
        {
            var sp = new Mock<IServiceProvider>();
            var pipeline = new Pipeline(sp.Object);

            pipeline.Add<Pipe1>();
            pipeline.Add<Pipe2>();
            pipeline.Add<Break>();
            pipeline.Add<Pipe3>();
            
            var result = await pipeline.Execute(CancellationToken.None);
            result.GetAll().Should().HaveCount(3);

            result.GetOptional<Pipe1>().Should().NotBeNull();
            result.GetOptional<Pipe2>().Should().NotBeNull();
            result.GetOptional<Break>().Should().NotBeNull();
            result.GetOptional<Pipe3>().Should().BeNull();
            
            var logic = new Action(() =>
            {
                result.Get<Pipe3>();
            });

            logic.Should().ThrowExactly<MissingDataFromPipelineResultException>();

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

        private class Break : IPipe
        {
            public ValueTask<PipeResult> Execute(CancellationToken cancellationToken)
            {
                return new ValueTask<PipeResult>(PipeResult.Break);
            }
        }
    }
}