using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Modulos.Messaging
{
    // todo: think about struct 
    // todo: maybe remove interface for HydraContext ?, or maybe use interface (keep in mind serialization)
    // todo: cleanup required 
    // todo: check usage of cloneable (probably needed) 

    [DataContract]
    public sealed class InvocationContext : IInvocationContext 
    {
        [DataMember]
        public ActionInfo Action { get; private set; }
        //public ActionInfo Action { get; internal set; }

        //todo: check: it was internal
        [JsonConstructor]
        private InvocationContext()
        {
        }

        public InvocationContext(ActionInfo actionInfo)
        {
            Action = actionInfo;
        }


        object ICloneable.Clone()
        {
            var actionInfo = new ActionInfo
            (
                Action.OperationId, 
                Action.Id, 
                Action.Name,
                Action.HostName,
                Action.AssemblyName, 
                Action.Mark, 
                false
            );

            return new InvocationContext(actionInfo);
        }
    }

}