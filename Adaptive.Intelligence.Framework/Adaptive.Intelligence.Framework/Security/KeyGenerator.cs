using System.Security.Cryptography;

namespace Adaptive.Intelligence.Security
{
    /// <summary>
    /// Provides static methods / functions for generating the key data for the cryptographic operations.
    /// </summary>
    public static class KeyGenerator
    {
        /// <summary>
        /// Shifts the content of the byte array one byte to the left.
        /// </summary>
        /// <param name="original">The original byte array to be copied.</param>
        /// <returns>
        /// A byte array containing the original data shifted one byte to the left with
        /// the original first byte appended to the end of the array.
        /// </returns>
        public static byte[]? ByteShift(byte[]? original)
        {
            byte[]? returnData = null;

            if (original != null && original.Length >= 3)
            {
                // Create the authentication strings and variants.
                returnData = new byte[original.Length];
                for (int count = 1; count < original.Length - 2; count++)
                {
                    returnData[count - 1] = original[count];
                }

                returnData[^1] = original[0];
            }

            return returnData;
        }
        /// <summary>
        /// Provides an SHA-512 hash for the specified data with bit splicing.
        /// </summary>
        /// <param name="original">
        /// The original byte array to be hashed.
        /// </param>
        /// <returns>
        /// A byte array containing the secured hash data.
        /// </returns>
        public static byte[]? HashContent(byte[]? original)
        {
            byte[]? returnContent = null;

            if (original != null)
            {
                // Create hash values for the authentication and variant arrays.
                byte[] hashedData = SHA512.HashData(original);
                returnContent = BitSplicer.SpliceBits(hashedData);
                Array.Clear(hashedData, 0, hashedData.Length);
            }

            return returnContent;
        }
        /// <summary>
        /// Creates the key data from the provided content.
        /// </summary>
        /// <param name="saltValue">
        /// A byte array containing the salt value.
        /// </param>
        /// <returns>
        /// A 48-element byte array where the first 32 bytes contain the encryption key,
        /// and the last 16 bytes contain the initialization vector (IV) data.
        /// </returns>
        public static byte[]? CreateKeyData(byte[] saltValue)
        {
            byte[]? randomizedPassword = new byte[256];

            // Generate a random list of password values and the salt.
            RandomNumberGenerator? rng = RandomNumberGenerator.Create();

            try
            {
                // Get 256 bytes as a random password with a randomized salt value.
                rng.GetBytes(randomizedPassword, 0, 256);
                rng.GetNonZeroBytes(saltValue);
            }
            catch
            {
                randomizedPassword = null;
            }
            rng.Dispose();

            return randomizedPassword;
        }
        /// <summary>
        /// Creates the key data from the provided content.
        /// </summary>
        /// <param name="derivedData">
        /// A byte array containing the derived data used to generate the key information.
        /// </param>
        /// <param name="saltValue">
        /// A byte array containing the salt value.
        /// </param>
        /// <param name="iterationCount">
        /// An integer specifying the number of iteration to use in the hash process.
        /// </param>
        /// <returns>
        /// A 48-element byte array where the first 32 bytes contain the encryption key,
        /// and the last 16 bytes contain the initialization vector (IV) data.
        /// </returns>
        public static byte[] CreateKeyData(byte[] derivedData, byte[] saltValue, int iterationCount)
        {
            // 32 bytes for the key and 16 for the IV.
            return
                Rfc2898DeriveBytes.Pbkdf2(
                    derivedData,
                    saltValue,
                    iterationCount,
                    HashAlgorithmName.SHA256, 48);

        }
    }
}