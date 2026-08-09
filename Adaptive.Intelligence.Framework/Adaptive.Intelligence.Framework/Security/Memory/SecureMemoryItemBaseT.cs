using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Abstractions.Logging;
using System.Security.Cryptography;

namespace Adaptive.Intelligence.Security.Memory
{
    /// <summary>
    /// Provides the base implementation for storing data items in memory securely.
    /// </summary>
    /// <typeparam name="T">
    /// The data type of the content being stored.
    /// </typeparam>
    /// <seealso cref="DisposableObjectBase" />
    public abstract class SecureMemoryItemBase<T> : LoggableBase
    {
        #region Private Member Declarations		
        /// <summary>
        /// The default number of random number generator key iterations.
        /// </summary>
        private const int DefaultKeyIterations = 2048;

        /// <summary>
        /// Thread synchronization object for this instance.
        /// </summary>
        private static readonly Lock _syncRoot = new();

        /// <summary>
        /// The number of iterations to use when generating the private key data.
        /// </summary>
        private int _iterations = DefaultKeyIterations;

        /// <summary>
        /// The AES cryptographic instance.
        /// </summary>
        private Aes? _aes;

        /// <summary>
        /// The encryptor transformation instance to use.
        /// </summary>
        private ICryptoTransform? _encryptor;

        /// <summary>
        /// The decryptor transformation instance to use.
        /// </summary>
        private ICryptoTransform? _decryptor;

        /// <summary>
        /// The stored content.
        /// </summary>
        private byte[]? _storage;

        /// <summary>
        /// The size of the data storage content.
        /// </summary>
        private int _storageLength = -1;

        private int _originalLength = -1;
        #endregion

