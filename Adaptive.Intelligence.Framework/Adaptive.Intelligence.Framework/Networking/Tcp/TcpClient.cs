using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Abstractions.Logging;
using Adaptive.Intelligence.Events.Delegates;
using Adaptive.Intelligence.Events.Arguments;
using Adaptive.Intelligence.Networking.Events;
using Adaptive.Intelligence.Networking.Events.Delegates;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Adaptive.Intelligence.Networking.Tcp
{
    /// <summary>
    /// Provides a class for communicating across a TCP connection.
    /// </summary>
    /// <seealso cref="DisposableObjectBase" />
    public sealed class TcpClient : LoggableBase
    {
        #region Public Events
        /// <summary>
        /// Occurs when an erorr occurs when binding to a local address.
        /// </summary>
        public event ExceptionEventHandler? BindingFailed;

        /// <summary>
        /// Occurs when the connection is closed.
        /// </summary>
        public event EventHandler? Closed;

        /// <summary>
        /// Occurs when an erorr occurs when closing the connection.
        /// </summary>
        public event ExceptionEventHandler? CloseError;

        /// <summary>
        /// Occurs when a connection is established.
        /// </summary>
        public event EventHandler? Connected;

        /// <summary>
        /// Occurs when an erorr occurs when creating a connection.
        /// </summary>
        public event ExceptionEventHandler? ConnectionFailure;

        /// <summary>
        /// Occurs when an erorr occurs when creating the socket.
        /// </summary>
        public event ExceptionEventHandler? CreateSocketFailure;

        /// <summary>
        /// Occurs when an exception is encountered when reading data.
        /// </summary>
        public event ExceptionEventHandler? DataReadFailure;

        /// <summary>
        /// Occurs when new data is received and read from the socket.
        /// </summary>
        public event DataReceievedEventHandler? DataReceived;

        /// <summary>
        /// Occurs when an erorr occurs when disconnecting from a remote host.
        /// </summary>
        public event ExceptionEventHandler? DisconnectError;

        /// <summary>
        /// Occurs when an erorr occurs when A DNS query fails.
        /// </summary>
        public event ExceptionEventHandler? DnsFailure;

        /// <summary>
        /// Occurs when an erorr occurs when attempting to use the instance without a valid socket.
        /// </summary>
        public event EventHandler? InvalidSocket;

        /// <summary>
        /// Occurs when an exception is encountered when polling the socket.
        /// </summary>
        public event ExceptionEventHandler? PollFailure;

        /// <summary>
        /// Occurs when an exception is encountered when starting the polling thread.
        /// </summary>
        public event ExceptionEventHandler? PollThreadStartFailure;

        /// <summary>
        /// Occurs when an exception is encountered when transmitting data.
        /// </summary>
        public event ExceptionEventHandler? SendFailure;

        /// <summary>
        /// Occurs when an erorr occurs when the setsockopt() call fails.
        /// </summary>
        public event ExceptionEventHandler? SetSocketOptionsFailure;

        /// <summary>
        /// Occurs when an erorr occurs when shutting down communications on the socket.
        /// </summary>
        public event ExceptionEventHandler? ShutdownError;

        /// <summary>
        /// Occurs when a socket error is detected.
        /// </summary>
        public event SocketErrorEventHandler? SocketError;

        /// <summary>
        /// Occurs when the socket is unexpectedly closed.  Usually closed by the remote host.
        /// </summary>
        public event EventHandler? UnexpectedClose;
        #endregion

        #region Private Member Declarations
        /// <summary>
        /// The threading synchronization instance.
        /// </summary>
        private static readonly Lock _syncRoot = new();

        /// <summary>
        /// The client socket instance - may be provided from a listner instance or created locally.
        /// </summary>
        private Socket? _clientSocket;

        /// <summary>
        /// The local binding, if used.
        /// </summary>
        private IPEndPoint? _localBinding;

        /// <summary>
        /// The data stream to write to and read from (for external use).
        /// </summary>
        private NetworkStream? _dataStream;

        /// <summary>
        /// The socket and connection options.
        /// </summary>
        private TcpClientOptions? _options;

        /// <summary>
        /// The polling thread instance.
        /// </summary>
        private Thread? _pollingThread;

        /// <summary>
        /// The polling thread execution flag.
        /// </summary>
        private bool _executePollingThread;

        /// <summary>
        /// The polling thread is running indicator.
        /// </summary>
        private bool _pollingThreadRunning;
        #endregion

        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="TcpClient"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public TcpClient() : this(TcpClientOptions.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpClient"/> class.
        /// </summary>
        /// <param name="options">
        /// The <see cref="TcpClientOptions"/> options instance used for connecting to remote clients.
        /// </param>
        public TcpClient(TcpClientOptions options)
        {
            LogInformation("TcpClient Created.");
            _options = options;
            CreateSocket();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpClient"/> class.
        /// </summary>
        /// <param name="localEndPoint">
        /// The local <see cref="IPEndPoint"/> to bind to.
        /// </param>
        public TcpClient(IPEndPoint localEndPoint)
        {
            LogInformation("TcpClient Created - Using default options.");
            LogInformation($"LocalEP Binding: {localEndPoint}");
            _options = TcpClientOptions.Default;

            CreateSocket();
            CreateBinding(localEndPoint);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpClient"/> class.
        /// </summary>
        /// <param name="hostname">
        /// A string containing the host name or IP address of the remote system to connect to.
        /// </param>
        /// <param name="port">
        /// An integer indicating the port number to connect to.
        /// </param>
        public TcpClient(string hostname, int port) : this()
        {
            LogInformation("TcpClient Created.");
            LogInformation($"\t{hostname}:{port}");

            _options = TcpClientOptions.Default;
            _options.RemoteHostNameOrAddress = hostname;
            _options.Port = port;
            CreateSocket();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpClient"/> class.
        /// </summary>
        /// <param name="acceptedSocket">
        /// A <see cref="Socket"/> provided from <see cref="TcpServer"/> or another listener instance that
        /// is the result of accepting a connection.
        /// </param>
        public TcpClient(Socket acceptedSocket)
        {
            LogInformation("TcpClient Created From Listener (TcpServer).");
            _options = TcpClientOptions.Default;
            _clientSocket = acceptedSocket;
            SetSocketOptions();
            _dataStream = GetStream();
            StartPollingThread();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            LogInformation("Disposing Start.");
            if (!IsDisposed && disposing)
            {
                LogInformation("Close and release network stream.");
                _dataStream?.Dispose();

                LogInformation("Closing instance.");
                Close();

                _options?.Dispose();
            }
            _options = null;
            LogInformation("Disposing End.");
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the reference to the socket instance - may be provided from a listner instance or created locally.
        /// </summary>
        /// <value>
        /// A <see cref="Socket"/> instance, or <b>null</b> if not available.
        /// </value>
        public Socket? ClientSocket => _clientSocket;

        /// <summary>
        /// Gets the reference to the data stream to write to and read from (for external use).
        /// </summary>
        /// <value>
        /// A <see cref="NetworkStream"/> instance, or <b>null</b> if not available.
        /// </value>
        public NetworkStream? DataStream => _dataStream;

        /// <summary>
        /// Gets the reference to the lhe local binding address, if used.
        /// </summary>
        /// <value>
        /// An <see cref="IPEndPoint"/> instance of active; otherwise, <b>null</b>.
        /// </value>
        public IPEndPoint? LocalBinding => _localBinding;

        /// <summary>
        /// Gets the reference to the socket and connection options.
        /// </summary>
        /// <value>
        /// The <see cref="TcpClientOptions"/> instance that is used.
        /// </value>
        public TcpClientOptions? Options => _options;

        /// <summary>
        /// Gets the reference to the underlying polling thread instance.
        /// </summary>
        /// <value>
        /// The <see cref="Thread"/> instance used to poll for data while connected, or <b>null</b> if 
        /// not connected or the thread is not running.
        /// </value>
        public Thread? PollingThread => _pollingThread;

        /// <summary>
        /// Gets a value indicating whether the instance is allowing the polling thread to run.
        /// </summary>
        /// <value>
        /// <b>true</b> if the polling thread is allowed to run; <b>false</b> if the thread is not 
        /// running, or the shutdown process has initiated and the thread is being terminated.
        /// </value>
        public bool PollingThreadAllowedToRun => _executePollingThread;

        /// <summary>
        /// Gets a value indicating whether the polling thread is currently executing.
        /// </summary>
        /// <value>
        /// <b>true</b> if the polling thread is executing; otherwise, <b>false</b>.
        /// </value>
        public bool PollingThreadIsRunning => _pollingThreadRunning;
        #endregion

        #region Public Methods / Functions    
        /// <summary>
        /// Closes all connections and disposes of the underlying socket.
        /// </summary>
        public void Close()
        {
            LogInformation("Closing...");

            // Stop the polling thread.
            LogInformation("Waiting for polling to terminate...");
            _executePollingThread = false;
            while (_pollingThreadRunning)
            {
                Thread.Sleep(50);
            }

            LogInformation("... Polling terminated.");
            if (_clientSocket != null)
            {
                if (_clientSocket.Connected)
                {
                    try
                    {
                        LogInformation("Shutting down socket...");
                        _clientSocket.Shutdown(SocketShutdown.Both);
                    }
                    catch (Exception ex)
                    {
                        LogError(ex);
                        OnShutdownError(ex);
                    }

                    try
                    {
                        LogInformation("Disconnecting...");
                        _clientSocket.Disconnect(false);
                    }
                    catch (Exception ex)
                    {
                        LogError(ex);
                        OnDisconnectError(ex);
                    }

                    try
                    {
                        LogInformation("Closing Socket...");
                        _clientSocket.Close();
                        _clientSocket.Dispose();
                    }
                    catch (Exception ex)
                    {
                        LogError(ex);
                        OnCloseError(ex);
                    }
                }
                LogInformation("Clearing memory...");
                _clientSocket = null;
                _dataStream = null;
                _pollingThread = null;
                _localBinding = null;
                _options = null;
                _executePollingThread = false;
                _pollingThreadRunning = false;

                OnClosed(EventArgs.Empty);
                GC.Collect();
            }
        }

        /// <summary>
        /// Gets the reference to the network stream.
        /// </summary>
        /// <returns>
        /// The <see cref="NetworkStream"/> instance, if present.
        /// </returns>
        public NetworkStream? GetStream()
        {
            NetworkStream? _stream = null;

            if (_clientSocket != null && _clientSocket.Connected)
            {
                _stream = new NetworkStream(_clientSocket);
            }
            return _stream;
        }

        #region Send Data Methods
        /// <summary>
        /// Sends the specified text to the remote host.
        /// </summary>
        /// <param name="text">
        /// A string containing the text to be send.  The encoding is assumed to be ASCII.
        /// </param>
        public void Send(string text)
        {
            Send(text, Encoding.ASCII);
        }

        /// <summary>
        /// Sends the specified text to the remote host.
        /// </summary>
        /// <param name="text">
        /// A string containing the text to be send.
        /// </param>
        /// <param name="encoding">
        /// The text <see cref="Encoding"/> used to translate the content to a byte array.
        /// </param>
        public void Send(string text, Encoding encoding)
        {
            byte[] data = encoding.GetBytes(text);
            Send(data);
        }

        /// <summary>
        /// Sends the specified data to the remote host.
        /// </summary>
        /// <param name="data">
        /// A byte array containing the data to be sent.
        /// The data.</param>
        public int Send(byte[] data)
        {
            int sentBytes = 0;
            if (_clientSocket != null && _clientSocket.Connected)
            {
                try
                {
                    sentBytes = _clientSocket.Send(data);
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    OnSendFailure(ex);
                }
            }
            return sentBytes;
        }

        /// <summary>
        /// Sends the specified text to the remote host.
        /// </summary>
        /// <param name="text">
        /// A string containing the text to be send.  The encoding is assumed to be ASCII.
        /// </param>
        public Task SendAsync(string text)
        {
            return SendAsync(text, Encoding.ASCII);
        }

        /// <summary>
        /// Sends the specified text to the remote host.
        /// </summary>
        /// <param name="text">
        /// A string containing the text to be send.
        /// </param>
        /// <param name="encoding">
        /// The text <see cref="Encoding"/> used to translate the content to a byte array.
        /// </param>
        public async Task SendAsync(string text, Encoding encoding)
        {
            byte[] data = encoding.GetBytes(text);
            await SendAsync(data);
        }

        /// <summary>
        /// Sends the specified data to the remote host.
        /// </summary>
        /// <param name="data">
        /// A byte array containing the data to be sent.
        /// The data.</param>
        public async Task SendAsync(byte[] data)
        {
            if (_clientSocket != null && _clientSocket.Connected)
            {
                try
                {
                    int sent = await _clientSocket.SendAsync(data).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    OnSendFailure(ex);
                }
            }
        }

        #endregion

        #endregion

        #region Private Event Methods
        /// <summary>
        /// Raises the <see cref="BindingFailed"/> event.
        /// </summary>
        /// <param name="ex">
        /// The reference to the <see cref="Exception"/> causing the event.
        /// </param>
        private void OnBindingFailed(Exception ex)
        {
            LogError(ex, nameof(OnBindingFailed));
            lock (_syncRoot)
            {
                BindingFailed?.Invoke(this, new ExceptionEventArgs(ex));
            }
        }

        /// <summary>
        /// Raises the <see cref="Closed" /> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="EventArgs"/> instance containing the event data.
        /// </param>
        private void OnClosed(EventArgs e)
        {
            LogInformation("OnClosed");
            lock (_syncRoot)
            {
                Closed?.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="BindingFailed"/> event.
        /// </summary>
        /// <param name="ex">
        /// The reference to the <see cref="Exception"/> causing the event.
        /// </param>
        private void OnCloseError(Exception ex)
        {
            LogError(ex, nameof(OnCloseError));
            lock (_syncRoot)
            {
                CloseError?.Invoke(this, new ExceptionEventArgs(ex));
            }
        }

        /// <summary>
        /// Raises the <see cref="Connected" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnConnected(EventArgs e)
        {
            LogInformation(nameof(OnConnected));
            lock (_syncRoot)
            {
                Connected?.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="ConnectionFailure"/> event.
        /// </summary>
        /// <param name="ex">
        /// The reference to the <see cref="Exception"/> causing the event.
        /// </param>
        private void OnConnectionFailure(Exception ex)
        {
            LogError(ex, nameof(OnConnectionFailure));
            lock (_syncRoot)
            {
                ConnectionFailure?.Invoke(this, new ExceptionEventArgs(ex));
            }
        }

        /// <summary>
        /// Raises the <see cref="CreateSocketFailure"/> event.
        /// </summary>
        /// <param name="ex">
        /// The reference to the <see cref="Exception"/> causing the event.
        /// </param>
        private void OnCreateSocketFailure(Exception ex)
        {
            LogError(ex, nameof(OnCreateSocketFailure));
            lock (_syncRoot)
            {
                CreateSocketFailure?.Invoke(this, new ExceptionEventArgs(ex));
            }
        }

        /// <summary>
        /// Raises the <see cref="DataReadFailure"/> event.
        /// </summary>
        /// <param name="ex">
        /// The reference to the <see cref="Exception"/> causing the event.
        /// </param>
        private void OnDataReadFailure(Exception ex)
        {
            LogError(ex, nameof(OnDataReadFailure));
            lock (_syncRoot)
            {
                DataReadFailure?.Invoke(this, new ExceptionEventArgs(ex));
            }
        }

        /// <summary>
        /// Raises the <see cref="DataReceived" /> event.
        /// </summary>
        /// <param name="e">The <see cref="DataReceivedEventArgs"/> instance containing the event data.
        /// </param>
        private void OnDataReceived(DataReceivedEventArgs e)
        {
            LogInformation("OnDataReceived");
            lock (_syncRoot)
            {
                DataReceived?.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="DisconnectError"/> event.
        /// </summary>
        /// <param name="ex">
        /// The reference to the <see cref="Exception"/> causing the event.
        /// </param>
        private void OnDisconnectError(Exception ex)
        {
            LogError(ex, nameof(OnDisconnectError));
            lock (_syncRoot)
            {
                DisconnectError?.Invoke(this, new ExceptionEventArgs(ex));
            }
        }

        /// <summary>
        /// Raises the <see cref="DnsFailure"/> event.
        /// </summary>
        /// <param name="ex">
        /// The reference to the <see cref="Exception"/> causing the event.
        /// </param>
        private void OnDnsFailure(Exception ex)
        {
            LogError(ex, nameof(OnDnsFailure));
            lock (_syncRoot)
            {
                DnsFailure?.Invoke(this, new ExceptionEventArgs(ex));
            }
        }

        /// <summary>
        /// Raises the <see cref="InvalidSocket" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnInvalidSocket(EventArgs e)
        {
            LogInformation(nameof(OnInvalidSocket));
            lock (_syncRoot)
            {
                InvalidSocket?.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="PollFailure"/> event.
        /// </summary>
        /// <param name="ex">
        /// The reference to the <see cref="Exception"/> causing the event.
        /// </param>
        private void OnPollFailure(Exception ex)
        {
            LogError(ex, nameof(OnPollFailure));
            lock (_syncRoot)
            {
                PollFailure?.Invoke(this, new ExceptionEventArgs(ex));
            }
        }

        /// <summary>
        /// Raises the <see cref="PollThreadStartFailure"/> event.
        /// </summary>
        /// <param name="ex">
        /// The reference to the <see cref="Exception"/> causing the event.
        /// </param>
        private void OnPollThreadStartFailure(Exception ex)
        {
            LogError(ex, nameof(OnPollThreadStartFailure));
            lock (_syncRoot)
            {
                PollThreadStartFailure?.Invoke(this, new ExceptionEventArgs(ex));
            }
        }

        /// <summary>
        /// Raises the <see cref="SendFailure"/> event.
        /// </summary>
        /// <param name="ex">
        /// The reference to the <see cref="Exception"/> causing the event.
        /// </param>
        private void OnSendFailure(Exception ex)
        {
            LogError(ex, nameof(OnSendFailure));
            lock (_syncRoot)
            {
                SendFailure?.Invoke(this, new ExceptionEventArgs(ex));
            }
        }

        /// <summary>
        /// Raises the <see cref="SetSocketOptionsFailure"/> event.
        /// </summary>
        /// <param name="ex">
        /// The reference to the <see cref="Exception"/> causing the event.
        /// </param>
        private void OnSetSocketOptionsFailure(Exception ex)
        {
            LogError(ex, nameof(OnSetSocketOptionsFailure));
            lock (_syncRoot)
            {
                SetSocketOptionsFailure?.Invoke(this, new ExceptionEventArgs(ex));
            }
        }

        /// <summary>
        /// Raises the <see cref="ShutdownError"/> event.
        /// </summary>
        /// <param name="ex">
        /// The reference to the <see cref="Exception"/> causing the event.
        /// </param>
        private void OnShutdownError(Exception ex)
        {
            LogError(ex, nameof(OnShutdownError));
            lock (_syncRoot)
            {
                ShutdownError?.Invoke(this, new ExceptionEventArgs(ex));
            }
        }

        /// <summary>
        /// Raises the <see cref="SocketError" /> event.
        /// </summary>
        /// <param name="e">The <see cref="SocketErrorEventArgs"/> instance containing the event data.</param>
        private void OnSocketError(SocketErrorEventArgs e)
        {
            LogInformation(nameof(OnSocketError));
            lock (_syncRoot)
            {
                SocketError?.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="UnexpectedClose" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnUnexpectedClose(EventArgs e)
        {
            LogInformation(nameof(OnUnexpectedClose));
            lock (_syncRoot)
            {
                UnexpectedClose?.Invoke(this, e);
            }
        }

        #endregion

        #region Private Methods / Functions
        /// <summary>
        /// Attempts to create the local binding.
        /// </summary>
        /// <param name="localEndPoint">
        /// The local <see cref="IPEndPoint"/> to bind to.
        /// </param>
        private void CreateBinding(IPEndPoint localEndPoint)
        {
            if (_clientSocket != null)
            {
                try
                {
                    LogInformation($"Creating Binding: {localEndPoint}");
                    _clientSocket.Bind(localEndPoint);
                    _localBinding = localEndPoint;
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    _localBinding = null;
                    OnBindingFailed(ex);

                }
            }
        }
        /// <summary>
        /// Attempts to create the socket to use.
        /// </summary>
        private void CreateSocket()
        {
            if (_clientSocket != null)
            {
                LogInformation("Socket already created.");
                OnInvalidSocket(EventArgs.Empty);
            }
            else
            {
                try
                {
                    LogInformation("Creating Socket...");
                    _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    LogInformation("...Success.");
                    SetSocketOptions();
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    _clientSocket = null;
                    OnCreateSocketFailure(ex);
                }
            }
        }

        /// <summary>
        /// Attempts to set the socket options.
        /// </summary>
        private void SetSocketOptions()
        {
            if (_clientSocket == null)
            {
                LogInformation("No socket was created.");
                OnInvalidSocket(EventArgs.Empty);
            }
            else if (_options == null)
            {
                LogCritical("CRITICAL: Options instance is missing.");
            }
            else
            {
                try
                {
                    LogInformation($"\tReceiveBufferSize = {_options.ReceiveBufferSize}");
                    _clientSocket.ReceiveBufferSize = _options.ReceiveBufferSize;

                    LogInformation($"\tSendBufferSize = {_options.SendBufferSize}");
                    _clientSocket.SendBufferSize = _options.SendBufferSize;

                    LogInformation($"\tReceiveTimeout = {_options.ReceiveTimeout}");
                    _clientSocket.ReceiveTimeout = _options.ReceiveTimeout;

                    LogInformation($"\tSendTimeout = {_options.SendTimeout}");
                    _clientSocket.SendTimeout = _options.SendTimeout;

                    LogInformation($"\tNoDelay = {_options.NoDelay}");
                    _clientSocket.NoDelay = _options.NoDelay;

                    LogInformation($"\tDontFragment = true");
                    _clientSocket.DontFragment = true;

                    if (_options.LingerState != null)
                    {
                        LogInformation($"\tLingerState = {_options.LingerState}");
                        _clientSocket.LingerState = _options.LingerState;
                    }

                    LogInformation("ExclusiveAddressUse = false");
                    _clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, false);

                    LogInformation("KeepAlive = true");
                    _clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);

                    LogInformation("ReuseAddress = true");
                    _clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                    LogInformation("DontLinger = true");
                    _clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    OnSetSocketOptionsFailure(ex);
                }
            }
        }

        /// <summary>
        /// Starts the data reception polling thread.
        /// </summary>
        private void StartPollingThread()
        {
            if (!_pollingThreadRunning)
            {
                lock (_syncRoot)
                {
                    LogInformation($"Starting Data and Status Polling Thread... {Environment.CurrentManagedThreadId}");
                    if (_pollingThread == null)
                    {
                        try
                        {
                            LogInformation("\tCreating Background Thread... AboveNormal priority...");
                            _executePollingThread = true;
                            _pollingThread = new Thread(ExecutePollingThread)
                            {
                                IsBackground = true,
                                Priority = ThreadPriority.AboveNormal
                            };
                            _pollingThread.Start();
                        }
                        catch (Exception ex)
                        {
                            _pollingThreadRunning = false;
                            _executePollingThread = false;
                            _pollingThread = null;

                            LogError(ex);
                            OnPollThreadStartFailure(ex);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Executes the thread to poll the socket for its status and data.
        /// </summary>
        private void ExecutePollingThread()
        {
            const int ReadBufferSize = 512000;

            if (_executePollingThread)
            {
                lock (_syncRoot)
                {
                    _pollingThreadRunning = true;
                    LogInformation($"Data And Status Polling Thread Start: {Environment.CurrentManagedThreadId}");
                }

                // Use one allocated buffer for all the data reads.
                byte[] dataReceptionBuffer = new byte[ReadBufferSize];

                // While everything is valid...
                while (_executePollingThread && _pollingThreadRunning && _clientSocket != null)
                {
                    // Poll for errors.
                    bool hasError = CheckForError();
                    if (hasError)
                    {
                        // Quit Polling on error.
                        _executePollingThread = false;
                    }
                    else
                    {

                        bool hasData = CheckForData();
                        if (hasData && _executePollingThread)
                        {
                            // Clear the allocated buffer before every read.
                            Array.Clear(dataReceptionBuffer, 0, ReadBufferSize);

                            // Read the data and raise an event.
                            ReadData(ref dataReceptionBuffer);
                        }
                    }
                }

                // Clear and exit the thread.
                lock (_syncRoot)
                {
                    Array.Clear(dataReceptionBuffer, 0, ReadBufferSize);
                    _pollingThreadRunning = false;
                }
            }
            _pollingThreadRunning = false;
        }

        /// <summary>
        /// Polls the socket for errors.
        /// </summary>
        /// <returns>
        /// <b>true</b> if the socket has one or more errors; otherwise, return <b>false</b>.
        /// </returns>
        private bool CheckForError()
        {
            bool hasError = false;

            if (_clientSocket != null)
            {
                try
                {
                    hasError = _clientSocket.Poll(100, SelectMode.SelectError);
                    if (hasError)
                    {
                        int? error = (int?)_clientSocket.GetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Error);
                        if (error != null)
                        {
                            LogInformation("--SOCKET ERROR: " + error);
                            OnSocketError(new SocketErrorEventArgs { Error = (int)error });

                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    OnPollFailure(ex);
                }
            }
            return hasError;
        }

        /// <summary>
        /// Polls the socket for data to be read.
        /// </summary>
        /// <returns>
        /// <b>true</b> if the socket has data to be read; otherwise, return <b>false</b>.
        /// </returns>
        private bool CheckForData()
        {
            bool hasData = false;

            if (_clientSocket != null)
            {
                try
                {
                    hasData = _clientSocket.Poll(100, SelectMode.SelectRead);
                    if (!_clientSocket.Connected)
                    {
                        LogInformation("Socket Unexpectedly Closed.");
                        OnUnexpectedClose(EventArgs.Empty);
                        _executePollingThread = false;
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    OnPollFailure(ex);
                }
            }
            return hasData;
        }

        /// <summary>
        /// Reads the data from the socket.
        /// </summary>
        /// <param name="buffer">
        /// The buffer that was allocated to hold the data.
        /// </param>
        /// <returns></returns>
        private void ReadData(ref byte[] buffer)
        {
            if (_clientSocket != null)
            {
                try
                {
                    // Read the data from the socket.
                    int numberOfBytesRead = _clientSocket.Receive(buffer, buffer.Length, SocketFlags.OutOfBand);
                    if (numberOfBytesRead > 0)
                    {
                        // Copy the content into the event arguments instance and raise the event.
                        DataReceivedEventArgs evArgs = new(buffer, numberOfBytesRead);
                        OnDataReceived(evArgs);
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    OnDataReadFailure(ex);
                }
            }
        }
        #endregion
    }
}
