using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

namespace Modulos.Messaging.OneOrManyExecutionRoutine
{
    /// <summary>
    /// Encapsulate "one or many" execution routine.
    /// </summary>
    /// <typeparam name="T">"Executor" type.</typeparam>
    internal interface IOneOrManyExecutionRoutine<out T> where T : class
    {
        int Count { get; }
        T Get();
        IEnumerable<T> GetAll();
        Task Execute(Func<T,Task> consume);
    }
}