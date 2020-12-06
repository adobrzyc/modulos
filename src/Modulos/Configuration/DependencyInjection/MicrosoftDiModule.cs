using Microsoft.Extensions.DependencyInjection;

namespace Modulos
{
    /// <summary>
    /// Microsoft dependency injection based module. 
    /// </summary>
    public abstract class MicrosoftDiModule : IModule<IServiceCollection>
    {
        public abstract LoadOrder Order { get; }
        public abstract bool AutoLoad { get;  }
        public abstract void Load(IServiceCollection services);
    }
}