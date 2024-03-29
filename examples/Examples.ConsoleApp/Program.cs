﻿// ReSharper disable UnusedType.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Examples.ConsoleApp
{
    using Microsoft.Extensions.DependencyInjection;
    using Modulos;

    internal class Program
    {
        private static void Main()
        {
            // 1. initialize
            var modulosApp = new ModulosApp();
            var iniResult = modulosApp.Initialize();


            // 2. organize dependency injection 
            var sc = new ServiceCollection();
            sc.AddModulos
            (
                modulosApp,
                // data from initialization pipeline, will be available for DI containers
                iniResult.GetAll()
            );
            var sp = sc.BuildServiceProvider();

            // 3. configure after dependency injection 
            modulosApp.Configure(sp);
        }
    }
}