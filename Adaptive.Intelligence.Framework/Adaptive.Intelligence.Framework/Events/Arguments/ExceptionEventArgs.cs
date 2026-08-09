namespace Adaptive.Intelligence.Events.Arguments
{
    /// <summary>
    /// Provides event arguments events that communicate exceptions.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ExceptionEventArgs"/> class.
    /// </remarks>
    /// <param name="exception">
    /// An <see cref="Exception"/> reference, or <b>null</b> if not supplied.
    /// </param>
    /// <param name="errorMessage">
    /// A string specifying the error message, or <b>null</b> if not supplied.
    /// </param>
    public sealed class ExceptionEventArgs(Exception? exception, string? errorMessage) : EventArgs
    {
        #region Constructor / Destructor Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionEventArgs"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public ExceptionEventArgs() : this(null, null)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionEventArgs"/> class.
        /// </summary>
        /// <param name="errorMessage">
        /// A string specifying the error message, or <b>null</b> if not supplied.
        /// </param>
        public ExceptionEventArgs(string? errorMessage) : this(null, errorMessage)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionEventArgs"/> class.
        /// </summary>
        /// <param name="exception">
        /// An <see cref="Exception"/> reference, or <b>null</b> if not supplied.
        /// </param>
        public ExceptionEventArgs(Exception? exception) : this(exception, null)
        {

        }

        /// <summary>
        /// Finalizes an instance of the <see cref="ExceptionEventArgs"/> class.
        /// </summary>
        ~ExceptionEventArgs()
        {
            ErrorMessage = null;
            Exception = null;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the current error message.
        /// </summary>
        /// <value>
        /// A string specifying the error message, or <b>null</b> if not supplied.
        /// </value>
        public string? ErrorMessage { get; set; } = errorMessage;
        /// <summary>
        /// Gets or sets the reference to the related exception.
        /// </summary>
        /// <value>
        /// An <see cref="Exception"/> reference, or <b>null</b> if not supplied.
        /// </value>
        public Exception? Exception { get; set; } = exception;
        #endregion
    }
}
