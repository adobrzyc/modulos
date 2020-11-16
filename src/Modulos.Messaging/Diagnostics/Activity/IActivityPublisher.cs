using System;
using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Metrics;
using Modulos.Messaging.Protocol.Request.Definitions;
using Modulos.Messaging.Protocol.Response.Definitions;

namespace Modulos.Messaging.Diagnostics.Activity
{
    /* [Last change: before 2018-12-12]
     *
     * #design-concept: All methods must throw an exception in as case of unexpected error.
     * #design-concept: Logging should not be running in fire and forget manner (e.q. Task.Run() and forgot).
     *
     * [26b3f0b2-1733-44f8-8ff3-8e359158d6ac]
     * #design-concept: Logging is as much important as business logic.
     * #design-concept: Logging should be predictable and always be considered during implementation.
     * #design-concept: Mostly there is no reason to turn off logging mechanism (ofc. with exceptions/exclusions).
     * #design-concept: Logging may support on/off mechanism.
     * #design-concept: Distinguish logging with level like debug/log/fatal/error/ect. is generally bad idea.
     * #design-concept: Logger should not be used everywhere. There are strict places where every of logger method
     *                  should be call.
     * #design-concept: HydraLogger should not be used outside Adito.Hydra internals. 
     * #design-concept: Overwriting Adito.Hydra is possible but, it's like overwriting other frameworks. Requires
     *                  some experience and deep understanding of how it works.
     */

    /*
     *[Last change: before 2018-12-12]
     *#design-concept: There was IHydraLogger here, but at last it's not logger but some transportId of mechanisms
     *                 to handle process boundaries/events/actions. 
     */
    public interface IActivityPublisher
    {
        /// <summary>
        /// Determined whether modulos process is enabled.
        /// </summary>
        bool Enabled { get; set; }

        Task PublishNewAction(IActionInfo newAction, IActionInfo previousAction);

        Task PublishActionFinished(IActionInfo action, IMetricBag metricBag, string reason, InvocationPlace where, object relatedObject, object host, Exception error);

        Task PublishNewRequest(IRequestData request, InvocationPlace were, IMetricBag metricBag);

        Task PublishNewResponse(IResponseData response, IMetricBag metricBag);

        //todo: IMPORTANT - redesign required 
        Task PublishNewSecurityContext(IInvocationContext invocationContext);

        /// <summary>
        /// Should be called when process is finished with an error without available <see cref="IActionInfo"/> in current context.
        /// </summary>
        /// <param name="error">Associated error.</param>
        /// <param name="message">Custom message to describe current context.</param>
        /// <param name="relatedObject">Related object associated with current context.</param>
        /// <param name="host">
        /// Host object. It's preferable from <paramref name="host"/> object to be <see cref="IActionHost"/> implementation.
        /// </param>
        Task PublishFinishWithFatalError(Exception error, string message, object relatedObject, object host);
    }
}