using System.Text.Json.Serialization;

namespace Adaptive.Intelligence.Common.Abstractions;

/// <summary>
/// Provides the base implementation of the <see cref="IDisposable"/>  pattern for <see langword="record"/> types.
/// </summary>
public abstract record DisposableRecordBase : IDisposable
{
    #region Public Events
    /// <summary>
    /// Occurs when the instance is disposed.
    /// </summary>
    public event EventHandler? Disposed;
    #endregion

    #region Private Member Declarations
    /// <summary>
    /// Disposed flag.
    /// </summary>
    private bool _disposed;
    #endregion

    #region Constructor / Dispose Methods
    /// <summary>
    /// Initializes a new instance of the <see cref="DisposableRecordBase"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    protected DisposableRecordBase()
    {
    }
    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing">
    /// <b>true</b> to release both managed and unmanaged resources;
    /// <b>false</b> to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            OnDisposed(EventArgs.Empty);
        }
        _disposed = true;

    }
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    /// <summary>
    /// Finalizes an instance of the <see cref="DisposableRecordBase"/> class.
    /// </summary>
    ~DisposableRecordBase()
    {
        Dispose(false);
    }
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets a value indicating whether this instance is disposed.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
    /// </value>
    [JsonIgnore()]
    protected bool IsDisposed => _disposed;
    #endregion

    #region Protected Event Methods
    /// <summary>
    /// Raises the <see cref="Disposed" /> event.
    /// </summary>
    /// <param name="e">
    /// The <see cref="EventArgs"/> instance containing the event data.
    /// </param>
    protected void OnDisposed(EventArgs e)
    {
        Disposed?.Invoke(this, e);
    }
    #endregion
}