using Adaptive.Intelligence.Collections;

namespace Adaptive.Intelligence.Abstractions
{
    /// <summary>
    /// Provides the signature definition for an instance that contains the result of an attempt to execute an operation.
    /// </summary>
    public interface IOperationResult : IDisposable
    {
        #region Properties
        /// <summary>
        /// Gets the reference to the list of <see cref="Exception"/>s that were encountered.
        /// </summary>
        /// <value>
        /// The <see cref="ExceptionCollection"/> containing the list of exceptions.
        /// </value>
        ExceptionCollection? Exceptions { get; }

        /// <summary>
        /// Gets the reference to the first exception in the list, if any are present.
        /// </summary>
        /// <value>
        /// The reference to the first <see cref="Exception"/> in the <see cref="Exceptions"/> list, if 
        /// the list is not null or empty.
        /// </value>
        Exception? FirstException { get; }

        /// <summary>
        /// Gets a value indicating whether this instance has exceptions.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has exceptions; otherwise, <c>false</c>.
        /// </value>
        bool HasExceptions { get; }

        /// <summary>
        /// Gets or sets the message text.
        /// </summary>
        /// <value>
        /// A string containing a user message.
        /// </value>
        string? Message { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the related operation
        /// was successful.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the related operation was successful; otherwise, <c>false</c>.
        /// </value>
        bool Success { get; set; }
        #endregion

        #region Methods / Functions		
        /// <summary>
        /// Adds exception instance to the current instance's list of exceptions.
        /// </summary>
        /// <param name="exception">
        /// The <see cref="Exception"/> instance to add to the current instance's exceptions list.
        /// </param>
        void AddException(Exception? exception);

        /// <summary>
        /// Adds the list of exceptions to the current instance's list of exceptions.
        /// </summary>
        /// <param name="exceptions">
        /// An <see cref="IEnumerable{T}"/> list of <see cref="Exception"/> instances to add to the current
        /// instance's list.
        /// </param>
        void AddExceptions(IEnumerable<Exception>? exceptions);

        /// <summary>
        /// Copies the data in the current instance to the provided instance.
        /// </summary>
        /// <param name="newResult">
        /// The <see cref="IOperationResult"/> to copy the field values to.
        /// </param>
        void CopyTo(IOperationResult? newResult);

        /// <summary>
        /// Sets the instance's <see cref="Message"/> property to the provided value, and 
        /// sets the <see cref="Success"/> property to <c>false</c>.
        /// </summary>
        void SetFailureMessage(string? message);
        #endregion
    }
}