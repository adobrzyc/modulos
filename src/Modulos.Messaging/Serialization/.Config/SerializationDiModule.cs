using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Modulos.Messaging.Serialization.Config
{
    public class SerializationDiModule : MicrosoftDiModule
    {
        public override LoadOrder Order { get; } = LoadOrder.Internal;
        public override bool AutoLoad { get;} = true;

        public override void Load(IServiceCollection collection)
        {
            foreach (var serializerType in GetType().Assembly.DefinedTypes
                .Where(type => type.IsClass && !type.IsAbstract)
                .Where(type => typeof(ISerializer).IsAssignableFrom(type)))
            {
                collection.AddSingleton(typeof(ISerializer), serializerType);
            }

            collection.AddSingleton<ISerializationAndCompressionProvider, SerializationAndCompressionProvider>();
        }
    }
}