using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Modulos.Messaging.Diagnostics.Activity.FatalError
{
    public class DefaultFatalActivityHandler : IActivityHandler<FatalErrorActivity>
    {
        private readonly ILoggerFactory loggerFactory;

        public DefaultFatalActivityHandler(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }

        public Task Handle(FatalErrorActivity activity)
        {
            //HydraInternalLogger.Fatal(@event.Error, @event.ErrorDescription);

            var logger = loggerFactory.CreateLogger(activity.Host);
            //logger.LogError(@event.Error, @event.Message);
            logger.LogCritical(activity.Error, activity.Message);
            //logger.Fatal(@event.Error, @event.ErrorDescription);

            return Task.CompletedTask;
        }
    }
} 