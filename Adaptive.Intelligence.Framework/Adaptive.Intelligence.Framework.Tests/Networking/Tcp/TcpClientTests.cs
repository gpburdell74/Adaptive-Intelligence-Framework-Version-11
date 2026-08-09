using Adaptive.Intelligence.Networking.Tcp;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TcpSocketClient = Adaptive.Intelligence.Networking.Tcp.TcpClient;

namespace Adaptive.Intelligence.Framework.Tests
{
    /// <summary>
    /// Provides tests for the <see cref="TcpClient"/> class constructors.
    /// </summary>
    public class TcpClientTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_Default_SetsDefaultOptionsAndCreatesSocket.
        /// </summary>
        public void Constructor_Default_SetsDefaultOptionsAndCreatesSocket()
        {
            // Arrange

            // Act
            using TcpSocketClient client = new TcpSocketClient();

            // Assert
            Assert.NotNull(client.Options);
            Assert.NotNull(client.ClientSocket);
            Assert.Equal(80, client.Options.Port);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_Options_SetsProvidedOptionsAndCreatesSocket.
        /// </summary>
        public void Constructor_Options_SetsProvidedOptionsAndCreatesSocket()
        {
            // Arrange
            using TcpClientOptions options = new TcpClientOptions
            {
                NoDelay = false,
                Port = 5010
            };

            // Act
            using TcpSocketClient client = new TcpSocketClient(options);

            // Assert
            Assert.Same(options, client.Options);
            Assert.NotNull(client.ClientSocket);
            Assert.False(client.ClientSocket.NoDelay);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_LocalEndPoint_CreatesSocketAndBinding.
        /// </summary>
        public void Constructor_LocalEndPoint_CreatesSocketAndBinding()
        {
            // Arrange
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Loopback, 0);

            // Act
            using TcpSocketClient client = new TcpSocketClient(localEndPoint);

            // Assert
            Assert.NotNull(client.Options);
            Assert.NotNull(client.ClientSocket);
            Assert.Same(localEndPoint, client.LocalBinding);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_HostnameAndPort_SetsRemoteHostAndPort.
        /// </summary>
        public void Constructor_HostnameAndPort_SetsRemoteHostAndPort()
        {
            // Arrange
            const string hostName = "127.0.0.1";
            const int port = 23456;

            // Act
            using TcpSocketClient client = new TcpSocketClient(hostName, port);

            // Assert
            Assert.NotNull(client.Options);
            Assert.NotNull(client.ClientSocket);
            Assert.Equal(hostName, client.Options.RemoteHostNameOrAddress);
            Assert.Equal(port, client.Options.Port);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_AcceptedSocket_UsesSocketCreatesStreamAndStartsPolling.
        /// </summary>
        public void Constructor_AcceptedSocket_UsesSocketCreatesStreamAndStartsPolling()
        {
            // Arrange
            using Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            listener.Listen(1);

            int listeningPort = ((IPEndPoint)listener.LocalEndPoint!).Port;

            using Socket connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectingSocket.Connect(IPAddress.Loopback, listeningPort);

            using Socket acceptedSocket = listener.Accept();

            // Act
            using TcpSocketClient client = new TcpSocketClient(acceptedSocket);

            // Assert
            Assert.Same(acceptedSocket, client.ClientSocket);
            Assert.NotNull(client.Options);
            Assert.NotNull(client.DataStream);
            Assert.NotNull(client.PollingThread);
            Assert.True(client.PollingThreadAllowedToRun);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Property_ClientSocket_AfterClose_ReturnsNull.
        /// </summary>
        public void Property_ClientSocket_AfterClose_ReturnsNull()
        {
            // Arrange
            using Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            listener.Listen(1);

            int listeningPort = ((IPEndPoint)listener.LocalEndPoint!).Port;

            using Socket connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectingSocket.Connect(IPAddress.Loopback, listeningPort);

            using Socket acceptedSocket = listener.Accept();
            using TcpSocketClient client = new TcpSocketClient(acceptedSocket);

            // Act
            client.Close();

            // Assert
            Assert.Null(client.ClientSocket);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Property_DataStream_DefaultConstructor_ReturnsNull.
        /// </summary>
        public void Property_DataStream_DefaultConstructor_ReturnsNull()
        {
            // Arrange

            // Act
            using TcpSocketClient client = new TcpSocketClient();

            // Assert
            Assert.Null(client.DataStream);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Property_LocalBinding_AfterClose_ReturnsNull.
        /// </summary>
        public void Property_LocalBinding_AfterClose_ReturnsNull()
        {
            // Arrange
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Loopback, 0);
            using TcpSocketClient client = new TcpSocketClient(localEndPoint);

            // Act
            client.Close();

            // Assert
            Assert.Null(client.LocalBinding);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Property_Options_AfterClose_ReturnsNull.
        /// </summary>
        public void Property_Options_AfterClose_ReturnsNull()
        {
            // Arrange
            using TcpSocketClient client = new TcpSocketClient();

            // Act
            client.Close();

            // Assert
            Assert.Null(client.Options);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Property_PollingThread_DefaultConstructor_ReturnsNull.
        /// </summary>
        public void Property_PollingThread_DefaultConstructor_ReturnsNull()
        {
            // Arrange

            // Act
            using TcpSocketClient client = new TcpSocketClient();

            // Assert
            Assert.Null(client.PollingThread);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Property_PollingThread_AfterClose_ReturnsNull.
        /// </summary>
        public void Property_PollingThread_AfterClose_ReturnsNull()
        {
            // Arrange
            using Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            listener.Listen(1);

            int listeningPort = ((IPEndPoint)listener.LocalEndPoint!).Port;

            using Socket connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectingSocket.Connect(IPAddress.Loopback, listeningPort);

            using Socket acceptedSocket = listener.Accept();
            using TcpSocketClient client = new TcpSocketClient(acceptedSocket);

            // Act
            client.Close();

            // Assert
            Assert.Null(client.PollingThread);
        }


        [Fact]
        /// <summary>
        /// Gets the definition for Property_PollingThreadIsRunning_DefaultConstructor_ReturnsFalse.
        /// </summary>
        public void Property_PollingThreadIsRunning_DefaultConstructor_ReturnsFalse()
        {
            // Arrange

            // Act
            using TcpSocketClient client = new TcpSocketClient();

            // Assert
            Assert.False(client.PollingThreadIsRunning);
        }


        [Fact]
        /// <summary>
        /// Gets the definition for Property_PollingThreadAllowedToRun_DefaultConstructor_ReturnsFalse.
        /// </summary>
        public void Property_PollingThreadAllowedToRun_DefaultConstructor_ReturnsFalse()
        {
            // Arrange

            // Act
            using TcpSocketClient client = new TcpSocketClient();

            // Assert
            Assert.False(client.PollingThreadAllowedToRun);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Property_PollingThreadIsRunning_AcceptedSocketConstructor_ReturnsTrueEventually.
        /// </summary>
        public void Property_PollingThreadIsRunning_AcceptedSocketConstructor_ReturnsTrueEventually()
        {
            // Arrange
            using Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            listener.Listen(1);

            int listeningPort = ((IPEndPoint)listener.LocalEndPoint!).Port;

            using Socket connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectingSocket.Connect(IPAddress.Loopback, listeningPort);

            using Socket acceptedSocket = listener.Accept();

            // Act
            using TcpSocketClient client = new TcpSocketClient(acceptedSocket);
            bool pollingThreadRunning = SpinWait.SpinUntil(() => client.PollingThreadIsRunning, 2000);

            // Assert
            Assert.True(pollingThreadRunning);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Close_WhenConnectedSocketPresent_ClearsStateAndRaisesClosed.
        /// </summary>
        public void Close_WhenConnectedSocketPresent_ClearsStateAndRaisesClosed()
        {
            // Arrange
            using Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            listener.Listen(1);

            int listeningPort = ((IPEndPoint)listener.LocalEndPoint!).Port;

            using Socket connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectingSocket.Connect(IPAddress.Loopback, listeningPort);

            using Socket acceptedSocket = listener.Accept();
            using TcpSocketClient client = new TcpSocketClient(acceptedSocket);

            int closedEventCount = 0;
            client.Closed += (_, _) => closedEventCount++;

            // Act
            client.Close();

            // Assert
            Assert.Equal(1, closedEventCount);
            Assert.Null(client.ClientSocket);
            Assert.Null(client.DataStream);
            Assert.Null(client.PollingThread);
            Assert.Null(client.LocalBinding);
            Assert.Null(client.Options);
            Assert.False(client.PollingThreadAllowedToRun);
            Assert.False(client.PollingThreadIsRunning);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Close_WhenCalledAfterAlreadyClosed_DoesNotThrow.
        /// </summary>
        public void Close_WhenCalledAfterAlreadyClosed_DoesNotThrow()
        {
            // Arrange
            using TcpSocketClient client = new TcpSocketClient();
            client.Close();

            // Act
            Exception? exception = Record.Exception(client.Close);

            // Assert
            Assert.Null(exception);
            Assert.Null(client.ClientSocket);
            Assert.False(client.PollingThreadAllowedToRun);
            Assert.False(client.PollingThreadIsRunning);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for GetStream_WhenSocketNotConnected_ReturnsNull.
        /// </summary>
        public void GetStream_WhenSocketNotConnected_ReturnsNull()
        {
            // Arrange
            using TcpSocketClient client = new TcpSocketClient();

            // Act
            NetworkStream? stream = client.GetStream();

            // Assert
            Assert.Null(stream);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for GetStream_WhenSocketConnected_ReturnsNetworkStream.
        /// </summary>
        public void GetStream_WhenSocketConnected_ReturnsNetworkStream()
        {
            // Arrange
            using Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            listener.Listen(1);

            int listeningPort = ((IPEndPoint)listener.LocalEndPoint!).Port;

            using Socket connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectingSocket.Connect(IPAddress.Loopback, listeningPort);

            using Socket acceptedSocket = listener.Accept();
            using TcpSocketClient client = new TcpSocketClient(acceptedSocket);

            // Act
            using NetworkStream? stream = client.GetStream();

            // Assert
            Assert.NotNull(stream);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Send_WithString_SendsAsciiEncodedBytesToRemoteHost.
        /// </summary>
        public void Send_WithString_SendsAsciiEncodedBytesToRemoteHost()
        {
            // Arrange
            using Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            listener.Listen(1);

            int listeningPort = ((IPEndPoint)listener.LocalEndPoint!).Port;

            using Socket connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectingSocket.Connect(IPAddress.Loopback, listeningPort);
            connectingSocket.ReceiveTimeout = 2000;

            using Socket acceptedSocket = listener.Accept();
            using TcpSocketClient client = new TcpSocketClient(acceptedSocket);

            const string payload = "A1z!";

            // Act
            client.Send(payload);

            byte[] receivedBuffer = new byte[32];
            int receivedLength = connectingSocket.Receive(receivedBuffer);

            // Assert
            byte[] expectedBytes = System.Text.Encoding.ASCII.GetBytes(payload);
            Assert.Equal(expectedBytes.Length, receivedLength);
            Assert.Equal(expectedBytes, receivedBuffer[..receivedLength]);
        }


        [Fact]
        /// <summary>
        /// Gets the definition for Send_WithStringAndEncoding_SendsEncodedBytesToRemoteHost.
        /// </summary>
        public void Send_WithStringAndEncoding_SendsEncodedBytesToRemoteHost()
        {
            // Arrange
            using Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            listener.Listen(1);

            int listeningPort = ((IPEndPoint)listener.LocalEndPoint!).Port;

            using Socket connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectingSocket.Connect(IPAddress.Loopback, listeningPort);
            connectingSocket.ReceiveTimeout = 2000;

            using Socket acceptedSocket = listener.Accept();
            using TcpSocketClient client = new TcpSocketClient(acceptedSocket);

            const string payload = "Hello π";
            Encoding encoding = Encoding.UTF8;
            byte[] expectedBytes = encoding.GetBytes(payload);

            // Act
            client.Send(payload, encoding);

            byte[] receivedBuffer = new byte[64];
            int receivedLength = connectingSocket.Receive(receivedBuffer);

            // Assert
            Assert.Equal(expectedBytes.Length, receivedLength);
            Assert.Equal(expectedBytes, receivedBuffer[..receivedLength]);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Send_WithByteArray_WhenConnected_ReturnsSentByteCountAndTransmitsData.
        /// </summary>
        public void Send_WithByteArray_WhenConnected_ReturnsSentByteCountAndTransmitsData()
        {
            // Arrange
            using Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            listener.Listen(1);

            int listeningPort = ((IPEndPoint)listener.LocalEndPoint!).Port;

            using Socket connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectingSocket.Connect(IPAddress.Loopback, listeningPort);
            connectingSocket.ReceiveTimeout = 2000;

            using Socket acceptedSocket = listener.Accept();
            using TcpSocketClient client = new TcpSocketClient(acceptedSocket);

            byte[] payload = [1, 2, 3, 4, 5];

            // Act
            int sentByteCount = client.Send(payload);

            byte[] receivedBuffer = new byte[32];
            int receivedLength = connectingSocket.Receive(receivedBuffer);

            // Assert
            Assert.Equal(payload.Length, sentByteCount);
            Assert.Equal(payload.Length, receivedLength);
            Assert.Equal(payload, receivedBuffer[..receivedLength]);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Send_WithByteArray_WhenSocketNotConnected_ReturnsZero.
        /// </summary>
        public void Send_WithByteArray_WhenSocketNotConnected_ReturnsZero()
        {
            // Arrange
            using TcpSocketClient client = new TcpSocketClient();
            byte[] payload = [10, 20, 30];

            // Act
            int sentByteCount = client.Send(payload);

            // Assert
            Assert.Equal(0, sentByteCount);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Send_WithNullByteArray_WhenSocketThrows_RaisesSendFailureAndReturnsZero.
        /// </summary>
        public void Send_WithNullByteArray_WhenSocketThrows_RaisesSendFailureAndReturnsZero()
        {
            // Arrange
            using Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            listener.Listen(1);

            int listeningPort = ((IPEndPoint)listener.LocalEndPoint!).Port;

            using Socket connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectingSocket.Connect(IPAddress.Loopback, listeningPort);

            using Socket acceptedSocket = listener.Accept();
            using TcpSocketClient client = new TcpSocketClient(acceptedSocket);

            int sendFailureCount = 0;
            Exception? capturedException = null;
            client.SendFailure += (_, e) =>
            {
                sendFailureCount++;
                capturedException = e.Exception;
            };

            // Act
            int sentByteCount = client.Send((byte[])null!);

            // Assert
            Assert.Equal(0, sentByteCount);
            Assert.Equal(1, sendFailureCount);
            Assert.NotNull(capturedException);
            Assert.IsType<ArgumentNullException>(capturedException);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for SendAsync_WithString_SendsAsciiEncodedBytesToRemoteHost.
        /// </summary>
        public async Task SendAsync_WithString_SendsAsciiEncodedBytesToRemoteHost()
        {
            // Arrange
            using Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            listener.Listen(1);

            int listeningPort = ((IPEndPoint)listener.LocalEndPoint!).Port;

            using Socket connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectingSocket.Connect(IPAddress.Loopback, listeningPort);
            connectingSocket.ReceiveTimeout = 2000;

            using Socket acceptedSocket = listener.Accept();
            using TcpSocketClient client = new TcpSocketClient(acceptedSocket);

            const string payload = "Async123";
            byte[] expectedBytes = Encoding.ASCII.GetBytes(payload);

            // Act
            await client.SendAsync(payload);

            byte[] receivedBuffer = new byte[64];
            int receivedLength = connectingSocket.Receive(receivedBuffer);

            // Assert
            Assert.Equal(expectedBytes.Length, receivedLength);
            Assert.Equal(expectedBytes, receivedBuffer[..receivedLength]);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for SendAsync_WithStringAndEncoding_SendsEncodedBytesToRemoteHost.
        /// </summary>
        public async Task SendAsync_WithStringAndEncoding_SendsEncodedBytesToRemoteHost()
        {
            // Arrange
            using Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            listener.Listen(1);

            int listeningPort = ((IPEndPoint)listener.LocalEndPoint!).Port;

            using Socket connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectingSocket.Connect(IPAddress.Loopback, listeningPort);
            connectingSocket.ReceiveTimeout = 2000;

            using Socket acceptedSocket = listener.Accept();
            using TcpSocketClient client = new TcpSocketClient(acceptedSocket);

            const string payload = "ЁЖ";
            Encoding encoding = Encoding.Unicode;
            byte[] expectedBytes = encoding.GetBytes(payload);

            // Act
            await client.SendAsync(payload, encoding);

            byte[] receivedBuffer = new byte[64];
            int receivedLength = connectingSocket.Receive(receivedBuffer);

            // Assert
            Assert.Equal(expectedBytes.Length, receivedLength);
            Assert.Equal(expectedBytes, receivedBuffer[..receivedLength]);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for SendAsync_WithByteArray_WhenSocketNotConnected_CompletesWithoutThrowing.
        /// </summary>
        public async Task SendAsync_WithByteArray_WhenSocketNotConnected_CompletesWithoutThrowing()
        {
            // Arrange
            using TcpSocketClient client = new TcpSocketClient();
            byte[] payload = [42];

            // Act
            Exception? exception = await Record.ExceptionAsync(() => client.SendAsync(payload));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for SendAsync_WithNullByteArray_WhenSocketThrows_RaisesSendFailure.
        /// </summary>
        public async Task SendAsync_WithNullByteArray_WhenSocketThrows_RaisesSendFailure()
        {
            // Arrange
            using Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            listener.Listen(1);

            int listeningPort = ((IPEndPoint)listener.LocalEndPoint!).Port;

            using Socket connectingSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectingSocket.Connect(IPAddress.Loopback, listeningPort);

            using Socket acceptedSocket = listener.Accept();
            using TcpSocketClient client = new TcpSocketClient(acceptedSocket);

            int sendFailureCount = 0;
            Exception? capturedException = null;
            client.SendFailure += (_, e) =>
            {
                sendFailureCount++;
                capturedException = e.Exception;
            };

            // Act
            Exception? exception = await Record.ExceptionAsync(() => client.SendAsync((byte[])null!));

            // Assert
            Assert.Null(exception);
            Assert.Equal(1, sendFailureCount);
            Assert.NotNull(capturedException);
            Assert.IsType<ArgumentNullException>(capturedException);
        }



    }
}
