namespace Adaptive.Intelligence.Security.Memory
{
    /// <summary>
    /// Provides the implementation for securely storing a byte array in memory.
    /// </summary>
    /// <seealso cref="SecureMemoryItemBase{T}" />
    public sealed class SecureByteArray : SecureMemoryItemBase<byte[]>
    {
        #region Constructors		
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureByteArray"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public SecureByteArray()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureByteArray"/> class.
        /// </summary>
        /// <param name="iterations">
        /// The number of key generation iterations to execute.
        /// </param>
        public SecureByteArray(int iterations) : base(iterations)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureByteArray"/> class.
        /// </summary>
        /// <param name="value">
        /// The value to be securely stored in memory.
        /// </param>
        public SecureByteArray(byte[] value) : base(value)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureByteArray"/> class.
        /// </summary>
        /// <param name="iterations">
        /// The number of key generation iterations to execute.
        /// </param>
        /// <param name="value">
        /// The value to be securely stored in memory.
        /// </param>
        public SecureByteArray(int iterations, byte[] value) : base(iterations, value)
        {
        }
        #endregion

        #region Protected Method Overrides
        /// <summary>
        /// Translates provided the byte array into a byte array.
        /// </summary>
        /// <param name="content">
        /// A byte array containing the binary representation of the value.
        /// </param>
        /// <returns>
        /// For this implementation, returns the same value that was passed in.
        /// </returns>
        protected override byte[]? TranslateValueFromBytes(byte[]? content)
        {
            return content;
        }
        /// <summary>
        /// Translates provided the byte array into a byte array.
        /// </summary>
        /// <param name="value">
        /// A byte array containing the binary representation of the value.
        /// </param>
        /// <returns>
        /// For this implementation, returns the same value that was passed in.
        /// </returns>
        protected override byte[]? TranslateValueToBytes(byte[]? value)
        {
            return value;
        }
        #endregion
    }

}

