using Adaptive.Intelligence.Abstractions;
using System.Net.Sockets;
using System.Text;

namespace Adaptive.Intelligence.IO
{
    /// <summary>
    /// Provides a stream-reader style class for reading lines of text without advancing the
    /// stream beyond the point at which it is being read.
    /// </summary>
    /// <remarks>
    /// The <see cref="StreamReader"/> class will read ahead in the stream and pre-cache some of
    /// the data.  This is undesirable when reading text header data from a network stream that
    /// also contains binary data in the payload.
    /// </remarks>
    /// <seealso cref="DisposableObjectBase" />
    /// <seealso cref="NetworkStream"/>
    /// <remarks>
    /// Initializes a new instance of the <see cref="NetworkStreamReader"/> class.
    /// </remarks>
    /// <param name="stream">
    /// The <see cref="NetworkStream"/> instance to read from.
    /// </param>
    public sealed class NetworkStreamReader(NetworkStream? stream) : DisposableObjectBase
    {
        #region Private Member Declarations
        /// <summary>
        /// The source stream to read from.
        /// </summary>
        private NetworkStream? _source = stream;
        #endregion

        #region Constructor / Dispose Methods
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            // Do not dispose of the source stream - just release the reference.  This stream
            // may still be being read from.
            _source = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Attempts to read the next line of text from the network stream.w
        /// </summary>
        /// <returns>
        /// A string containing the data that was read; otherwise, returns <b>null</b>
        /// on failure.
        /// </returns>
        public string? ReadLine()
        {
            bool isDone = false;
            string? returnValue = null;
            int waitCounter = 0;

            if (_source != null)
            {
                // HTTP lines of text are rarely more than 80 characters.
                StringBuilder builder = new(80);

                // Wait for data.
                while (!_source.DataAvailable && waitCounter < 256)
                {
                    Thread.Sleep(10);
                    waitCounter++;
                }

                if (_source.DataAvailable)
                {
                    // Read each byte until a linefeed character is found.
                    do
                    {
                        // ReadByte() will return a -1 if the read fails or no more data is present.
                        int nextChar = -1;
                        if (_source.DataAvailable)
                        {
                            try
                            {
                                nextChar = _source.ReadByte();
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Trace.TraceError(ex.Message);
                            }
                        }

                        // If -1 is done, either the stream failed or no more data is to be read.
                        if (nextChar == -1)
                        {
                            isDone = true;
                        }
                        else
                        {
                            // Add the character that was read.
                            builder.Append((char)nextChar);

                            // If a linefeed character is read, the line of text is done.
                            // (Carriage-return, if present, always precedes linefeed.  Carriage-return
                            //  may not be sent by UNIX/Linux systems.)
                            if (nextChar == 10)
                            {
                                isDone = true;
                            }
                        }

                    } while (!isDone);
                }

                // Return the string buffer that was read.
                if (builder.Length > 0)
                {
                    returnValue = builder.ToString();
                }
            }

            return returnValue;
        }
        /// <summary>
        /// Attempts to read the next line of text from the network stream.w
        /// </summary>
        /// <returns>
        /// A string containing the data that was read; otherwise, returns <b>null</b>
        /// on failure.
        /// </returns>
        public async Task<string?> ReadLineAsync()
        {
            await Task.Yield();
            string? returnValue = ReadLine();
            return returnValue;
        }
        #endregion
    }
}
