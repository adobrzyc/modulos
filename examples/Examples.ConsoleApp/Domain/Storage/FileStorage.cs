namespace Examples.ConsoleApp.Storage
{
    public class FileStorage : IStorage
    {
        public override string ToString()
        {
            return GetType().Name;
        }
    }
}