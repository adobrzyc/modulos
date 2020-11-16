// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Modulos
{
    /// <summary>
    /// Defines module during auto registration process. 
    /// </summary>
    public sealed class AutoRegistrationModule
    {
        /// <summary>
        /// Instance of the module.
        /// </summary>
        public IModule Instance { get; }
        
        public LoadOrder Order { get; set; }
        
        /// <summary>
        /// True if module should be loaded otherwise false.
        /// </summary>
        public bool AutoLoad { get; set; }

        public AutoRegistrationModule(IModule instance)
        {
            Instance = instance;
            AutoLoad = instance.AutoLoad;
            Order = instance.Order;
        }
    }
}