namespace Modulos.Messaging.Tests.Fixtures.Domain
{
    [TypeMark("E56E9738-B040-4E5B-935C-5EF6D2A29EC2")]
    public class EnterChainQuery : IQuery<string>
    {
        public string Data { get; set; }
    }
}