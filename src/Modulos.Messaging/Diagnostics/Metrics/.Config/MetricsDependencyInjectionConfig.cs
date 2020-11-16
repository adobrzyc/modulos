using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Modulos.Messaging.Diagnostics.Metrics.Config
{
    public class MetricsDependencyInjectionConfig: MicrosoftDiModule
    {
        public override LoadOrder Order { get; } = LoadOrder.Internal;
        public override bool AutoLoad { get;} = true;

        public override void Load(IServiceCollection collection)
        {
            foreach (var estimatorType in GetType().Assembly.ExportedTypes.Where(type => !type.IsAbstract && type.IsClass && type.IsPublic)
                .Where(type => typeof(IResultEstimator).IsAssignableFrom(type)))
            {
                collection.AddTransient(typeof(IResultEstimator), estimatorType);
                collection.AddTransient(estimatorType, estimatorType);
            }
        }
    }
}