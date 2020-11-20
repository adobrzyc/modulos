namespace Examples.ConsoleApp.Storage
{
    public class InMemoryStorage : IStorage
    {
        public override string ToString()
        {
            return GetType().Name;
        }
    }
}