        #region Constructor / Dispose Methods		
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureMemoryItemBase{T}"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        protected SecureMemoryItemBase()
        {
            Initialize();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureMemoryItemBase{T}"/> class.
        /// </summary>
        /// <param name="iterations">
        /// The number of key generation iterations to execute.
        /// </param>
        protected SecureMemoryItemBase(int iterations)
        {
            _iterations = iterations;
            Initialize();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureMemoryItemBase{T}"/> class.
        /// </summary>
        /// <param name="value">
        /// The value to be securely stored in memory.
        /// </param>
        protected SecureMemoryItemBase(T value)
        {
            Initialize();
            Value = value;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SecureMemoryItemBase{T}"/> class.
        /// </summary>
        /// <param name="iterations">
        /// The number of key generation iterations to execute.
        /// </param>
        /// <param name="value">
        /// The value to be securely stored in memory.
        /// </param>
        protected SecureMemoryItemBase(int iterations, T value)
        {
            _iterations = iterations;
            Initialize();
            Value = value;
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            // Wipe the memory storage.
            ClearStorage();

            // Dispose.
            _encryptor?.Dispose();
            _decryptor?.Dispose();
            _aes?.Dispose();

            _aes = null;
            _encryptor = null;
            _decryptor = null;
            _storage = null;
            _storageLength = -1;
            _originalLength = -1;
            _iterations = 0;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties		
        /// <summary>
        /// Gets a value indicating whether this instance represents a <b>null</b> value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance represents a <b>null</b> value; otherwise, <c>false</c>.
        /// </value>
        public bool IsNull => _storageLength < 1;

        /// <summary>
        /// Gets the size of the original data, in bytes.
        /// </summary>
        /// <value>
        /// An intege specifying the length of the original data, in bytes, or -1 if no data is currently stored.
        /// </value>
        public int Length => _originalLength;

        /// <summary>
        /// Gets or sets the value being stored in memory.
        /// </summary>
        /// <value>
        /// The <typeparamref name="T"/> value currently stored in memory, or the default value if nothing is stored.
        /// </value>
        public T? Value
        {
            get
            {
                T? clearValue = default;
                try
                {
                    clearValue = TranslateValue();
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
                return clearValue!;
            }
            set
            {
                ClearStorage();

                try
                {
                    SetValue(value);
                }
                catch (Exception ex)
                {
                    LogError(ex);
                }
            }
        }
        #endregion

        #region Protected Abstract Methods / Functions		
        /// <summary>
        /// Translates provided byte array into a value of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="content">
        /// A byte array containing the binary representation of the value.
        /// </param>
        /// <returns></returns>
        protected abstract T? TranslateValueFromBytes(byte[]? content);
        /// <summary>
        /// Translates the value to a byte array.
        /// </summary>
        /// <param name="value">
        /// The <typeparamref name="T"/> value to be translated.
        /// </param>
        /// <returns>
        /// A byte array representing the binary representation of the specified value.
        /// </returns>
        protected abstract byte[]? TranslateValueToBytes(T? value);
        #endregion

        #region Public Methods / Functions		
        /// <summary>
        /// Clears and removes the byte array acting as the storage for the memory item.
        /// </summary>
        public void ClearStorage()
        {
            if (_storage != null && _storageLength > 0)
            {
                Array.Clear(_storage, 0, _storageLength);
                _storage = null;
                _storageLength = -1;
            }
        }
        #endregion

        #region Private Methods / Functions		
        /// <summary>
        /// Initializes this instance for use.
        /// </summary>
        private void Initialize()
        {
            byte[]? randomizedPassword = new byte[256];
            byte[]? salt = new byte[64];

            // Generate a random list of password values and the salt.
            RandomNumberGenerator? rng = RandomNumberGenerator.Create();

            try
            {
                // Get 256 bytes as a random password with a randomized salt value.
                rng.GetBytes(randomizedPassword, 0, 256);
                rng.GetNonZeroBytes(salt);
            }
            catch (Exception ex)
            {
                LogError(ex);
                randomizedPassword = null;
                salt = null;
            }
            rng.Dispose();

            if (randomizedPassword != null && salt != null)
            {
                // Derive the instance's cryptographic keys from the randomly generated values.
                byte[] generatedKeyData =
                    Rfc2898DeriveBytes.Pbkdf2(
                        randomizedPassword,
                        salt,
                        _iterations,
                        HashAlgorithmName.SHA512,
                        48);

                byte[] key = new byte[32];
                byte[] iv = new byte[16];
                Array.Copy(generatedKeyData, 0, key, 0, 32);
                Array.Copy(generatedKeyData, 32, iv, 0, 16);

                // Create the cryptographic engine.
                _aes = Aes.Create();
                _aes.Key = key;
                _aes.IV = iv;

                _encryptor = _aes.CreateEncryptor();
                _decryptor = _aes.CreateDecryptor();

                // Clear memory.
                Array.Clear(key, 0, key.Length);
                Array.Clear(iv, 0, iv.Length);
                Array.Clear(randomizedPassword, 0, randomizedPassword.Length);
                Array.Clear(salt, 0, salt.Length);
            }
        }
        /// <summary>
        /// Reads and returns the value from encrypted storage.
        /// </summary>
        /// <returns>
        /// A byte array containing the clear representation of the value in memory.
        /// </returns>
        private byte[]? ReadFromStorage()
        {
            byte[]? returnData = null;

            lock (_syncRoot)
            {
                if (_storage != null && _storageLength > 0 && _decryptor != null && _aes != null)
                {
                    // Create the stream and reader object(s).
                    MemoryStream sourceStream = new(_storage);
                    CryptoStream decryptionStream = new(sourceStream, _decryptor, CryptoStreamMode.Read);
                    BinaryReader reader = new(decryptionStream);

                    // Try to decrypt.
                    byte[]? interimData;

                    try
                    {
                        int length = (int)sourceStream.Length;
                        interimData = reader.ReadBytes(length);
                    }
                    catch (Exception ex)
                    {
                        LogError(ex);
                        interimData = null;
                    }

                    if (interimData != null)
                    {
                        // De-splice the bits for an added bit of fun.
                        returnData = BitSplicer.UnSpliceBits(interimData);
                        Array.Clear(interimData, 0, interimData.Length);
                    }

                    // Dispose and clear.
                    reader.Dispose();
                    decryptionStream.Dispose();
                    sourceStream.Dispose();

                }
            }
            return returnData;
        }
        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">
        /// Translates the provided value into a byte array, and then encrypts the content and stores in memory.
        /// </param>
        private void SetValue(T? value)
        {
            // Remove any old data.
            ClearStorage();

            // Translate the value to a byte array.
            byte[]? data = TranslateValueToBytes(value);
            if (data != null)
            {
                _originalLength = data.Length;

                if (data != null)
                {
                    WriteToStorage(data);
                }
            }
        }
        /// <summary>
        /// Translates the content currently being securely stored to a value.
        /// </summary>
        /// <returns>
        /// A value of <typeparamref name="T"/> being stored securely in memory, or the default if the operation
        /// fails.
        /// </returns>
        private T TranslateValue()
        {
            T? returnValue = default;

            byte[]? content = ReadFromStorage();
            if (content != null)
            {
                returnValue = TranslateValueFromBytes(content);
            }

            return returnValue!;
        }
        /// <summary>
        /// Writes the provided byte array to local storage.
        /// </summary>
        /// <param name="dataContentToSecure">The data content to secure.</param>
        private void WriteToStorage(byte[]? dataContentToSecure)
        {
            if (dataContentToSecure != null && _encryptor != null && _aes != null)
            {
                // Splice the bits for an added bit of fun.
                byte[]? contentToEncrypt = BitSplicer.SpliceBits(dataContentToSecure);
                if (contentToEncrypt != null)
                {
                    // Create the stream and writer object(s).
                    MemoryStream destinationStream = new(contentToEncrypt.Length * 2);
                    CryptoStream encryptionStream = new(destinationStream, _encryptor, CryptoStreamMode.Write);
                    BinaryWriter writer = new(encryptionStream);

                    try
                    {
                        // Attempt to write all data to the stream.
                        writer.Write(contentToEncrypt);
                        writer.Flush();
                        encryptionStream.Flush();
                        encryptionStream.FlushFinalBlock();

                        // If successful, store the encrypted content in local memory.
                        _storage = destinationStream.ToArray();
                        _storageLength = _storage.Length;

                    }
                    catch (Exception ex)
                    {
                        LogError(ex);
                        _storage = null;
                        _storageLength = -1;
                    }

                    // Dispose.
                    writer.Dispose();
                    encryptionStream.Dispose();
                    destinationStream.Dispose();
                    Array.Clear(contentToEncrypt, 0, contentToEncrypt.Length);
                }
            }
        }
        #endregion
    }
}
