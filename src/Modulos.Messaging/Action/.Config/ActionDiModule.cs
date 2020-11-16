using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable UnusedType.Global

namespace Modulos.Messaging.Config
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class ActionDiModule : MicrosoftDiModule
    {
        public override LoadOrder Order { get; } = LoadOrder.Internal;
        public override bool AutoLoad { get;} = true;

        public override void Load(IServiceCollection collection)
        {
            collection.AddSingleton<IActionInfoFactory, ActionInfoFactory>();
        }
    }
}