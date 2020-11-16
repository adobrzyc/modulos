using System.Collections.Generic;

namespace Modulos.Messaging.Maintenance
{
    /// <summary>
    /// Source used to obtain information about registered instances of services.
    /// </summary>
    public interface IInstanceInfoSource
    {
        /// <summary>
        /// Unique source name. Max length 50.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Read data from source.
        /// </summary>
        /// <returns>Reader instances information.</returns>
        IEnumerable<IInstanceInfo> Read();
    }
}