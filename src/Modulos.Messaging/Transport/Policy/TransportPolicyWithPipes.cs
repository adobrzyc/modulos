using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Modulos.Errors;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Diagnostics.Activity;
using Modulos.Messaging.Diagnostics.Annotations;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Protocol;
using Modulos.Messaging.Protocol.Request.Definitions;
using Modulos.Messaging.Protocol.Response.Definitions;
using Modulos.Messaging.Transport.Policy.Pipes;
using Modulos.Pipes;
using Kind = Modulos.Messaging.Diagnostics.Metrics.Kind;

namespace Modulos.Messaging.Transport.Policy
{
    [Verified]
    // ReSharper disable once UnusedType.Global
    internal class TransportPolicyWithPipes : ITransportPolicy, IActionHost
    {
        #region Fields

        public string HostName { get; } = nameof(TransportPolicyWithPipes);

        private readonly ITransportEngineProvider transportEngineProvider;
        private readonly IExceptionTransformer exceptionTransformer;
        private readonly IActivityPublisher activityPublisher;
        private readonly IMessageConfigProvider messageConfigProvider;
        private readonly IActionInfoFactory actionInfoFactory;
       // private readonly ISecurityContextFactory securityContextFactory;
        private readonly IEndpointConfigProvider endpointConfigProvider;
        private readonly ITransportPolicyConfig transportConfig;

        #endregion

        public TransportPolicyWithPipes(ITransportEngineProvider transportEngineProvider,
            IExceptionTransformer exceptionTransformer,
            IActivityPublisher activityPublisher,
            IMessageConfigProvider messageConfigProvider,
            IActionInfoFactory actionInfoFactory,
            //ISecurityContextFactory securityContextFactory,
            IEndpointConfigProvider endpointConfigProvider, 
            ITransportPolicyConfig transportConfig)
        {
            this.transportEngineProvider = transportEngineProvider;
            this.exceptionTransformer = exceptionTransformer;
            this.activityPublisher = activityPublisher;
            this.messageConfigProvider = messageConfigProvider;
            this.actionInfoFactory = actionInfoFactory;
            //this.securityContextFactory = securityContextFactory;
            this.endpointConfigProvider = endpointConfigProvider;
            this.transportConfig = transportConfig;
        }


        public Task<TResult> ExecuteQuery<TResult>(IQueryBase query, Action<IMessageConfig> configModification, CancellationToken token, InvocationContext context)
        {
            return Execute<TResult>(query, configModification, context, token);
        }

        public Task ExecuteCommand(ICommandBase command, Action<IMessageConfig> configModification, CancellationToken token, InvocationContext context)
        {
            return Execute<int>(command, configModification, context, token);
        }


        [Verified]
        private async Task<TResult> Execute<TResult>([NotNull] IMessage message,
            Action<IMessageConfig> configModification,
            [CanBeNull] InvocationContext invocationContext,
            CancellationToken token)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            var invocationCounter = 0;

            IMetricBag metricBag = new MetricBag();

            invocationContext = await PrepareContext(message, invocationContext);

            var messageConfig = await ObtainMessageConfiguration(message, configModification, invocationContext, metricBag);

            if (messageConfig.TransportEngine.Value == null)
            {
                throw new TodoException($"Configuration for message: {message.GetType().FullName} is not correct." +
                                        "Transport engine is not set.");
            }

            var transportEngine = await ObtainTransportEngine(message, invocationContext, messageConfig, metricBag);

            var isFirstCall = true;

