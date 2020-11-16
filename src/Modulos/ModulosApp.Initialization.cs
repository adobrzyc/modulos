using System.Threading;
using System.Threading.Tasks;
using Modulos.Pipes;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Modulos
{
    public partial class ModulosApp
    {
        public interface IUpdateInitializationPipeline
        {
            void Update(IPipeline pipeline);
        }

        public class Initialization
        {
            /// <summary>
            /// Do nothing, just point where pipeline start. 
            /// </summary>
            public class Begin : IPipe
            {
                public Task<PipeResult> Execute(CancellationToken cancellationToken)
                {
                    return Task.FromResult(new PipeResult(PipeActionAfterExecute.Continue));
                }
            }
        }
    }
}