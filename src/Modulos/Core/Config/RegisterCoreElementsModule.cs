// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace Modulos.Config
{
    using Microsoft.Extensions.DependencyInjection;

    public class RegisterCoreElementsModule : MicrosoftDiModule
    {
        private readonly IAppInfo _appInfo;
        private readonly IAssemblyExplorer _assemblyExplorer;
        private readonly ITypeExplorer _typeExplorer;

        public RegisterCoreElementsModule(IAssemblyExplorer assemblyExplorer, ITypeExplorer typeExplorer, IAppInfo appInfo)
        {
            _assemblyExplorer = assemblyExplorer;
            _typeExplorer = typeExplorer;
            _appInfo = appInfo;
        }

        public override LoadOrder Order => LoadOrder.Core;
        public override bool AutoLoad => true;

        public override void Load(IServiceCollection collection)
        {
            collection.AddSingleton(_typeExplorer);
            collection.AddSingleton(_assemblyExplorer);
            collection.AddSingleton(_appInfo);
        }
    }
}