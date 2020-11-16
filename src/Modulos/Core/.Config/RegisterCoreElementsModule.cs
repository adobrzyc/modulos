using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable UnusedType.Global

namespace Modulos.Config
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class RegisterCoreElementsModule : MicrosoftDiModule
    {
        private readonly IAssemblyExplorer assemblyExplorer;
        private readonly ITypeExplorer typeExplorer;
        private readonly IAppInfo appInfo;

        public override LoadOrder Order { get; } = LoadOrder.Internal;
        public override bool AutoLoad { get;} = true;

        public RegisterCoreElementsModule(IAssemblyExplorer assemblyExplorer, ITypeExplorer typeExplorer, IAppInfo appInfo)
        {
            this.assemblyExplorer = assemblyExplorer;
            this.typeExplorer = typeExplorer;
            this.appInfo = appInfo;
        }
        public override void Load(IServiceCollection collection)
        {
            collection.AddSingleton(typeExplorer);
            collection.AddSingleton(assemblyExplorer);
            collection.AddSingleton(appInfo);
        }
    }
}