namespace Examples.ConsoleApp.Config
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Modulos;
    using Storage;

    public class RegisterStorageModule : MicrosoftDiModule
    {
        // config is available from initialization pipeline 
        private readonly IConfiguration config;

        public RegisterStorageModule(IConfiguration config)
        {
            this.config = config;
        }

        public override LoadOrder Order { get; } = LoadOrder.Project;
        public override bool AutoLoad { get; } = true;

        public override void Load(IServiceCollection services)
        {
            if (config["Storage"] == "InMemory")
            {
                services.AddTransient<IStorage, InMemoryStorage>();
                services.AddTransient<InMemoryStorage, InMemoryStorage>();
            }
            else
            {
                services.AddTransient<IStorage, FileStorage>();
                services.AddTransient<FileStorage, FileStorage>();
            }
        }
    }
}