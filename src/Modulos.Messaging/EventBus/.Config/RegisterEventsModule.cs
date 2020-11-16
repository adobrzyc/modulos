using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Modulos.Messaging.EventBus.Config
{
    [UsedImplicitly]
    public class RegisterEventsModule : MicrosoftDiModule
    {
        private readonly ITypeExplorer typeExplorer;
        public override LoadOrder Order { get; } = LoadOrder.Internal;
        public override bool AutoLoad { get;} = true;

        public RegisterEventsModule(ITypeExplorer typeExplorer)
        {
            this.typeExplorer = typeExplorer;
        }

        public override void Load(IServiceCollection collection)
        {
            collection.AddTransient<IEventBus, EventBus>();

            var defaultOption = new AutoRegistrationOptionsAttribute(ServiceLifetime.Transient);

            var exportedTypes = typeExplorer.GetSearchableClasses()
                .Select(e=> new {Type = e, Options = e.GetCustomAttribute<AutoRegistrationOptionsAttribute>() ?? defaultOption})
                .ToArray();

            foreach (var t in exportedTypes)
            {
                if (TypeUtils.IsAssignableToGenericType(t.Type, typeof(IHandleEvent<>), out var concreteGeneric, out _))
                {
                    collection.Add(new ServiceDescriptor(concreteGeneric, t.Type, t.Options.ServiceLifetime));
                }
            }
        }
    }
}