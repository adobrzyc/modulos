using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Diagnostics.Activity;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Maintenance;
using Modulos.Messaging.Protocol;
using Modulos.Messaging.Protocol.Request.Definitions;
using Modulos.Messaging.Protocol.Response.Definitions;
using Modulos.Messaging.Security;
using Modulos.Messaging.Security.Exceptions;
using Kind = Modulos.Messaging.Diagnostics.Metrics.Kind;

/* 
 * #consideration: Should IModulosHost save final action via IHydraLogger.LogActionFinished()
 *                 or it should be implemented in ambient layer ?
 *
 * 
 */
namespace Modulos.Messaging.Hosting
{
    [SuppressMessage("ReSharper", "ConvertToLambdaExpression")]
    internal class ModulosHost : IModulosHost, IActionHost
    {
        #region Fields

        //private readonly ISecurityContextVerifier securityContextVerifier;
        private readonly IHandlerFactory handlerFactory;
        private readonly IEndpointConfigValidator endpointConfigValidator;
        private readonly IMessageConfigProvider msgConfigProvider;
        private readonly IServiceStateProvider serviceStateProvider;
        private readonly IActivityPublisher activityPublisher;
        private readonly IActionInfoFactory actionInfoFactory;
        private readonly IParseRequest parseRequest;
        private readonly ICreateEmptyResponse createEmptyResponse;
        private readonly ICreateFaultResponse createFaultResponse;
        private readonly ICreateObjectResponse createObjectResponse;
        private readonly ICurrentPrincipal currentPrincipal;
        private readonly ICurrentAuthenticationData currentAuthenticationData;
        private readonly IEnumerable<IAuthenticationHandler> authenticationHandlers;
        //private readonly IHostConfiguration hostConfiguration;

        public string HostName { get; } = nameof(ModulosHost);

        #endregion //Fields

        public ModulosHost(IHandlerFactory handlerFactory,
            IEndpointConfigValidator endpointConfigValidator,
            IMessageConfigProvider msgConfigProvider,
            IServiceStateProvider serviceStateProvider,
            IActivityPublisher activityPublisher,
            IActionInfoFactory actionInfoFactory, 
            IParseRequest parseRequest, 
            ICreateEmptyResponse createEmptyResponse, 
            ICreateFaultResponse createFaultResponse,
            ICreateObjectResponse createObjectResponse, 
            ICurrentPrincipal currentPrincipal, 
            ICurrentAuthenticationData currentAuthenticationData,
            IEnumerable<IAuthenticationHandler> authenticationHandlers)
        {
            this.handlerFactory = handlerFactory;
            this.endpointConfigValidator = endpointConfigValidator;
            this.msgConfigProvider = msgConfigProvider;
            this.serviceStateProvider = serviceStateProvider;
            this.activityPublisher = activityPublisher;
            this.actionInfoFactory = actionInfoFactory;
            this.parseRequest = parseRequest;
            this.createEmptyResponse = createEmptyResponse;
            this.createFaultResponse = createFaultResponse;
            this.createObjectResponse = createObjectResponse;
            this.currentPrincipal = currentPrincipal;
            this.currentAuthenticationData = currentAuthenticationData;
            this.authenticationHandlers = authenticationHandlers;
        }

