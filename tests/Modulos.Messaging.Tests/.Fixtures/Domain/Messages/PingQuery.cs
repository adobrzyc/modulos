namespace Modulos.Messaging.Tests.Fixtures.Domain
{
    [TypeMark("BB08A2E1-87E1-44E6-B680-916547484F24")]
    public class PingQuery : IQuery<string>
    {
        public string Data { get; set; }
    }
}