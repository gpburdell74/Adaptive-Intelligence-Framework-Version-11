using Adaptive.Intelligence.Abstractions;

namespace Adaptive.Intelligence.Common
{
    /// <summary>
    /// Represents a name-and-value pair.
    /// </summary>
    /// <typeparam name="T">
    /// The data type of the value.
    /// </typeparam>
    /// <seealso cref="DisposableObjectBase" />
    public class NameValuePair<T> : DisposableObjectBase
    {
        #region Private Member Declarations		
        /// <summary>
        /// The name to associate with the value.
        /// </summary>
        private string? _name;
        /// <summary>
        /// The value to associate with the name.
        /// </summary>
        private T? _value;
        #endregion

        #region Constructor / Dispose Methods		
        /// <summary>
        /// Initializes a new instance of the <see cref="NameValuePair{T}"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public NameValuePair()
        {
            _value = default;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="NameValuePair{T}"/> class.
        /// </summary>
        /// <param name="name">
        /// A string containing the name.
        /// </param>
        /// <param name="value">
        /// The value of <typeparamref name="T"/> to be stored.
        /// </param>
        public NameValuePair(string name, T value)
        {
            _name = name;
            _value = value;
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            _name = null;
            _value = default;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties		
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// A string containing the name to associate with the value.
        /// </value>
        public string? Name
        {
            get => _name;
            set => _name = value;
        }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The <typeparamref name="T"/> value to associate with the name.
        /// </value>
        public T? Value
        {
            get => _value;
            set => _value = value;
        }
        #endregion
    }
}