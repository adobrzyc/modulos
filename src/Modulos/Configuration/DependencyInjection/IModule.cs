﻿namespace Modulos
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Defines auto-loadable dependency injection module.
    /// </summary>
    public interface IModule
    {
        LoadOrder Order { get; }
        bool AutoLoad { get; }
    }

    /// <summary>
    ///     <inheritdoc cref="IModule" />
    /// </summary>
    /// <typeparam name="TBuilder">
    /// Defines builder type e.q: <see cref="IServiceCollection" />.
    /// </typeparam>
    public interface IModule<in TBuilder> : IModule
    {
        /// <summary>
        /// Register services for container represented by <paramref name="services" />.
        /// </summary>
        void Load(TBuilder services);
    }
}