using Adaptive.Intelligence.Utility;
using System.IO.Compression;
using System.Text;

namespace Adaptive.Intelligence.IO
{
    /// <summary>
    /// Provides static methods / functions for compressing and decompressing data.
    /// </summary>
    public static class AdaptiveCompression
    {
        #region Public Static Methods / Functions

        #region Compression

        /// <summary>
        /// Compresses the specified string into a byte array.
        /// </summary>
        /// <param name="sourceContent">
        /// The Unicode string to be compressed.
        /// </param>
        /// <returns>
        /// A byte array containing the compressed data.
        /// </returns>
        /// <exception cref="ArgumentNullException">sourceContent</exception>
        public static byte[] Compress(string sourceContent)
        {
            ArgumentNullException.ThrowIfNull(sourceContent);

            return Compress(Encoding.Unicode.GetBytes(sourceContent));
        }

        /// <summary>
        /// Compresses the specified source data into a byte array.
        /// </summary>
        /// <param name="sourceStream">
        /// An open, readable <see cref="MemoryStream"/> instance containing
        /// the content to be compressed.
        /// </param>
        /// <returns>
        /// A byte array containing the compressed data.
        /// </returns>
        /// <exception cref="ArgumentNullException">sourceStream</exception>
        /// <exception cref="ArgumentOutOfRangeException">Cannot read from source stream.</exception>
        public static byte[] Compress(MemoryStream sourceStream)
        {
            ArgumentNullException.ThrowIfNull(sourceStream);
            return !sourceStream.CanRead
                ? throw new ArgumentOutOfRangeException(nameof(sourceStream), "Could not read from the specified stream.")
                : Compress(sourceStream.ToArray());
        }

        /// <summary>
        /// Compresses the specified byte array.
        /// </summary>
        /// <param name="sourceContent">
        /// The byte array containing the data to be compressed.
        /// </param>
        /// <returns>
        /// A byte array containing the compressed data.
        /// </returns>
        /// <exception cref="ArgumentNullException">sourceContent</exception>
        public static byte[] Compress(byte[] sourceContent)
        {
            ArgumentNullException.ThrowIfNull(sourceContent);

            byte[]? compressedData = null;

            // Create the streams.
            var outputStream = new MemoryStream();
            GZipStream? compressionStream = CreateCompressionStream(outputStream);
            if (compressionStream != null)
            {
                // Perform the actual compression.
                if (WriteData(compressionStream, sourceContent))
                {
                    compressionStream.Dispose();
                    compressedData = outputStream.ToArray();
                }
                else
                {
                    compressionStream.Dispose();
                }
            }

            // On failure, return the original content.
            if (compressedData == null)
            {
                compressedData = new byte[sourceContent.Length];
                Array.Copy(sourceContent, compressedData, sourceContent.Length);
            }

            outputStream.Dispose();

            return compressedData;
        }

        /// <summary>
        /// Compresses the content of the input stream and writes it to the output stream.
        /// </summary>
        /// <param name="inputStream">
        /// The reference to the input <see cref="Stream"/> instance.
        /// </param>
        /// <param name="outputStream">
        /// The reference to the output <see cref="Stream"/> instance.
        /// </param>
        public static void Compress(Stream inputStream, Stream outputStream)
        {
            GZipStream? compressionStream = CreateCompressionStream(outputStream);
            if (compressionStream != null)
            {
                // Perform the actual compression.
                if (WriteData(compressionStream, inputStream))
                {
                    compressionStream.Flush();
                    compressionStream.Dispose();
                }

                compressionStream.Dispose();
            }
        }

