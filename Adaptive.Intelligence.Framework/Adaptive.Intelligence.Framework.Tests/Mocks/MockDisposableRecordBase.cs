using Adaptive.Intelligence.Abstractions;

namespace Adaptive.Intelligence.Framework.Tests.Mocks
{
    /// <summary>
    /// Provides a testable wrapper for the <see cref="DisposableRecordBase"/> abstract class.
    /// </summary>
    public record MockDisposableRecordBase : DisposableRecordBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MockDisposableRecordBase"/> class.
        /// </summary>
        public MockDisposableRecordBase()
        {
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="MockDisposableBase"/> class and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// A value indicating whether the current instance is being disposed.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets a value indicating whether the instance has already been disposed.
        /// </summary>
        /// <value>
        /// <b>true</b> if the instance has been disposed; otherwise, <b>false</b>.
        /// </value>
        public bool MockIsDisposed => IsDisposed;
    }
}