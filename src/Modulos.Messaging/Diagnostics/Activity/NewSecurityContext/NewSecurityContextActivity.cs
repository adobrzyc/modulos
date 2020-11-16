namespace Modulos.Messaging.Diagnostics.Activity.NewSecurityContext
{
    public class NewSecurityContextActivity : IActivity
    {
        public IInvocationContext InvocationContext { get; }

        public NewSecurityContextActivity(IInvocationContext invocationContext)
        {
            InvocationContext = invocationContext;
        }
    }
}