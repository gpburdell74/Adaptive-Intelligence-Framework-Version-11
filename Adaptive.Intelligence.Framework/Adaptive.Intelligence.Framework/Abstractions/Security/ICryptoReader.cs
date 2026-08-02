namespace Adaptive.Intelligence.Abstractions.Security
{
    /// <summary>
    /// Provides the signature definition for classes to read data streams from encrypted content.
    /// </summary>
    /// <seealso cref="IExceptionTracking" />
    public interface ICryptoReader : IExceptionTracking
    {
        /// <summary>
        /// Initializes the key data being used by the reader.
        /// </summary>
        /// <param name="keyTable">
        /// The reference to the <see cref="IKeyTable"/> implementation.
        /// </param>
        void InitializeKeys(IKeyTable keyTable);
        /// <summary>
        /// Reads and decrypts the content from the original source and writes the clear content to 
        /// the provided stream.
        /// </summary>
        /// <param name="destinationStream">
        /// The source <see cref="Stream"/> to which the decrypted data is to be written.
        /// </param>
        void ReadStream(Stream destinationStream);
    }
}
