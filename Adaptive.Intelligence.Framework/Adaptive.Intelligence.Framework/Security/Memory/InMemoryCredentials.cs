using Adaptive.Intelligence.Abstractions;
using System.Text;

namespace Adaptive.Intelligence.Security.Memory
{
    /// <summary>
    /// Represents and manages a set of user and password credentials in memory.
    /// </summary>
    /// <seealso cref="DisposableObjectBase" />
    public sealed class InMemoryCredentials : DisposableObjectBase, ICloneable
    {
        #region Private Member Declarations
        /// <summary>
        /// The user identifier stored as a secure string.
        /// </summary>
        private SecureByteArray? _userId;
        /// <summary>
        /// The password value stored as a secure string.
        /// </summary>
        private SecureByteArray? _password;
        /// <summary>
        /// The pin value stored as a secure integer.
        /// </summary>
        private SecureInt32? _pinValue;

        /// <summary>
        /// The primary key and IV values.
        /// </summary>
        private SecureByteArray? _primaryKey;
        private SecureByteArray? _primaryIV;

        /// <summary>
        /// The secondary key and IV values.
        /// </summary>
        private SecureByteArray? _secondaryKey;
        private SecureByteArray? _secondaryIV;

        /// <summary>
        /// The tertiary key and IV values.
        /// </summary>
        private SecureByteArray? _tertiaryKey;
        private SecureByteArray? _tertiaryIV;
        #endregion

        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCredentials"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public InMemoryCredentials()
        {
            GenerateKeyData();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCredentials"/> class.
        /// </summary>
        /// <param name="userId">
        /// A string containing the user ID value.
        /// </param>
        /// <param name="password">
        /// A string containing the password value.
        /// </param>
        public InMemoryCredentials(string? userId, string? password)
        {
            GenerateKeyData();
            _userId = EncodeString(userId);
            _password = EncodeString(password);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryCredentials"/> class.
        /// </summary>
        /// <param name="userId">
        /// A string containing the user ID value.
        /// </param>
        /// <param name="password">
        /// A string containing the password value.
        /// </param>
        /// <param name="pinValue">
        /// An integer containing a personal identification number.
        /// </param>
        public InMemoryCredentials(string? userId, string? password, int? pinValue)
        {
            GenerateKeyData();
            _userId = EncodeString(userId);
            _password = EncodeString(password);
            if (pinValue != null)
            {
                _pinValue = new SecureInt32
                {
                    Value = pinValue.Value
                };
#pragma warning disable IDE0059 // Unnecessary assignment of a value
                pinValue = 0;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            }
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                _userId?.Dispose();
                _password?.Dispose();
                _primaryKey?.Dispose();
                _primaryKey?.Dispose();
                _secondaryKey?.Dispose();
                _secondaryIV?.Dispose();
                _tertiaryKey?.Dispose();
                _tertiaryIV?.Dispose();
            }

            _userId = null;
            _password = null;
            _primaryKey = null;
            _primaryIV = null;
            _secondaryKey = null;
            _secondaryIV = null;
            _tertiaryKey = null;
            _tertiaryIV = null;

            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the user ID value.
        /// </summary>
        /// <value>
        /// A string containing the user ID value.
        /// </value>
        public string? UserId
        {
            get => DecodeString(_userId);
            set => _userId = EncodeString(value);
        }
        /// <summary>
        /// Gets or sets the password value.
        /// </summary>
        /// <value>
        /// A string containing the password value.
        /// </value>
        public string? Password
        {
            get => DecodeString(_password);
            set => _password = EncodeString(value);
        }


        /// <summary>
        /// Gets or sets the PIN value being stored.
        /// </summary>
        /// <value>
        /// The integer containing the user PIN value, or <b>null</b> if not used.
        /// </value>
        public int? PIN
        {
            get
            {
                if (_pinValue == null)
                {
                    return null;
                }
                else
                {
                    return _pinValue.Value;
                }
            }
            set
            {
                _pinValue?.Dispose();
                _pinValue = null;
                if (value != null)
                {
                    _pinValue = new SecureInt32(value.Value);
                }

            }
        }
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new <see cref="InMemoryCredentials"/> instance that is a copy of this instance.
        /// </returns>
        public InMemoryCredentials Clone()
        {
            InMemoryCredentials credentials;

            if (_pinValue != null)
            {
                credentials = new InMemoryCredentials(
                    DecodeString(_userId),
                    DecodeString(_password),
                    _pinValue.Value);
            }
            else
            {
                credentials = new InMemoryCredentials(
                    DecodeString(_userId),
                    DecodeString(_password));
            }
            return credentials;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// Generates AES key data from the user's login and and password values, and stores them in memory.
        /// </summary>
        private void GenerateKeyData()
        {
            // This has to remain the same in order to generate the same values for the same password each time.
            byte[] saltValuePrimary = [228, 128, 2, 47, 90, 89, 212, 244];
            byte[] saltValueSecondary = [128, 128, 95, 02, 90, 19, 212, 176];
            byte[] saltValueTertiary = [32, 128, 22, 47, 90, 89, 212, 084];

            byte[]? primaryKey = KeyGenerator.CreateKeyData(saltValuePrimary);
            byte[]? primaryIV = KeyGenerator.CreateKeyData(saltValuePrimary);
            byte[]? secondaryKey = KeyGenerator.CreateKeyData(saltValueSecondary);
            byte[]? secondaryIV = KeyGenerator.CreateKeyData(saltValueSecondary);
            byte[]? tertiaryKey = KeyGenerator.CreateKeyData(saltValueTertiary);
            byte[]? tertiaryIV = KeyGenerator.CreateKeyData(saltValueTertiary);

            if (primaryKey != null && primaryIV != null)
            {
                _primaryKey = new SecureByteArray(primaryKey);
                _primaryIV = new SecureByteArray(primaryIV);
                Array.Clear(primaryKey, 0, primaryKey.Length);
                Array.Clear(primaryIV, 0, primaryIV.Length);
            }

            if (secondaryKey != null && secondaryIV != null)
            {
                _secondaryKey = new SecureByteArray(secondaryKey);
                _secondaryIV = new SecureByteArray(secondaryIV);
                Array.Clear(secondaryKey, 0, secondaryKey.Length);
                Array.Clear(secondaryIV, 0, secondaryIV.Length);
            }

            if (tertiaryKey != null && tertiaryIV != null)
            {
                _tertiaryKey = new SecureByteArray(tertiaryKey);
                _tertiaryIV = new SecureByteArray(tertiaryIV);
                Array.Clear(tertiaryKey, 0, tertiaryKey.Length);
                Array.Clear(tertiaryIV, 0, tertiaryIV.Length);
            }

            Array.Clear(saltValuePrimary, 0, saltValuePrimary.Length);
            Array.Clear(saltValueSecondary, 0, saltValueSecondary.Length);
            Array.Clear(saltValueTertiary, 0, saltValueTertiary.Length);

        }
        #endregion

        #region Private Methods / Functions
        /// <summary>
        /// Encodes the string as a byte array.
        /// </summary>
        /// <param name="original">
        /// A string containing the original data.
        /// </param>
        /// <returns>
        /// A byte array representing the string, or <b>null.</b>
        /// </returns>
        private SecureByteArray? EncodeString(string? original)
        {
            SecureByteArray? result = null;

            if (!string.IsNullOrEmpty(original))
            {
                byte[]? data = Encoding.ASCII.GetBytes(original);
                if (data != null)
                {
                    AesProvider provider = new();
                    provider.SetKeyIV(_primaryKey?.Value, _primaryIV?.Value);
                    byte[]? first = provider.Encrypt(data);

                    provider.SetKeyIV(_secondaryKey?.Value, _secondaryIV?.Value);
                    byte[]? second = provider.Encrypt(first);

                    provider.SetKeyIV(_tertiaryKey?.Value, _tertiaryIV?.Value);
                    byte[]? third = provider.Encrypt(second);
                    provider.Dispose();

                    if (third != null)
                    {
                        result = new SecureByteArray(third);
                        Array.Clear(third, 0, third.Length);
                    }
                    if (first != null)
                    {
                        Array.Clear(first, 0, first.Length);
                    }

                    if (second != null)
                    {
                        Array.Clear(second, 0, second.Length);
                    }
                }

            }
            return result;
        }
        /// <summary>
        /// Decodes the byte array into a string.
        /// </summary>
        /// <param name="data">
        /// A byte array containing the text data, or <b>null</b>.
        /// </param>
        /// <returns>
        /// A string containing the data or <see cref="string.Empty"/> if the data is <b>null</b>.
        /// </returns>
        private string? DecodeString(SecureByteArray? data)
        {
            string? clearValue = null;

            if (data != null)
            {
                byte[]? content = data.Value;
                if (content == null || content.Length == 0)
                {
                    clearValue = string.Empty;
                }
                else
                {
                    AesProvider provider = new();
                    byte[]? encrypted = data.Value;

                    provider.SetKeyIV(_tertiaryKey?.Value, _tertiaryIV?.Value);
                    byte[]? third = provider.Decrypt(encrypted);

                    provider.SetKeyIV(_secondaryKey?.Value, _secondaryIV?.Value);
                    byte[]? second = provider.Decrypt(third);

                    provider.SetKeyIV(_primaryKey?.Value, _primaryIV?.Value);
                    byte[]? first = provider.Decrypt(second);
                    provider.Dispose();

                    if (first != null)
                    {
                        clearValue = Encoding.ASCII.GetString(first);
                        Array.Clear(first, 0, first.Length);
                    }

                    Array.Clear(content, 0, content.Length);

                    if (second != null)
                    {
                        Array.Clear(second, 0, second.Length);
                    }

                    if (third != null)
                    {
                        Array.Clear(third, 0, third.Length);
                    }

                    if (encrypted != null)
                    {
                        Array.Clear(encrypted, 0, encrypted.Length);
                    }
                }
            }
            return clearValue;
        }
        #endregion
    }
}
