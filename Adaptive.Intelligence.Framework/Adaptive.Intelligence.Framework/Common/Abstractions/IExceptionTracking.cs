namespace Adaptive.Intelligence.Common.Abstractions;

/// <summary>
/// Provides the signature definition for classes that can track the exceptions that occur during its operation.
/// </summary>
/// <seealso cref="IDisposable" />
public interface IExceptionTracking : IDisposable
{
    #region Properties
    /// <summary>
    /// Gets the reference to the list of exceptions that have been caught.
    /// </summary>
    /// <value>
    /// An <see cref="ExceptionCollection"/> of <see cref="Exception"/> instances.
    /// </value>
    ExceptionCollection? Exceptions { get; }

    /// <summary>
    /// Gets a value indicating whether there are currently any exceptions in the list.
    /// </summary>
    /// <value>
    ///   <c>true</c> if exceptions have been caught; otherwise, <c>false</c>.
    /// </value>
    bool HasExceptions { get; }

    #endregion

    #region Methods / Functions
    /// <summary>
    /// Adds the exception to the list.
    /// </summary>
    /// <param name="ex">
    /// The <see cref="Exception"/> instance to be added to the list.
    /// </param>
    void AddException(Exception ex);

    /// <summary>
    /// Clears the current exception list.
    /// </summary>
    void ClearExceptions();
    #endregion
}
