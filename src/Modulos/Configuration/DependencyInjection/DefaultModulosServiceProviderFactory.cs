using System;
using Microsoft.Extensions.DependencyInjection;

namespace Modulos
{
    /// <summary>
    /// Used to unify installation 'modulos' for .net core 3.1+ environments.
    /// May be used to register modulos app in .net 3.1+ environments.
    /// </summary>
    public class ModulosServiceProviderFactory : ModulosServiceProviderFactoryBase<FakeBuilder>
    {
        /// <summary>
        /// Register modulos dependencies and executes defined DI modules.
        /// </summary>
        /// <param name="modulos">Instance of modulos app.</param>
        /// <param name="modifier"></param>
        /// <param name="parameters"></param>
        public ModulosServiceProviderFactory(ModulosApp modulos,
            Action<AutoRegistrationModule> modifier = null, params object[] parameters) 
            : base(modulos, collection => new FakeBuilder(collection), modifier, parameters)
        {
        }
        
        protected override void Populate(FakeBuilder builder, IServiceCollection collection)
        {
            builder.Populate(collection);
        }

        protected override IServiceProvider Build(FakeBuilder builder)
        {
            return builder.Build();
        }
    }
}