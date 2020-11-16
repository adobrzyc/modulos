using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Messaging.Configuration;
using Modulos.Messaging.Hosting;
using Modulos.Messaging.Protocol;
using Modulos.Messaging.Protocol.Request;
using Modulos.Messaging.Protocol.Request.Definitions;
using Modulos.Messaging.Protocol.Response;
using Modulos.Messaging.Protocol.Response.Definitions;
using Modulos.Messaging.Protocol.Serializers;
using Modulos.Messaging.Protocol.Serializers.Definitions;
using Modulos.Messaging.Transport;
using Modulos.Messaging.Transport.Policy;

// ReSharper disable UnusedType.Global

namespace Modulos.Messaging.Config
{
    public class MessagingDiModule : MicrosoftDiModule
    {
        private readonly ITypeExplorer typeExplorer;
        public override LoadOrder Order { get; } = LoadOrder.Internal;
        public override bool AutoLoad { get;} = true;

        public MessagingDiModule(ITypeExplorer typeExplorer)
        {
            this.typeExplorer = typeExplorer;
        }

        public override void Load(IServiceCollection collection)
        {
            collection.AddSingleton<IMessageTypeProvider, MessageTypeProvider>();

            var defaultOption = new AutoRegistrationOptionsAttribute(ServiceLifetime.Transient);
            var handlerDescriptions = new List<IHandlerDescription>();

            var handlers = typeExplorer
                .GetSearchableClasses<IMessageHandler>()
                .Select(e => new
                {
                    Type = e, 
                    Options = e.GetCustomAttribute<AutoRegistrationOptionsAttribute>() ?? defaultOption
                })
                .ToArray();

            foreach (var handler in handlers)
            {
                if (TypeUtils.IsAssignableToGenericType(handler.Type, typeof(IQueryHandler<,>),
                    out var queryGenericWithTypes,
                    out var genericsQuery))
                {
                    collection.Add(new ServiceDescriptor(queryGenericWithTypes, handler.Type,
                        handler.Options.ServiceLifetime));
                    handlerDescriptions.Add(new HandlerDescription(handler.Type, genericsQuery.First()));
                }

                if (TypeUtils.IsAssignableToGenericType(handler.Type, typeof(ICommandHandler<>),
                    out var commandGenericWithTypes,
                    out var genericsCommand))
                {
                    collection.Add(new ServiceDescriptor(commandGenericWithTypes, handler.Type,
                        handler.Options.ServiceLifetime));
                    handlerDescriptions.Add(new HandlerDescription(handler.Type, genericsCommand.First()));
                }

                collection.AddSingleton<IHandlersRegistry>(new HandlersRegistry(handlerDescriptions));
            }

            collection.AddSingletonAsImplementedInterfacesAndSelf<EndpointConfigProvider>();
           
            collection.AddTransient<IModulosHost, ModulosHost>();
            collection.AddTransient<IHandlerFactory, HandlerFactory>();
            collection.AddTransient<IMessageInvoker, MessageInvoker>();

            //collection.AddTransient<ITransportPolicy, TransportPolicyWithPipes>();
            collection.AddTransient<ITransportPolicy, TransportPolicy>();
            collection.AddScoped<ITransportEngineProvider, TransportEngineProvider>();


            collection.AddSingleton<IMessageConfigProvider, MessageConfigProvider>();
            collection.AddSingleton<IConnectionMarkProvider, ConnectionMarkProvider>();
            collection.AddSingleton<IEndpointConfigValidator, EndpointConfigValidator>();
            collection.AddSingleton<IServiceStateProvider, ServiceStateProvider>();

    
            collection.AddSingleton<ITransferObjectFactory, TransferObjectFactory>();
            collection.AddSingleton<IObjectSerializer, ObjectSerializer>();
            collection.AddSingleton<IHeaderSerializer, HeaderSerializer>();
            collection.AddSingleton<ICreateRequest, CreateRequest>();
            collection.AddSingleton<IParseRequest, ParseRequest>();

            collection.AddSingleton<ICreateObjectResponse, CreateObjectResponse>();
            collection.AddSingleton<ICreateFaultResponse, CreateFaultResponse>();
            collection.AddSingleton<ICreateEmptyResponse, CreateEmptyResponse>();

            collection.AddSingleton<IParseResponse, ParseResponse>();
            collection.AddSingleton<IParseFaultResponse, ParseFaultResponse>();
            collection.AddSingleton<IParseEmptyResponse, ParseEmptyResponse>();
            collection.AddSingleton<IParseObjectResponse, ParseObjectResponse>();
        }

    }
}