using Modulos.Pipes;

namespace Modulos.Messaging.Hosting.Config
{
    public interface IHostConfiguration
    {
        IPipeline IncomingPipes { get; }
        IPipeline OutgoingPipes { get; }
    }
}