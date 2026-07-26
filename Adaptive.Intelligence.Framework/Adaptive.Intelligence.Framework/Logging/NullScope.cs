using Adaptive.Intelligence.Common.Abstractions;

namespace Adaptive.Intelligence.Logging
{
    /// <summary>
    /// Provides the NullScope implementation for simple loggers.
    /// </summary>
    public sealed class NullScope : DisposableObjectBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NullScope"/> class.
        /// </summary>
        private NullScope() { }
        /// <summary>
        /// Releases resources used by the <see cref="NullScope"/> instance.
        /// </summary>
        /// <param name="disposing">
        /// <b>true</b> when called from <see cref="Dispose"/>; otherwise, <b>false</b>.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets a new "scope" instance.
        /// </summary>
        /// <value>
        /// Always a new <see cref="NullScope"/> instance.
        /// </value>
        public static NullScope Instance => new();

    }
}