        [SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
        public async Task<IResponseData> Execute([JetBrains.Annotations.NotNull] ITransferObject transferObject, [JetBrains.Annotations.NotNull] IMetricBag metricBag, CancellationToken token)
        {
            if (transferObject == null) throw new ArgumentNullException(nameof(transferObject));
            if (metricBag == null) throw new ArgumentNullException(nameof(metricBag));

            var startTimeUtc = DateTime.UtcNow;
            var estimator = new Stopwatch();
            estimator.Start();

            IRequestData request;
            try
            {
                request = await parseRequest.Parse(transferObject, metricBag).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await activityPublisher.PublishFinishWithFatalError(ex, "error during parsing request", transferObject, this).ConfigureAwait(false);

                // #design-concept: If there is a modulos logger error it should be returned as an error. No for silent logger error. 
                // #considered: hiding 'real' problem by logger error is preferable solution due to [26b3f0b2-1733-44f8-8ff3-8e359158d6ac] concept. 

                // #abandoned-concept: It's worth to consider if counter bag should not be passed into CreateFaultResponseWithoutContext.
                // #why: Because there is no context (IActionInfo) to save counters.  
                // #why-not: Event with low possibility there is chance to use collected counters. 
                // #why-not: Protocol methods should measure themselves.
                // #why-not: CounterBag is required inside this method anyway /it depends on implementation so it's not solid argument/.
                return await createFaultResponse.CreateWithoutContext(ex, metricBag).ConfigureAwait(false);
            }
           
            var invocationContext = CreateContextWithNewAction(request, startTimeUtc, estimator);
            
            IResponseData response = null;
            Exception error = null;
            try
            {
                await activityPublisher.PublishNewAction(invocationContext.Action, request.MessageHeader.Context.Action).ConfigureAwait(false);
                await activityPublisher.PublishNewRequest(request, InvocationPlace.Target, metricBag).ConfigureAwait(false);
                await activityPublisher.PublishNewSecurityContext(invocationContext).ConfigureAwait(false);

                Validate(request);

                dynamic handler = handlerFactory.GetHandler(request.Message);

                if (!(request.Message is IMaintenanceMessage))
                    serviceStateProvider.ReportOperationStarted();

                var msgConfig = msgConfigProvider.GetConfig(request.Message);

                //var additionalParameters = new List<object>
                //{
                //   metricBag
                //};
                
                ////todo: #consideration: maybe it's a good idea to change everything into pipeline?
                ////      not sure, but may be pretty flexible. For now pipes aren't allow to change 
                ////      most of the cases. Maybe it shouldn't. Dunno. 
                //// 
                ////      Almost for sure, security, configuration, validations may be converted into a pipe  
                //var pipeData = new IncomingPipeData
                //(
                //    request.Message,
                //    msgConfig,
                //    (IMessageHandler) handler,
                //    invocationContext, false
                //);

                //await using var pipelineResult = await hostConfiguration.IncomingPipes.Execute(token,pipeData, metricBag);


                // check authentication for message 
                await HandleSecurity(request, msgConfig, metricBag, invocationContext).ConfigureAwait(false);



                await CheckEndpointConfiguration(request.MessageHeader).ConfigureAwait(false);

                metricBag.Add(Kind.Initialization, InvocationPlace.Target, invocationContext.Action.ElapsedMilliseconds);

                object result = null;
                var t1 = invocationContext.Action.ElapsedMilliseconds;
                try
                {
                    if (request.Message is IQueryBase)
                        result = await handler.Handle((dynamic) request.Message, (InvocationContext) invocationContext.Clone(), token).ConfigureAwait(false);
                    else await handler.Handle((dynamic) request.Message, (InvocationContext) invocationContext.Clone(), token).ConfigureAwait(false);
                }
                //#design-concept: streams belongs to message handler
                //catch
                //{
                //    // ReSharper disable once SuspiciousTypeConversion.Global
                //    var directStreamMessage = requestMessage as IContainDirectStream;
                //    directStreamMessage?.Stream.Dispose();
                //    throw;
                //}
                finally
                {
                    var t2 = invocationContext.Action.ElapsedMilliseconds;
                    metricBag.Add(Kind.Execution, InvocationPlace.Target, t2 - t1, invocationContext.Action);
                }

                response = result == null
                    ? await createEmptyResponse.Create(request.MessageHeader, metricBag, invocationContext).ConfigureAwait(false)
                    : await createObjectResponse.Create(result, request.MessageHeader, metricBag, invocationContext).ConfigureAwait(false);

                await activityPublisher.PublishNewResponse(response, metricBag).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                error = ex;
                //todo: [L0] check it out, invocation context may be null
                response = await createFaultResponse.Create(error, request.MessageHeader, metricBag, invocationContext).ConfigureAwait(false);
            }
            finally
            {
                if (!(request.Message is IMaintenanceMessage))
                    serviceStateProvider.ReportOperationPerformed();

                //var pipeData = new OutgoingPipeData
                //(
                //    response,
                //    error,
                //    request.Message,
                //    false
                //);

               // await using var pipelineResult = await hostConfiguration.OutgoingPipes.Execute(token, pipeData, metricBag);

            }

            return response;
        }

        private IInvocationContext CreateContextWithNewAction(IRequestData request, DateTime startTimeUtc, Stopwatch estimator)
        {
            var newAction = actionInfoFactory.Create(request.Message, this, request.MessageHeader.Context.Action.OperationId);
            var newContext = new InvocationContext(newAction);
            newContext.Action.Start(startTimeUtc, estimator);
            return newContext;
        }


        private async Task CheckEndpointConfiguration(IMessageHeader requestHeader)
        {
            //todo: check if it's ever possible 
            if (requestHeader.EndpointConfig?.Info == null) return;

            var configState = await endpointConfigValidator.CheckConfig(requestHeader.EndpointConfig.Info);
              
            if (configState != EndpointConfigState.Valid)
                throw new ReconfigureRequiredException();
        }

        private void Validate(IRequestData request)
        {
            if (request.Message == null)
                throw new UnsupportedCqrsMessageFormat("Message is an empty or does not inherit from IMessage.");

            if (!(request.Message is IQueryBase) && !(request.Message is ICommandBase))
                throw new UnsupportedCqrsMessageFormat(
                    $"Message: {request.Message.GetType().FullName} must inherit from IQueryBase or ICommandBase.");

            if (request.MessageHeader.Context == null)
                throw new UnableToDetermineModulosContext(request.Message);
        }

        private async ValueTask HandleSecurity(IRequestData request, IMessageConfig msgConfig, IMetricBag metricBag, IInvocationContext invocationContext)
        {
            var t1 = invocationContext.Action.ElapsedMilliseconds;

            try
            {
                // read authentication data 
                var scheme = request.MessageHeader.SecurityContext.Scheme;
                var authHandler = authenticationHandlers.LastOrDefault(e => e.Scheme == scheme);
                if (authHandler != null)
                {
                    var authData = await authHandler.Handle(request.MessageHeader.SecurityContext)
                        .ConfigureAwait(false);
                    
                    await currentAuthenticationData.Set(authData).ConfigureAwait(false);
                }

                switch (msgConfig.AuthenticationMode)
                {
                    case AuthenticationMode.None:
                        return;
                    case AuthenticationMode.Required:
                         var principal = await currentPrincipal.Get().ConfigureAwait(false);
                        if(principal.IsAnonymous)
                            throw new AnonymousAccessIsNotAllowedException();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
            }
            finally
            {
                var t2 = invocationContext.Action.ElapsedMilliseconds;
                metricBag.Add(Kind.Security, "check security", InvocationPlace.Target, t2 - t1, invocationContext.Action);
            }
        }

        //private bool ParseRequest(ITransferObject transferObject,
        //    out IMessageHeader header,
        //    out ITransportEngine transportEngine,
        //    out IMessage message,
        //    out HydraContext hydraContext,
        //    out IResponseData faultResponse)
        //{
        //    faultResponse = null;
        //    header = null;
        //    message = null;
        //    hydraContext = null;
        //    transportEngine = null;

        //    try
        //    {
        //        hydraProtocol.ParseRequest(transferObject: transferObject, 
        //            transportEngine: out transportEngine,
        //            header: out header, 
        //            message: out message,
        //            serializedObject: out var __);
        //        hydraContext = header.Context;
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        HydraInternalLogger.Error(ex, ex.GetMessage());
        //        var logger = loggerFactory.GetLogger(GetType());
        //        logger.Fatal(ex, ex.GetMessage());

        //        // if transport engine is available then create fault response, otherwise throw wrapped exception
        //        var internalError = new InternalEndpointError("Target service can not execute message due to internal error.");
        //        if (transportEngine != null)
        //            faultResponse = hydraProtocol.CreateFaultResponseWithoutContext(transportEngine, internalError).Result; //todo: tututut, wrap with try/catch to
        //        else throw internalError;

        //        return false;
        //    }
        //}

        //private async Task HandleSecurity(IMessageConfig msgConfig, IMetricBag metricBag, IInvocationContext invocationContext)
        //{
        //    var t1 = invocationContext.Action.ElapsedMilliseconds;
        //    try
        //    {
        //        await securityContextVerifier.ValidateSecurity(msgConfig.AuthenticationMode, invocationContext.SecurityContext);
        //    }
        //    finally
        //    {
        //        var t2 = invocationContext.Action.ElapsedMilliseconds;
        //        metricBag.Add(Schema.Security, "check security", InvocationPlace.Target, t2 - t1, invocationContext.Action);
        //    }
        //}

        //private object GetHandler(IRequestData request)
        //{
        //    return monitor.Measure(() =>
        //    {
        //        // ReSharper disable once ConvertToLambdaExpression
        //        return handlerFactory.GetHandler(request.Message);

        //    }, GetCounter(request.MessageHeader.Context, Transport.Configuration));
        //}

        //private void AddExecutionTime(Stopwatch estimator, IHydraContext context)
        //{
        //    estimator.Stop();
        //    var total = new Counter(context.Action)
        //    {
        //        Transport = Transport.Execution,
        //        Where = InvocationPlace.Target,
        //        Value = estimator.ElapsedMilliseconds
        //    };

        //    monitor.Add(total);
        //}

        //private static ICounter GetCounter(IHydraContext context, Transport transportId, string info = "")
        //{
        //    return new Counter(context.Action)
        //    {
        //        Transport = transportId,
        //        Where = InvocationPlace.Target,
        //        What = info
        //    };
        //}

    }
}