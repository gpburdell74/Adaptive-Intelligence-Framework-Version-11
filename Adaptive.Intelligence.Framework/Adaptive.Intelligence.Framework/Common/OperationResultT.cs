using Adaptive.Intelligence.Abstractions;

namespace Adaptive.Intelligence.Common
{
    /// <summary>
    /// Represents and contains the result of an attempt to execute an operation.
    /// </summary>
    public class OperationResult<T> : OperationResult, IOperationResult<T>
    {
        #region Constructor(s)
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult{T}" /> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public OperationResult()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class.
        /// </summary>
        /// <param name="success">
        /// A value indicating whether the related operation was successful.
        /// </param>
        public OperationResult(bool success) : base(success, null)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class.
        /// </summary>
        /// <param name="success">
        /// A value indicating whether the related operation was successful.
        /// </param>
        /// <param name="message">
        /// A string containing a user message.
        /// </param>
        public OperationResult(bool success, string message) : base(success, message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class.
        /// </summary>
        /// <remarks>
        /// This overload will set the <see cref="OperationResult.Success"/> property to <b>false</b>.
        /// </remarks>
        /// <param name="ex">
        /// A reference to the <see cref="Exception"/> that was caught.
        /// </param>
        public OperationResult(Exception ex) : base(ex, null)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class.
        /// </summary>
        /// <remarks>
        /// This overload will set the <see cref="OperationResult.Success"/> property to <b>false</b>.
        /// </remarks>
        /// <param name="ex">
        /// A reference to the <see cref="Exception"/> that was caught.
        /// </param>
        /// <param name="message">
        /// A string containing a user message.
        /// </param>
        public OperationResult(Exception ex, string message) : base(ex, message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationResult"/> class.
        /// </summary>
        /// <remarks>
        /// This overload will set the <see cref="OperationResult.Success"/> property to <b>true</b>.
        /// </remarks>
        /// <param name="dataContent">
        /// A reference to the object / data content stored as the result of an operation.
        /// </param>
        public OperationResult(T? dataContent)
        {
            DataContent = dataContent;
            Success = true;
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            DataContent = default;
            base.Dispose(disposing);
        }
        #endregion

        /// <summary>
        /// Gets or sets the content of the data to be returned from a
        /// successful operation.
        /// </summary>
        /// <value>
        /// A reference to the data or instance to return from a successful
        /// operation.
        /// </value>
        public T? DataContent { get; set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="DataContent"/> property has data.
        /// </summary>
        /// <value>
        /// <c>true</c> if the <see cref="DataContent"/> property has data; otherwise, <c>false</c>.
        /// </value>
        public virtual bool HasData => DataContent != null;
    }
}