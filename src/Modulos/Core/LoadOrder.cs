// ReSharper disable UnusedMember.Global

namespace Modulos
{
    /// <summary>
    /// Defines load order.
    /// </summary>
    public enum LoadOrder
    {
        /// <summary>
        /// Reserved for core libraries e.q. modulos.
        /// </summary>
        Core,

        /// <summary>
        /// Reserved for elements located in the external libraries.
        /// </summary>
        Library,

        /// <summary>
        /// Reserved for elements located in the solution projects.
        /// </summary>
        Project,

        /// <summary>
        /// Reserved for elements located in the application projects (eq.: console app, web api).
        /// </summary>
        App,

        /// <summary>
        /// Reserved for elements located in the test projects.
        /// </summary>
        Test
    }
}