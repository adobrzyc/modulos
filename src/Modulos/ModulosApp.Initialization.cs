﻿// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Modulos
{
    using System.Threading;
    using System.Threading.Tasks;
    using Pipes;

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
                public ValueTask<PipeResult> Execute(CancellationToken token)
                {
                    return new ValueTask<PipeResult>(PipeResult.Continue);
                }
            }
        }
    }
}