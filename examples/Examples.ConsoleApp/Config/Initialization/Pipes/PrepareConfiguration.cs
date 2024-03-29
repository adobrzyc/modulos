﻿// ReSharper disable ClassNeverInstantiated.Global

namespace Examples.ConsoleApp.Config
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Configuration.Memory;
    using Modulos.Pipes;

    public sealed class PrepareConfiguration : IPipe
    {
        public ValueTask<PipeResult> Execute(CancellationToken cancellationToken)
        {
            Console.WriteLine("PrepareConfiguration...");

            var builder = new ConfigurationBuilder();
            builder.Add(new MemoryConfigurationSource
            {
                InitialData = new[]
                {
                    new KeyValuePair<string, string>("AppVersion", "1.0.0"),
                    new KeyValuePair<string, string>("Storage", "InMemory")
                }
            });
            var config = builder.Build();

            var result = new PipeResult(PipeActionAfterExecute.Continue, config);

            return new ValueTask<PipeResult>(result);
        }
    }
}