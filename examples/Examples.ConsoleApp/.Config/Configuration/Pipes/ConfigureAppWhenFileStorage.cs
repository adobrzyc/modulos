using System;
using System.Threading;
using System.Threading.Tasks;
using Examples.ConsoleApp.Storage;
using Modulos.Pipes;

// ReSharper disable ClassNeverInstantiated.Global

namespace Examples.ConsoleApp.Config
{
    // pipes can be optional, created and executed only if all params in ctor are available 
    // this pipe will not be created either executed, because FileStorage is not available
    public class ConfigureAppWhenFileStorage : IOptionalPipe
    {
        private readonly FileStorage storage;

        public ConfigureAppWhenFileStorage(FileStorage storage)
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