using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Utility;
using Adaptive.Intelligence.Security.Memory;
using System.Text;

namespace Adaptive.Intelligence.Security
{
    /// <summary>
    /// Performs advanced cryptographic operations on data using user-provided code phrase, password, and PIN data.
    /// </summary>
    /// <seealso cref="ExceptionTrackingBase" />
    /// <remarks>
    /// Initializes a new instance of the <see cref="SuperCrypt"/> class.
    /// </remarks>
    /// <param name="codePhrase">
    /// A string containing the code phrase value.
    /// </param>
    /// <param name="password">
    /// A string containing the password value.
    /// </param>
    /// <param name="pin">
    /// An integer specifying the pin value.
    /// </param>
    public sealed class SuperCrypt(string codePhrase, string password, int pin) : ExceptionTrackingBase
    {
        #region Private Member Declarations		
        /// <summary>
        /// The code phrase value.
        /// </summary>
        private SecureString? _codePhrase = new(codePhrase);
        /// <summary>
        /// The password value.
        /// </summary>
        private SecureString? _password = new(password);
        /// <summary>
        /// The pin value.
        /// </summary>
        private int? _pin = pin;

        #endregion
        #region Constructor / Dispose Methods		
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                _codePhrase?.Dispose();
                _password?.Dispose();
                _pin = 0;
            }

            _codePhrase = null;
            _password = null;
            _pin = null;

