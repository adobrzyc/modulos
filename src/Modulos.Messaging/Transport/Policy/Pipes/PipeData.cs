using System;
using Modulos.Messaging.Configuration;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Modulos.Messaging.Transport.Policy.Pipes
{
    internal class PipeData
    {
        public IMessage Message { get; }
        public IMessageConfig Config { get; }
        public IInvocationContext Context { get; }
        public ITransportEngine TransportEngine { get; }
        public int InvocationCounter { get; }
        public Type ResultType { get; }

        public PipeData(IMessage message, IMessageConfig config, IInvocationContext context, 
            ITransportEngine transportEngine, int invocationCounter, Type resultType)
        {
            Message = message;
            Config = config;
            Context = context;
            TransportEngine = transportEngine;
            InvocationCounter = invocationCounter;
            ResultType = resultType;
        }
    }
}