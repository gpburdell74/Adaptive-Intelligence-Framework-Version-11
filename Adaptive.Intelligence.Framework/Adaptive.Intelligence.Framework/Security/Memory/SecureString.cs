using System.Text;

namespace Adaptive.Intelligence.Security.Memory
{
    /// <summary>
    /// Provides the implementation for securely storing a string in memory.
    /// </summary>
    /// <seealso cref="SecureMemoryItemBase{T}" />
    public sealed class SecureString : SecureMemoryItemBase<string>
    {
        #region Private Member Declarations		
        /// <summary>
        /// The ASCII encoding reference.
        /// </summary>
        private Encoding? _asciiRef = Encoding.ASCII;
        /// <summary>
        /// The UTF-8 encoding reference.
        /// </summary>
        private Encoding? _utfRef = Encoding.UTF8;
        #endregion

        #region Constructors		
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureString"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public SecureString()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureString"/> class.
        /// </summary>
        /// <param name="iterations">
        /// The number of key generation iterations to execute.
        /// </param>
        public SecureString(int iterations) : base(iterations)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureString"/> class.
        /// </summary>
        /// <param name="value">
        /// The value to be securely stored in memory.
        /// </param>
        public SecureString(string? value) : base(value!)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureString"/> class.
        /// </summary>
        /// <param name="iterations">
        /// The number of key generation iterations to execute.
        /// </param>
        /// <param name="value">
        /// The value to be securely stored in memory.
        /// </param>
        public SecureString(int iterations, string? value) : base(iterations, value!)
        {
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            _asciiRef = null;
            _utfRef = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties		
        /// <summary>
        /// Gets or sets a value indicating whether this instance uses ASCII encoding.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance uses ASCII encoding; otherwise, <c>false</c> if the 
        ///   instance uses UTF-8 encoding.
        /// </value>
        public bool IsAscii { get; set; }
        #endregion

        #region Protected Method Overrides		
        /// <summary>
        /// Translates provided the byte array into a string value.
        /// </summary>
        /// <param name="content">
        /// A byte array containing the binary representation of the string.
        /// </param>
        /// <returns>
        /// A string encoded in ASCII or UTF-8, depending on the value of <see cref="IsAscii"/>.
        /// </returns>
        protected override string? TranslateValueFromBytes(byte[]? content)
        {
            if (content == null)
            {
                return null;
            }
            else
            {
                if (IsAscii)
                {
                    return _asciiRef?.GetString(content);
                }
                else
                {
                    return _utfRef?.GetString(content);
                }
            }

        }
        /// <summary>
        /// Translates provided the string into a byte array.
        /// </summary>
        /// <param name="value">
        /// A string encoded in ASCII or UTF-8, depending on the value of <see cref="IsAscii"/>.
        /// </param>
        /// <returns>
        /// Returns a byte array containing the binary representation of the string.
        /// </returns>
        protected override byte[]? TranslateValueToBytes(string? value)
        {
            if (value == null)
            {
                return null;
            }
            else
            {
                if (IsAscii)
                {
                    return _asciiRef?.GetBytes(value);
                }
                else
                {
                    return _utfRef?.GetBytes(value);
                }
            }
        }
        #endregion
    }
}
