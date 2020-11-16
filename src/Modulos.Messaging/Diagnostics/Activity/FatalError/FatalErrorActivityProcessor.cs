using System;
using System.Threading.Tasks;
using Modulos.Errors;
using Modulos.Messaging.Diagnostics.Options;
using Newtonsoft.Json;

namespace Modulos.Messaging.Diagnostics.Activity.FatalError
{
    internal class FatalErrorActivityProcessor : IFatalErrorActivityProcessor
    {
        private readonly IOneOrManyActivityHandlerInvoker<FatalErrorActivity> activityHandlerInvoker;
        private readonly ILogMarkOption markOption;

        public FatalErrorActivityProcessor(IOneOrManyActivityHandlerInvoker<FatalErrorActivity> activityHandlerInvoker,
            ILogMarkOption markOption)
        {
            this.activityHandlerInvoker = activityHandlerInvoker;
            this.markOption = markOption;
        }

        //todo: consider related object null or not 
        public async Task Process([JetBrains.Annotations.NotNull] Exception error, string message, [JetBrains.Annotations.NotNull] Type host, object relatedObject)
        {
            if (error == null) throw new ArgumentNullException(nameof(error));
            if (host == null) throw new ArgumentNullException(nameof(host));
            if (activityHandlerInvoker.Count == 0) return;

            string serializedTransferObject = null;
            try
            {
                serializedTransferObject = JsonConvert.SerializeObject(relatedObject, Formatting.Indented, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            catch
            {
                // ignore
            }

            var relatedStr = serializedTransferObject != null ? $"\r\nrelated object: {serializedTransferObject}" : null;
            var errorDescription = $"message: {error.GetMessage()}host: {host.FullName}{relatedStr}";

            var @event = new FatalErrorActivity(error, message, markOption.Value, errorDescription, host);

            await activityHandlerInvoker.Invoke(@event);
        }
    }
}