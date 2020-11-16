using System.IO;
using System.Threading;
using System.Threading.Tasks;

// ReSharper disable UnusedType.Global

namespace Modulos.Messaging.Tests.Fixtures.Domain.Handlers
{
    public class UploadAndDownloadQueryHandler : IQueryHandler<UploadAndDownloadQuery, FileResult>
    {
        public async Task<FileResult> Handle(UploadAndDownloadQuery query, InvocationContext invocationContext, CancellationToken token)
        {
            var ms = new MemoryStream();
            await query.Stream.CopyToAsync(ms, token);
            ms.Position = 0;

            return new FileResult
            {
                FileName = Path.GetFileName("no file name boy"),
                Stream = ms
            };
        }
    }
}