namespace Modulos.Messaging.Diagnostics.Options
{
    public sealed class LogMarkOption : OptionBase<LogMark>, ILogMarkOption
    {
        public LogMarkOption()
            : base(false) { }

        public override LogMark GetDefaultValue()
        {
            return LogMark.Empty;
        }
    }
}