            base.Dispose(disposing);
        }
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Decrypts the provided data.
        /// </summary>
        /// <param name="originalData">
        /// A byte array containing the encrypted data - must be a result of this class' <see cref="Encrypt(byte[])"/> method.
        /// </param>
        /// <returns>
        /// A byte array containing the decrypted data value.
        /// </returns>
        public byte[]? Decrypt(byte[] originalData)
        {
            byte[]? decryptedData = null;

            if (_codePhrase != null && _password != null && _pin != null)
            {
                // Create the key information from the provided data content.
                //
                // Use Rfc2898DeriveBytes to create a 48-byte set of key data (32 byte key, 16 byte IV).
                byte[] derivedDataA = PasswordKeyCreator.CreatePrimaryKeyData(
                    _codePhrase.ToString()!,
                    _password.ToString()!);

                // Splice the bits of the derived data to create an new key set (32 byte key, 16 byte IV).
                byte[]? splicedData = BitSplicer.SpliceBits(derivedDataA);
                if (splicedData != null)
                {
                    byte[]? derivedDataB = PasswordKeyCreator.CreateKeyVariant(splicedData);

                    // XOR the derived data with the PIN value - and create a new key variant.
                    byte[] derivedDataC = PasswordKeyCreator.CreateKeyVariant(CryptoUtil.XorByteArray(derivedDataB, _pin.Value));

                    // Create the AES provider instances with the key data.
                    AesProvider providerA = new();
                    providerA.SetKeyIV(derivedDataA[0..32], derivedDataA[32..48]);

                    AesProvider providerB = new();
                    providerB.SetKeyIV(derivedDataB[0..32], derivedDataB[32..48]);

                    AesProvider providerC = new();
                    providerC.SetKeyIV(derivedDataC[0..32], derivedDataC[32..48]);

                    // Decrypt the second encryption layer of the data content in reverse order from Encrypt().
                    //
                    // Decrypt the data using each provider in sequence.
                    byte[] stateA = providerA.Decrypt(originalData)!;
                    byte[] stateB = providerB.Decrypt(stateA)!;
                    byte[] stateC = providerC.Decrypt(stateB)!;

                    // Un-splice the bits.
                    byte[] stateD = BitSplicer.UnSpliceBits(stateC)!;

                    // Decrypt the data content in reverse order from Encrypt().
                    byte[] stateE = providerC.Decrypt(stateD)!;
                    byte[] stateF = providerB.Decrypt(stateE)!;
                    decryptedData = providerA.Decrypt(stateF)!;

                    // Dispose of the providers and clear memory.
                    providerC.Dispose();
                    providerB.Dispose();
                    providerA.Dispose();
                    CryptoUtil.SecureClear(stateA);
                    CryptoUtil.SecureClear(stateB);
                    CryptoUtil.SecureClear(stateC);
                    CryptoUtil.SecureClear(stateD);
                    CryptoUtil.SecureClear(stateE);
                    CryptoUtil.SecureClear(stateF);
                    CryptoUtil.SecureClear(derivedDataA);
                    CryptoUtil.SecureClear(derivedDataB);
                    CryptoUtil.SecureClear(derivedDataC);
                }
            }

            return decryptedData;
        }
        /// <summary>
        /// Decrypts the provided data to a string value.
        /// </summary>
        /// <param name="originalData">
        /// A byte array containing the encrypted data - must be a result of this class' <see cref="Encrypt(byte[])"/> method.
        /// </param>
        /// <returns>
        /// A string containing the decrypted data value.
        /// </returns>
        public string? DecryptAsString(byte[] originalData)
        {
            string? decoded = null;

            byte[]? decrypted = Decrypt(originalData);
            if (decrypted != null)
            {
                decoded = Encoding.ASCII.GetString(decrypted);
                ByteArrayUtil.Clear(decrypted);
            }
            return decoded;
        }
        /// <summary>
        /// Encrypts the provided data.
        /// </summary>
        /// <param name="originalData">
        /// A byte array containing the original data.
        /// </param>
        /// <returns>
        /// A byte array containing the encrypted data value.
        /// </returns>
        public byte[]? Encrypt(byte[] originalData)
        {
            byte[]? encryptedData = null;

            if (_codePhrase != null && _password != null && _pin != null)
            {
                // Create the key information from the provided data content.
                //
                // Use Rfc2898DeriveBytes to create a 48-byte set of key data (32 byte key, 16 byte IV).
                byte[] derivedDataA = PasswordKeyCreator.CreatePrimaryKeyData(_codePhrase.ToString()!, _password.ToString()!);
                // Splice the bits of the derived data to create an new key set (32 byte key, 16 byte IV).
                byte[]? splicedData = BitSplicer.SpliceBits(derivedDataA);
                if (splicedData != null)
                {
                    byte[]? derivedDataB = PasswordKeyCreator.CreateKeyVariant(splicedData);

                    // XOR the derived data with the PIN value - and create a new key variant.
                    byte[] derivedDataC = PasswordKeyCreator.CreateKeyVariant(CryptoUtil.XorByteArray(derivedDataB, _pin.Value));

                    // Create the AES provider instances with the key data.
                    AesProvider providerA = new();
                    providerA.SetKeyIV(derivedDataA[0..32], derivedDataA[32..48]);

                    AesProvider providerB = new();
                    providerB.SetKeyIV(derivedDataB[0..32], derivedDataB[32..48]);

                    AesProvider providerC = new();
                    providerC.SetKeyIV(derivedDataC[0..32], derivedDataC[32..48]);

                    // Encrypt the data using each provider in sequence.
                    byte[] stateA = providerA.Encrypt(originalData)!;
                    byte[] stateB = providerB.Encrypt(stateA)!;
                    byte[] stateC = providerC.Encrypt(stateB)!;

                    // Splice the bits to try to counter pattern recognition.
                    byte[] stateD = BitSplicer.SpliceBits(stateC)!;

                    // Encrypt a second time.
                    byte[] stateE = providerC.Encrypt(stateD)!;
                    byte[] stateF = providerB.Encrypt(stateE)!;
                    encryptedData = providerA.Encrypt(stateF)!;

                    // Dispose of the providers and clear memory.
                    providerC.Dispose();
                    providerB.Dispose();
                    providerA.Dispose();
                    CryptoUtil.SecureClear(stateA);
                    CryptoUtil.SecureClear(stateB);
                    CryptoUtil.SecureClear(stateC);
                    CryptoUtil.SecureClear(stateD);
                    CryptoUtil.SecureClear(stateE);
                    CryptoUtil.SecureClear(stateF);
                    CryptoUtil.SecureClear(derivedDataA);
                    CryptoUtil.SecureClear(derivedDataB);
                    CryptoUtil.SecureClear(derivedDataC);
                }
            }
            return encryptedData;
        }
        /// <summary>
        /// Encrypts the provided data.
        /// </summary>
        /// <param name="originalData">
        /// A string containing the original data.
        /// </param>
        /// <returns>
        /// A byte array containing the encrypted data value.
        /// </returns>
        public byte[]? Encrypt(string originalData)
        {
            return Encrypt(Encoding.ASCII.GetBytes(originalData));
        }
        #endregion
    }

}