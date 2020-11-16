using System.Threading;
using System.Threading.Tasks;
using Modulos.Messaging.EventBus;
using Modulos.Messaging.Hosting.Options;
using Modulos.Messaging.Maintenance.Events;

namespace Modulos.Messaging.Maintenance.Commands
{
    public class ChangeServiceModeToNormalModeCommandHandler : ICommandHandler<ChangeServiceModeToNormalModeCommand>
    {
        private readonly IOperationModeOption operationModeOption;
        private readonly IEventBus eventBus;

        public ChangeServiceModeToNormalModeCommandHandler(IOperationModeOption operationModeOption, IEventBus eventBus)
        {
            this.operationModeOption = operationModeOption;
            this.eventBus = eventBus;
        }

        public Task Handle(ChangeServiceModeToNormalModeCommand command, InvocationContext invocationContext, CancellationToken token)
        {
            operationModeOption.Value = OperatingMode.Normal;

            eventBus.Publish(new ServiceModeChangedEvent(operationModeOption.Value));

            return Task.CompletedTask;
        }
    }
}