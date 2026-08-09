using Adaptive.Intelligence.Utility;

namespace Adaptive.Intelligence.Networking.Events
{
    /// <summary>
    /// Provides the event arguments definition for events that handle reception of data on network connections.
    /// </summary>
    public class DataReceivedEventArgs : EventArgs
    {
        #region Private Member Declarations
        /// <summary>
        /// The buffer containing the data that was received.
        /// </summary>
        private byte[]? _buffer;

        /// <summary>
        /// The date/time the data was received.
        /// </summary>
        private DateTime? _receptionDate;
        #endregion

        #region Constructor / Deconstructor Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="DataReceivedEventArgs"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public DataReceivedEventArgs()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="rawData">
        /// A byte array containing the raw data that was read from a network connection.
        /// </param>
        public DataReceivedEventArgs(byte[] rawData)
        {
            _receptionDate = DateTime.Now;

            // Copy the provided data.
            _buffer = new byte[rawData.Length];
            Array.Copy(rawData, 0, _buffer, 0, rawData.Length);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="socketBuffer">
        /// A reference to the byte array buffer being used by the socket polling mechanism.
        /// </param>
        /// <param name="dataLength">
        /// An integer specifying the amount of data to read.
        /// </param>
        public DataReceivedEventArgs(byte[] socketBuffer, int dataLength)
        {
            _receptionDate = DateTime.Now;

            // Copy the provided data.
            _buffer = new byte[dataLength];
            if (dataLength > 0)
            {
                Array.Copy(socketBuffer, 0, _buffer, 0, dataLength);
            }
        }
        /// <summary>
        /// Finalizes an instance of the <see cref="DataReceivedEventArgs"/> class.
        /// </summary>
        ~DataReceivedEventArgs()
        {
            ByteArrayUtil.Clear(_buffer);
            _buffer = null;
            _receptionDate = null;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the refernce to the data.
        /// </summary>
        /// <value>
        /// A byte array containing the data that was read, or <b>null</b>.
        /// </value>
        public byte[]? Data => _buffer;

        /// <summary>
        /// Gets a copy of the the data as a string.
        /// </summary>
        /// <remarks>
        /// This property assumes the data encoding is ASCII.
        /// </remarks>
        /// <value>
        /// A string representing the data content in <see cref="Data"/>.
        /// </value>
        public string? DataAsString
        {
            get
            {
                string? content = null;
                if (_buffer != null)
                {
                    content = System.Text.Encoding.ASCII.GetString(_buffer);
                }
                return content;
            }
        }

        /// <summary>
        /// Gets a copy of the the data as a string.
        /// </summary>
        /// <remarks>
        /// This property assumes the data encoding is UTF-8.
        /// </remarks>
        /// <value>
        /// A string representing the data content in <see cref="Data"/>.
        /// </value>
        public string? DataAsUTF8String
        {
            get
            {
                string? content = null;
                if (_buffer != null)
                {
                    content = System.Text.Encoding.UTF8.GetString(_buffer);
                }
                return content;
            }
        }

        /// <summary>
        /// Gets a copy of the the data as a string.
        /// </summary>
        /// <remarks>
        /// This property assumes the data encoding is UTF-32.
        /// </remarks>
        /// <value>
        /// A string representing the data content in <see cref="Data"/>.
        /// </value>
        public string? DataAsUTF32String
        {
            get
            {
                string? content = null;
                if (_buffer != null)
                {
                    content = System.Text.Encoding.UTF32.GetString(_buffer);
                }
                return content;
            }
        }

        /// <summary>
        /// Gets the length of the data.
        /// </summary>
        /// <value>
        /// An integer specifying the length of the data.
        /// </value>
        public int Length
        {
            get
            {
                if (_buffer == null)
                {
                    return 0;
                }
                else
                {
                    return _buffer.Length;
                }
            }
        }

        /// <summary>
        /// Gets the date and time the data was received.
        /// </summary>
        /// <value>
        /// A <see cref="DateTime"/> specifying the date/time the data was received, or <b>null</b>.
        /// </value>
        public DateTime? ReceivedDate => _receptionDate;
        #endregion

        /// <summary>
        /// Determines whether the event arguments data content is null or empty.
        /// </summary>
        /// <param name="e">The <see cref="DataReceivedEventArgs"/> instance containing the event data.</param>
        /// <returns>
        ///   <c>true</c> if the <see cref="DataReceivedEventArgs.Data"/> property is <b>null</b> or zero-length;
        ///   otherwise, <b>false</b>.
        /// </returns>
        public static bool IsNullOrEmpty(DataReceivedEventArgs? e)
        {
            return e == null || e.Data == null || e.Length == 0;
        }
    }
}