namespace Modulos.Messaging.Serialization
{
    /// <summary>
    /// Handles serialization and deserialization process.
    /// </summary>
    public interface ISerializer 
    {
        /// <summary>
        /// Defines unique serialization engine identifier.
        /// </summary>
        SerializerId Id { get; }

        /// <summary>
        /// Specified serialization transportId.
        /// </summary>
        SerializationKind Kind { get; }

        /// <summary>
        /// Defines media type of serialized data.
        /// </summary>
        string MediaType { get; } 
    }
}