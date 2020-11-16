using System.Runtime.Serialization;

namespace Modulos.Messaging.Diagnostics.Metrics
{
    [DataContract]
    public enum Kind
    {
        [EnumMember] [KindInfo(Group.Time, false, true)] Total,

        [EnumMember] [KindInfo(Group.Time, true, true)] Transfer,

        //[EnumMember] [KindInfo(Group.Time, true, true, DisplayName = "Transfer to")] TransferTo,

        //[EnumMember] [KindInfo(Group.Time, true, true, DisplayName = "Transfer from")] TransferFrom,

        [EnumMember] [KindInfo(Group.Time, false, true)] Execution,
        [EnumMember] [KindInfo(Group.Time, false, true, DisplayName = "DB")] Database,
        [EnumMember] [KindInfo(Group.Time, false, true)] Serialization,
        [EnumMember] [KindInfo(Group.Time, false, true, DisplayName = "Config")] Configuration,
        [EnumMember] [KindInfo(Group.Time, false, true)] Initialization,
        [EnumMember] [KindInfo(Group.Time, false, true)] Security,
        [EnumMember] [KindInfo(Group.Time, false, true)] Validation,

        /// <summary>
        /// Operations related with monitoring/logging operations.
        /// </summary>
        [EnumMember] [KindInfo(Group.Time, false, true)] Monitoring,
        

        [EnumMember] [KindInfo(Group.Time, false, true, DisplayName = "Read content")] ReadContent,


        // due to possible difference between time on two machines, it may not be
        // a good idea to depends on this value to calculate 'transfer times'
        // ...but it still can be useful 

        [EnumMember] [KindInfo(Group.Date, true, false)] RequestStart,
        [EnumMember] [KindInfo(Group.Date, true, false)] RequestArrived,
        [EnumMember] [KindInfo(Group.Date, true, false)] ResponseStart,
        [EnumMember] [KindInfo(Group.Date, true, false)] ResponseArrived,

        [EnumMember] [KindInfo(Group.Memory, false, true, DisplayName = "Request size")] RequestSize,
        [EnumMember] [KindInfo(Group.Memory, false, true, DisplayName = "Response size")] ResponseSize,
        [EnumMember] [KindInfo(Group.Memory, true, true, DisplayName = "Size")] TotalSize,
    }
}