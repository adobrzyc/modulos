﻿using System.Threading;
using System.Threading.Tasks;
using Modulos.Pipes;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Modulos
{
    public partial class ModulosApp
    {
        public interface IUpdateConfigPipeline
        {
            void Update(IPipeline pipeline);
        }

        public class Configuration
        {
            /// <summary>
            /// Do nothing, just point where pipeline start. 
            /// </summary>
            public class Begin : IPipe
            {
                public ValueTask<PipeResult> Execute(CancellationToken cancellationToken)
                {
                    return new ValueTask<PipeResult>(PipeResult.Continue);
                }
            }
        }
    }
}