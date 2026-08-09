namespace Adaptive.Intelligence.Security
{
    /// <summary>
    /// Provides the signature definition for classes that implement a symmetric key store,
    /// which is responsible for storing the cryptographic key data for a symmetric cryptographic provider 
    /// implementation.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface ISymmetricKeyStore : IDisposable, ICloneable
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets the initialization vector data, if needed for the algorithm.
        /// </summary>
        /// <value>
        /// A byte array containing the initialization vector data, or <b>null</b>.
        /// </value>
        byte[]? IV { get; set; }

        /// <summary>
        /// Gets or sets the size of the byte array that contains the initialization vector data.
        /// </summary>
        /// <value>
        /// An integer specifying the size of the byte array that contains the initialization vector data, 
        /// if needed for the algorithm; otherwise, zero (0).
        /// </value>
        int IVSize { get; }

        /// <summary>
        /// Gets or sets the cryptographic key data.
        /// </summary>
        /// <value>
        /// A byte array containing the cryptographic key data.
        /// </value>
        byte[]? Key { get; set; }

        /// <summary>
        /// Gets the size of the byte array that contains the key data.
        /// </summary>
        /// <value>
        /// An integer specifying the size of the byte array that contains the key data.
        /// </value>
        int KeySize { get; }

        /// <summary>
        /// Gets or sets the secondary cryptographic key data.
        /// </summary>
        /// <value>
        /// A byte array containing the secondary cryptographic key data, if used; otherwise,
        /// <b>null</b>.
        /// </value>
        byte[]? SecondaryKey { get; set; }

        /// <summary>
        /// Gets the size of the byte array that contains the secondary key data.
        /// </summary>
        /// <value>
        /// An integer specifying the size of the byte array that contains the secondary key data, 
        /// if needed for the algorithm; otherwise, zero (0).
        /// </value>
        int SecondaryKeySize { get; }

        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Clears the internal storage of the key data, and releases any resources used to store the key data.
        /// </summary>
        void ClearKeyMemory();

        /// <summary>
        /// Gets the concatenation of the key and initialization vector data as a single concatenated byte array.
        /// </summary>
        /// <returns>
        /// A byte array where the first elements contain the key data, including the secondary key data if present,
        /// and the last elements contain the initialization vector data.
        /// </returns>
        byte[]? ConcatenateKeyIVData();

        /// <summary>
        /// Gets the concatenation of the key and initialization vector data as a single concatenated byte 
        /// array converted to a base-64 encoded string.
        /// </summary>
        /// <returns>
        /// A base-64 encoded string representing the byte array where the first elements contain the key data, including the secondary key data if present,
        /// and the last elements contain the initialization vector data.
        /// </returns>
        string? ConcatenateKeyIVDataAsBase64String();

        /// <summary>
        /// Sets the cryptographic key and initialization vector data from the provided byte array.
        /// </summary>
        /// <param name="cryptographicData">
        /// A byte array where the first elements contain the key data and the last elements
        /// contain the IV data.
        /// </param>
        void SetKeyIVData(byte[]? cryptographicData);

        /// <summary>
        /// Sets the cryptographic key and initialization vector data from the specified base-64 encoded string.
        /// </summary>
        /// <param name="keyData">
        /// A base-64 encoded string representing a byte array where the first elements contain the key data,
        /// and if used, the secondary key data, and the last elements contain the initialization vector data.
        /// </param>
        void SetKeyIVDataFromBase64String(string? keyData);

        /// <summary>
        /// Verifies the provided byte array contains valid initialization vector data of the expected size.
        /// </summary>
        /// <param name="cryptographicData">
        /// A byte array containing the cryptographic initialization vector data to verify.
        /// </param>
        /// <returns>
        /// <b>true</b> if the data is present and is of the correct length;
        /// otherwise, returns <b>false</b>.
        /// </returns>
        bool VerifyIVSize(byte[]? cryptographicData);

        /// <summary>
        /// Verifies the provided byte array contains valid cryptographic key data of the expected size.
        /// </summary>
        /// <param name="cryptographicData">
        /// A byte array containing the cryptographic key data to verify.
        /// </param>
        /// <returns>
        /// <b>true</b> if the data is present and is of the correct length;
        /// otherwise, returns <b>false</b>.
        /// </returns>
        bool VerifyKeySize(byte[]? cryptographicData);

        /// <summary>
        /// Verifies the provided byte array contains valid cryptographic secondary key data of the expected size.
        /// </summary>
        /// <param name="cryptographicData">
        /// A byte array containing the cryptographic secondary key data to verify.
        /// </param>
        /// <returns>
        /// <b>true</b> if the data is present and is of the correct length;
        /// otherwise, returns <b>false</b>.
        /// </returns>
        bool VerifySecondaryKeySize(byte[]? cryptographicData);
        #endregion
    }
}