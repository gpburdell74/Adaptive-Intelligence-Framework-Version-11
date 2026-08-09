using Adaptive.Intelligence.Abstractions;
using System.Net;
using System.Net.Sockets;

namespace Adaptive.Intelligence.Networking.Tcp
{
    /// <summary>
    /// Provides a container for specifying connection options for TCP clients.
    /// </summary>
    /// <seealso cref="DisposableObjectBase" />
    public sealed class TcpClientOptions : DisposableObjectBase
    {
        #region Private Member Declarations

        private const int DefaultBufferReceive = 1024000;
        private const int DefaultBufferSend = 1024000;
        private const int DefaultTimeoutReceive = 3000;
        private const int DefaultTimeoutSend = 3000;
        private const int DefaultPort = 80;
        private const int MaxPort = 65536;
        private const int MinPort = 0;

        private short _port;

        #endregion

        #region Dispose
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            LingerState = null;
            RemoteHostEntry = null;
            RemoteHostNameOrAddress = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the reference to an object indicating whether and how the socket will delay closing
        /// in an attempt ot send all data.
        /// </summary>
        /// <value>
        /// A <see cref="LingerOption"/> instance.
        /// </value>
        public LingerOption? LingerState { get; set; } = new LingerOption(false, 0);

        /// <summary>
        /// Gets or sets a value indicating whether the stream socket is using the Nagle algorithm.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the Nagle algorithm is used; otherwise, <c>false</c>.
        /// </value>
        public bool NoDelay { get; set; } = true;

        /// <summary>
        /// Gets or sets the port number to connect to.
        /// </summary>
        /// <value>
        /// An integer specifying the port number.
        /// </value>
        public int Port
        {
            get => (int)_port;
            set
            {
                if (value is >= MinPort and < MaxPort)
                {
                    _port = (short)value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IPEndPoint"/> instance specifying the remote host to connect to.
        /// </summary>
        /// <value>
        /// An <see cref="IPEndPoint"/> instance specifying the remote host address.
        /// </value>
        public IPEndPoint? RemoteHostEntry { get; set; }

        /// <summary>
        /// Gets or sets the domain name or IP Address of the remote host to connect to.
        /// </summary>
        /// <value>
        /// A string containing the remote host name or IP address.
        /// </value>
        public string? RemoteHostNameOrAddress { get; set; }

        /// <summary>
        /// Gets or sets the size of the receive buffer, in bytes.
        /// </summary>
        /// <value>
        /// An integer specifying the size of the receive buffer.
        /// </value>
        public int ReceiveBufferSize { get; set; } = DefaultBufferReceive;

        /// <summary>
        /// Gets or sets the receive timeout, in milliseconds.
        /// </summary>
        /// <value>
        /// An integer specifying the recieve timeout value.
        /// </value>
        public int ReceiveTimeout { get; set; } = DefaultTimeoutReceive;

        /// <summary>
        /// Gets or sets the size of the send buffer, in bytes.
        /// </summary>
        /// <value>
        /// An integer specifying the size of the send buffer.
        /// </value>
        public int SendBufferSize { get; set; } = DefaultBufferSend;

        /// <summary>
        /// Gets or sets the send timeout, in milliseconds.
        /// </summary>
        /// <value>
        /// An integer specifying the send timeout value.
        /// </value>
        public int SendTimeout { get; set; } = DefaultTimeoutSend;
        #endregion

        #region Default Instance
        /// <summary>
        /// Gets the default <see cref="TcpClientOptions"/> instance.
        /// </summary>
        /// <value>
        /// The <see cref="TcpClientOptions"/> instance.
        /// </value>
        public static TcpClientOptions Default => new()
        {
            Port = DefaultPort,
            NoDelay = true,
            SendBufferSize = DefaultBufferSend,
            ReceiveBufferSize = DefaultBufferReceive,
            ReceiveTimeout = DefaultTimeoutReceive,
            SendTimeout = DefaultTimeoutSend,
        };
        #endregion
    }
}