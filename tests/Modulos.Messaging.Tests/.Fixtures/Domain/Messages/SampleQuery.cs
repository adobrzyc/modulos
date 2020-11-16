namespace Modulos.Messaging.Tests.Fixtures.Domain
{
    [TypeMark("C325D8B7-B07A-43C2-A640-6A28D9522AE9")]
    public class SampleQuery : IQuery<string>
    {
        public string Data { get; set; }
        public bool ThrowException { get; set; }
    }
}