        /// <summary>
        /// Compresses the content of the input stream and writes it to the output stream.
        /// </summary>
        /// <param name="inputStream">
        /// The reference to the input <see cref="Stream"/> instance.
        /// </param>
        /// <param name="outputStream">
        /// The reference to the output <see cref="Stream"/> instance.
        /// </param>
        public static async Task CompressAsync(Stream inputStream, Stream outputStream)
        {
            GZipStream? compressionStream = CreateCompressionStream(outputStream);
            if (compressionStream != null)
            {
                // Perform the actual compression.
                if (await WriteDataAsync(compressionStream, inputStream).ConfigureAwait(false))
                {
                    await compressionStream.FlushAsync().ConfigureAwait(false);
                    await compressionStream.DisposeAsync().ConfigureAwait(false);
                }

                await compressionStream.DisposeAsync().ConfigureAwait(false);
            }
        }

        #endregion

        #region Decompression

        /// <summary>
        /// Decompresses the specified data stream.
        /// </summary>
        /// <param name="sourceContent">
        /// An open, readable <see cref="MemoryStream"/> instance containing the
        /// compressed data.
        /// </param>
        /// <returns>
        /// A byte array containing the uncompressed data.
        /// </returns>
        /// <exception cref="ArgumentNullException">sourceContent</exception>
        /// <exception cref="ArgumentOutOfRangeException">Cannot read from specified stream.</exception>
        public static byte[]? Decompress(MemoryStream sourceContent)
        {
            ArgumentNullException.ThrowIfNull(sourceContent);
            if (!sourceContent.CanRead)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceContent), "Could not read from the specified stream.");
            }

            byte[]? result = null;

            GZipStream? decompressionStream = CreateDecompressionStream(sourceContent);
            if (decompressionStream != null)
            {
                result = DecompressContent(decompressionStream);
            }

            return result;
        }

        /// <summary>
        /// Decompresses the specified data array.
        /// </summary>
        /// <param name="sourceContent">
        /// A byte array containing the compressed data.
        /// </param>
        /// <returns>
        /// A byte array containing the uncompressed data.
        /// </returns>
        /// <exception cref="ArgumentNullException">sourceContent</exception>
        public static byte[]? Decompress(byte[] sourceContent)
        {
            ArgumentNullException.ThrowIfNull(sourceContent);

            byte[]? result = null;

            using (var inStream = new MemoryStream(sourceContent))
            {
                GZipStream? decompressionStream = CreateDecompressionStream(inStream);
                if (decompressionStream != null)
                {
                    result = DecompressContent(decompressionStream);
                }
            }

            return result;
        }

        /// <summary>
        /// Decompresses the specified data array and writes it to the destination stream.
        /// </summary>
        /// <param name="sourceContent">
        /// A byte array containing the compressed data.
        /// </param>
        /// <param name="destinationStream">
        /// The <see cref="Stream"/> instance to which the decompressed data content is to be written.
        /// </param>
        /// <returns>
        /// A byte array containing the uncompressed data.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the <i>sourceContent</i> parameter is <b>null</b>.
        /// </exception>
        public static void Decompress(byte[] sourceContent, Stream destinationStream)
        {
            ArgumentNullException.ThrowIfNull(sourceContent);

            using var inStream = new MemoryStream(sourceContent);
            GZipStream? decompressionStream = CreateDecompressionStream(inStream);
            if (decompressionStream != null)
            {
                destinationStream.Write(DecompressContent(decompressionStream));
                destinationStream.Flush();
                decompressionStream.CopyTo(destinationStream);
            }
        }

        /// <summary>
        /// Decompresses the specified data from one stream to another.
        /// </summary>
        /// <param name="sourceStream">
        /// An open, readable <see cref="Stream"/> instance containing the
        /// compressed data.
        /// </param>
        /// <param name="destinationStream">
        /// An open, writable <see cref="Stream"/> instance to where the expanded data content will be written.
        /// </param>
        /// <exception cref="ArgumentNullException">sourceContent</exception>
        /// <exception cref="ArgumentOutOfRangeException">Cannot read from specified stream.</exception>
        public static void Decompress(Stream sourceStream, Stream destinationStream)
        {
            if (!sourceStream.CanRead)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceStream), "Could not read from the specified stream.");
            }

            if (!destinationStream.CanWrite)
            {
                throw new ArgumentOutOfRangeException(nameof(destinationStream), "Could not write to the specified stream.");
            }

            sourceStream.Seek(0, SeekOrigin.Begin);

            GZipStream? decompressionStream = CreateDecompressionStream(sourceStream);
            if (decompressionStream != null)
            {
                DecompressContent(decompressionStream, destinationStream);
                decompressionStream.Dispose();
            }

            destinationStream.Flush();
        }


        /// <summary>
        /// Decompresses the specified data from one stream to another.
        /// </summary>
        /// <param name="sourceStream">
        /// An open, readable <see cref="Stream"/> instance containing the
        /// compressed data.
        /// </param>
        /// <param name="destinationStream">
        /// An open, writable <see cref="Stream"/> instance to where the expanded data content will be written.
        /// </param>
        /// <exception cref="ArgumentNullException">sourceContent</exception>
        /// <exception cref="ArgumentOutOfRangeException">Cannot read from specified stream.</exception>
        public static async Task DecompressAsync(Stream sourceStream, Stream destinationStream)
        {
            if (!sourceStream.CanRead)
            {
                throw new ArgumentOutOfRangeException(nameof(sourceStream), "Could not read from the specified stream.");
            }

            if (!destinationStream.CanWrite)
            {
                throw new ArgumentOutOfRangeException(nameof(destinationStream), "Could not write to the specified stream.");
            }

            sourceStream.Seek(0, SeekOrigin.Begin);

            GZipStream? decompressionStream = CreateDecompressionStream(sourceStream);
            if (decompressionStream != null)
            {
                await DecompressContentAsync(decompressionStream, destinationStream).ConfigureAwait(false);
                await decompressionStream.DisposeAsync().ConfigureAwait(false);
            }

            await destinationStream.FlushAsync().ConfigureAwait(false);
        }

        #endregion

        #endregion

        #region Private Static Methods / Functions

        /// <summary>
        /// Attempts to create the compression stream.
        /// </summary>
        /// <param name="outputStream">
        /// The output <see cref="Stream"/> to which the compressed data
        /// will be written.
        /// </param>
        /// <returns>
        /// A new <see cref="GZipStream"/> instance, or <b>null</b> if the operation
        /// fails.
        /// </returns>
        private static GZipStream? CreateCompressionStream(Stream? outputStream)
        {
            GZipStream? compressionStream = null;

            if (outputStream != null)
            {
                try
                {
                    compressionStream = new GZipStream(outputStream, CompressionLevel.Optimal, true);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }

            return compressionStream;
        }

        /// <summary>
        /// Attempts to create the decompression stream.
        /// </summary>
        /// <param name="inputStream">
        /// The output <see cref="Stream"/> from which the compressed data
        /// will be read.
        /// </param>
        /// <returns>
        /// A new <see cref="GZipStream"/> instance, or <b>null</b> if the operation
        /// fails.
        /// </returns>
        private static GZipStream? CreateDecompressionStream(Stream? inputStream)
        {
            GZipStream? decompressionStream = null;
            if (inputStream != null)
            {
                try
                {
                    decompressionStream = new GZipStream(inputStream, CompressionMode.Decompress);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }

            return decompressionStream;
        }

        /// <summary>
        /// Decompresses the content from the provided stream.
        /// </summary>
        /// <param name="decompressionStream">
        /// The <see cref="GZipStream"/> decompression stream.
        /// </param>
        /// <returns>
        /// A byte array containing the uncompressed data, or <b>null</b>.
        /// </returns>
        private static byte[]? DecompressContent(GZipStream? decompressionStream)
        {
            byte[]? result = null;

            if (decompressionStream != null)
            {
                using var outputStream = new MemoryStream();
                try
                {
                    decompressionStream.CopyTo(outputStream);
                    decompressionStream.Dispose();
                    result = outputStream.ToArray();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// Decompresses the content from the provided stream.
        /// </summary>
        /// <param name="decompressionStream">
        /// The <see cref="GZipStream"/> decompression stream.
        /// </param>
        /// <param name="destinationStream">
        /// The <see cref="Stream"/> instance to write to.
        /// </param>
        /// <returns>
        /// A byte array containing the uncompressed data, or <b>null</b>.
        /// </returns>
        private static void DecompressContent(GZipStream? decompressionStream, Stream? destinationStream)
        {
            if (decompressionStream != null && destinationStream != null)
            {
                try
                {
                    decompressionStream.CopyTo(destinationStream);
                    decompressionStream.Dispose();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }
        }

        /// <summary>
        /// Decompresses the content from the provided stream.
        /// </summary>
        /// <param name="decompressionStream">
        /// The <see cref="GZipStream"/> decompression stream.
        /// </param>
        /// <param name="destinationStream">
        /// The <see cref="Stream"/> instance to write to.
        /// </param>
        /// <returns>
        /// A byte array containing the uncompressed data, or <b>null</b>.
        /// </returns>
        private static async Task DecompressContentAsync(GZipStream? decompressionStream, Stream? destinationStream)
        {
            if (decompressionStream != null && destinationStream != null)
            {
                try
                {
                    await decompressionStream.CopyToAsync(destinationStream).ConfigureAwait(false);
                    await decompressionStream.DisposeAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }
        }

        /// <summary>
        /// Writes the original data to the compression stream.
        /// </summary>
        /// <param name="compressionStream">
        /// The <see cref="GZipStream"/> compression stream.
        /// </param>
        /// <param name="sourceContent">
        /// The original data to be compressed.
        /// </param>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise,
        /// returns <b>false</b>.
        /// </returns>
        private static bool WriteData(GZipStream? compressionStream, byte[]? sourceContent)
        {
            var success = false;
            if ((compressionStream != null) && (sourceContent != null))
            {
                try
                {
                    compressionStream.Write(sourceContent, 0, sourceContent.Length);
                    compressionStream.Flush();
                    success = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }

            return success;
        }

        /// <summary>
        /// Writes the original data to the compression stream.
        /// </summary>
        /// <param name="compressionStream">
        /// The <see cref="GZipStream"/> compression stream.
        /// </param>
        /// <param name="sourceContent">
        /// The <see cref="Stream"/> containing the original data to be compressed.
        /// </param>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise,
        /// returns <b>false</b>.
        /// </returns>
        private static bool WriteData(GZipStream? compressionStream, Stream? sourceContent)
        {
            var success = false;
            if ((compressionStream != null) && (sourceContent != null))
            {
                try
                {

                    byte[] sourceArray = new byte[sourceContent.Length];
                    sourceContent.ReadExactly(sourceArray, 0, sourceArray.Length);
                    compressionStream.Write(sourceArray, 0, sourceArray.Length);
                    ByteArrayUtil.Clear(sourceArray);

                    compressionStream.Flush();
                    success = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }

            return success;
        }

        /// <summary>
        /// Writes the original data to the compression stream.
        /// </summary>
        /// <param name="compressionStream">
        /// The <see cref="GZipStream"/> compression stream.
        /// </param>
        /// <param name="sourceContent">
        /// The <see cref="Stream"/> containing the original data to be compressed.
        /// </param>
        /// <param name="token">
        /// An optional <see cref="CancellationToken"/> used to cancel the operation.
        /// </param>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise,
        /// returns <b>false</b>.
        /// </returns>
        private static async Task<bool> WriteDataAsync(GZipStream? compressionStream, Stream? sourceContent,
            CancellationToken token = default)
        {
            bool success = false;
            if ((compressionStream != null) && (sourceContent != null))
            {
                try
                {

                    Memory<byte> sourceArray = new byte[sourceContent.Length];
                    await sourceContent.ReadExactlyAsync(sourceArray, token).ConfigureAwait(false);
                    await compressionStream.WriteAsync(sourceArray, token).ConfigureAwait(false);
                    await compressionStream.FlushAsync(token).ConfigureAwait(false);
                    success = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }

            return success;
        }

        #endregion
    }
}