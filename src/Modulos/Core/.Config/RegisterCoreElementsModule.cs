using Microsoft.Extensions.DependencyInjection;

// ReSharper disable UnusedType.Global

namespace Modulos.Config
{
    public class RegisterCoreElementsModule : MicrosoftDiModule
    {
        private readonly IAssemblyExplorer _assemblyExplorer;
        private readonly ITypeExplorer _typeExplorer;
        private readonly IAppInfo _appInfo;

        public override LoadOrder Order { get; } = LoadOrder.Internal;
        public override bool AutoLoad { get;} = true;

        public RegisterCoreElementsModule(IAssemblyExplorer assemblyExplorer, ITypeExplorer typeExplorer, IAppInfo appInfo)
        {
            _assemblyExplorer = assemblyExplorer;
            _typeExplorer = typeExplorer;
            _appInfo = appInfo;
        }
        public override void Load(IServiceCollection collection)
        {
            collection.AddSingleton(_typeExplorer);
            collection.AddSingleton(_assemblyExplorer);
            collection.AddSingleton(_appInfo);
        }
    }
}