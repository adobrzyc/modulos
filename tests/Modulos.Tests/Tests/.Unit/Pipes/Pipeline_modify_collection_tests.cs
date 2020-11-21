using System;
using System.Linq;
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
    public class Pipeline_modify_collection_tests
    {
        [Fact]
        public void Add()
        {
            var sp = new Mock<IServiceProvider>();
            var pipeline = new Pipeline(sp.Object);
            pipeline.Add<Pipe1>();
            pipeline.Add<Pipe2>();
            pipeline.Count().Should().Be(2);
            pipeline.IndexOf<Pipe1>().Should().Be(0);
            pipeline.IndexOf<Pipe2>().Should().Be(1);
        }

        [Fact]
        public void Add_same_pipe()
        {
            var sp = new Mock<IServiceProvider>();
            var pipeline = new Pipeline(sp.Object);
            
            var logic = new Action(() =>
            {
                pipeline.Add<Pipe1>();
                pipeline.Add<Pipe1>();
            });

            logic.Should().ThrowExactly<ArgumentException>();
        }
       
        [Fact]
        public void Add_wrong_type__FORBIDDEN()
        {
            var sp = new Mock<IServiceProvider>();
            var pipeline = new Pipeline(sp.Object);
            
            var logic = new Action(() =>
            {
                pipeline.Add(typeof(NotPipe));
            });

            logic.Should().ThrowExactly<ArgumentException>();
        }


        [Fact]
        public void AddOrReplace()
        {
            var sp = new Mock<IServiceProvider>();
            var pipeline = new Pipeline(sp.Object);
            
            pipeline.Add<Pipe1>();
            pipeline.Add<Pipe2>();
            pipeline.Add<Pipe3>();
            pipeline.AddOrReplace<Pipe2>();
            pipeline.Count().Should().Be(3);
            pipeline.IndexOf<Pipe2>().Should().Be(1);
        }

        [Fact]
        public void AddOrReplace_wrong_type__FORBIDDEN()
        {
            var sp = new Mock<IServiceProvider>();
            var pipeline = new Pipeline(sp.Object);
            
            var logic = new Action(() =>
            {
                pipeline.AddOrReplace(typeof(NotPipe));
            });

            logic.Should().ThrowExactly<ArgumentException>();
        }


        [Fact]
        public void Remove()
        {
            var sp = new Mock<IServiceProvider>();
            var pipeline = new Pipeline(sp.Object);
            
            pipeline.Add<Pipe1>();
            pipeline.Add<Pipe2>();
            pipeline.Add<Pipe3>();
            pipeline.Remove<Pipe2>();
            pipeline.Count().Should().Be(2);
            pipeline.IndexOf<Pipe1>().Should().Be(0);
            pipeline.IndexOf<Pipe2>().Should().Be(-1);
            pipeline.IndexOf<Pipe3>().Should().Be(1);
        }

        [Fact]
        public void Remove_non_existing()
        {
            var sp = new Mock<IServiceProvider>();
            var pipeline = new Pipeline(sp.Object);
            
            pipeline.Add<Pipe1>();
            pipeline.Add<Pipe3>();
            pipeline.Remove<Pipe2>().Should().BeFalse();
            pipeline.Count().Should().Be(2);
            pipeline.IndexOf<Pipe1>().Should().Be(0);
            pipeline.IndexOf<Pipe2>().Should().Be(-1);
            pipeline.IndexOf<Pipe3>().Should().Be(1);
        }
       
        [Fact]
        public void Remove__wrong_type__FORBIDDEN()
        {
            var sp = new Mock<IServiceProvider>();
            var pipeline = new Pipeline(sp.Object);
            
            var logic = new Action(() =>
            {
                pipeline.Remove(typeof(NotPipe));
            });

            logic.Should().ThrowExactly<ArgumentException>();
        }



        [Fact]
        public void Insert_before()
        {
            var sp = new Mock<IServiceProvider>();
            var pipeline = new Pipeline(sp.Object);
            
            pipeline.Add<Pipe1>();
            pipeline.Add<Pipe3>();
            pipeline.Insert<Pipe1,Pipe2>(InsertType.Before);
            pipeline.Count().Should().Be(3);
            pipeline.IndexOf<Pipe1>().Should().Be(1);
            pipeline.IndexOf<Pipe2>().Should().Be(0);
            pipeline.IndexOf<Pipe3>().Should().Be(2);
        }

        [Fact]
        public void Insert_after()
        {
            var sp = new Mock<IServiceProvider>();
            var pipeline = new Pipeline(sp.Object);
            
            pipeline.Add<Pipe1>();
            pipeline.Add<Pipe3>();
            pipeline.Insert<Pipe1,Pipe2>(InsertType.After);
            pipeline.Count().Should().Be(3);
            pipeline.IndexOf<Pipe1>().Should().Be(0);
            pipeline.IndexOf<Pipe2>().Should().Be(1);
            pipeline.IndexOf<Pipe3>().Should().Be(2);
        }

        [Fact]
        public void Insert_wrong_type__FORBIDDEN()
        {
            var sp = new Mock<IServiceProvider>();
            var pipeline = new Pipeline(sp.Object);
            var logic = new Action(() =>
            {
                pipeline.Insert(InsertType.Before, typeof(NotPipe),typeof(Pipe1));
            });

            logic.Should().ThrowExactly<ArgumentException>();

            logic = () =>
            {
                pipeline.Insert(InsertType.Before, typeof(Pipe1),typeof(NotPipe));
            };

            logic.Should().ThrowExactly<ArgumentException>();
        }



        [Fact]
        public void RemoveOldAndAdd()
        {
            var sp = new Mock<IServiceProvider>();
            var pipeline = new Pipeline(sp.Object);
            
            pipeline.Add<Pipe1>();
            pipeline.Add<Pipe2>();
            pipeline.Add<Pipe3>();
            pipeline.TryRemoveAndAdd<Pipe2>();
            pipeline.Count().Should().Be(3);
            pipeline.IndexOf<Pipe1>().Should().Be(0);
            pipeline.IndexOf<Pipe2>().Should().Be(2);
            pipeline.IndexOf<Pipe3>().Should().Be(1);
        }

        [Fact]
        public void RemoveOldAndAdd_when_not_exist()
        {
            var sp = new Mock<IServiceProvider>();
            var pipeline = new Pipeline(sp.Object);
            
            pipeline.Add<Pipe1>();
            pipeline.Add<Pipe3>();
            pipeline.TryRemoveAndAdd<Pipe2>();
            pipeline.Count().Should().Be(3);
            pipeline.IndexOf<Pipe1>().Should().Be(0);
            pipeline.IndexOf<Pipe2>().Should().Be(2);
            pipeline.IndexOf<Pipe3>().Should().Be(1);
        }




        private class NotPipe
        {

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