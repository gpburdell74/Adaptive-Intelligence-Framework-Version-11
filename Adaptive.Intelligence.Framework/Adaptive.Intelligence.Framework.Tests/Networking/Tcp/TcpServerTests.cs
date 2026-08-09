using Adaptive.Intelligence.Networking.Tcp;
using System.Net;

namespace Adaptive.Intelligence.Framework.Tests.Networking.Tcp
{
    public class TcpServerTests
    {
        [Fact]
        public void TcpServer_ValidEndpointAndMaxConnections_InitializesBoundServer()
        {
            // Arrange
            IPEndPoint listenEndpoint = new(IPAddress.Loopback, 0);

            // Act
            using TcpServer server = new(listenEndpoint, 5);

            // Assert
            Assert.False(server.IsListening);
            Assert.Equal(5, server.MaxConnections);

            EndPoint? localEndpoint = server.LocalEndpoint;
            Assert.NotNull(localEndpoint);

            IPEndPoint localIpEndpoint = Assert.IsType<IPEndPoint>(localEndpoint);
            Assert.Equal(IPAddress.Loopback, localIpEndpoint.Address);
            Assert.True(localIpEndpoint.Port > 0);
        }

        [Fact]
        public void TcpServer_MaxConnectionsSet_UpdatesMaxConnections()
        {
            // Arrange
            IPEndPoint listenEndpoint = new(IPAddress.Loopback, 0);
            using TcpServer server = new(listenEndpoint);

            // Act
            server.MaxConnections = 12;

            // Assert
            Assert.Equal(12, server.MaxConnections);
        }

        [Fact]
        public void IsListening_StartListeningCalled_ReturnsTrue()
        {
            // Arrange
            IPEndPoint listenEndpoint = new(IPAddress.Loopback, 0);
            using TcpServer server = new(listenEndpoint, 2);

            // Act
            server.StartListening();

            // Assert
            Assert.True(server.IsListening);

            server.StopListening();
        }

        [Fact]
        public void LocalEndpoint_StopListeningCalled_ReturnsNull()
        {
            // Arrange
            IPEndPoint listenEndpoint = new(IPAddress.Loopback, 0);
            using TcpServer server = new(listenEndpoint);

            // Act
            server.StopListening();

            // Assert
            Assert.Null(server.LocalEndpoint);
        }

        [Fact]
        public void Dispose_DisposedCalled_ReleasesResourcesAndRaisesDisposedEventOnce()
        {
            // Arrange
            IPEndPoint listenEndpoint = new(IPAddress.Loopback, 0);
            TcpServer server = new(listenEndpoint);
            int disposedEventCallCount = 0;
            server.Disposed += (_, _) => disposedEventCallCount++;
            server.StartListening();

            // Act
            server.Dispose();
            server.Dispose();

            // Assert
            Assert.False(server.IsListening);
            Assert.Null(server.LocalEndpoint);
            Assert.Equal(1, disposedEventCallCount);
        }

        [Fact]
        public void StartListening_AlreadyListening_DoesNotRaiseListenStartedAgain()
        {
            // Arrange
            IPEndPoint listenEndpoint = new(IPAddress.Loopback, 0);
            using TcpServer server = new(listenEndpoint);
            int listenStartedCallCount = 0;
            server.ListenStarted += (_, _) => listenStartedCallCount++;

            // Act
            server.StartListening();
            server.StartListening();

            // Assert
            Assert.True(server.IsListening);
            Assert.Equal(1, listenStartedCallCount);

            server.StopListening();
        }

        [Fact]
        public void StartListening_AfterStopListening_DoesNotStartAgain()
        {
            // Arrange
            IPEndPoint listenEndpoint = new(IPAddress.Loopback, 0);
            using TcpServer server = new(listenEndpoint);
            int listenStartedCallCount = 0;
            server.ListenStarted += (_, _) => listenStartedCallCount++;
            server.StartListening();
            server.StopListening();

            // Act
            server.StartListening();

            // Assert
            Assert.False(server.IsListening);
            Assert.Null(server.LocalEndpoint);
            Assert.Equal(1, listenStartedCallCount);
        }

        [Fact]
        public void StopListening_CalledTwice_RaisesListenStoppedForEachCall()
        {
            // Arrange
            IPEndPoint listenEndpoint = new(IPAddress.Loopback, 0);
            using TcpServer server = new(listenEndpoint);
            int listenStoppedCallCount = 0;
            server.ListenStopped += (_, _) => listenStoppedCallCount++;

            // Act
            server.StopListening();
            server.StopListening();

            // Assert
            Assert.False(server.IsListening);
            Assert.Null(server.LocalEndpoint);
            Assert.Equal(2, listenStoppedCallCount);
        }


    }
}
