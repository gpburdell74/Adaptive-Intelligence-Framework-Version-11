using System.Text;

namespace Adaptive.Intelligence.Common.Abstractions;

/// <summary>
/// Provides a class that can track the exceptions that occur during its operation.
/// </summary>
/// <seealso cref="DisposableObjectBase" />
public abstract class ExceptionTrackingBase : DisposableObjectBase, IExceptionTracking
{
    #region Private Member Declarations
    /// <summary>
    /// The exception list.
    /// </summary>
    private ExceptionCollection? _exceptionList;
    #endregion

    #region Constructor / Dispose Methods
    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionTrackingBase"/> class.
    /// </summary>
    /// <remarks>
    /// This is the default constructor.
    /// </remarks>
    protected ExceptionTrackingBase()
    {
        _exceptionList = [];
    }
    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
    /// <b>false</b> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
        if (!IsDisposed && disposing)
        {
            _exceptionList?.Dispose();
        }

        _exceptionList = null;
        base.Dispose(disposing);
    }
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets the reference to the list of exceptions that have been caught.
    /// </summary>
    /// <value>
    /// An <see cref="ExceptionCollection"/> containing the list of exceptions.
    /// </value>
    public ExceptionCollection? Exceptions
    {
        get
        {
            if (_exceptionList == null)
            {
                ObjectDisposedException.ThrowIf(IsDisposed, this);

                _exceptionList = [];
                return _exceptionList;
            }
            else
            {
                return _exceptionList;
            }
        }
    }
    /// <summary>
    /// Gets the messages from all the captured exceptions as a string.
    /// </summary>
    /// <value>
    /// A string containing all the exception messages, or <see cref="string.Empty"/>.
    /// </value>
    public string ExceptionMessages
    {
        get
        {
            if (_exceptionList == null || _exceptionList.Count == 0)
            {
                return string.Empty;
            }
            else
            {
                StringBuilder builder = new();
                foreach (Exception ex in _exceptionList)
                {
                    builder.AppendLine(ex.Message);
                    builder.AppendLine();
                }
                return builder.ToString();
            }

        }
    }
    /// <summary>
    /// Gets the reference to the first exception in the list, if present.
    /// </summary>
    /// <value>
    /// The first <see cref="Exception"/> that was caught, if any.
    /// </value>
    public Exception? FirstException
    {
        get
        {
            if (_exceptionList == null || _exceptionList.Count == 0)
            {
                return null;
            }
            else
            {
                return _exceptionList[0];
            }
        }
    }
    /// <summary>
    /// Gets a value indicating whether there are currently any exceptions in the list.
    /// </summary>
    /// <value>
    ///   <c>true</c> if exceptions have been caught; otherwise, <c>false</c>.
    /// </value>
    public bool HasExceptions => (_exceptionList != null && _exceptionList.Count > 0);
    #endregion

    #region Public Methods / Functions
    /// <summary>
    /// Adds the exception to the list.
    /// </summary>
    /// <param name="ex">
    /// The <see cref="Exception"/> instance to be added to the list.
    /// </param>
    public void AddException(Exception ex)
    {
        if (!IsDisposed)
        {
            _exceptionList ??= [];
            _exceptionList?.Add(ex);
        }
    }
    /// <summary>
    /// Clears the current exception list.
    /// </summary>
    public void ClearExceptions()
    {
        if (!IsDisposed)
        {
            _exceptionList?.Clear();
        }
    }
    /// <summary>
    /// Copies the exceptions from the result instance to the local instance's exceptions list.
    /// </summary>
    /// <param name="result">
    /// The <see cref="IOperationalResult"/> result from a SQL operation.
    /// </param>
    public void CopyExceptions(IOperationalResult? result)
    {
        if (!IsDisposed)
        {

            if (_exceptionList != null && result != null && result.Exceptions != null)
            {
                _exceptionList.AddRange(result.Exceptions!);
            }
        }
    }
    #endregion
}
