using System.Threading;
using System.Threading.Tasks;

namespace System.IO
{
    public static class StreamExtensions
    {
        public static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize, IProgress<long> progress, CancellationToken cancellationToken = default)
        {
            if (source == null) {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination == null) {
                throw new ArgumentNullException(nameof(destination));
            }

            if (bufferSize < 0) {
                throw new ArgumentOutOfRangeException(nameof(bufferSize), bufferSize, $"{nameof(bufferSize)} has to be greater than zero");
            }

            if (progress == null) {
                throw new ArgumentNullException(nameof(progress));
            }

            if (!source.CanRead) {
                throw new ArgumentException($"{nameof(source)} is not readable.", nameof(source));
            }

            if (!destination.CanWrite) {
                throw new ArgumentException($"{nameof(destination)} is not writeable.", nameof(source));
            }

            var buffer = new byte[bufferSize];
            int totalBytesRead = 0;
            int bytesRead;
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0) {
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
                totalBytesRead += bytesRead;
                progress.Report(totalBytesRead);
            }
        }
    }
}
