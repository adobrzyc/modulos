using System.Threading;
using System.Threading.Tasks;
using Modulos.Messaging.EventBus;
using Modulos.Messaging.Hosting;
using Modulos.Messaging.Hosting.Options;
using Modulos.Messaging.Maintenance.Events;

namespace Modulos.Messaging.Maintenance.Commands
{
    public class ChangeServiceModeToMaintenanceModeCommandHandler : ICommandHandler<ChangeServiceModeToMaintenanceModeCommand>
    {
        private readonly IOperationModeOption operationModeOption;
        private readonly IEventBus eventBus;
        private readonly IServiceStateProvider serviceStateProvider;

        public ChangeServiceModeToMaintenanceModeCommandHandler(IOperationModeOption operationModeOption, IEventBus eventBus, 
            IServiceStateProvider serviceStateProvider)
        {
            this.operationModeOption = operationModeOption;
            this.eventBus = eventBus;
            this.serviceStateProvider = serviceStateProvider;
        }

        public async Task Handle(ChangeServiceModeToMaintenanceModeCommand command, InvocationContext invocationContext, CancellationToken token)
        {
            operationModeOption.Value = OperatingMode.Maintenance;

            if (!command.WaitUntilAllOperationsArePerformed)
                return;

            var result = await serviceStateProvider.WaitUntilAllOperationsArePerformed(command.WaitTimeout);
        
            eventBus.Publish(new ServiceModeChangedEvent(operationModeOption.Value));

            if(result.ActiveOperationCount > 0)
                throw new TodoException("Wait for all operation timeout.");

            //if(!serviceStateProvider.WaitUntillAllOperationsArePerformed(TimeSpan.FromMinutes(2)))
            //    throw new TodoException("Wait for all operation timeout.");
        }
    }
}