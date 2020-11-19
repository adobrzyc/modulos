using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modulos;

namespace Examples.ConsoleApp.Storage.Config
{
    public class RegisterStorageModule : MicrosoftDiModule
    {
        public override LoadOrder Order { get; } = LoadOrder.Project;
        public override bool AutoLoad { get; } = true;

        // config is available from initialization pipeline 
        private readonly IConfiguration config;

        public RegisterStorageModule(IConfiguration config)
        {
            this.config = config;
        }

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