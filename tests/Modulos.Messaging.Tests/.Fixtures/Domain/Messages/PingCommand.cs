

// ReSharper disable once UnusedMember.Global

namespace Modulos.Messaging.Tests.Fixtures.Domain
{
    [TypeMark("076AFB1A-22D2-468D-8618-989FFB5C6FF2")]
    public class PingCommand : ICommand
    {
        public string Data { get; set; }
    }
}