using Adaptive.Intelligence.Abstractions;
using System.Text;

namespace Adaptive.Intelligence.Utility
{
    /// <summary>
    /// Provides a class for concatenating byte arrays.
    /// </summary>
    /// <remarks>
    /// This class operates much like the <see cref="StringBuilder"/>, but for byte arrays.
    /// </remarks>
    public class BufferBuilder : DisposableObjectBase
    {
        #region Private Member Declarations
        /// <summary>
        /// The storage stream.
        /// </summary>
        private MemoryStream? _stream;
        #endregion

        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="BufferBuilder"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public BufferBuilder()
        {
            _stream = new MemoryStream();
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                _stream?.Dispose();
            }

            _stream = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Appends the byte array to the buffer.
        /// </summary>
        /// <param name="data">
        /// A byte array to be appended to the current buffer.
        /// </param>
        public void Append(byte[]? data)
        {
            if (!ByteArrayUtil.IsNullOrEmpty(data))
            {
                _stream?.Write(data, 0, data.Length);
            }
        }
        /// <summary>
        /// Appends the byte array to the buffer.
        /// </summary>
        /// <param name="data">
        /// A byte array to be appended to the current buffer.
        /// </param>
        /// <param name="offset">
        /// The offset in the source array at which to begin copying.
        /// </param>
        /// <param name="length">
        /// The number of bytes to be copied.
        /// </param>
        public void Append(byte[]? data, int offset, int length)
        {
            if (!ByteArrayUtil.IsNullOrEmpty(data))
            {
                if (offset > -1 && length > 0)
                {
                    _stream?.Write(data, offset, length);
                }
            }
        }
        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            _stream?.Close();
            _stream?.Dispose();
            _stream = null;
        }
        /// <summary>
        /// Writes the buffer contents to a byte array.
        /// </summary>
        /// <returns>
        /// A byte array containing the current buffer content.
        /// </returns>
        public byte[]? ToArray()
        {
            return _stream?.ToArray();
        }
        #endregion
    }
}
