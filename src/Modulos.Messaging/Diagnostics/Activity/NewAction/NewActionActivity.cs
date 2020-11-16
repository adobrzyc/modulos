namespace Modulos.Messaging.Diagnostics.Activity.NewAction
{
    public sealed class NewActionActivity : IActivity
    {
        public IActionInfo Action { get; } 
        public IActionInfo PreviousAction { get; }

        public NewActionActivity(IActionInfo action, IActionInfo previousAction)
        {
            Action = action;
            PreviousAction = previousAction;
        }
    }
}