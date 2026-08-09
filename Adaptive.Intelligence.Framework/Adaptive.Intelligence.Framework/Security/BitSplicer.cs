using System.Collections;

namespace Adaptive.Intelligence.Security
{
    /// <summary>
    /// Provides static methods / functions for performing bit manipulation for byte arrays.
    /// </summary>
    public static class BitSplicer
    {
        #region Public Static Methods / Functions
        /// <summary>
        /// Splices the bits in the byte array.
        /// </summary>
        /// <remarks>
        /// This works to move the values of the odd-numbered bits (1,3,5,7) to the first half of the
        /// byte array, and the even-numbered bits (2,4,6,8) to the second half of the byte array.
        /// </remarks>
        /// <param name="original">
        /// The byte array to be obfuscated.
        /// </param>
        /// <returns>
        /// A byte array containing the obfuscated data, or <b>null</b> if the <i>original</i>
        /// value is <b>null</b>.
        /// </returns>
        public static byte[]? SpliceBits(byte[]? original)
        {
            byte[]? splicedData = null;

            if ((original != null) && (original.Length > 0))
            {
                // Ensure the original array is even in length.
                var evenByteArray = CreateEvenLengthByteArray(original);

                // Create the bit arrays.
                if (evenByteArray != null)
                {
                    var originalLength = evenByteArray.Length;
                    var halfLength = evenByteArray.Length / 2;
                    var bitArraySize = halfLength * 8;

                    var originalBitList = new BitArray(evenByteArray);
                    var leftBitList = new BitArray(bitArraySize);
                    var rightBitList = new BitArray(bitArraySize);

                    SplitBits(originalBitList, leftBitList, rightBitList);

                    splicedData = RecombineBitArrays(leftBitList, rightBitList, originalLength);
                }
            }

            return splicedData;
        }
        /// <summary>
        /// Un-splices the bits in the byte array.
        /// </summary>
        /// <remarks>
        /// This works in the inverse of the <see cref="SpliceBits"/> method to restore the
        /// original data.
        /// </remarks>
        /// <param name="splicedData">
        /// The obfuscated byte array to be restored.
        /// </param>
        /// <returns>
        /// A byte array containing the obfuscated data, or <b>null</b> if the <i>original</i>
        /// value is <b>null</b> or an error occurs in processing.
        /// </returns>
        public static byte[]? UnSpliceBits(byte[]? splicedData)
        {
            if ((splicedData != null) && (splicedData.Length > 1))
            {
                var originalLength = splicedData.Length;
                var halfLength = originalLength / 2;

                // Create the bit arrays.
                BitArray leftBitArray = GetBits(splicedData, 0, halfLength);
                BitArray rightBitArray = GetBits(splicedData, halfLength, halfLength);
                BitArray? combinedArray = MergeBits(leftBitArray, rightBitArray);

                // Translate to byte array.
                var combined = new byte[originalLength];
                combinedArray?.CopyTo(combined, 0);

                // Get original content.
                return ReadOriginalArray(combined);
            }
            return null;
        }
        #endregion

        #region Private Static Methods / Functions

        /// <summary>
        /// Creates an even length byte array from the original array.
        /// </summary>
        /// <param name="original">
        /// The original byte array.
        /// </param>
        /// <returns>
        /// A byte array of event length prefixed with a 32-bit length indicator.
        /// </returns>
        private static byte[]? CreateEvenLengthByteArray(byte[]? original)
        {
            byte[]? returnValue = null;

            if (original != null)
            {
                var stream = new MemoryStream(original.Length + 4);
                var writer = new BinaryWriter(stream);

                // Write the length.
                writer.Write(original.Length);

                // Write the original array.
                writer.Write(original);

                // If the original array's length is an odd number, add an extra byte.
                if (original.Length % 2 != 0)
                {
                    writer.Write((byte)0);
                }

                writer.Flush();

                // Get the array to return.
                returnValue = stream.ToArray();

                // Close and return.
                writer.Close();
                stream.Close();
            }
            return returnValue;
        }
        /// <summary>
        /// Reads the byte array and returns the original content.
        /// </summary>
        /// <param name="source">The byte array containing the source data.
        /// </param>
        /// <returns>
        /// A byte array containing the original data.
        /// </returns>
        private static byte[]? ReadOriginalArray(byte[]? source)
        {
            byte[]? returnArray = null;

            if (source != null)
            {
                using var stream = new MemoryStream(source);
                using var reader = new BinaryReader(stream);

                // Read the length.
                try
                {

                    var length = reader.ReadInt32();

                    // Read the original array.
                    returnArray = reader.ReadBytes(length);
                }
                catch
                {
                    returnArray = null;
                }
            }
            return returnArray;
        }

