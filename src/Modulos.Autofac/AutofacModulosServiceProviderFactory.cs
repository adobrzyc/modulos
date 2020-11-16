using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// ReSharper disable UnusedType.Global

namespace Modulos.Autofac
{
    public class AutofacModulosServiceProviderFactory : ModulosServiceProviderFactory<ContainerBuilder>
    {
        public AutofacModulosServiceProviderFactory(ModulosApp modulos, HostBuilderContext context, Action<AutoRegistrationModule> modifier = null) 
            : base
            (
                modulos,
                () => new ContainerBuilder(), 
                modifier,
                context, context.Configuration, context.HostingEnvironment
            )
        {
        }

        protected override void Populate(ContainerBuilder builder, IServiceCollection collection)
        {
            builder.Populate(collection);
        }

        protected override IServiceProvider Build(ContainerBuilder builder)
        {
            return new AutofacServiceProvider(builder.Build());
        }
    }
}