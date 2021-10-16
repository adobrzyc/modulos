// ReSharper disable UnusedMember.Global

namespace Modulos.Pipes
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IPipeline : IEnumerable<Type>
    {
        IEnumerable<Type> GetPipes();

        Task<IPipelineResult> Execute(CancellationToken cancellationToken, params object[] additionalReferences);


        void Add<T>() where T : IPipe;
        void Add(Type pipeType);

        void AddOrReplace<T>() where T : IPipe;
        void AddOrReplace(Type pipeType);

        bool Remove<T>() where T : IPipe;
        bool Remove(Type pipeType);

        void Insert<TPipeToFind, TPipeToInsert>(InsertMode mode)
            where TPipeToFind : IPipe
            where TPipeToInsert : IPipe;

        void Insert(InsertMode mode, Type pipeToFind, Type pipeToInsert);


        void TryRemoveAndAdd<T>() where T : IPipe;
        void TryRemoveAndAdd(Type pipeType);

        int IndexOf<T>() where T : IPipe;
        int IndexOf(Type pipeType);
    }
}