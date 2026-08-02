using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace Adaptive.Intelligence.Utility
{
    /// <summary>
    /// Provides utility methods for working with byte arrays.
    /// </summary>
    public static class ByteArrayUtil
    {
        /// <summary>
        /// Clears the specified array, if it is not null.
        /// </summary>
        /// <typeparam name="T">
        /// The data type of the array.
        /// </typeparam>
        /// <param name="array">
        /// The reference to the source array to be cleared.
        /// </param>
        public static void Clear<T>(T[]? array)
        {
            if (array != null && array.Length > 0)
            {
                Array.Clear(array, 0, array.Length);
            }
        }
        /// <summary>
        /// Creates a byte array that is pinned in memory and will not be moved by the garbage collector.
        /// </summary>
        /// <param name="length">
        /// An integer specifying the length of the array.
        /// </param>
        /// <returns>
        /// A byte array of the specified length that is pinned in memory.
        /// </returns>
        public static byte[] CreatePinnedArray(int length)
        {
            return GC.AllocateArray<byte>(length, true);
        }
        /// <summary>
        /// Creates a byte array of the specified length filled with random data.
        /// </summary>
        /// <param name="length">
        /// An integer specifying the length of the array.
        /// </param>
        /// <returns>
        /// A byte array containing the random bytes.
        /// </returns>
        public static byte[] CreateRandomArray(int length)
        {
            byte[] data = new byte[length];
            RandomNumberGenerator.Fill(data);
            return data;
        }
        /// <summary>
        /// Concatenates the arrays.
        /// </summary>
        /// <param name="listOfArrays">The list of arrays.</param>
        /// <returns></returns>
        public static byte[] ConcatenateArrays(params byte[][] listOfArrays)
        {
            MemoryStream ms = new();
            foreach (byte[] array in listOfArrays)
            {
                ms.Write(array, 0, array.Length);
            }
            ms.Flush();
            byte[] result = ms.ToArray();
            ms.Dispose();
            return result;
        }
        /// <summary>
        /// Provides a simple method for copying one array to a new array instance with the same values.
        /// </summary>
        /// <typeparam name="T">
        /// The data type of the array.
        /// </typeparam>
        /// <param name="array">
        /// The reference to the source array to be copied.
        /// </param>
        /// <returns>
        /// A new instance of the array with the same values if successful; otherwise, returns <b>null</b>.
        /// </returns>
        public static T[]? CopyToNewArray<T>(T[]? array)
        {
            if (array == null)
            {
                return null;
            }

            T[] newArray = new T[array.Length];
            Array.Copy(array, newArray, array.Length);

            return newArray;
        }
        /// <summary>
        /// Determines whether the specified byte array is null or empty.
        /// </summary>
        /// <param name="arrayToCheck">
        /// The array of bytes to be checked.
        /// </param>
        /// <returns>
        ///   <c>true</c> if <i>arrayToCheck</i> is <b>null</b> or has zero-length; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty([NotNullWhen(false)] byte[]? arrayToCheck)
        {
            return arrayToCheck == null || arrayToCheck.Length == 0;
        }
    }
}