using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Abstractions.Logging;
using Adaptive.Intelligence.Events.Arguments;
using Adaptive.Intelligence.Events.Delegates;
using Adaptive.Intelligence.Networking.Events;
using Adaptive.Intelligence.Networking.Events.Delegates;
using System.Net;
using System.Net.Sockets;

namespace Adaptive.Intelligence.Networking.Tcp
{
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

    /// <summary>
    /// Provides a class for listening for connections on a TCP port for IPv4 connections, and managing
    /// the lifetime of the client connections.
    /// </summary>
    /// <seealso cref="DisposableObjectBase" />
    /// <seealso cref="ExceptionTrackingBase" />
    /// <seealso cref="LoggableBase" />
    public class TcpServer : LoggableBase
    {
        #region Public Events

        /// <summary>
        /// Occurs when an attempt to accept a connection fails.
        /// </summary>
        public event ExceptionEventHandler? AcceptFailed;

        /// <summary>
        /// Occurs when the attempt to bind a socket to an address fails.
        /// </summary>
        public event ExceptionEventHandler? BindingFailed;

        /// <summary>
        /// Occurs when a client connects to the socket and requests a connection.
        /// </summary>
        public event TcpClientEventHandler? ClientConnected;

        /// <summary>
        /// Occurs when the attempt to create a socket fails.
        /// </summary>
        public event ExceptionEventHandler? CreateSocketFailed;

        /// <summary>
        /// Occurs when a listen operation is started.
        /// </summary>
        public event EventHandler? ListenStarted;

        /// <summary>
        /// Occurs when a listen operation is stopped.
        /// </summary>
        public event EventHandler? ListenStopped;

        /// <summary>
        /// Occurs when an attemtp to start listening fails.
        /// </summary>
        public event ExceptionEventHandler? ListenStartFailed;

        /// <summary>
        /// Occurs when the polling thread fails or encounters an exception.
        /// </summary>
        public event ExceptionEventHandler? PollingFailed;

        #endregion

        #region Private Member Declarations
        /// <summary>
        /// The listener end point.
        /// </summary>
        private IPEndPoint? _listenerEndPoint;

        /// <summary>
        /// The listener socket instance.
        /// </summary>
        private Socket? _listenerSocket;

        /// <summary>
        /// The listening flag.
        /// </summary>
        private bool _listening;

        /// <summary>
        /// The poll thread is executing flag.
        /// </summary>
        private bool _pollExecuting;

        /// <summary>
        /// The flag to allow to stop the polling thread.
        /// </summary>
        private bool _runExecuteThread;

        /// <summary>
        /// The maximum number of connections.
        /// </summary>
        private int _maxConnections;

        /// <summary>
        /// The polling interval.
        /// </summary>
        private readonly int _pollingInterval = 100;

        /// <summary>
        /// The polling thread.
        /// </summary>
        private Thread? _pollingThread;

        /// <summary>
        /// The list of connected clients.
        /// </summary>
        private List<TcpClient>? _clientList = [];

        #endregion

