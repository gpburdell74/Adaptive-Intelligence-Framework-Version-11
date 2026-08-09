using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Abstractions.Security;

namespace Adaptive.Intelligence.Security
{
    /// <summary>
    /// Provides an implementation for an AES cryptographic key table.
    /// </summary>
    /// <seealso cref="DisposableObjectBase" />
    /// <seealso cref="IKeyTable" />
    public sealed class AesKeyTable : DisposableObjectBase, IKeyTable
    {
        #region Private Member Declarations

        #region Constants		
        /// <summary>
        /// The size of an AES 256-bit key and 128-bit Initialization vector as a single array.
        /// </summary>
        private const int KeyIVSize = 48;
        /// <summary>
        /// The total size of 6 AES key and IV pairs.
        /// </summary>
        private const int SixKeySize = 288;
        /// <summary>
        /// The list length.
        /// </summary>
        private const int ListLength = 6;
        #endregion

        /// <summary>
        /// The list of AES key and IV values.
        /// </summary>
        private List<AesKeyStore>? _keyData;
        /// <summary>
        /// The current enumeration index.
        /// </summary>
        private int _currentIndex = -1;
        #endregion

        #region Constructor / Dispose Methods		
        /// <summary>
        /// Initializes a new instance of the <see cref="AesKeyTable"/> class.
        /// </summary>
        /// <param name="primary">
        /// A byte array containing the primary AES key and IV pair.
        /// </param>
        /// <param name="secondary">
        /// A byte array containing the secondary AES key and IV pair.
        /// </param>
        /// <param name="tertiary">
        /// A byte array containing the tertiary AES key and IV pair.
        /// </param>
        /// <param name="quaternary">
        /// A byte array containing the quaternary (4th) AES key and IV pair.
        /// </param>
        /// <param name="quinary">
        /// A byte array containing the quinary (5th) AES key and IV pair.
        /// </param>
        /// <param name="senary">
        /// A byte array containing the senary (6th) AES key and IV pair.
        /// </param>
        public AesKeyTable(byte[] primary, byte[] secondary, byte[] tertiary, byte[] quaternary, byte[] quinary, byte[] senary)
        {
            // Validate the array sizes.
            if (primary.Length != KeyIVSize ||
                secondary.Length != KeyIVSize ||
                tertiary.Length != KeyIVSize ||
                quaternary.Length != KeyIVSize ||
                quinary.Length != KeyIVSize ||
                senary.Length != KeyIVSize)
            {
                throw new ArgumentException("Invalid key content length.");
            }

            // Store the data securely in memory.
            _keyData =
            [
                new AesKeyStore(primary),
                new AesKeyStore(secondary),
                new AesKeyStore(tertiary),
                new AesKeyStore(quaternary),
                new AesKeyStore(quinary),
                new AesKeyStore(senary)
            ];
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AesKeyTable"/> class.
        /// </summary>
        /// <param name="keyContent">
        /// A byte array of 288 elements, containing six AES 256-bit key and 128-bit initialization vector
        /// values combined as a single array.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Occurs when the <i>keyContent</i> array is not 288 elements in length.
        /// </exception>
        public AesKeyTable(byte[] keyContent)
        {
            ArgumentNullException.ThrowIfNull(keyContent);

            if (keyContent.Length != SixKeySize)
            {
                throw new ArgumentException("Invalid key content length.");
            }

            // Create the storage list.
            _keyData = new List<AesKeyStore>(ListLength);
            for (var pos = 0; pos < SixKeySize; pos += 48)
            {
                // Extract each section of 48 bytes ( a 32 byte key and 16 byte IV pair. )
                var keyData = new byte[KeyIVSize];
                Array.Copy(keyContent, pos, keyData, 0, KeyIVSize);

                // Add the key to memory, in sequential order.
                _keyData.Add(new AesKeyStore(keyData));
                CryptoUtil.SecureClear(keyData);
            }
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (_keyData != null)
            {
                ClearKeyData();
                _keyData?.Clear();
            }

            _keyData = null;
            _currentIndex = -1;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties		
        /// <summary>
        /// Gets the primary key data.
        /// </summary>
        /// <value>
        /// A byte array containing the primary (1st) key data.
        /// </value>
        public byte[] Primary => ReadKeyByIndex(0);
        /// <summary>
        /// Gets the secondary key data.
        /// </summary>
        /// <value>
        /// A byte array containing the secondary (2nd) key data.
        /// </value>
        public byte[] Secondary => ReadKeyByIndex(1);
        /// <summary>
        /// Gets the tertiary key data.
        /// </summary>
        /// <value>
        /// A byte array containing the tertiary (3rd) key data.
        /// </value>
        public byte[] Tertiary => ReadKeyByIndex(2);
        /// <summary>
        /// Gets the quaternary key data.
        /// </summary>
        /// <value>
        /// A byte array containing the quaternary (4th) key data.
        /// </value>
        public byte[] Quaternary => ReadKeyByIndex(3);
        /// <summary>
        /// Gets the quinary key data.
        /// </summary>
        /// <value>
        /// A byte array containing the quinary (5th) key data.
        /// </value>
        public byte[] Quinary => ReadKeyByIndex(4);
        /// <summary>
        /// Gets the senary key data.
        /// </summary>
        /// <value>
        /// A byte array containing the senary (6th) key data.
        /// </value>
        public byte[] Senary => ReadKeyByIndex(5);
        #endregion

        #region Public Methods / Functions		
        /// <summary>
        /// Clears the key data from memory.
        /// </summary>
        /// <remarks>
        /// Once this method is invoked, the current instance is no longer usable.
        /// </remarks>
        public void ClearKeyData()
        {
            if (_keyData != null)
            {
                for (var count = 0; count < 6; count++)
                {
                    _keyData[count].ClearKeyMemory();
                    _keyData[count].Dispose();
                }

                _keyData.Clear();
            }

            _keyData = null;
        }
        /// <summary>
        /// Gets the next key value in sequence, and increments the internal enumerator.
        /// </summary>
        /// <returns>
        /// A byte array containing the key data.
        /// </returns>
        /// <remarks>
        /// When the last key is reached, the method will return to the first key value.  Subsequent calls to
        /// the method will return the keys in sequential order.
        /// </remarks>
        public byte[] Next()
        {
            _currentIndex++;
            if (_currentIndex > 5)
            {
                _currentIndex = 0;
            }

            return EmptyIfNull(ReadKeyByIndex(_currentIndex), KeyIVSize);
        }

        /// <summary>
        /// Gets the next key value in sequence, and increments the internal enumerator.
        /// </summary>
        /// <remarks>
        /// When the last key is reached, the method will return to the first key value.  Subsequent calls to
        /// the method will return the keys in sequential order.
        /// </remarks>
        public byte[] NextKeyValue()
        {
            return Next();
        }

        /// <summary>
        /// Resets the internal key enumerator to the 1st key.
        /// </summary>
        public void Reset()
        {
            _currentIndex = -1;
        }
        #endregion

        #region Public Static Methods / Functions		
        /// <summary>
        /// Creates an AES Key table with randomly generated key and IV pairs.
        /// </summary>
        /// <returns>
        /// An <see cref="AesKeyTable"/> instance containing randomly generated key data.
        /// </returns>
        public static AesKeyTable CreateRandom()
        {
            var keyData = new byte[SixKeySize];
            var provider = new AesProvider();

            // Generate and store 6 key-IV pairs into a single array.
            var pos = 0;
            for (var count = 0; count < ListLength; count++)
            {
                provider.GenerateNewKey();
                var data = provider.GetKeyData();
                Array.Copy(data!, 0, keyData, pos, KeyIVSize);
                pos += KeyIVSize;
                CryptoUtil.SecureClear(data);
            }

            provider.Dispose();

            // Create the key table.
            var table = new AesKeyTable(keyData);
            CryptoUtil.SecureClear(keyData);
            return table;
        }
        #endregion

        #region Private Methods / Functions		
        /// <summary>
        /// Reads the key value at the specified index.
        /// </summary>
        /// <param name="index">
        /// An integer specifying the zero-based index of the key data to be retrieved.
        /// </param>
        /// <returns>
        /// A byte array of 48 elements.
        /// </returns>
        private byte[] ReadKeyByIndex(int index)
        {
            byte[]? keyData = null;
            if (_keyData != null && _keyData[index] != null)
            {
                keyData = _keyData[index].ConcatenateKeyIVData();
            }

            // If the key data is null, return an empty array.
            keyData ??= new byte[KeyIVSize];

            return keyData!;
        }

        /// <summary>
        /// Returns a copy of the content of the original data, if present, or returns an empty array.
        /// </summary>
        /// <param name="originalData">
        /// The reference to the original byte array whose contents are to be returned.
        /// </param>
        /// <param name="expectedSize">
        /// An integer specifying the expected size of the returned array.
        /// </param>
        /// <returns>
        /// A copy of the original byte array, if not null; otherwise, returns an empty array of the expected size.
        /// </returns>
        private static byte[] EmptyIfNull(byte[]? originalData, int expectedSize)
        {
            var data = new byte[expectedSize];

            if (originalData != null)
            {
                Array.Copy(originalData, 0, data, 0, expectedSize);
            }

            return data;
        }

        #endregion
    }
}