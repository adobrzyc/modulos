using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable UnusedType.Global

namespace Modulos.Messaging.Tests.Fixtures.Domain.Handlers
{
    public class DownloadQueryHandler : IQueryHandler<DownloadQuery, FileResult>
    {
        [SuppressMessage("ReSharper", "ConditionIsAlwaysTrueOrFalse")]
        public Task<FileResult> Handle(DownloadQuery query, InvocationContext invocationContext, CancellationToken token)
        {
            var longStream = false;

            var file = Utils.EvilPngPath;
            var  bytes = new List<byte>();
            for (var i = 0; i < 50; i++)
            {
                bytes.Add(244);
            }

            var stream = longStream
                ? File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read)
                : new MemoryStream(bytes.ToArray()) as Stream;
            
            
            var output = new FileResult
            {
                FileName = query.FileName,
                Stream =stream
            };

            return Task.FromResult(output);
        }
    }
}