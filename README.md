
# Modulos
Supports creating applications based on reconfigurable: DI modules, pipelines 
and configurations.

# Installation
All Modulos components are distributed as NuGet packages 
```Install-Package Modulos```

# Idea 
Share functionalities over projects/solutions/organizations via modules 
that affects application in predictible way. It's also usefull to split 
single application into more readable parts. 

# Hello World
For a better understanding of how Modulos works, it will be useful to bring hello world example.

```csharp
class Program
{
    static void Main(string[] args)
    {
        // 1. Initialization.
        var modulosApp = new ModulosApp();
        modulosApp.UseNetCore();
        modulosApp.Initialize<Program>();


        // 2. Configure dependecy injection. 
        var sc = new ServiceCollection();
        sc.AddModulos(modulosApp);
        var sp = sc.BuildServiceProvider();


        //3. Configure after dependency injection is setup.
        modulosApp.Configure(sp);
(...)
```


# MAP - Modulos Application Pipeline
Modulos is focused on three main areas. Each of them is shortly described below, but the most
important feature is common idea of modularity.  

1. **Initialization.**  
   A place to perform actions before dependency injection container is built. Great to prepare 
some files, configurations, migrations (discussable), ect.
2. **Configure dependecy injection.**   
   Modulos brings modules (very similar to Autofac) even for the default Microsoft DI container. 
Main difference with similar solutions is possibility to order and filter these modules.
3. **Configuration after dependency injection.**  
   Modulos enables to split configuration process, performed after DI container is built. 
   

## Initialization (MAP)
#### TL;DR
```csharp
var modulosApp = new ModulosApp();
// decide to use NetCore integration
modulosApp.UseNetCore(); 
// execute initialization process
modulosApp.Initialize<ClassFromMainProject>();
```
#### Pipeline
Initialization is build with pipeline pattern. Pipeline can be modified directly from 
`Initialize` method or by class inherited from `ModulosApp.IUpdateInitializationPipeline`.
Classes are auto explored and executed during `Initialize` method invocation.
```csharp
public IPipelineResult Initialize<TClassInProject>
(
    Action<IPipeline> updatePipeline, // can be used to inline edit pipeline
    params object[] additionalParameters

) where TClassInProject : class
```
```csharp
//usage
modulosApp.Initialize<Program>(update => 
{
    update.Add<SomeNewPipe>();
});
```
and/or
```csharp
public class UpdateInitializationPipeline : ModulosApp.IUpdateInitializationPipeline
{
    public void Update(IPipeline pipeline)
    {
        pipeline.Add<SomeNewPipe>();
    }
}
```

Pipes can be defined as simple as inheritance from `IPipe` interface. 
```csharp
public class SomeNewPipe : IPipe
{
    public Task<PipeResult> Execute(CancellationToken cancellationToken)
    {
        return Task.FromResult(PipeResult.Continue);
    }
}
```
##### Recomendations and tips 
- Use `IUpdateInitializationPipeline` with your libraries to modularize the initialization process. 
- There are various methods to modify pipeline. 
- For better understanding pipeline or initialization process look at available examples.

## Configure dependecy injection (MAP)
#### TL;DR
Modules are just simple classes with components registrations, loaded in controlled manner.
```csharp
//AddModulos(...) will explore and load any available module in application
collection.AddModulos(modulosApp);
```
#### DI modules
Modulos as mentioned before brings modules into dependency injection container configuration. 
Modules are used to organize, control and simplify configuration process. Instead of put every 
registration in `ConfigureServices(IServiceCollection services)` - separate them 
into classes.

Code below shows how sample module may look like.
```csharp
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
```
#### Ordering
As you may notice module has defined property `Order`. It's `enum` listed below and it's used
to control ordering process during loading modules. By choosing higher-order you may overwrite previous
registration. 

```csharp
public enum LoadOrder
{
    // Reserved for elements located in external libraries.
    Library,

    // Reserved for elements located in solution projects.
    Project,

    // Reserved for elements located in application project (eq.: console app, web api).
    App,

    // Reserved for elements located in test projects.
    Test
}
```

#### Filtering
Filtering is available during `AddModulos` method invocation.
```csharp
sc.AddModulos(modulosApp, module =>
{
    if(module.Instance is RegisterStorageModule)
        module.AutoLoad = false;
});
```

#### Using additional data 
Each of module can use passed into `AddModulos` method extra parameters.
```csharp
collection.AddModulos(modulosApp, someExtraData1,someExtraData2);
```

It's not a bad idea to put here data from the initialization process.

```csharp
var iniResult = modulosApp.Initialize<Program>();

sc.AddModulos
(
    modulosApp, 
    // data from initialization pipeline, will be available for DI modules
    iniResult.GetAll() 
);
```

By default each module may obtain (via ctor) below components:
- `ITypeExplorer`
- `IAssemblyExplorer`
- `IAppInfo`
- `Assembly[]`

