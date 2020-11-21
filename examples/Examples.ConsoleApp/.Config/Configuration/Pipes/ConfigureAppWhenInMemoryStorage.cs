using System;
using System.Threading;
using System.Threading.Tasks;
using Examples.ConsoleApp.Storage;
using Modulos.Pipes;

// ReSharper disable ClassNeverInstantiated.Global

namespace Examples.ConsoleApp.Config
{
    public class ConfigureAppWhenInMemoryStorage : IOptionalPipe
    {
        private readonly InMemoryStorage storage;

        public ConfigureAppWhenInMemoryStorage(InMemoryStorage storage)
        {
            this.storage = storage;
        }

        public ValueTask<PipeResult> Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{GetType().Name}...");
            Console.WriteLine(storage.ToString());
            return new ValueTask<PipeResult>(PipeResult.Continue);
        }
    }
}