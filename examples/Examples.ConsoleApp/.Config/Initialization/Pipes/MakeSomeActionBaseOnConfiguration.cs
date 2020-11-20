using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Modulos.Pipes;

// ReSharper disable ClassNeverInstantiated.Global

namespace Examples.ConsoleApp.Config
{
    public class MakeSomeActionBaseOnConfiguration : IPipe
    {
        // pipes can use previous pipes data 
        private readonly IConfiguration config;

        public MakeSomeActionBaseOnConfiguration(IConfiguration config)
        {
            this.config = config;
        }

        public Task<PipeResult> Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine("MakeSomeActionBaseOnConfiguration...");
            foreach (var pair in config.AsEnumerable())
            {
                Console.WriteLine(pair);
            }
            
            return Task.FromResult(PipeResult.Continue);
        }
    }
}