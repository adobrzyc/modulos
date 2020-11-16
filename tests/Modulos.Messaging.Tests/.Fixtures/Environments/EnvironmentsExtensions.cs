using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Messaging.Tests.Fixtures.Blocks;
using Modulos.Testing;

namespace Modulos.Messaging.Tests.Fixtures
{
    public static class EnvironmentsExtensions
    {
        [UsedImplicitly]
        public static ITestEnvironment UpdateIoc(this ITestEnvironment env, Action<IServiceCollection> update)
        {
            if (env.IndexOf<InitializeIoc>() < 0)
                throw new NotSupportedException(
                    $"Specific env: {env.GetType().Name} does not contains: {nameof(InitializeIoc)} block.");

            env.Update<InitializeIoc>((block, environment, prevSetup) =>
            {
                prevSetup(block);
                var old = block.RegisterServices;
                block.RegisterServices = (hydra, collection) =>
                {
                    old(hydra, collection);
                    update(collection);
                };
            });
            return env;
        }
    }
}