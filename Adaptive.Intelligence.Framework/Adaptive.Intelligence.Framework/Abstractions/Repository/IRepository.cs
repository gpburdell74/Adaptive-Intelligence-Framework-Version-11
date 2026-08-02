using Adaptive.Intelligence.Events.Delegates;

namespace Adaptive.Intelligence.Abstractions.Repository
{
    /// <summary>
    /// Provides the signature definition for standard data access / repository methods and functions to
    /// support data access operations.
    /// </summary>
    public interface IRepository : IExceptionTracking
    {
        #region Public Events         
        /// <summary>
        /// Occurs when an asynchronous query is started.
        /// </summary>
        event StringEventHandler? AsyncQueryStarted;
        /// <summary>
        /// Occurs when an asynchronous query is completed.
        /// </summary>
        event StringEventHandler? AsyncQueryCompleted;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the cancellation token used to cancel asynchronous operations.
        /// </summary>
        /// <value>
        /// A <see cref="CancellationToken"/> instance that can be used to cancel asynchronous operations.
        /// </value>
        CancellationToken CancelToken { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the last data burst operation
        /// was successful.
        /// </summary>
        /// <value>
        ///   <b>true</b> if the last data burst operation was successful; otherwise, <b>false</b>.
        /// </value>
        bool LastOperationSuccess { get; }

        /// <summary>
        /// Gets or sets the text of the last operation error.
        /// </summary>
        /// <value>
        /// A string containing the text of the last operation error.
        /// </value>
        string? LastOperationError { get; }

        /// <summary>
        /// Gets the number of asynchronous queries that are currently executing.
        /// </summary>
        /// <value>
        /// An integer indicating the number of async queries that are currently executing.
        /// </value>
        int QueriesRunning { get; }
        #endregion

        #region Protected Abstract Methods		
        /// <summary>
        /// Records, logs, or otherwise stores the exception information when an exception is caught.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="Exception"/> instance that was caught.
        /// </param>
        void RecordException(Exception ex);
        #endregion
    }
}