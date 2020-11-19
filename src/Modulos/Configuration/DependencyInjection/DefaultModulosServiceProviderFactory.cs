using System;
using Microsoft.Extensions.DependencyInjection;

namespace Modulos
{
    /// <summary>
    /// Used to unify installation 'modulos' for .net core 3.1+ environments.
    /// May be used to register modulos app in .net 3.1+ environments.
    /// </summary>
    public sealed class DefaultModulosServiceProviderFactory : ModulosServiceProviderFactory<FakeBuilder>
    {
        public DefaultModulosServiceProviderFactory(
            ModulosApp modulos,
            IServiceCollection collection,
            Action<AutoRegistrationModule> modifier = null, params object[] parameters) 
            : base(modulos, ()=> new FakeBuilder(collection), modifier, parameters)
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