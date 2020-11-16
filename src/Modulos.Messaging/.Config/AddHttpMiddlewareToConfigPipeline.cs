using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Modulos.Messaging.Transport.Http;
using Modulos.Pipes;

// ReSharper disable UnusedType.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Modulos.Messaging.Config
{
    public class AddHttpMiddlewareToConfigPipeline : ModulosApp.IUpdateConfigPipeline
    {
        public void Update(IPipeline pipeline)
        {
            pipeline.Add<UseModulosMiddleware>();
        }

        public class UseModulosMiddleware : IOptionalPipe
        {
            private readonly IApplicationBuilder app;

            public UseModulosMiddleware(IApplicationBuilder app)
            {
                this.app = app;
            }

            public Task<PipeResult> Execute(CancellationToken cancellationToken)
            {
                app.UseMiddleware<HttpMiddleware>();
                return Task.FromResult(new PipeResult(PipeActionAfterExecute.Continue));
            }
        }
    }
}