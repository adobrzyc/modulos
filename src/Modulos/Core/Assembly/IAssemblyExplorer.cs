namespace Modulos
{
    using System.Reflection;

    /// <summary>
    /// Enables explore assemblies of modulos application.
    /// </summary>
    public interface IAssemblyExplorer
    {
        /// <summary>
        /// Available assemblies.
        /// </summary>
        Assembly[] Assemblies { get; }
    }
}