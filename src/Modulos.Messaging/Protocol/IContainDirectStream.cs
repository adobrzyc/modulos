using System.IO;

// ReSharper disable once CheckNamespace
namespace Modulos.Messaging
{
    public interface IContainDirectStream
    {
        /// <summary>
        /// Defines media type for stream.
        /// </summary>
        string MediaType { get; set; }

        /// <summary>
        /// BEWARE! This member must never be serialized.
        /// </summary>
        Stream Stream { get; set; }
    }
}