        /// <summary>
        /// Splits the content of the original bit array into the two parts.
        /// </summary>
        /// <param name="originalBitList">The <see cref="BitArray" /> containing the bits for the original data.</param>
        /// <param name="leftBitList">The <see cref="BitArray" /> to contain the odd-number bits.</param>
        /// <param name="rightBitList">The <see cref="BitArray" /> to contain the even-number bits.</param>
        private static void SplitBits(BitArray? originalBitList, BitArray? leftBitList, BitArray? rightBitList)
        {
            if (originalBitList != null && leftBitList != null &&
                rightBitList != null)
            {
                var arrayIndex = 0;
                var length = originalBitList.Length;

                for (var count = 0; count < length; count += 2)
                {
                    // Copy the bits (0, 2, 4, 6, etc.) to the left-side array.
                    leftBitList[arrayIndex] = originalBitList[count];

                    // Copy the bits (1, 3, 5, 7, etc.) to the right-side array.
                    rightBitList[arrayIndex] = originalBitList[count + 1];
                    arrayIndex++;
                }
            }
        }
        /// <summary>
        /// Merges the two bit arrays into a single array.
        /// </summary>
        /// <param name="leftBitList">The left bit list containing the odd-numbered bits of the data.</param>
        /// <param name="rightBitList">The right bit list containing the even-numbered bits of the data.</param>
        /// <returns>
        /// A <see cref="BitArray"/> instance containing the re-ordered bits.
        /// </returns>
        private static BitArray? MergeBits(BitArray? leftBitList, BitArray? rightBitList)
        {
            BitArray? returnArray = null;
            if (leftBitList != null && rightBitList != null)
            {
                var length = leftBitList.Length;
                returnArray = new BitArray(length * 2);

                var arrayIndex = 0;
                for (var count = 0; count < length; count++)
                {
                    // Merge from the left side (0, 2, 4, 6, etc.) bits...
                    returnArray[arrayIndex] = leftBitList[count];

                    // Merge from the right side (1, 3, 5, 7, etc.) bits...
                    returnArray[arrayIndex + 1] = rightBitList[count];
                    arrayIndex += 2;
                }
            }
            return returnArray;
        }
        /// <summary>
        /// Recombines the provided <see cref="BitArray"/>s into a single byte array.
        /// </summary>
        /// <param name="leftBitList">
        /// The <see cref="BitArray"/> containing the bits for the first half of the byte array.
        /// </param>
        /// <param name="rightBitList">
        /// The <see cref="BitArray"/> containing the bits for the second half of the byte array.
        /// </param>
        /// <param name="size">The total size of the resulting array.
        /// </param>
        /// <returns>
        /// A byte array containing the bytes.
        /// </returns>
        private static byte[] RecombineBitArrays(BitArray? leftBitList, BitArray? rightBitList, int size)
        {
            // Copy the data out of the bit arrays into a byte array.
            var returnData = new byte[size];
            if (leftBitList != null && rightBitList != null)
            {
                leftBitList.CopyTo(returnData, 0);
                rightBitList.CopyTo(returnData, size / 2);
            }
            return returnData;
        }

        /// <summary>
        /// Gets a <see cref="BitArray"/> for the specified portion of the byte array.
        /// </summary>
        /// <param name="splicedData">
        /// The byte array containing the source information.
        /// </param>
        /// <param name="index">The index at which to start reading bytes.</param>
        /// <param name="length">The number of bytes to read and copy.</param>
        /// <returns>
        /// A <see cref="BitArray"/> instance containing the bits.
        /// </returns>
        private static BitArray GetBits(byte[]? splicedData, int index, int length)
        {
            var subBytes = new byte[length];
            if (splicedData != null)
            {
                Array.Copy(splicedData, index, subBytes, 0, length);
            }

            return new BitArray(subBytes);
        }
        #endregion
    }

}