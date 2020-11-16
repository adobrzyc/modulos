

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Modulos.Messaging.Tests.Fixtures.Domain
{
    [TypeMark("BF69D381-0DB7-438F-BECB-8C7516B23007")]
    public class EndChainQuery : IQuery<string>
    {
        public string Data { get; set; }
    }
}