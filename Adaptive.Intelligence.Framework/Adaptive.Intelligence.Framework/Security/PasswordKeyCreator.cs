using Adaptive.Intelligence.Utility;

namespace Adaptive.Intelligence.Security
{
    /// <summary>
    /// Provides static methods and functions for creating encryption keys from password values.
    /// </summary>
    public static class PasswordKeyCreator
    {
        private static readonly byte[] _salt =
            [22, 33, 21,97, 128, 255, 231, 190, 76, 42, 111, 231, 75, 99, 41, 65 ,
            87, 45, 177, 123, 159, 221, 9, 223, 4, 57, 171, 78, 80, 11, 32 ];

        /// <summary>
        /// Creates the primary key data.
        /// </summary>
        /// <remarks>
        /// Any string values can be used in <i>userId</i> and <i>password</i>, excepting <b>null</b> or <see cref="string.Empty"/>.
        /// </remarks>
        /// <param name="userId">
        /// A string containing the user id or name or other string value.
        /// </param>
        /// <param name="filePassword">
        /// A string containing the file password value.
        /// </param>
        /// <returns>
        /// A byte array to be used as an AES key and IV pair.
        /// </returns>
        public static byte[] CreatePrimaryKeyData(string userId, string filePassword)
        {
            // Translate ASCII into bytes.
            byte[] clearUserId = ByteArrayUtil.CreatePinnedArray(userId.Length);
            byte[] clearPassword = ByteArrayUtil.CreatePinnedArray(filePassword.Length);

            System.Text.Encoding.ASCII.GetBytes(userId, clearUserId);
            System.Text.Encoding.ASCII.GetBytes(filePassword, clearPassword);

            // Copy user ID and password bytes into a single array.
            byte[] clearData = new byte[userId.Length + filePassword.Length];
            Array.Copy(clearUserId, 0, clearData, 0, clearUserId.Length);
            Array.Copy(clearPassword, 0, clearData, clearUserId.Length, clearPassword.Length);

            // Clear the sub-arrays.
            CryptoUtil.SecureClear(clearUserId);
            CryptoUtil.SecureClear(clearPassword);

            // Generate the key data.
            byte[] keyData = KeyGenerator.CreateKeyData(clearData, _salt, 1024);

            // Clear and return.
            CryptoUtil.SecureClear(clearData);

            return keyData;
        }
        /// <summary>
        /// Creates the a cryptographic key variant from the original key.
        /// </summary>
        /// <param name="originalContent">
        /// A byte array containing the original key value to create a variant from.;
        /// </param>
        /// <returns>
        /// A byte array containing the key variant value.
        /// </returns>
        public static byte[] CreateKeyVariant(byte[] originalContent)
        {
            // Generate new key data from the original.
            byte[] clearDataModified = KeyGenerator.CreateKeyData(originalContent, _salt, 1024);

            // Generate key data from the generated key data.
            byte[] secondaryDataModified = KeyGenerator.CreateKeyData(clearDataModified, _salt, 1024);

            // Clear and return.
            Array.Clear(clearDataModified, 0, clearDataModified.Length);
            return secondaryDataModified;
        }
    }

}
