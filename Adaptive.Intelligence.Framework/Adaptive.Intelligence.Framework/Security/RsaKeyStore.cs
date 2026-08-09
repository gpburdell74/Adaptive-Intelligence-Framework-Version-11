using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Security.Memory;
using System.Runtime.Versioning;
using System.Security.Cryptography;

namespace Adaptive.Intelligence.Security
{
    /// <summary>
    /// Provides a simple storage mechanism for storing AES Key and Initialization Vector
    /// information.
    /// </summary>
    /// <seealso cref="DisposableObjectBase" />
    [SupportedOSPlatform("Windows")]
    public sealed class RsaKeyStore : DisposableObjectBase, ICloneable
    {
        #region Private Member Declarations
        /// <summary>
        /// The byte array containing the original key.
        /// </summary>
        private SecureByteArray? _original;

        /// <summary>
        /// The key instance.
        /// </summary>
        private CngKey? _key;
        #endregion

        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="RsaKeyStore"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public RsaKeyStore()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RsaKeyStore"/> class.
        /// </summary>
        /// <param name="key">
        /// A byte array containing the RSA public key data.
        /// </param>
        [SupportedOSPlatform("Windows")]
        public RsaKeyStore(byte[]? key)
        {
            if (key != null)
            {
                _original = new SecureByteArray(key);
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    _key = CngKey.Import(key, CngKeyBlobFormat.GenericPublicBlob);
                }
                else
                {
                    throw new PlatformNotSupportedException("The underlying next generation cryptography (CNG) API is only supported on Windows platforms.");
                }
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
        /// Gets or sets the reference to the CNG key instance.
        /// </summary>
        /// <value>
        /// The <see cref="CngKey"/> instance.
        /// </value>
        public CngKey? Key => _key;
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Clears the byte arrays, erasing the key data memory content.
        /// </summary>
        public void ClearKeyMemory()
        {
            _key?.Dispose();
            _key = null;

            _original?.Dispose();
            _original = null;
        }

        /// <summary>
        /// Clones the current instance.
        /// </summary>
        /// <returns>
        /// An <see cref="RsaKeyStore"/> that is a copy of the current instance.
        /// </returns>
        public RsaKeyStore? Clone()
        {
            if (_original == null)
            {
                return null;
            }
            else
            {
                return new RsaKeyStore(_original.Value);
            }
        }
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        object ICloneable.Clone()
        {
            return Clone()!;
        }

        /// <summary>
        /// Gets the RSA public key data as a base-64 encoded string.
        /// </summary>
        /// <returns>
        /// A base-64 encoded string representing a byte array containing the RSA public key.
        /// </returns>
        public byte[]? GetKeyData()
        {
            byte[]? keyData = null;
            if (_original != null)
            {
                keyData = _original.Value;
            }
            return keyData;
        }

        /// <summary>
        /// Gets the RSA public key data as a base-64 encoded string.
        /// </summary>
        /// <returns>
        /// A base-64 encoded string representing a byte array containing the RSA public key.
        /// </returns>
        public string? GetKeyDataAsBase64String()
        {
            if (_original == null)
            {
                return null;
            }
            else
            {
                return Convert.ToBase64String(_original.Value!);
            }
        }

        /// <summary>
        /// Sets the RSA public key data.
        /// </summary>
        /// <param name="keyData">
        /// A byte array containing the RSA public key.
        /// </param>
        public void SetKeyData(byte[]? keyData)
        {
            if (keyData != null)
            {
                // Store the byte array.
                _original?.Dispose();
                _original = new SecureByteArray(keyData);

                // Create the key object.
                _key = CngKey.Import(keyData, CngKeyBlobFormat.GenericPublicBlob);
            }
        }

        /// <summary>
        /// Sets the RSA public key data from the specified string.
        /// </summary>
        /// <param name="keyData">
        /// A base-64 encoded string representing a byte array containing the RSA public key.
        /// </param>
        public void SetKeyDataFromBase64String(string? keyData)
        {
            if (keyData != null)
            {
                // Store the byte array.
                byte[] data = Convert.FromBase64String(keyData);
                _original?.Dispose();
                _original = new SecureByteArray(data);

                // Create the key object.
                _key = CngKey.Import(data, CngKeyBlobFormat.GenericPublicBlob);
                Array.Clear(data, 0, data.Length);
            }
        }
        #endregion
    }
}