using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Modulos.Messaging.Diagnostics.Activity.FatalError;
using Modulos.Messaging.Diagnostics.Activity.FinishAction;
using Modulos.Messaging.Diagnostics.Activity.NewAction;
using Modulos.Messaging.Diagnostics.Activity.NewRequest;
using Modulos.Messaging.Diagnostics.Activity.NewResponse;
using Modulos.Messaging.Diagnostics.Activity.NewSecurityContext;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Protocol.Request.Definitions;
using Modulos.Messaging.Protocol.Response.Definitions;

namespace Modulos.Messaging.Diagnostics.Activity
{
    internal class ActivityPublisher : IActivityPublisher
    {
        private readonly INewResponseActivityProcessor newResponseActivityProcessor;
        private readonly INewRequestActivityProcessor newRequestActivityProcessor;
        private readonly IFatalErrorActivityProcessor fatalErrorActivityProcessor;
        private readonly IFinishActionActivityProcessor finishActionActivityProcessor;
        private readonly INewActionActivityProcessor newActionActivityProcessor;
        private readonly INewSecurityContextActivityProcessor newSecurityContextActivityProcessor;

        public bool Enabled { get; set; } = true;


        public ActivityPublisher(
            INewResponseActivityProcessor newResponseActivityProcessor, 
            INewRequestActivityProcessor newRequestActivityProcessor, 
            IFatalErrorActivityProcessor fatalErrorActivityProcessor, 
            IFinishActionActivityProcessor finishActionActivityProcessor, 
            INewActionActivityProcessor newActionActivityProcessor, INewSecurityContextActivityProcessor newSecurityContextActivityProcessor)
        {
            this.newResponseActivityProcessor = newResponseActivityProcessor;
            this.newRequestActivityProcessor = newRequestActivityProcessor;
            this.fatalErrorActivityProcessor = fatalErrorActivityProcessor;
            this.finishActionActivityProcessor = finishActionActivityProcessor;
            this.newActionActivityProcessor = newActionActivityProcessor;
            this.newSecurityContextActivityProcessor = newSecurityContextActivityProcessor;
        }


        public Task PublishNewAction([NotNull] IActionInfo newAction, [CanBeNull] IActionInfo previousAction)
        {
            if (!Enabled) 
                return Task.CompletedTask;

            return newActionActivityProcessor.Process( newAction,  previousAction);
        }

        //todo: redesign required (important)
        public Task PublishNewSecurityContext(IInvocationContext invocationContext)
        {
            if (!Enabled) 
                return Task.CompletedTask;

            return newSecurityContextActivityProcessor.Process(invocationContext);
        }

        public Task PublishActionFinished(IActionInfo action, IMetricBag metricBag, string reason, 
            InvocationPlace where, object relatedObject, object host, Exception error)
        {
            if (!Enabled) 
                return Task.CompletedTask;

            return finishActionActivityProcessor.Process(action, metricBag, reason, where, relatedObject, host, error);
        }

        public Task PublishFinishWithFatalError(Exception error, string message, object relatedObject, object host)
        {
            if (!Enabled) 
                return Task.CompletedTask;
            
            return fatalErrorActivityProcessor.Process(error, message, host.GetType(), relatedObject);
        }

        public Task PublishNewRequest(IRequestData request, InvocationPlace where, IMetricBag metricBag)
        {
            if (!Enabled) 
                return Task.CompletedTask;

            return newRequestActivityProcessor.Process(request, where, metricBag);
        }

        public Task PublishNewResponse(IResponseData response, IMetricBag metricBag)
        {
            if (!Enabled) 
                return Task.CompletedTask;

            return newResponseActivityProcessor.Process(response, metricBag);
        }
    }
}