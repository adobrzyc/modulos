namespace Modulos.Messaging.Tests.Fixtures.Domain
{
    [TypeMark("007D3466-9A5E-4141-B0D5-B10D1D2B6EFF")]
    public class ObjectQuery : IQuery<object>
    {
        public object Object { get; set; }
    }
}