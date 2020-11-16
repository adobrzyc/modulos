using Microsoft.Extensions.DependencyInjection;

namespace Modulos
{
    /// <summary>
    /// Microsoft dependency injection based module. 
    /// </summary>
    public abstract class MicrosoftDiModule : IModule<IServiceCollection>
    {
        public int ModuleOrder { get; } = 0;
        public abstract LoadOrder Order { get; }
        public abstract bool AutoLoad { get;  }
        public abstract void Load(IServiceCollection services);
    }
}