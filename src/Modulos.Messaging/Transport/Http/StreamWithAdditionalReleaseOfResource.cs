using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Modulos.Messaging.Transport.Http
{
    /// <summary>
    /// Wrapper for a stream which is able to dispose additional resources. 
    /// </summary>
    /// <remarks>
    /// Used to dispose request, response and client.
    /// </remarks>
    internal sealed class StreamWithAdditionalReleaseOfResource : Stream
    {
        private Stack<IDisposable> disposables = new Stack<IDisposable>();

        /// <summary>
        /// Registers an object for disposal by the stream once the stream has finished processing.
        /// </summary>
        /// <param name="disposable">The object to be disposed.</param>
        public void RegisterForDispose(IDisposable disposable)
        {
            disposables.Push(disposable);
        }


        private readonly Stream stream;

        public StreamWithAdditionalReleaseOfResource([JetBrains.Annotations.NotNull] Stream stream)
        {
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
        }

        public override void Flush()
        {
            stream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return stream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            stream.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var isRead = stream.Read(buffer, offset, count);
            if (isRead != 0) return isRead;

            Dispose(true);
            return isRead;
        }


        public override void Write(byte[] buffer, int offset, int count)
        {
            stream.Write(buffer, offset, count);
        }

        public override bool CanRead => stream.CanRead;

        public override bool CanSeek => stream.CanSeek;

        public override bool CanWrite => stream.CanWrite;

        public override long Length => stream.Length;

        public override long Position
        {
            get => stream.Position;
            set => stream.Position = value;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return stream.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return stream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void Close()
        {
            stream.Close();
        }


        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            return stream.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return stream.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            stream.EndWrite(asyncResult);
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
           
            return stream.FlushAsync(cancellationToken);
        }

     
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return stream.ReadAsync(buffer, offset, count, cancellationToken);
        }


        /* .NET Core 3.0+
        public override int Read(Span<byte> buffer)
        {
            return stream.Read(buffer);
        }

        public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = new CancellationToken())
        {
            return stream.ReadAsync(buffer, cancellationToken);
        }

        public override void Write(ReadOnlySpan<byte> buffer)
        {
            stream.Write(buffer);
        }

        public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = new CancellationToken())
        {
            return stream.WriteAsync(buffer, cancellationToken);
        }

        public override void CopyTo(Stream destination, int bufferSize)
        {
            stream.CopyTo(destination, bufferSize);
        }
        */

        public override int ReadByte()
        {
            return stream.ReadByte();
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return stream.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override void WriteByte(byte value)
        {
            stream.WriteByte(value);
        }

        public override bool CanTimeout => stream.CanTimeout;

        public override int ReadTimeout
        {
            get => stream.ReadTimeout;
            set => stream.ReadTimeout = value;
        }

        public override int WriteTimeout  {
            get => stream.WriteTimeout;
            set => stream.WriteTimeout = value;
        }
       
        public override object InitializeLifetimeService()
        {
            return stream.InitializeLifetimeService();
        }

        public override bool Equals(object obj)
        {
            return stream.Equals(obj);
        }

        public override int GetHashCode()
        {
            return stream.GetHashCode();
        }

        public override string ToString()
        {
            return stream.ToString();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                stream.Dispose();
                while (disposables?.Count > 0)
                    disposables.Pop().Dispose();

                disposables = null;
            }
            base.Dispose(disposing);
        }
    }
}