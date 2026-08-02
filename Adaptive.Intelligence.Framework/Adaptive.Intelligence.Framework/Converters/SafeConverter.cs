using Adaptive.Intelligence.Constants;
using Adaptive.Intelligence.Utility;
using System.Globalization;
using System.Text;

namespace Adaptive.Intelligence.Converters
{
    /// <summary>
    /// Provides static methods and functions for common data conversions and formatting in a safe manner.
    /// </summary>
    public static class SafeConverter
    {
        /// <summary>
        /// Adds a back slash to the end of the string, if needed.
        /// </summary>
        /// <param name="original">
        /// The original string to be modified.
        /// </param>
        /// <returns>
        /// The original string with a backslash at the end.
        /// </returns>
        public static string AddBackslash(string original)
        {
            string text = original;
            if (string.IsNullOrEmpty(text))
            {
                text = CharacterConstants.Backslash;
            }
            else
            {
                if (original.Substring(original.Length - 1, 1) != CharacterConstants.Backslash)
                {
                    text += CharacterConstants.Backslash;
                }
            }
            return text;
        }
        /// <summary>
        /// Converts the hexadecimal string to a byte array.
        /// </summary>
        /// <param name="hexString">
        /// A string containing a hexadecimal value.
        /// </param>
        /// <returns>
        /// A byte array containing the value represented by the hexadecimal string.
        /// </returns>
        public static byte[] HexToBytes(string hexString)
        {
            int num = 0;
            int length = hexString.Length;

            ArgumentNullException.ThrowIfNullOrEmpty(nameof(hexString), "Hexadecimal string cannot be null or empty.");
            if (length == 0)
            {
                throw new ArgumentException("Hexadecimal string cannot be empty.", nameof(hexString));
            }

            if (length % 2 != 0)
            {
                throw new ArgumentException("Hexadecimal string is not a valid length.  Each byte must be represented by 2 hex characters.", nameof(hexString));
            }

            byte[] array = new byte[length / 2];

            for (int i = 0; i < length; i += 2)
            {
                array[num] = Convert.ToByte(hexString.Substring(i, 2), 16);
                num++;
            }

            return array;
        }
        /// <summary>
        /// Converts the byte array to a hexadecimal string.
        /// </summary>
        /// <param name="data">
        /// A byte array containing the data to be converted.
        /// </param>
        /// <returns>
        /// A string containing the hexadecimal representation of the contents of the byte array.
        /// </returns>
        public static string ToHex(byte[]? data)
        {
            StringBuilder stringBuilder = new();

            if (data != null && data.Length > 0)
            {
                int num = data.Length;
                for (int i = 0; i < num; i++)
                {
                    string text = data[i].ToString("X", CultureInfo.InvariantCulture);
                    if (text.Length == 1)
                    {
                        text = AiConstants.Zero + text;
                    }

                    stringBuilder.Append(text);
                }
            }

            return stringBuilder.ToString();
        }
        /// <summary>
        /// Determines whether the specified string is a numeric value.
        /// </summary>
        /// <param name="itemToTest">
        /// The string to be tested.
        /// </param>
        /// <returns>
        ///   <c>true</c> if the specified string to test is numeric; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNumeric(string itemToTest)
        {
            bool isNumeric = false;


            if (!string.IsNullOrEmpty(itemToTest))
            {
                int pos = 0;
                int length = itemToTest.Length;
                do
                {
                    isNumeric = char.IsNumber(itemToTest, pos);
                    pos++;
                } while (isNumeric && pos < length);
            }

            return isNumeric;
        }
        /// <summary>
        /// Safely converts the specified string to an integer.
        /// </summary>
        /// <param name="original">
        /// The original string to be converted.
        /// </param>
        /// <returns>
        /// An integer containing the converted value.
        /// </returns>
        public static int ToInt32(string? original)
        {
            int rvalue = 0;
            if (!string.IsNullOrEmpty(original))
            {
                try
                {
                    rvalue = Convert.ToInt32(original, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message, ex);
                    rvalue = 0;
                }
            }

            return rvalue;
        }
        /// <summary>
        /// Safely converts the specified string to a single-precision floating point number.
        /// </summary>
        /// <param name="original">
        /// The original string to be converted.
        /// </param>
        /// <returns>
        /// An single-precision floating point number containing the converted value.
        /// </returns>
        public static float SafeToSingle(string? original)
        {
            float rvalue = 0;
            if (!string.IsNullOrEmpty(original))
            {
                try
                {
                    rvalue = Convert.ToSingle(original, CultureInfo.InvariantCulture);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message, ex);
                    rvalue = 0;
                }
            }
            return rvalue;
        }
        /// <summary>
        /// Converts a <see cref="decimal"/> value to a byte array.
        /// </summary>
        /// <param name="value">
        /// The <see cref="decimal"/> value to be converted.
        /// </param>
        /// <returns>
        /// A byte array representing the decimal value.
        /// </returns>
        public static byte[] DecimalToArray(decimal value)
        {
            // Translate to the 4-integer parts.
            int[] data = decimal.GetBits(value);

            // Convert the integers to byte arrays, and concatenate the arrays.
            byte[] decimalArray = ByteArrayUtil.ConcatenateArrays(
                BitConverter.GetBytes(data[0]),
                BitConverter.GetBytes(data[1]),
                BitConverter.GetBytes(data[2]),
                BitConverter.GetBytes(data[3]));

            // Clear.
            Array.Clear(data, 0, 4);

            return decimalArray;
        }
        /// <summary>
        /// Converts a byte array to a decimal.
        /// </summary>
        /// <param name="data">
        /// The 16-element byte array containing the data for the decimal structure.
        /// </param>
        /// <returns>
        /// The <see cref="decimal"/> value represented by the byte array.	
        /// </returns>
        public static decimal DecimalFromArray(byte[] data)
        {
            decimal newValue = decimal.Zero;

            if (data != null && data.Length == 16)
            {
                // Read the four integer parts.
                int a = BitConverter.ToInt32(data, 0);
                int b = BitConverter.ToInt32(data, 4);
                int c = BitConverter.ToInt32(data, 8);
                int d = BitConverter.ToInt32(data, 12);

                // Calculate the sign and scale parameters from the last integer.
                bool sign = (d & 0x80000000) != 0;
                byte scale = (byte)((d >> 16) & 0x7F);

                // Create the new structure.
                newValue = new decimal(a, b, c, sign, scale);
            }
            return newValue;
        }
    }
}