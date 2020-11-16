namespace Modulos.Messaging.Hosting.Options
{
    public class OperationModeOption : OptionBase<OperatingMode>, IOperationModeOption
    {
        public OperationModeOption()
            : base(true) { }

        public override OperatingMode GetDefaultValue()
        {
            return OperatingMode.Normal;
        }
    }
}