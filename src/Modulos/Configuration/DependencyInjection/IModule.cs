using Microsoft.Extensions.DependencyInjection;

namespace Modulos
{
    /// <summary>
    /// Defines auto-loadable dependency injection module.
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// Defines order for modules with the same <see cref="Order"/>.
        /// </summary>
        int ModuleOrder { get; }
        LoadOrder Order { get; }
        bool AutoLoad { get; }
    }

    /// <summary>
    /// <inheritdoc cref="IModule"/>
    /// </summary>
    /// <typeparam name="TBuilder">
    /// Defines builder type e.q: <see cref="IServiceCollection"/>.
    /// </typeparam>
    public interface IModule<in TBuilder> : IModule
    {
        /// <summary>
        /// Register services for container represented by <paramref name="services"/>.
        /// </summary>
        void Load(TBuilder services);
    }
}