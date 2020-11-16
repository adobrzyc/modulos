using System.Runtime.Serialization;
using Modulos.Messaging.Diagnostics.Options;
using Newtonsoft.Json;

namespace Modulos.Messaging.Maintenance.Commands
{
    [DataContract]
    public class SetLogMarkCommand : ICommand, IMaintenanceMessage
    {
        [DataMember]
        public LogMark LogMark { get; set; }

        [JsonConstructor]
        private SetLogMarkCommand()
        {
            
        }

        public SetLogMarkCommand(LogMark mark)
        {
            LogMark = mark;
        }
    }
}