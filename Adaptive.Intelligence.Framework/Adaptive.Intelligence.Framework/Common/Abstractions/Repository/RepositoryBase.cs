using Adaptive.Intelligence.Common.Events.Arguments;
using Adaptive.Intelligence.Common.Events.Delegates;
using Microsoft.Extensions.Logging;

namespace Adaptive.Intelligence.Common.Abstractions.Repository;

/// <summary>
/// Provides a base definition for standard data access / repository methods and functions to
/// support data access operations.
/// </summary>
/// <param name="logger">
/// A reference to the <see cref="ILogger"/> instance used for logging.
/// </param>
public abstract class RepositoryBase(ILogger? logger) : ExceptionTrackingBase, IRepository
{
    #region Public Events         
    /// <summary>
    /// Occurs when an asynchronous query is started.
    /// </summary>
    public event StringEventHandler? AsyncQueryStarted;
    /// <summary>
    /// Occurs when an asynchronous query is completed.
    /// </summary>
    public event StringEventHandler? AsyncQueryCompleted;
    #endregion

    #region Private Member Declarations
    /// <summary>
    /// The logger instance.
    /// </summary>
    private ILogger? _logger = logger;
    /// <summary>
    /// The thread synchronization instance.
    /// </summary>
    private readonly object _syncRoot = new();
    /// <summary>
    /// Keeps track of the number of queries executing at one time.
    /// </summary>
    private int _queriesRunning;

    /// <summary>
    /// The cancellation token source used to cancel asynchronous operations.
    /// </summary>
    private CancellationTokenSource? _cancelSource = new();
    #endregion

    #region Event Methods
    /// <summary>
    /// Called when a remote query starts to raise the <see cref="AsyncQueryStarted"/> event.
    /// </summary>
    protected void OnAsyncQueryStarted(string methodName)
    {
        lock (_syncRoot)
        {
            _queriesRunning++;
        }

        // Don't let the subscriber do anything goofy.
        try
        {
            AsyncQueryStarted?.Invoke(this, new StringEventArgs { Content = methodName });
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Implementation of AsyncQueryStarted event handler threw an exception.");
        }
    }
    /// <summary>
    /// Called when a remote query ends to raise the <see cref="AsyncQueryCompleted"/> event.
    /// </summary>
    protected void OnAsyncQueryCompleted(string methodName)
    {
        lock (_syncRoot)
        {
            _queriesRunning--;
            if (_queriesRunning < 0)
            {
                _queriesRunning = 0;
            }
        }

        // Don't let the subscriber do anything goofy.
        try
        {
            AsyncQueryCompleted?.Invoke(this, new StringEventArgs { Content = methodName });
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Implementation of AsyncQueryCompleted event handler threw an exception.");
        }
    }
    #endregion

    #region Dispose Method		
    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
    /// <b>false</b> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
        if (!IsDisposed && disposing)
        {
            _cancelSource?.Dispose();
        }
        _cancelSource = null;
        _logger = null;
        LastOperationError = null;
        base.Dispose(disposing);
    }
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets the cancellation token used to cancel asynchronous operations.
    /// </summary>
    /// <value>
    /// A <see cref="CancellationToken"/> instance that can be used to cancel asynchronous operations.
    /// </value>
    public CancellationToken CancelToken
    {
        get
        {
            ObjectDisposedException.ThrowIf((_cancelSource == null), this);
            return _cancelSource.Token;
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the last data burst operation
    /// was successful.
    /// </summary>
    /// <value>
    ///   <b>true</b> if the last data burst operation was successful; otherwise, <b>false</b>.
    /// </value>
    public bool LastOperationSuccess { get; protected set; }
    /// <summary>
    /// Gets or sets the text of the last operation error.
    /// </summary>
    /// <value>
    /// A string containing the text of the last operation error.
    /// </value>
    public string? LastOperationError { get; protected set; }


    /// <summary>
    /// Gets the number of asynchronous queries that are currently executing.
    /// </summary>
    /// <value>
    /// An integer indicating the number of async queries that are currently executing.
    /// </value>
   public virtual int QueriesRunning
    {
        get
        {
            return _queriesRunning;
        }
    }
    #endregion

    #region Logging Exceptions
    /// <summary>
    /// Records, logs, or otherwise stores the exception information when an exception is caught.
    /// </summary>
    /// <param name="ex">
    /// The <see cref="Exception"/> instance that was caught.
    /// </param>
    public virtual void RecordException(Exception ex)
    {
        AddException(ex);
        LastOperationError = ex.Message;
        _logger?.LogError(ex, ex.Message);
    }
    #endregion
}