        #region Constructor / Dispose Methods

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpServer"/> class.
        /// </summary>
        /// <param name="endPoint">The end point.</param>
        /// <param name="maxConnections">
        /// An integer specifying the maximum number of connections.
        /// </param>
        public TcpServer(IPEndPoint endPoint, int maxConnections = Int32.MaxValue - 1)
        {
            _listenerEndPoint = endPoint;
            CreateSocket();
            BindSocket();
            _maxConnections = maxConnections;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            StopListening();

            _listenerEndPoint = null;
            _pollingThread = null;
            _clientList = null;
            _listenerSocket = null;

            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets a value indicating whether the current instance is bound to and address
        /// and listening for new connections.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is listening; otherwise, <c>false</c>.
        /// </value>
        public bool IsListening => _listening;

        /// <summary>
        /// Gets the local endpoint.
        /// </summary>
        /// <value>
        /// The local <see cref="IPEndPoint"/> the listener is bound to, or <b>null</b>.
        /// </value>
        public EndPoint? LocalEndpoint
        {
            get
            {
                if (_listenerSocket == null || _listenerEndPoint == null)
                {
                    return null;
                }

                return _listenerSocket.LocalEndPoint;
            }
        }

        /// <summary>
        /// Gets or sets the maximum number of connections.
        /// </summary>
        /// <value>
        /// An integer specifying the maximum number of connections.
        /// </value>
        public int MaxConnections
        {
            get => _maxConnections;
            set => _maxConnections = value;
        }
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Starts listening for new connections.
        /// </summary>
        public void StartListening()
        {
            if (!_listening && _listenerSocket != null)
            {
                // Attempt to start listening for new connections.
                try
                {
                    _listenerSocket.Listen(_maxConnections);
                    _listening = true;
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    OnListenStartFailed(ex);
                    _listenerSocket.Close();
                    _listenerSocket.Dispose();
                    _listenerSocket = null;
                    _listening = false;
                }

                // If listening, start the polling thread.
                if (_listening)
                {
                    if (!StartPollingThread())
                    {
                        StopListening();
                    }
                    else
                    {
                        OnListenStarted(EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Stops listening for new connections.
        /// </summary>
        public void StopListening()
        {
            if (_listenerSocket != null)
            {
                try
                {
                    _listenerSocket?.Close();
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
                try
                {
                    _listenerSocket?.Dispose();
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            }
            _listenerSocket = null;
            _listening = false;
            OnListenStopped(EventArgs.Empty);
        }
        #endregion

        #region Private Methods / Functions
        /// <summary>
        /// Attempts to bind the socket to the specified end point.
        /// </summary>
        private void BindSocket()
        {
            if (_listenerSocket != null && _listenerEndPoint != null)
            {
                try
                {
                    _listenerSocket.Bind(_listenerEndPoint);
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    OnBindingFailed(ex);
                    _listenerSocket?.Dispose();
                    _listenerSocket = null;
                }
            }
        }

        /// <summary>
        /// Attempts to create the socket to listen on.
        /// </summary>
        private void CreateSocket()
        {
            if (_listenerSocket == null && _listenerEndPoint != null)
            {
                try
                {
                    _listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    OnCreateSocketFailed(ex);
                    _listenerSocket = null;
                }
            }
        }


        /// <summary>
        /// Executes the polling thread.
        /// </summary>
        private void ExecutePollingThread()
        {
            if (!_pollExecuting)
            {
                _pollExecuting = true;
                while (_runExecuteThread && _listening && _listenerSocket != null)
                {
                    bool hasData;
                    try
                    {
                        hasData = _listenerSocket.Poll(_pollingInterval, SelectMode.SelectRead);
                    }
                    catch (Exception ex)
                    {
                        LogError(ex);
                        _pollExecuting = false;
                        hasData = false;
                        OnPollingFailed(ex);
                    }
                    if (hasData)
                    {
                        StartAcceptProcessAsync();
                    }
                }
            }
            _runExecuteThread = false;
            _pollExecuting = false;
        }

        /// <summary>
        /// Starts the process of accepting an incoming connection.
        /// </summary>
        private async Task StartAcceptProcessAsync()
        {
            if (_listenerSocket != null)
            {
                Socket? newSocket;
                try
                {
                    newSocket = await _listenerSocket.AcceptAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    newSocket = null;
                    LogError(ex);
                    OnAcceptFailed(ex);
                }

                if (newSocket != null)
                {
                    TcpClient newClient = new(newSocket);
                    StartNewClientSession(newClient);
                    OnClientConnected(new TcpClientEventArgs(newClient));
                }
            }
        }

        /// <summary>
        /// Starts the sepearate polling thread.
        /// </summary>
        /// <returns>
        /// <b>true</b> if the polling thread starts successfully; 
        /// otherwise, returns <b>null</b>.
        /// </returns>
        private bool StartPollingThread()
        {
            _pollingThread = new Thread(ExecutePollingThread)
            {
                IsBackground = true,
                Priority = ThreadPriority.AboveNormal
            };
            _runExecuteThread = true;

            try
            {
                _pollingThread.Start();
            }
            catch (Exception ex)
            {
                LogError(ex);
                OnPollingFailed(ex);
                OnListenStartFailed(ex);
                _pollingThread = null;
                _pollExecuting = false;
                _runExecuteThread = false;
            }

            return _pollingThread != null;
        }

        /// <summary>
        /// Starts the new client session.
        /// </summary>
        /// <param name="client">
        /// The <see cref="TcpClient"/> instance resulting from an accept().
        /// </param>
        private void StartNewClientSession(TcpClient client)
        {
            _clientList ??= [];
            _clientList.Add(client);
        }
        #endregion

        #region Private Event Methods
        /// <summary>
        /// Raises the <see cref="AcceptFailed"/> event.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="Exception"/> from the calling method.
        /// </param>
        private void OnAcceptFailed(Exception ex)
        {
            AcceptFailed?.Invoke(this, new ExceptionEventArgs(ex));
        }

        /// <summary>
        /// Raises the <see cref="BindingFailed"/> event.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="Exception"/> from the calling method.
        /// </param>
        private void OnBindingFailed(Exception ex)
        {
            BindingFailed?.Invoke(this, new ExceptionEventArgs(ex));
        }

        /// <summary>
        /// Raises the <see cref="ClientConnected" /> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="TcpClientEventArgs"/> instance containing the event data.
        /// </param>
        private void OnClientConnected(TcpClientEventArgs e)
        {
            ClientConnected?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="CreateSocketFailed"/> event.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="Exception"/> from the calling method.
        /// </param>
        private void OnCreateSocketFailed(Exception ex)
        {
            CreateSocketFailed?.Invoke(this, new ExceptionEventArgs(ex));
        }

        /// <summary>
        /// Raises the <see cref="ListenStarted" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnListenStarted(EventArgs e)
        {
            ListenStarted?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ListenStopped" /> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnListenStopped(EventArgs e)
        {
            ListenStopped?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="ListenStartFailed"/> event.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="Exception"/> from the calling method.
        /// </param>
        private void OnListenStartFailed(Exception ex)
        {
            ListenStartFailed?.Invoke(this, new ExceptionEventArgs(ex));
        }

        /// <summary>
        /// Raises the <see cref="PollingFailed"/> event.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="Exception"/> from the calling method.
        /// </param>
        private void OnPollingFailed(Exception ex)
        {
            PollingFailed?.Invoke(this, new ExceptionEventArgs(ex));
        }
        #endregion
    }
}