#### FAQ
- Can I still use the 'standard' way for dependency injection configuration 
  - Yes you can, either mix them.
- Can I use external dependency injection containers like Autofac.
  - Yes you can. Modulos has also some integrations packages (e.q: Autofac).

## Configuration after dependency injection (MAP) 
Configuration is very similar to initialization, it's built with a pipeline, accept 
additional data, and can use results from previously executed pipes. The only difference 
is that configuration process may use a dependency injection container to obtain data (if
not available from `additionalParameters`). 

```csharp
IPipelineResult Configure
(
    IServiceProvider serviceProvider,
    Action<IPipeline> updatePipeline, 
    params object[] additionalParameters
)
```

#### Pipeline
To modifie pipeline use:
- `ModulosApp.IUpdateConfigPipeline` 
- `Action<IPipeline> updatePipeline`



# Example
For more examples please explore repository (Examples directory).

```csharp
class Program
{
    static void Main()
    {
        // 1. initialize
        var modulosApp = new ModulosApp();
        modulosApp.UseNetCore();
        modulosApp.Initialize<Program>();


        // 2. organize dependency injection 
        var sc = new ServiceCollection();
        sc.AddModulos
        (
            modulosApp, 
            // data from initialization pipeline, will be available for DI containers
            iniResult.GetAll() 
        );
        var sp = sc.BuildServiceProvider();


        // 3. configure after dependency injection 
        modulosApp.Configure(sp);
    }
}
```

**output**
```bat
PrepareConfiguration...
MakeSomeActionBaseOnConfiguration...
[Storage, InMemory]
[AppVersion, 1.0.0]
ConfigureAppWhenInMemoryStorage...
InMemoryStorage
````

**'application components'**

```csharp
//
// Initialization: it can be delivered event from external package
// 
public class UpdateInitializationPipeline : ModulosApp.IUpdateInitializationPipeline
{
    public void Update(IPipeline pipeline)
    {
        pipeline.Add<PrepareConfiguration>();
        pipeline.Add<MakeSomeActionBaseOnConfiguration>();
    }
}

public class PrepareConfiguration : IPipe
{
    public Task<PipeResult> Execute(CancellationToken cancellationToken)
    {
        Console.WriteLine("PrepareConfiguration...");

        var builder = new ConfigurationBuilder();
        builder.Add(new MemoryConfigurationSource
        {
            InitialData = new []
            {
                new KeyValuePair<string, string>("AppVersion","1.0.0"),
                new KeyValuePair<string, string>("Storage","InMemory")
            }
        });
        var config = builder.Build();

        var result = new PipeResult(PipeActionAfterExecute.Continue, config);

            
        return Task.FromResult(result);
    }
}

public class MakeSomeActionBaseOnConfiguration : IPipe
{
    // pipes can use previous pipes data 
    private readonly IConfiguration config;

    public MakeSomeActionBaseOnConfiguration(IConfiguration config)
    {
        this.config = config;
    }

    public Task<PipeResult> Execute(CancellationToken cancellationToken)
    {
        Console.WriteLine("MakeSomeActionBaseOnConfiguration...");
        foreach (var pair in config.AsEnumerable())
        {
            Console.WriteLine(pair);
        }
            
        return Task.FromResult(PipeResult.Continue);
    }
}

//
// DI modules
// 
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

public interface IStorage {}

public class InMemoryStorage : IStorage
{
    public override string ToString()
    {
        return GetType().Name;
    }
}

public class FileStorage : IStorage
{
    public override string ToString()
    {
        return GetType().Name;
    }
}

//
// Configuration
// 
public class UpdateConfigPipeline : ModulosApp.IUpdateConfigPipeline
{
    public void Update(IPipeline pipeline)
    {
        pipeline.Add<ConfigureAppWhenInMemoryStorage>();
        pipeline.Add<ConfigureAppWhenFileStorage>();
    }
}

// pipes can be optional, created and executed only if all params in ctor are available 
public class ConfigureAppWhenInMemoryStorage : IOptionalPipe
{
    private readonly InMemoryStorage storage;

    public ConfigureAppWhenInMemoryStorage(InMemoryStorage storage)
    {
        this.storage = storage;
    }

    public Task<PipeResult> Execute(CancellationToken cancellationToken)
    {
        Console.WriteLine($"{GetType().Name}...");
        Console.WriteLine(storage.ToString());
        return Task.FromResult(PipeResult.Continue);
    }
}

// this pipe will not be created either executed, because FileStorage is not available
public class ConfigureAppWhenFileStorage : IOptionalPipe
{
    private readonly FileStorage storage;

    public ConfigureAppWhenFileStorage(FileStorage storage)
    {
        this.storage = storage;
    }

    public Task<PipeResult> Execute(CancellationToken cancellationToken)
    {
        Console.WriteLine($"{GetType().Name}...");
        Console.WriteLine(storage.ToString());
        return Task.FromResult(PipeResult.Continue);
    }
}
```
