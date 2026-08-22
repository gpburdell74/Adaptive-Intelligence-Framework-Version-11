using Adaptive.Intelligence.Framework.Tests.Mocks;

namespace Adaptive.Intelligence.Framework.Tests.Abstractions
{
    /// <summary>
    /// Provides the tests for the <see cref="DisposableObjectBase"/> abstract class.
    /// </summary>
    public class DisposableObjectBaseTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for Can_Create.
        /// </summary>
        public void Can_Create()
        {
            MockDisposableBase mock = new();
            Assert.NotNull(mock);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Can_Dispose.
        /// </summary>
        public void Can_Dispose()
        {
            MockDisposableBase mock = new();
            Assert.False(mock.MockIsDisposed);

            mock.Dispose();
            Assert.True(mock.MockIsDisposed);
            mock.Dispose();
            Assert.True(mock.MockIsDisposed);
            mock.Dispose();
            Assert.True(mock.MockIsDisposed);

        }

        [Fact]
        /// <summary>
        /// Gets the definition for Can_Dispose_Safely.
        /// </summary>
        public void Can_Dispose_Safely()
        {
            MockDisposableBase mock = new();
            mock.Dispose();
            Assert.True(mock.MockIsDisposed);

            mock.Dispose();
            mock.Dispose();
            mock.Dispose();
            mock.Dispose();
            mock.Dispose();
            Assert.True(mock.MockIsDisposed);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Dispose_Raises_Disposed_Event_Once.
        /// </summary>
        public void Dispose_Raises_Disposed_Event_Once()
        {
            MockDisposableBase mock = new();
            int eventCount = 0;
            object? eventSender = null;
            EventArgs? eventArgs = null;

            mock.Disposed += (sender, args) =>
            {
                eventCount++;
                eventSender = sender;
                eventArgs = args;
            };

            mock.Dispose();
            mock.Dispose();
            mock.Dispose();

            Assert.Equal(1, eventCount);
            Assert.Same(mock, eventSender);
            Assert.Same(EventArgs.Empty, eventArgs);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Disposed_Event_Is_Not_Raised_Before_Dispose.
        /// </summary>
        public void Disposed_Event_Is_Not_Raised_Before_Dispose()
        {
            MockDisposableBase mock = new();
            int eventCount = 0;

            mock.Disposed += (_, _) => eventCount++;

            Assert.Equal(0, eventCount);
            Assert.False(mock.MockIsDisposed);
        }
    }
}