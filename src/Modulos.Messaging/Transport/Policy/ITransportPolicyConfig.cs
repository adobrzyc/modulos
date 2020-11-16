using Modulos.Pipes;

namespace Modulos.Messaging.Transport.Policy
{
    public interface ITransportPolicyConfig
    {
        IPipeline Pipes { get; }
    }
}