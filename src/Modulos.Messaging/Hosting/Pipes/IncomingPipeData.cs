using Modulos.Messaging.Configuration;

namespace Modulos.Messaging.Hosting.Pipes
{
    public sealed class IncomingPipeData
    {
        public IMessage Message { get; }
        public IMessageConfig Config { get; }
        public IMessageHandler Handler { get; }
        public IInvocationContext Context { get; }
        public bool IsLocalInvocation { get; }

        public IncomingPipeData(IMessage message, 
            IMessageConfig config, 
            IMessageHandler handler, 
            IInvocationContext context,
            bool isLocalInvocation)
        {
            Message = message;
            Config = config;
            Handler = handler;
            Context = context;
            IsLocalInvocation = isLocalInvocation;
        }
    }
}