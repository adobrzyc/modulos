namespace Modulos.Messaging.Security
{
    public sealed class ReadAuthenticationDataResult 
    {
        public IAuthenticationData Data { get; }

        public bool HasCredentials => Data != null;

        public static ReadAuthenticationDataResult NoResult()
        {
            return new ReadAuthenticationDataResult(null);
        }

        public ReadAuthenticationDataResult(IAuthenticationData data)
        {
            Data = data;
        }
    }
}