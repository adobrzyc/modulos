using System;
using System.Threading.Tasks;

namespace Modulos.Messaging.Hosting
{
    public interface IServiceStateProvider
    {
        Task<IServiceState> WaitUntilAllOperationsArePerformed(TimeSpan timeout);

        void ReportOperationStarted();
        void ReportOperationPerformed();

        IServiceState GetServiceState();
    }
}