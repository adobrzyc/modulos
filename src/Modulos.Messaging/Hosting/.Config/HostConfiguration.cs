using Modulos.Pipes;

namespace Modulos.Messaging.Hosting.Config
{
    //todo: [plan for a future] check it 
    public class HostConfiguration : IHostConfiguration
    {
        public IPipeline IncomingPipes { get; }
        public IPipeline OutgoingPipes { get; }

        public HostConfiguration(IPipeline incomingPipes, IPipeline outgoingPipes)
        {
            IncomingPipes = incomingPipes;
            OutgoingPipes = outgoingPipes;
        }
    }
}