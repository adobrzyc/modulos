using Xunit;

namespace Modulos.Messaging.Tests.Fixtures.Collections
{
    [CollectionDefinition(nameof(InMemoryCollection))]
    public class InMemoryCollection : ICollectionFixture<ImMemoryEnv>
    {
    }
}