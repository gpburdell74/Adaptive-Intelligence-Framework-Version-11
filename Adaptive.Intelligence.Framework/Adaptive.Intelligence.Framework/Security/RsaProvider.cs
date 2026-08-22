using Adaptive.Intelligence.Abstractions.Logging;
using System.Security.Cryptography;

namespace Adaptive.Intelligence.Security
{
    /// <summary>
    /// Provides a utility class for performing RSA cryptographic operations.
    /// </summary>
    /// <seealso cref="LoggableBase" />
    /// <seealso cref="RSA"/>
    /// <seealso cref="RSAParameters"/>
    public sealed class RsaProvider : LoggableBase
    {
        #region Private Member Declarations
        /// <summary>
        /// The RSA provider instance to use.
        /// </summary>
        private RSA? _provider;

        /// <summary>
        /// The RSA parameters instance containing the current key data.
        /// </summary>
        private RSAParameters? _currentKey;

        /// <summary>
        /// RSA Key size.
        /// </summary>
        private readonly int _keySize = 3072;
        #endregion

        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="RsaProvider"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public RsaProvider()
        {
            _provider = RSA.Create();
            _provider.KeySize = _keySize;
            _currentKey = _provider.ExportParameters(true);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RsaProvider"/> class with the specified key size.
        /// </summary>
        /// <param name="keySize">
        /// An integer specifying the size of the key to use.
        /// </param>
        public RsaProvider(int keySize)
        {
            _provider = RSA.Create();
            _keySize = keySize;
            _provider.KeySize = _keySize;
            _currentKey = _provider.ExportParameters(true);

        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                ClearKeyMemory();
                _provider?.Dispose();
            }

            _provider = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Cryptographic Methods / Functions
        /// <summary>
        /// Attempts to decrypt the provided data.
        /// </summary>
        /// <remarks>
        /// The key data must be imported into the current instance before this operation
        /// is used.  This method assumes OAEP padding is used.
        /// </remarks>
        /// <param name="encryptedData">
        /// A byte array containing the encrypted data.
        /// </param>
        /// <returns>
        /// A byte array containing the decrypted data, if successful; otherwise, returns
        /// <b>null</b>.
        /// </returns>
        public byte[]? Decrypt(byte[]? encryptedData)
        {
            byte[]? result = null;

            if (encryptedData != null && encryptedData.Length > 0 && _provider != null)
            {
                try
                {
                    result = _provider.Decrypt(encryptedData, RSAEncryptionPadding.OaepSHA512);
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            }

            return result;
        }
        /// <summary>
        /// Attempts to decrypt the provided data.
        /// </summary>
        /// <remarks>
        /// The key data must be imported into the current instance before this operation
        /// is used.  This method assumes OAEP padding is used.
        /// </remarks>
        /// <param name="encryptedData">
        /// A base-64 encoded string representing the byte array containing the encrypted data.
        /// </param>
        /// <returns>
        /// A byte array containing the decrypted data, if successful; otherwise, returns
        /// <b>null</b>.
        /// </returns>
        public byte[]? DecryptFromBase64String(string? encryptedData)
        {
            if (!string.IsNullOrEmpty(encryptedData))
            {
                byte[]? encryptedBytes;
                try
                {
                    encryptedBytes = Convert.FromBase64String(encryptedData);
                }
                catch (Exception ex)
                {
                    LogError(ex);
                    encryptedBytes = null;
                }

                if (encryptedBytes != null)
                {
                    return Decrypt(encryptedBytes);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Attempts to encrypt the provided data.
        /// </summary>
        /// <remarks>
        /// This method assumes OAEP padding is used.
        /// </remarks>
        /// <param name="clearData">
        /// A byte array containing the clear data.
        /// </param>
        /// <returns>
        /// A byte array containing the encrypted data, if successful; otherwise, returns
        /// <b>null</b>.
        /// </returns>
        public byte[]? Encrypt(byte[]? clearData)
        {
            byte[]? result = null;

            if (clearData != null && clearData.Length > 0 && _provider != null)
            {
                try
                {
                    result = _provider.Encrypt(clearData, RSAEncryptionPadding.OaepSHA512);
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            }

            return result;
        }
        #endregion

        #region Public Key-Related Methods / Functions
        /// <summary>
        /// Gets the RSA public key value for exporting to another client or consumer.
        /// </summary>
        /// <returns>
        /// A string containing the base-64 encoding byte array that contains the RSA public key
        /// for use by another client/user to encrypt data.
        /// </returns>
        public string? GetKeyValueForExport()
        {
            if (_provider == null)
            {
                return null;
            }

            try
            {
                byte[] spki = _provider.ExportSubjectPublicKeyInfo();
                return Convert.ToBase64String(spki);
            }
            catch (Exception ex)
            {
                LogError(ex, nameof(GetKeyValueForExport));
                return null;
            }
        }
        /// <summary>
        /// Gets the RSA public key value for exporting to another client or consumer.
        /// </summary>
        /// <returns>
        /// A byte array that contains the RSA public key for use by another client/user to encrypt data.
        /// </returns>
        public byte[]? GetKeyValueForExportAsByteArray()
        {
            if (_provider == null)
            {
                return null;
            }

            try
            {
                return _provider.ExportSubjectPublicKeyInfo();
            }
            catch (Exception ex)
            {
                LogError(ex, nameof(GetKeyValueForExportAsByteArray));
                return null;
            }
        }
        /// <summary>
        /// Gets the RSA private key value for storage and later re-importing.
        /// </summary>
        /// <returns>
        /// A string containing the base-64 encoding byte array that contains the RSA private key
        /// data.  This is to be imported at a later date to decrypt data that was encrypted
        /// with the associated public key.
        /// </returns>
        public string? GetPrivateKeyValueForStorage()
        {
            string? serialized = null;

            byte[]? data = SerializePrivateKey();
            if (data != null)
            {
                serialized = Convert.ToBase64String(data);
                Array.Clear(data, 0, data.Length);
            }
            return serialized;
        }
        /// <summary>
        /// Imports the public key from another provider as represented in the
        /// provided string data.
        /// </summary>
        /// <param name="keyData">
        /// A base-64 encoded string containing the concatenated modulus and exponent
        /// byte array(s).
        /// </param>
        public void ImportPublicKeyFromBase64String(string? keyData)
        {
            if (_provider == null || string.IsNullOrWhiteSpace(keyData))
            {
                return;
            }

            byte[]? spki = null;
            try
            {
                spki = Convert.FromBase64String(keyData);
                _provider.ImportSubjectPublicKeyInfo(spki, out int bytesRead);

                if (bytesRead != spki.Length)
                {
                    throw new CryptographicException("Invalid SubjectPublicKeyInfo payload.");
                }
                _currentKey = _provider.ExportParameters(false);
            }
            catch (Exception ex)
            {
                LogError(ex, nameof(ImportPublicKeyFromBase64String));
            }
            finally
            {
                if (spki != null)
                {
                    Array.Clear(spki, 0, spki.Length);
                }
            }
        }
        /// <summary>
        /// Imports the public key from another provider.
        /// </summary>
        /// <param name="modulus">
        /// A byte array containing the modulus data.
        /// </param>
        /// <param name="exponent">
        /// A byte array containing the exponent data.
        /// </param>
        public void ImportPublicKey(byte[]? modulus, byte[]? exponent)
        {
            if (modulus != null && exponent != null)
            {
                ClearKeyMemory();

                RSAParameters rsaParams = new()
                {
                    Modulus = modulus,
                    Exponent = exponent
                };

                _provider?.ImportParameters(rsaParams);
                _currentKey = rsaParams;
            }
        }

        /// <summary>
        /// Sets the RSA private key value from the provided data.
        /// </summary>
        /// <param name="keyData">
        /// A base-64 encoded string representing the byte array containing the concatenation of
        /// all the fields on the internal <see cref="RSAParameters"/> instance containing the key data.
        /// </param>
        public void SetPrivateKeyFromBase64String(string keyData)
        {
            byte[]? content = null;

            try
            {
                content = Convert.FromBase64String(keyData);
            }
            catch (ArgumentNullException ex)
            {
                LogError(ex, nameof(SetPrivateKeyFromBase64String));
            }
            catch (FormatException formatEx)
            {
                LogError(formatEx, nameof(SetPrivateKeyFromBase64String));
            }

            if (content != null)
            {
                SetPrivateKey(content);
                Array.Clear(content, 0, content.Length);
            }
        }
        /// <summary>
        /// Serializes the private key data into a single byte array.
        /// </summary>
        /// <returns>
        /// A byte array containing the concatenation of all the fields on the
        /// internal <see cref="RSAParameters"/> instance containing the key data.
        /// </returns>
        public byte[]? SerializePrivateKey()
        {
            if (_provider is null)
                return null;

            try
            {
                return _provider.ExportRSAPrivateKey();
            }
            catch (Exception ex)
            {
                LogError(ex, nameof(SerializePrivateKey));
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyData"></param>
        /// <exception cref="CryptographicException"></exception>
        public void SetPrivateKey(byte[] keyData)
        {
            if (_provider is null || keyData is null || keyData.Length == 0)
                return;

            ClearKeyMemory();

            try
            {
                _provider.ImportRSAPrivateKey(keyData, out int bytesRead);
                if (bytesRead != keyData.Length)
                    throw new CryptographicException("Invalid RSA private key payload.");

                _currentKey = _provider.ExportParameters(true);
            }
            catch (Exception ex)
            {
                LogError(ex, nameof(SetPrivateKey));
            }
        }
        #endregion

        #region Private Methods / Functions
        /// <summary>
        /// Clears the arrays in the encryption key parameter container.
        /// </summary>
        private void ClearKeyMemory()
        {
            if (_currentKey != null && _currentKey.HasValue)
            {
                if (_currentKey.Value.D != null)
                {
                    Array.Clear(_currentKey.Value.D, 0, _currentKey.Value.D.Length);
                }

                if (_currentKey.Value.DP != null)
                {
                    Array.Clear(_currentKey.Value.DP, 0, _currentKey.Value.DP.Length);
                }

                if (_currentKey.Value.DQ != null)
                {
                    Array.Clear(_currentKey.Value.DQ, 0, _currentKey.Value.DQ.Length);
                }

                if (_currentKey.Value.Exponent != null)
                {
                    Array.Clear(_currentKey.Value.Exponent, 0, _currentKey.Value.Exponent.Length);
                }

                if (_currentKey.Value.InverseQ != null)
                {
                    Array.Clear(_currentKey.Value.InverseQ, 0, _currentKey.Value.InverseQ.Length);
                }

                if (_currentKey.Value.Modulus != null)
                {
                    Array.Clear(_currentKey.Value.Modulus, 0, _currentKey.Value.Modulus.Length);
                }

                if (_currentKey.Value.P != null)
                {
                    Array.Clear(_currentKey.Value.P, 0, _currentKey.Value.P.Length);
                }

                if (_currentKey.Value.Q != null)
                {
                    Array.Clear(_currentKey.Value.Q, 0, _currentKey.Value.Q.Length);
                }
            }
        }
        #endregion
    }
}