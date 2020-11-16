using System.IO;
using System.Threading.Tasks;

namespace Modulos.Messaging.Transport.Http
{
    internal static class IoExtensions
    {
        //private static readonly RecyclableMemoryStreamManager manager = new RecyclableMemoryStreamManager();

        public static async Task<byte[]> ToByteArrayAsync(this Stream stream)
        {
            if (stream is MemoryStream memoryStream)
                return memoryStream.ToArray();

            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            return ms.ToArray();

            //using (var ms = manager.GetStream())
            //{
            //    await stream.CopyToAsync(ms);
            //    return ms.ToArray();
            //}
        }
    }
}