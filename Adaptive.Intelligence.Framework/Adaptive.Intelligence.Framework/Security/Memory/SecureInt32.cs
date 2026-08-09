namespace Adaptive.Intelligence.Security.Memory
{
    /// <summary>
    /// Provides the implementation for securely storing a 32-bit integer in memory.
    /// </summary>
    /// <seealso cref="SecureMemoryItemBase{T}" />
    public sealed class SecureInt32 : SecureMemoryItemBase<int>
    {
        #region Constructors		
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureInt32"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public SecureInt32()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureInt32"/> class.
        /// </summary>
        /// <param name="value">
        /// The value to be securely stored in memory.
        /// </param>
        public SecureInt32(int value) : base(2048, value)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureInt32"/> class.
        /// </summary>
        /// <param name="iterations">
        /// The number of key generation iterations to execute.
        /// </param>
        /// <param name="value">
        /// The value to be securely stored in memory.
        /// </param>
        public SecureInt32(int iterations, int value) : base(iterations, value)
        {
        }
        #endregion

        #region Protected Method Overrides		
        /// <summary>
        /// Translates provided the byte array into a 32-bit integer.
        /// </summary>
        /// <param name="content">
        /// A byte array containing the binary representation of the value.
        /// </param>
        /// <returns>
        /// An <see cref="int"/> from the provided byte array.
        /// </returns>
        protected override int TranslateValueFromBytes(byte[]? content)
        {
            if (content == null)
            {
                return 0;
            }
            else
            {
                return BitConverter.ToInt32(content, 0);
            }
        }
        /// <summary>
        /// Translates provided the integer into a byte array.
        /// </summary>
        /// <param name="value">
        /// An integer to be translates to a byte array.
        /// </param>
        /// <returns>
        /// Returns a byte array representing the integer value.
        /// </returns>
        protected override byte[]? TranslateValueToBytes(int value)
        {
            return BitConverter.GetBytes(value);
        }
        #endregion
    }

}
