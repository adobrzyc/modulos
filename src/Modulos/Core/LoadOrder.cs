// ReSharper disable UnusedMember.Global
namespace Modulos
{
    /// <summary>
    /// Defines load order for some of modulos mechanisms. 
    /// </summary>
    public enum LoadOrder
    {
        /// <summary>
        /// Reserved for internal modulos usage.
        /// </summary>
        Internal,

        /// <summary>
        /// Reserved for elements located in external libraries.
        /// </summary>
        Library,

        /// <summary>
        /// Reserved for elements located in solution projects.
        /// </summary>
        Project,

        /// <summary>
        /// Reserved for elements located in application projects (eq.: console app, web api).
        /// </summary>
        App,

        /// <summary>
        /// Reserved for elements located in test projects.
        /// </summary>
        Test
    }
}