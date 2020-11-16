using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable UnusedMember.Global

namespace Modulos.Pipes
{
    public interface IPipeline : IEnumerable<Type> 
    {
        void Add<T>() where T : IPipe;
        void Add(Type pipeType);

        bool Remove<T>() where T : IPipe;

        void Insert<TPipeToFind, TPipeToInsert>(InsertType insertType)
            where TPipeToFind : IPipe
            where TPipeToInsert : IPipe;

        IEnumerable<Type> GetPipes();

        Task<IPipelineResult> Execute(CancellationToken cancellationToken, params object[] additionalReferences);

    }
}