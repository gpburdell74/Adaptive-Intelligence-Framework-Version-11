namespace Adaptive.Intelligence.Abstractions.Security
{
    /// <summary>
    /// Provides the signature definition for classes to write data streams as encrypted content.
    /// </summary>
    /// <seealso cref="IExceptionTracking" />
    public interface ICryptoWriter : IExceptionTracking
    {
        /// <summary>
        /// Initializes the key data being used by the writer.
        /// </summary>
        /// <param name="keyTable">
        /// The reference to the <see cref="IKeyTable"/> implementation.
        /// </param>
        void InitializeKeys(IKeyTable keyTable);

        /// <summary>
        /// Encrypts and writes the stream content.
        /// </summary>
        /// <param name="sourceStream">
        /// The source <see cref="Stream"/> from which the data is to be read and then encrypted and written.
        /// </param>
        void WriteStream(Stream sourceStream);
    }
}
