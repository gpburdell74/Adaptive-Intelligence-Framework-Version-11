using Adaptive.Intelligence.Networking.Tcp;

namespace Adaptive.Intelligence.Framework.Tests
{
    /// <summary>
    /// Provides tests for <see cref="TcpClientOptions"/>.
    /// </summary>
    public class TcpClientOptionsTests
    {
        [Fact]
        /// <summary>
        /// Verifies that setting Port to a valid value updates the property.
        /// </summary>
        public void Port_SetToValidValue_StoresAndReturnsValue()
        {
            using TcpClientOptions options = new();

            options.Port = 8080;

            Assert.Equal(8080, options.Port);
        }

        [Fact]
        /// <summary>
        /// Verifies that setting Port to a value below the valid range is ignored.
        /// </summary>
        public void Port_SetBelowMinimum_DoesNotChangeValue()
        {
            using TcpClientOptions options = new();
            options.Port = 1234;

            options.Port = -1;

            Assert.Equal(1234, options.Port);
        }

        [Fact]
        /// <summary>
        /// Verifies that setting Port to a value at the exclusive upper bound is ignored.
        /// </summary>
        public void Port_SetAtMaximum_DoesNotChangeValue()
        {
            using TcpClientOptions options = new();
            options.Port = 2345;

            options.Port = 65536;

            Assert.Equal(2345, options.Port);
        }

        [Fact]
        /// <summary>
        /// Verifies that setting Port to the minimum valid value updates the property.
        /// </summary>
        public void Port_SetToMinimumValue_StoresValue()
        {
            using TcpClientOptions options = new();

            options.Port = 0;

            Assert.Equal(0, options.Port);
        }

        [Fact]
        /// <summary>
        /// Verifies that Default returns an options instance with expected default values.
        /// </summary>
        public void Default_Get_ReturnsExpectedConfiguredInstance()
        {
            using TcpClientOptions options = TcpClientOptions.Default;

            Assert.Equal(80, options.Port);
            Assert.True(options.NoDelay);
            Assert.Equal(1024000, options.SendBufferSize);
            Assert.Equal(1024000, options.ReceiveBufferSize);
            Assert.Equal(3000, options.ReceiveTimeout);
            Assert.Equal(3000, options.SendTimeout);
        }

        [Fact]
        /// <summary>
        /// Verifies that each Default call returns a new instance.
        /// </summary>
        public void Default_GetCalledTwice_ReturnsDifferentInstances()
        {
            using TcpClientOptions first = TcpClientOptions.Default;
            using TcpClientOptions second = TcpClientOptions.Default;

            Assert.NotSame(first, second);
        }
    }
}
