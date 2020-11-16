using System;

// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedMember.Global

namespace Modulos.Messaging
{
    public interface IMessageTypeProvider
    {
        /// <summary>
        /// Returns single matched <see cref="Type"/> based on specified <param name="typeInfo"></param>.
        /// </summary>
        /// <param name="typeInfo">
        /// Type information.
        /// </param>
        /// <param name="throwExceptionIfNotFound"></param>
        /// <returns>
        /// Instance of <see cref="Type"/> class or null if not found.
        /// </returns>
        Type FindType(TypeInfo typeInfo, bool throwExceptionIfNotFound);
    }
}