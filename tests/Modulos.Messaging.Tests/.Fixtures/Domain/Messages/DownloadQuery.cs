namespace Modulos.Messaging.Tests.Fixtures.Domain
{
    [TypeMark("74E375B8-4CF9-4EFF-80ED-F5652BBAE3DA")]
    public class DownloadQuery : IQuery<FileResult>
    {
        public string FileName { get; set; }
    }
}