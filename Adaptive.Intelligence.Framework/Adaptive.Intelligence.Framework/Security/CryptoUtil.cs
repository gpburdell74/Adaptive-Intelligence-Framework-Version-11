using Adaptive.Intelligence.Utility;
using System.Security.Cryptography;

namespace Adaptive.Intelligence.Security
{
    /// <summary>
    /// Provides static methods and functions for performing common cryptographic operations.
    /// </summary>
    public static class CryptoUtil
    {
        /// <summary>
        /// Performs an XOR operation against the byte array using the PIN number value.
        /// </summary>
        /// <param name="data">
        /// A byte array specifying the data to be operated on.
        /// </param>
        /// <param name="pinNumber">
        /// An integer specifying the pin number value to use.</param>
        /// <returns>
        /// The modified byte array.
        /// </returns>
        public static byte[] XorByteArray(byte[] data, int pinNumber)
        {
            // Convert the integer value to a byte array.
            byte[] intBytes = BitConverter.GetBytes(pinNumber);
            int intArrayLength = intBytes.Length;
            int intArrayPosIndex = 0;

            // Create the XOR'd data array.
            int dataLength = data.Length;
            short[] newData = new short[dataLength];

            // For each byte in the data array, perform the XOR operation against the "next" byte in the integer array.
            int shortPosIndex = 0;
            for (int count = 0; count < dataLength; count++)
            {
                // Perform the XOR - returns a short integer.
                newData[shortPosIndex] = (short)(data[count] ^ intBytes[intArrayPosIndex]);
                shortPosIndex++;
                intArrayPosIndex++;

                // Reset the integer array position index if needed.
                if (intArrayPosIndex >= intArrayLength)
                {
                    intArrayPosIndex = 0;
                }
            }

            // Convert the short integer byte array to a regular byte array.
            int shortArrayLen = shortPosIndex;
            int position = 0;
            int finalLength = 0;
            byte[] xordByteArray = new byte[shortArrayLen * 2];
            for (int finalCount = 0; finalCount < shortArrayLen; finalCount++)
            {
                // Convert each short integer to a byte array.
                byte[] shortBytes = BitConverter.GetBytes(newData[finalCount]);

                // Re-add each byte (if not a zero) to the return array.
                foreach (byte byteValue in shortBytes)
                {
                    xordByteArray[position] = byteValue;
                    position++;
                    finalLength++;
                }
            }

            // Re-size the array.
            byte[] returnData = new byte[finalLength];
            Array.Copy(xordByteArray, 0, returnData, 0, finalLength);

            // Clear memory.
            ByteArrayUtil.Clear(xordByteArray);
            ByteArrayUtil.Clear(newData);
            ByteArrayUtil.Clear(intBytes);
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            intArrayLength = 0;
            intArrayPosIndex = 0;
            dataLength = 0;
            shortPosIndex = 0;
            shortArrayLen = 0;
            position = 0;
            finalLength = 0;
#pragma warning restore IDE0059 // Unnecessary assignment of a value

            return returnData;
        }
        /// <summary>
        /// Securely clears the specified byte array.
        /// </summary>
        /// <remarks>
        /// For optimal results,  the byte array should be supplied by <see cref="ByteArrayUtil.CreatePinnedArray(int)"/>.
        /// </remarks>
        /// <param name="originalData">
        /// The original byte array whose contents are to be cleared.
        /// </param>
        public static void SecureClear(byte[]? originalData)
        {
            if (originalData != null)
            {
                CryptographicOperations.ZeroMemory(originalData);
            }
        }
    }

}