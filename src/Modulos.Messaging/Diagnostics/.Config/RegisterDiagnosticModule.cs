using Microsoft.Extensions.DependencyInjection;
using Modulos.Messaging.Diagnostics.Activity;
using Modulos.Messaging.Diagnostics.Activity.FatalError;
using Modulos.Messaging.Diagnostics.Activity.FinishAction;
using Modulos.Messaging.Diagnostics.Activity.NewAction;
using Modulos.Messaging.Diagnostics.Activity.NewRequest;
using Modulos.Messaging.Diagnostics.Activity.NewResponse;
using Modulos.Messaging.Diagnostics.Activity.NewSecurityContext;
using Modulos.Messaging.OneOrManyExecutionRoutine;

// ReSharper disable UnusedType.Global

namespace Modulos.Messaging.Diagnostics.Config
{
    public class RegisterDiagnosticModule : MicrosoftDiModule
    {
        public override LoadOrder Order { get; } = LoadOrder.Internal;
        public override bool AutoLoad { get;} = true;

        public override void Load(IServiceCollection collection)
        {
            collection.AddTransient<IActivityPublisher, ActivityPublisher>();

            collection.AddSingleton<IActivityHandler<FatalErrorActivity>, DefaultFatalActivityHandler>();
            collection.AddSingleton<IFatalErrorActivityProcessor, FatalErrorActivityProcessor>();
            collection.AddSingleton<INewActionActivityProcessor, NewActionActivityProcessor>();
            collection.AddSingleton<INewResponseActivityProcessor, NewResponseActivityProcessor>();
            collection.AddSingleton<INewRequestActivityProcessor, NewRequestActivityProcessor>();
            collection.AddSingleton<IFinishActionActivityProcessor, FinishActionActivityProcessor>();
            collection.AddSingleton<INewSecurityContextActivityProcessor, NewSecurityContextActivityProcessor>();
            
            collection.AddTransient(typeof(IOneOrManyExecutionRoutine<>), typeof(OneOrManyExecutionRoutine<>));

            // #considerations
            // maybe invoker should be singleton due to bypass eventually different DI registration scope of each handler 
            // ~ if invoker will be singleton then all handlers will be singletons to
            collection.AddTransient(typeof(IOneOrManyActivityHandlerInvoker<>), typeof(OneOrManyActivityHandlerInvoker<>));
        }
    }
}