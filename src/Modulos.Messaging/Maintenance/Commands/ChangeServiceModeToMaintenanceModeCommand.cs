using System;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Modulos.Messaging.Maintenance.Commands
{
    public class ChangeServiceModeToMaintenanceModeCommand : ICommand, IMaintenanceMessage
    {
        public ChangeServiceModeToMaintenanceModeCommand()
        {
            WaitTimeout = TimeSpan.FromSeconds(30);
        }

        public bool WaitUntilAllOperationsArePerformed { get; set; }
        public TimeSpan WaitTimeout { get; set; }
    }
}