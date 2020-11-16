using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Modulos.Messaging.Diagnostics.Options;
using Modulos.Messaging.Hosting.Options;

namespace Modulos.Messaging.Hosting
{
    public sealed class ServiceStateProvider : IServiceStateProvider
    {
        private readonly IOperationModeOption operationModeOption;
        private readonly ILogMarkOption markOption;
        private readonly ServiceState serviceState = new ServiceState();

        private readonly object locker = new object();
       
        public ServiceStateProvider(IOperationModeOption operationModeOption, ILogMarkOption markOption)
        {
            this.operationModeOption = operationModeOption;
            this.markOption = markOption;
        }

        //todo: it doesn't look nice
        public async Task<IServiceState> WaitUntilAllOperationsArePerformed(TimeSpan timeout)
        {
            return await Task.Run(() =>
            {
                var sw = new Stopwatch();
                sw.Start();

                while (true)
                {
                    lock (locker)
                    {
                        if (sw.Elapsed > timeout)
                        {
                            sw.Stop();
                            return GetServiceState();
                        }

                        if (serviceState.ActiveOperationCount <= 0)
                            return GetServiceState();
                    }
                    Task.Delay(1000).Wait();
                }
            });
        }

        public void ReportOperationStarted()
        {
            lock (locker)
            {
                serviceState.ActiveOperationCount++;
                serviceState.TotalOperationCount++;
                serviceState.LastOperationTimeUtc = DateTime.UtcNow;
            }
        }

        public void ReportOperationPerformed()
        {
            lock (locker)
            {
                if (serviceState.ActiveOperationCount > 0)
                    serviceState.ActiveOperationCount--;
            }
        }

        public IServiceState GetServiceState()
        {
            lock (locker)
            {
                serviceState.OperatingMode = operationModeOption.Value;
                serviceState.LogMark = markOption.Value;

                return serviceState;
            }
        }
    }
}