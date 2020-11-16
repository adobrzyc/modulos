using Modulos.Pipes;

namespace Modulos.Messaging.Transport.Policy
{
    public class TransportPolicyConfig : ITransportPolicyConfig
    {
        public IPipeline Pipes { get; }

        public TransportPolicyConfig(IPipeline pipes)
        {
            Pipes = pipes;
        }
    }
}