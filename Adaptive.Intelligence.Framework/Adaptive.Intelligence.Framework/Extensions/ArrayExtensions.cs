namespace Adaptive.Intelligence.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="Array"/> instances.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Determines whether the specified array is null or empty.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <returns>
        ///   <c>true</c> if the array is null or empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNullOrEmpty(Array? array)
        {
            return array == null || array.Length == 0;
        }
        /// <summary>
        /// Clears the array.
        /// </summary>
        /// <param name="arrayToClear">
        /// The array to clear.
        /// </param>
        public static void ClearArray(this Array? arrayToClear)
        {
            if (arrayToClear != null)
            {
                Array.Clear(arrayToClear, 0, arrayToClear.Length);
            }
        }
        /// <summary>
        /// Compares the length and contents of one array to another.
        /// </summary>
        /// <param name="firstArray">
        /// The first <see cref="Array"/> to be compared.
        /// </param>
        /// <param name="secondArray">
        /// The second <see cref="Array"/> to be compared.
        /// </param>
        /// <returns>
        ///  A value that indicates the relative order of the objects being compared. The
        ///    return value has these meanings:
        ///
        ///    Value – Meaning
        ///    Less than zero – This instance precedes obj in the sort order.
        ///    Zero – This instance occurs in the same position in the sort order as obj.
        ///    Greater than zero – This instance follows obj in the sort order.
        /// </returns>
        public static int Compare(this Array? firstArray, Array? secondArray)
        {
            int result;

            if (firstArray == null && secondArray == null)
            {
                result = 0;
            }
            else if (firstArray == null)
            {
                result = -1;
            }
            else if (secondArray == null)
            {
                result = 1;
            }
            else
            {
                var firstLen = firstArray.Length;
                var secondLen = secondArray.Length;

                if (firstLen < secondLen)
                {
                    result = -1;
                }
                else if (firstLen > secondLen)
                {
                    result = 1;
                }
                else
                {
                    result = 0;
                    var pos = 0;
                    bool match;
                    do
                    {

                        match = firstArray.GetValue(pos) == secondArray.GetValue(pos);
                        if (!match)
                        {
                            var left = firstArray.GetValue(pos);
                            var right = secondArray.GetValue(pos);
                            if (left is IComparable && left != null && right != null)
                            {
                                var leftCompare = (IComparable)left;
                                var rightCompare = (IComparable)right;
                                result = leftCompare.CompareTo(rightCompare);
                                if (result == 0)
                                {
                                    match = true;
                                }
                            }
                            else
                            {
                                result = -1;
                            }
                        }

                        pos++;
                    } while (pos < firstLen && match);
                }
            }
            return result;
        }
        /// <summary>
        /// Finds the index of the element in the string array where the white space starts.
        /// </summary>
        /// <param name="array">The array to be checked.</param>
        /// <returns>
		/// An integer indicating the index of the first element of training whitespace, or -1 if no whitespace is found, or
		/// the array is empty.
		/// </returns>
        public static int FindTrailingWhitespace(this string[] array)
        {
            var whitespaceStart = -1;
            var length = array.Length;
            if (array.Length > 0)
            {
                var pos = length - 1;
                var done = false;
                do
                {
                    if (!string.IsNullOrWhiteSpace(array[pos]))
                    {
                        whitespaceStart = pos + 1;
                        if (whitespaceStart >= length)
                        {
                            whitespaceStart = -1;
                        }

                        done = true;
                    }
                    pos--;
                } while (pos > 0 && !done);
            }
            return whitespaceStart;
        }
    }
}