            while (true)
            {
                invocationCounter++;
                ITransferObject response = null;
                ICreatedRequestData requestData = null;
                Exception error = null;
                IPipelineResult pipelineResult = null;

                try
                {
                    if (invocationCounter > messageConfig.MaxInvokeAttempts)
                        throw new ServiceIsUnavailableException(messageConfig.EndpointConfigMark,
                            "maximum invocations count exceeded");

                    if (invocationCounter > 1)
                    {
                        var newAction = actionInfoFactory.Create(message, this, invocationContext.Action.OperationId);
                        await activityPublisher.PublishNewAction(newAction, previousAction: invocationContext.Action);
                        invocationContext = new InvocationContext(newAction);
                    }

                    token.ThrowIfCancellationRequested();

                    var pipeData = new PipeData
                    (
                        message,
                        messageConfig,
                        invocationContext,
                        transportEngine,
                        invocationCounter,
                        typeof(TResult)
                    );

                    pipelineResult = await transportConfig.Pipes.Execute(token, pipeData, metricBag);
                    
                    requestData = pipelineResult.Get<ICreatedRequestData>();
                    response = pipelineResult.Get<ITransferObject>();
                    var parsedResponseData = pipelineResult.Get<IParsedResponseData<TResult>>();

                    return parsedResponseData.Result;

                }
                catch (Exception ex)
                {
                    error = ex;

                    if (ex is ServiceIsUnavailableException && invocationCounter > messageConfig.MaxInvokeAttempts)
                        throw;

                    var action = ActionAfterHandleException.NotSet;

                    try
                    {
                        action = await HandleException(transportEngine, isFirstCall, ex,
                            messageConfig.EndpointConfigMark, requestData, response, metricBag);

                        switch (action)
                        {
                            case ActionAfterHandleException.Continue:
                                break;
                            case ActionAfterHandleException.Throw:
                                throw;
                            case ActionAfterHandleException.DecreaseInvocationCounter:
                                invocationCounter--;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(action), "Unable to handle exception.");
                        }
                    }
                    catch (Exception ex2)
                    {
                        if (action != ActionAfterHandleException.Throw)
                            throw error = new AggregateException(ex2.Message, ex, ex2);
                        throw;
                    }
                }
                finally
                {
                    isFirstCall = false;

                    await activityPublisher.PublishActionFinished
                    (
                        invocationContext.Action, metricBag, "request proceeded",
                        InvocationPlace.Caller, message, this, error
                    );

                    if (pipelineResult != null)
                        await pipelineResult.DisposeAsync();
                }
            }
        }

        [Verified]
        [NotMeasuredOnPurpose]
        private async Task<ITransportEngine> ObtainTransportEngine(IMessage message, InvocationContext invocationContext, IMessageConfig messageConfig, IMetricBag metricBag)
        {
            ITransportEngine transportEngine;

            try
            {
                transportEngine = transportEngineProvider.GetTransportEngine(messageConfig.TransportEngine);
                if (transportEngine == null)
                    throw new TodoException($"Unable to determine transport engine: {messageConfig.TransportEngine.ToString()}");

                await transportEngine.Validate(message, messageConfig, metricBag, invocationContext);
            }
            catch (Exception error)
            {
                await activityPublisher.PublishActionFinished
                (
                    action: invocationContext.Action,
                    metricBag: metricBag,
                    reason: "error during determine transport engine",
                    @where: InvocationPlace.Caller,
                    relatedObject: message,
                    host: this,
                    error: error
                );

                throw;
            }

            return transportEngine;
        }

        [Verified]
        [NotMeasuredOnPurpose]
        private async Task<InvocationContext> PrepareContext(IMessage message, InvocationContext invocationContext)
        {
            try
            {
                var newAction = actionInfoFactory.Create(message, this);

                if (invocationContext == null)
                {
                    //var securityContext = await securityContextFactory.CreateAsLoggedUser();
                    invocationContext = new InvocationContext(newAction);
                    await activityPublisher.PublishNewAction(newAction, previousAction: null);
                }
                else
                {
                    //var securityContext = (SecurityContext) invocationContext.SecurityContext.Clone();
                    var previousAction = invocationContext.Action;
                    invocationContext = new InvocationContext(newAction);
                    await activityPublisher.PublishNewAction(newAction, previousAction);
                }
            }
            catch (Exception error)
            {
                // #design-concept
                // Error during creating new context and action should not be associated
                // with an old context or action (of course it may not exists anyway).
                // Every error during context creation should be treated as a fatal error.
                await activityPublisher.PublishFinishWithFatalError(error, "unable to create modulos context", message, this);

                throw;
            }

            return invocationContext;
        }

        [Verified]
        private async Task<IMessageConfig> ObtainMessageConfiguration(IMessage message,
            Action<IMessageConfig> configModification,
            IInvocationContext invocationContext,
            IMetricBag metricBag)
        {
            var t1 = invocationContext.Action.ElapsedMilliseconds;
            Exception error = null;

            try
            {
                var config = messageConfigProvider.GetConfig(message);

                configModification?.Invoke(config);

                config.Freeze();

                return config;
            }
            catch (Exception ex)
            {
                error = ex;
                throw;
            }
            finally
            {
                metricBag.Add
                (
                    Kind.Configuration,
                    "configure message",
                    InvocationPlace.Caller,
                    invocationContext.Action.ElapsedMilliseconds - t1,
                    invocationContext.Action
                );

                if (error != null)
                {
                    await activityPublisher.PublishActionFinished
                    (
                        action: invocationContext.Action,
                        metricBag: metricBag,
                        reason: "error during configure message",
                        @where: InvocationPlace.Caller,
                        relatedObject: message, host: this, error: error
                    );
                }
            }
        }

        //[Verified]
        //private async Task HandleSecurity(IMessageConfig messageConfig, IInvocationContext invocationContext, IMetricBag metricBag)
        //{
        //    var t1 = invocationContext.Action.ElapsedMilliseconds;

        //    try
        //    {
        //        var signed = await securityContextSigner.Sign(messageConfig.AuthenticationMode, invocationContext.SecurityContext);
        //        invocationContext.SecurityContext = signed;
        //        await activityPublisher.PublishNewSecurityContext(invocationContext);
        //    }
        //    finally
        //    {
        //        metricBag.Add
        //        (
        //            Schema.Security,
        //            "sign",
        //            InvocationPlace.Caller,
        //            invocationContext.Action.ElapsedMilliseconds - t1,
        //            invocationContext.Action
        //        );
        //    }
        //}

        [Verified]
        [NotMeasuredOnPurpose]
        private async Task<ActionAfterHandleException> HandleException(ITransportEngine transportEngine, bool isFirstCall,
            Exception ex, EndpointConfigMark endpointConfigMark, IRequestData requestData, ITransferObject response, IMetricBag metricBag)
        {
            // if it's transport exception then request should be logged on client side;
            // there is possibility to log request in both sides client and server 
            if (transportEngine.IsTransportException(ex) && requestData != null)
            {
                await activityPublisher.PublishNewRequest(requestData, InvocationPlace.Caller, metricBag);
            }

            requestData?.TransferObject?.Stream?.Dispose();
            response?.Stream?.Dispose();


            // each error forces configuration refreshes, it should not be a problem because exceptions should be exceptional :)
            await endpointConfigProvider.Discard(endpointConfigMark);

            // timeout acts like transport exception
            if (ex is TimeoutException)
                return ActionAfterHandleException.Continue;

            // non understandable exceptions must be throw as they're - there's no point to wrap them into ModulosException 
            if (!transportEngine.IsTransportException(ex) && !(ex is ModulosException))
                return ActionAfterHandleException.Throw;

            ModulosException modulosException;
            if (ex is ModulosException baseException)
                modulosException = baseException;
            else
            {
                if (!exceptionTransformer.ToModulosException(ex, out modulosException))
                {
                    // unknown modulos exception 
                    return ActionAfterHandleException.Continue;
                }
            }

            // reconfigure required or re-callable exception give us one more chance to try with new configuration 
            // but only if it's first try otherwise it may finish with infinity loop 
            if (isFirstCall)
            {
                if (modulosException is ReconfigureRequiredException || modulosException is ReCallableExecutionException)
                    return ActionAfterHandleException.DecreaseInvocationCounter;
            }

            // other modulos exceptions are throw 
            throw modulosException;
        }

        private enum ActionAfterHandleException
        {
            NotSet,
            Continue,
            Throw,
            DecreaseInvocationCounter
        }
    }
}