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
        /// 
        /// </summary>
        /// <param name="disposing"></param>
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
