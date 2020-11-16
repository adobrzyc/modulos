namespace Modulos.Messaging.Protocol
{
    internal class TransferObjectFactory : ITransferObjectFactory
    {
        public ITransferObject CreateTransferObject()
        {
            return new TransferObject();
        }
    }
}
