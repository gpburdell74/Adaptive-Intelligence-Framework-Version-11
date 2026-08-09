using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Security.Memory;

namespace Adaptive.Intelligence.Security
{
    /// <summary>
    /// Provides a simple storage mechanism for storing AES Key and Initialization Vector
    /// information.
    /// </summary>
    /// <seealso cref="DisposableObjectBase" />
    public sealed class AesKeyStore : DisposableObjectBase, ISymmetricKeyStore
    {
        #region Private Member Declarations

        /// <summary>
        /// The AES key length.
        /// </summary>
        private const int KeyLength = 32;

        /// <summary>
        /// The IV key length.
        /// </summary>
        private const int IVLength = 16;

        /// <summary>
        /// The total length of the key and IV values.
        /// </summary>
        private const int TotalKeyIVLength = 48;

        /// <summary>
        /// The total length of the key and IV values when encoded as a base-64 string.
        /// </summary>
        private const int TotalKeyIVInBase64Length = 64;

        /// <summary>
        /// The 32-byte key data.
        /// </summary>
        private SecureByteArray? _key;

        /// <summary>
        /// The 16-byte initialization vector.
        /// </summary>
        private SecureByteArray? _iv;
        #endregion

        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="AesKeyStore"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public AesKeyStore()
        {
            _key = new SecureByteArray();
            _iv = new SecureByteArray();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AesKeyStore"/> class.
        /// </summary>
        /// <param name="key">
        /// A 32-element byte array containing the AES key data.
        /// </param>
        /// <param name="iv">
        /// A 16-element byte array containing the AES initialization data.
        /// </param>
        public AesKeyStore(byte[] key, byte[] iv)
        {
            _key = new SecureByteArray(key);
            _iv = new SecureByteArray(iv);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AesKeyStore"/> class.
        /// </summary>
        /// <param name="keyData">
        /// A 48-element byte array containing the AES key data followed by the AES
        /// initialization vector data.
        /// </param>
        public AesKeyStore(byte[] keyData)
        {
            // Assuming a 48-byte (or longer) encryption key byte array where the first 32 bytes are
            // the AES encryption key, and the next 16 bytes are the AES initialization vector (IV).
            if (keyData.Length >= TotalKeyIVLength)
            {
                SetKeyIVData(keyData);
            }
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            ClearKeyMemory();
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the AES initialization vector data.
        /// </summary>
        /// <value>
        /// A 16-element byte array containing the AES initialization vector data.
        /// </value>
        public byte[]? IV
        {
            get
            {
                if (_iv == null || _iv.IsNull)
                {
                    return null;
                }
                else
                {
                    return _iv.Value;
                }
            }
            set
            {
                if (value == null || value.Length != IVLength)
                {
                    _iv?.Dispose();
                    _iv = null;
                }
                else
                {
                    if (_iv == null)
                    {
                        _iv = new SecureByteArray(value);
                    }
                    else
                    {
                        _iv.Value = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of the byte array that contains the initialization vector data.
        /// </summary>
        /// <remarks>
        /// The IV size for the AES algorithm is fixed at 16 bytes (128 bits), so this property will always return 16.
        /// </remarks>
        /// <value>
        /// An integer specifying the size of the byte array that contains the initialization vector data,
        /// if needed for the algorithm; otherwise, zero (0).
        /// </value>
        public int IVSize => IVLength;

        /// <summary>
        /// Gets or sets the AES key data.
        /// </summary>
        /// <value>
        /// A 32-element byte array containing the AES key data.
        /// </value>
        public byte[]? Key
        {
            get
            {
                if (_key == null || _key.IsNull)
                {
                    return null;
                }
                else
                {
                    return _key.Value;
                }
            }
            set
            {
                if (value == null || value.Length != 32)
                {
                    _key?.Dispose();
                    _key = null;
                }
                else
                {
                    if (_key == null)
                    {
                        _key = new SecureByteArray(value);
                    }
                    else
                    {
                        _key.Value = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the size of the byte array that contains the key data.
        /// </summary>
        /// <remarks>
        /// The key size for the AES algorithm is fixed at 32 bytes (256 bits), so this property will always return 32.
        /// </remarks>
        /// <value>
        /// An integer specifying the size of the byte array that contains the key data.
        /// </value>
        public int KeySize => KeyLength;

        /// <summary>
        /// Gets or sets the secondary cryptographic key data.
        /// </summary>
        /// <remarks>
        /// A secondary key value is not used for this algorithm.
        /// </remarks>
        /// <value>
        /// Always returns <b>null</b>.
        /// </value>
        public byte[]? SecondaryKey { get => null; set => throw new NotImplementedException(); }

        /// <summary>
        /// Gets the size of the byte array that contains the secondary key data.
        /// </summary>
        /// <value>
        /// An integer specifying the size of the byte array that contains the secondary key data, 
        /// if needed for the algorithm; otherwise, zero (0).
        /// </value>
        public int SecondaryKeySize => 0;
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Clears the byte arrays, erasing the key data memory content.
        /// </summary>
        public void ClearKeyMemory()
        {
            _key?.Dispose();
            _iv?.Dispose();

            _key = null;
            _iv = null;
        }
        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>
        /// An <see cref="AesKeyStore"/> instance that is a copy of this instance.
        /// </returns>
        public AesKeyStore Clone()
        {
            if (_iv == null || _key == null || _key.Value == null || _iv.Value == null)
            {
                return new AesKeyStore();
            }
            else
            {
                return new AesKeyStore(_key.Value, _iv.Value);
            }
        }
        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>
        /// An <see cref="AesKeyStore"/> instance that is a copy of this instance.
        /// </returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Gets the concatenation of the key and initialization vector data as a single concatenated byte array.
        /// </summary>
        /// <returns>
        /// A byte array where the first elements contain the key data, including the secondary key data if present,
        /// and the last elements contain the initialization vector data.
        /// </returns>
        public byte[]? ConcatenateKeyIVData()
        {
            byte[]? data = null;

            if (_key != null &&
                !_key.IsNull &&
                _key.Value != null &&
                _iv != null &&
                !_iv.IsNull &&
                _iv.Value != null)
            {
                data = new byte[TotalKeyIVLength];
                Array.Copy(_key.Value, 0, data, 0, KeyLength);
                Array.Copy(_iv.Value, 0, data, KeyLength, IVLength);
            }
            return data;
        }

        /// <summary>
        /// Gets the AES key and IV data as a concatenated byte array.
        /// </summary>
        /// <returns>
        /// A 48-element byte array where
        /// the first 32 elements contain the key data and the last 16 elements
        /// contain the IV data.
        /// </returns>
        [Obsolete("Use the ContactenateKeyIVData method instead.")]
        public byte[]? GetKeyIVData()
        {
            return ConcatenateKeyIVData();
        }

        /// <summary>
        /// Gets the concatenation of the key and initialization vector data as a single concatenated byte
        /// array converted to a base-64 encoded string.
        /// </summary>
        /// <returns>
        /// A base-64 encoded string representing the byte array where the first elements contain the key data, including the secondary key data if present,
        /// and the last elements contain the initialization vector data.
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        public string? ConcatenateKeyIVDataAsBase64String()
        {
            string? content = null;

            if (_key != null && _iv != null)
            {
                byte[]? data = ConcatenateKeyIVData();
                if (data != null)
                {
                    content = Convert.ToBase64String(data);
                }
            }
            return content;
        }

        /// <summary>
        /// Gets the AES key and IV data as a base-64 encoded string.
        /// </summary>
        /// <returns>
        /// A base-64 encoded string representing a 48-element byte array where
        /// the first 32 elements contain the key data and the last 16 elements
        /// contain the IV data.
        /// </returns>
        [Obsolete("Use the ConcatenateKeyIVDataAsBase64String method instead.")]
        public string? GetKeyIVDataAsBase64String()
        {
            return ConcatenateKeyIVDataAsBase64String();
        }
        /// <summary>
        /// Sets the AES Key and IV data from the byte array.
        /// </summary>
        /// <param name="cryptographicData">
        /// A 48-element byte array where
        /// the first 32 elements contain the key data and the last 16 elements
        /// contain the IV data.
        /// </param>
        public void SetKeyIVData(byte[]? cryptographicData)
        {
            // Assuming a 48-byte (or longer) encryption key byte array where the first 32 bytes are
            // the AES encryption key, and the next 16 bytes are the AES initialization vector (IV).
            if (cryptographicData != null && cryptographicData.Length >= TotalKeyIVLength)
            {
                // Read the key data from the original array.
                byte[]? key = new byte[KeyLength];
                Array.Copy(cryptographicData, 0, key, 0, KeyLength);
                _key = new SecureByteArray(key);
                Array.Clear(key, 0, KeyLength);

#pragma warning disable IDE0059 // Unnecessary assignment of a value
                key = null;
#pragma warning restore IDE0059 // Unnecessary assignment of a value

                // Read the IV data from the original array.
                byte[]? iv = new byte[IVLength];
                Array.Copy(cryptographicData, KeyLength, iv, 0, IVLength);
                _iv = new SecureByteArray(iv);
                Array.Clear(iv, 0, IVLength);
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                iv = null;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            }

        }
        /// <summary>
        /// Sets the AES key and IV data from the specified string.
        /// </summary>
        /// <param name="keyData">
        /// A base-64 encoded string representing a 48-element byte array where
        /// the first 32 elements contain the key data and the last 16 elements
        /// contain the IV data.
        /// </param>
        public void SetKeyIVDataFromBase64String(string? keyData)
        {
            if (keyData != null && keyData.Length == TotalKeyIVInBase64Length)
            {
                byte[] content = Convert.FromBase64String(keyData);
                SetKeyIVData(content);
                Array.Clear(content, 0, TotalKeyIVLength);
            }
        }

        /// <summary>
        /// Verifies the provided byte array contains valid initialization vector data of the expected size.
        /// </summary>
        /// <param name="cryptographicData">A byte array containing the cryptographic initialization vector data to verify.</param>
        /// <returns>
        /// <b>true</b> if the data is present and is of the correct length;
        /// otherwise, returns <b>false</b>.
        /// </returns>
        public bool VerifyIVSize(byte[]? cryptographicData)
        {
            return cryptographicData != null && cryptographicData.Length == IVLength;

        }

        /// <summary>
        /// Verifies the provided byte array contains valid cryptographic key data of the expected size.
        /// </summary>
        /// <param name="cryptographicData">A byte array containing the cryptographic key data to verify.</param>
        /// <returns>
        /// <b>true</b> if the data is present and is of the correct length;
        /// otherwise, returns <b>false</b>.
        /// </returns>
        public bool VerifyKeySize(byte[]? cryptographicData)
        {
            return cryptographicData != null && cryptographicData.Length == KeyLength;
        }

        /// <summary>
        /// Verifies the provided byte array contains valid cryptographic secondary key data of the expected size.
        /// </summary>
        /// <param name="cryptographicData">A byte array containing the cryptographic secondary key data to verify.</param>
        /// <returns>
        /// <b>true</b> if the data is present and is of the correct length;
        /// otherwise, returns <b>false</b>.
        /// </returns>
        public bool VerifySecondaryKeySize(byte[]? cryptographicData)
        {
            return cryptographicData == null || cryptographicData.Length == 0;
        }
        #endregion
    }
}
