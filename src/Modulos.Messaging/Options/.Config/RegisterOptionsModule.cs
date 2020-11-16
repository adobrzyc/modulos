using Microsoft.Extensions.DependencyInjection;
using Modulos.Messaging.Diagnostics.Options;
using Modulos.Messaging.Hosting.Options;

// ReSharper disable UnusedType.Global

namespace Modulos.Messaging.Options.Config
{
    public class RegisterOptionsModule  : MicrosoftDiModule
    {
        public override LoadOrder Order { get; } = LoadOrder.Internal;
        public override bool AutoLoad { get;} = true;

        public override void Load(IServiceCollection collection)
        {
            collection.AddSingleton<ILogMarkOption, LogMarkOption>();
            collection.AddSingleton<IOperationModeOption, OperationModeOption>();
        }
    }
}