namespace Modulos
{
    using System;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Allow using <see cref="IServiceProviderFactory{TContainerBuilder}" /> with additional
    /// initialization parameters.
    /// </summary>
    public class ModulosServiceProviderFactory : ModulosServiceProviderFactoryBase<IServiceCollection>
    {
        /// <summary>
        /// Register modulos dependencies and executes defined DI modules.
        /// </summary>
        /// <param name="modulos">Instance of modulos app.</param>
        /// <param name="modifier">
        /// Allows changing modulus auto registration modules (e.q. prevents load).
        /// </param>
        /// <param name="parameters">
        /// Additional parameters available for auto registration modules.
        /// </param>
        public ModulosServiceProviderFactory(ModulosApp modulos,
            Action<AutoRegistrationModule> modifier = null, params object[] parameters)
            : base(modulos, collection => collection, modifier, parameters)
        {
        }

        protected override void Populate(IServiceCollection builder, IServiceCollection collection)
        {
            // intended do nothing
        }

        protected override IServiceProvider Build(IServiceCollection builder)
        {
            return builder.BuildServiceProvider();
        }
    }
}