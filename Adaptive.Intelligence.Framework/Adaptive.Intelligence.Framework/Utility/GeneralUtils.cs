using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Adaptive.Intelligence.Constants;
using Adaptive.Intelligence.Extensions;

namespace Adaptive.Intelligence.Utility
{
    /// <summary>
    /// Provides static methods / functions for shared utility operations
    /// for the application.
    /// </summary>
    public static class GeneralUtils
    {
        #region Private Static Members
        /// <summary>
        /// Blank date value.
        /// </summary>
        private static readonly DateTimeOffset _blankDate = new(1900, 1, 1, 0, 0, 0, new TimeSpan(0));
        #endregion

        #region Public Static Methods / Functions
        /// <summary>
        /// Transforms a list of items in to a list of lists, each sub-list being the specified
        /// block size.
        /// </summary>
        /// <param name="originalList">
        /// The original <see cref="List{T}"/> to be split into parts.
        /// </param>
        /// <param name="blockSize">
        /// An integer specifying the maximum size of each of the sub-lists to be returned.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> of <see cref="List{T}"/> instances.
        /// </returns>
        public static List<List<T>> CreateListBlocks<T>(List<T>? originalList, int blockSize)
        {
            List<List<T>> blockList = [];

            if (originalList != null)
            {
                int length = originalList.Count;
                for (int index = 0; index < length; index += blockSize)
                {
                    int itemCount = length - index;
                    if (itemCount > blockSize)
                    {
                        itemCount = blockSize;
                    }

                    blockList.Add(originalList.GetRange(index, itemCount));
                }
            }

            return blockList;
        }

        /// <summary>
        /// Creates the strings to use in SQL queries that take comma-delimited lists of ID values.
        /// </summary>
        /// <remarks>
        /// This is used since the maximum length for an NVARCHAR parameter is 4000 characters.  In order to
        /// pass a list of ID values to a stored procedure call using a parameter, the list of IDs must be split
        /// into groups of strings that are less than 4000 characters each.
        /// </remarks>
        /// <param name="listOfIdValues">
        /// A <see cref="List{T}"/> of <see cref="string"/> values that contains GUID/ROWGUID ID values.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> of <see cref="string"/> containing the comma-delimited ID values in
        /// separate lists of strings for use in queries, or an empty list of the parameters are invalid.
        /// </returns>
        public static List<string> CreateIterationListsFromIdList(List<string>? listOfIdValues)
        {
            List<string> list = [];
            if (listOfIdValues != null && listOfIdValues.Count > 0)
            {
                // Separate each item and add ~ 100 ID values to each list, maximum.
                StringBuilder builder = new();
                int length = listOfIdValues.Count;
                int addCount = 0;

                for (int index = 0; index < length; index++)
                {
                    builder.Append(listOfIdValues[index] + ",");
                    addCount++;
                    if (addCount > 100)
                    {
                        // Render the SQL List string (e.g. '123,234,345,456,...,')
                        string items = builder.ToString();
                        list.Add(string.Concat(CharacterConstants.SingleQuote, items.AsSpan(0, items.Length - 1), CharacterConstants.SingleQuote));

                        // Reset for next string creation iteration.
                        builder = new StringBuilder();
                        addCount = 0;
                    }
                }

                // Add the remaining items that were not processed.
                string lastItems = builder.ToString();
                if (!string.IsNullOrEmpty(lastItems))
                {
                    list.Add(string.Concat(CharacterConstants.SingleQuote, lastItems.AsSpan(0, lastItems.Length - 1), CharacterConstants.SingleQuote));
                }
            }

            return list;
        }
        /// <summary>
        /// Gets the plural English-language word for the specified English word in singular form.
        /// </summary>
        /// <param name="word">
        /// A string containing the singular form of the English word.
        /// </param>
        /// <returns>
        /// A string containing the plural form of the provided word.
        /// </returns>
        public static string GetPluralEnglishForm(string? word)
        {
            return !string.IsNullOrEmpty(word) ? word.Pluralize() : string.Empty;
        }
        /// <summary>
        /// Gets the singular English-language word for the specified English word in plural form.
        /// </summary>
        /// <param name="word">
        /// A string containing the singular form of the English word.
        /// </param>
        /// <returns>
        /// A string containing the plural form of the provided word.
        /// </returns>
        public static string GetSingleEnglishForm(string? word)
        {
            return !string.IsNullOrEmpty(word) ? word.Singularize() : string.Empty;
        }
        /// <summary>
        /// Optionally pluralizes the text according to English rules.
        /// </summary>
        /// <param name="value">The integer value to be represented.</param>
        /// <param name="unitText">The unit text.</param>
        /// <param name="pluralText">The pluralization text.</param>
        /// <returns>
        /// A formatted string with the number and unit text.
        /// </returns>
        public static string EnglishPlural(int value, string unitText, string pluralText)
        {
            return value == 1
                ? value.ToString(CultureInfo.InvariantCulture) + " " + unitText
                : value.ToString(CultureInfo.InvariantCulture) + " " + unitText + pluralText;
        }
        /// <summary>
        /// Concatenates the supplied strings as if in an English sentence, with commas
        /// and 'and'.
        /// </summary>
        /// <param name="items">
        /// An array of strings to be combined.
        /// </param>
        /// <returns>
        /// The concatenated string.
        /// </returns>
        public static string EnglishStringAppend(string[]? items)
        {
            string returnValue = string.Empty;

            if (items != null && items.Length > 0)
            {
                List<string> nonEmptyList = [];
                foreach (string content in items)
                {
                    if (!string.IsNullOrEmpty(content.Trim()))
                    {
                        nonEmptyList.Add(content.Trim());
                    }
                }

                // Process by count.
                int length = nonEmptyList.Count;
                if (length == 1)
                {
                    returnValue = nonEmptyList[0];
                }
                else if (length == 2)
                {
                    returnValue = nonEmptyList[0] + " and " + nonEmptyList[1];
                }
                else
                {
                    StringBuilder builder = new();
                    if (nonEmptyList.Count > 0)
                    {
                        int pos = 0;
                        do
                        {
                            builder.Append(nonEmptyList[pos]);
                            if (pos <= length - 3)
                            {
                                builder.Append(", ");
                            }
                            else if (pos == length - 2)
                            {
                                builder.Append(" and ");
                            }

                            pos++;
                        } while (pos < length);
                    }
                    returnValue = builder.ToString();
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Determines whether the list reference is null or empty.
        /// </summary>
        /// <typeparam name="T">
        /// The data type of the contents of the list.
        /// </typeparam>
        /// <param name="listInstance">
        /// The list instance reference.
        /// </param>
        /// <returns>
        ///   <b>true</b> if the list is null or empty.; otherwise, <b>false</b>.
        /// </returns>
        public static bool IsListNullOrEmpty<T>(List<T>? listInstance)
        {
            return listInstance == null || listInstance.Count == 0;
        }
        /// <summary>
        /// Provides a standard function for creating new ID values as as string based on GUID values.
        /// </summary>
        /// <returns>
        /// A string containing the ID value from a  <see cref="Guid"/> instance.
        /// </returns>
        public static string CreateGuidIdString()
        {
            return Guid.NewGuid().ToString("N");
        }
        /// <summary>
        /// Returns the timezone from a string value from the database
        /// </summary>
        /// <param name="hoursOffsetStr"></param>
        /// <returns>
        /// The <see cref="TimeZoneInfo"/> for the specified offset value.
        /// </returns>
        public static TimeZoneInfo FindTimeZoneForOffset(string hoursOffsetStr)
        {
            return int.TryParse(hoursOffsetStr, out int hoursOffset) ? FindTimeZoneForOffset(hoursOffset) : TimeZoneInfo.Local;
        }
        /// <summary>
        /// gets the timezone info from the hours offset in the database
        /// </summary>
        /// <param name="hoursOffset"></param>
        /// <param name="matchLocal">
        /// An optional parameter value indicating whether to match the local timezone rules as well as the offset.
        /// </param>
        /// <returns>
        /// A <see cref="TimeZoneInfo"/> for the specified offset value."/>
        /// </returns>
        public static TimeZoneInfo FindTimeZoneForOffset(int hoursOffset, bool matchLocal = true)
        {
            // The time zone information reference.
            TimeZoneInfo? timeZoneResult = null;

            // Get the system list and query for the specific offset.  There may be multiple results.
            ReadOnlyCollection<TimeZoneInfo> systemList = TimeZoneInfo.GetSystemTimeZones(true);
            List<TimeZoneInfo> candidateList = matchLocal
                ? [.. systemList.Where(x => x.BaseUtcOffset.Hours == hoursOffset
                                                      && x.HasSameRules(TimeZoneInfo.Local))]
                : [.. systemList.Where(x => x.BaseUtcOffset.Hours == hoursOffset)];
            if (candidateList.Count == 1)
            {
                timeZoneResult = candidateList[0];
            }
            else
            {
                // Look for US timezones.
                timeZoneResult = candidateList.FirstOrDefault(x => x.DisplayName.Contains("US"));
                if (timeZoneResult == null)
                {
                    if (candidateList.Count > 0)
                    {
                        timeZoneResult = candidateList[0];
                    }
                }
            }

            // Return the default if the other(s) cannot be found.
            timeZoneResult ??= TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            return timeZoneResult;

        }

        /// <summary>
        /// Returns the default blank date value.
        /// </summary>
        /// <returns>
        /// A <see cref="DateTimeOffset"/> for the date value of 1/1/1900 at midnight with no
        /// GMT offset value.
        /// </returns>
        public static DateTimeOffset BlankDate()
        {
            return _blankDate;
        }
        /// <summary>
        /// Clean up a phone number so that it can be stored in the database
        /// </summary>
        /// <param name="phoneNumber">
        /// A string containing the value to be formatted.
        /// </param>
        /// <returns>
        /// A string containing the cleaned-up phone number value.
        /// </returns>
        public static string CleanUpPhoneNumber(this string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return string.Empty;
            }

            //use LINQ to remove all chars except for numbers! SWEET!
            string newCell = new([.. from char c in phoneNumber where char.IsDigit(c) select c]);
            if (newCell.Length > 0)
            {
                if (newCell[0] == '1')
                {
                    newCell = newCell[1..];
                }
            }

            return newCell;
        }
        /// <summary>
        /// Convert a text dollar amount to an easy-to-use decimal value.
        /// </summary>
        /// <param name="moneyText">
        /// A string containing the currency text to be converted to
        /// a decimal value.
        /// </param>
        /// <returns>
        /// A <see cref="float"/> value from the provided text.
        /// </returns>
        public static float ConvertToSQLMoney(this string moneyText)
        {
            float returnValue = 0;

            if (!string.IsNullOrEmpty(moneyText))
            {
                string noSign = moneyText.Replace("$", string.Empty);
                if (!float.TryParse(noSign, out returnValue))
                {
                    returnValue = 0;
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Convert a text amount to a double value.
        /// </summary>
        /// <param name="doubleText">
        /// A string containing the text to be converted to a double value.
        /// </param>
        /// <returns>
        /// A <see cref="double"/> value from the provided text.
        /// </returns>
        public static double ConvertToDouble(this string doubleText)
        {
            double returnValue = 0;

            if (!string.IsNullOrEmpty(doubleText))
            {
                string noSign = doubleText.Replace("$", string.Empty);
                if (!double.TryParse(noSign, out returnValue))
                {
                    returnValue = 0;
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Convert a text amount to a decimal value.
        /// </summary>
        /// <param name="decimalText">
        /// A string containing the text to be converted to a decimal value.
        /// </param>
        /// <returns>
        /// A <see cref="decimal"/> value from the provided text.
        /// </returns>
        public static decimal ConvertToDecimal(this string decimalText)
        {
            decimal returnValue = 0;

            if (!string.IsNullOrEmpty(decimalText))
            {
                string noSign = decimalText.Replace("$", string.Empty);
                if (!decimal.TryParse(noSign, out returnValue))
                {
                    returnValue = 0;
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Creates a color from the specified ARGB values.
        /// </summary>
        /// <param name="a">The alpha color component in hexadecimal format.</param>
        /// <param name="r">The red color component in hexadecimal format.</param>
        /// <param name="g">The green color component in hexadecimal format.</param>
        /// <param name="b">The blue color component in hexadecimal format.</param>
        /// <returns>
        /// A <see cref="Color"/> structure from the specified components.
        /// </returns>
        public static Color FromArgbString(string a, string r, string g, string b)
        {
            int aValue = 255;
            int rValue = 255;
            int gValue = 255;
            int bValue = 255;

            if (!string.IsNullOrEmpty(a))
            {
                aValue = Convert.ToInt32(a, 16);
            }

            if (!string.IsNullOrEmpty(r))
            {
                rValue = Convert.ToInt32(r, 16);
            }

            if (!string.IsNullOrEmpty(g))
            {
                gValue = Convert.ToInt32(g, 16);
            }

            if (!string.IsNullOrEmpty(b))
            {
                bValue = Convert.ToInt32(b, 16);
            }

            return Color.FromArgb(aValue, rValue, gValue, bValue);
        }
        /// <summary>
        /// Creates an MD5 hash value for the specified content.
        /// </summary>
        /// <param name="originalContent">
        /// A string containing the content to be hashed.
        /// </param>
        /// <returns>
        /// A string containing the base-64 encoding of the byte
        /// array containing the hash.
        /// </returns>
        public static string? HashString(string originalContent)
        {
            string? result = null;
            if (!string.IsNullOrEmpty(originalContent))
            {
                byte[] input = Encoding.UTF8.GetBytes(originalContent);
                byte[] output = SHA512.HashData(input);
                result = Convert.ToBase64String(output);
            }

            return result;
        }
        /// <summary>
        /// Launches the Windows process used to view the specified file.
        /// </summary>
        /// <param name="fileName">
        /// The fully-qualified path and name of the file to be opened.
        /// </param>
        public static void LaunchProcess(string fileName)
        {
            using Process process = new();
            {
                try
                {
                    process.StartInfo.FileName = fileName;
                    process.Start();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }
        }
        /// <summary>
        /// Formats the specified phone number value.
        /// </summary>
        /// <param name="phone">
        /// A string containing the phone number value to be formatted.
        /// </param>
        /// <returns>
        /// A string containing the phone number in US Phone number
        /// format.
        /// </returns>
        public static string MarkupPhoneNumber(this string phone)
        {
            string returnValue = string.Empty;
            if (!string.IsNullOrEmpty(phone))
            {
                returnValue = phone.Length == 10
                    ? string.Format(CultureInfo.InvariantCulture, AiConstants.PhoneNumberFormat,
                        phone[..3],
                        phone.Substring(3, 3),
                        phone[6..])
                    : phone;
            }
            return returnValue;
        }
        /// <summary>
        /// Parses the color string.
        /// </summary>
        /// <param name="colorName">
        /// The value or name of the color.</param>
        /// <returns>
        /// A <see cref="Color"/> structure or <see cref="Color.Empty"/>
        /// if the <i>colorName</i> cannot be properly parsed.
        /// </returns>
        public static Color ParseColor(string colorName)
        {
            Color color;

            if (colorName.StartsWith('#'))
            {
                colorName = colorName[1..];
            }

            if (colorName.Length == 6)
            {
                color = FromArgbString("FF",
                    colorName[..2],
                    colorName.Substring(2, 2),
                    colorName.Substring(4, 2));
            }
            else if (colorName.Length == 8)
            {
                color = FromArgbString(colorName[..2],
                    colorName.Substring(2, 2),
                    colorName.Substring(4, 2),
                    colorName.Substring(6, 2));

            }
            else
            {
                color = Color.FromName(colorName);
            }

            return color;
        }
        /// <summary>
        /// Provides an extension method that performs a smart trim operation on the specified string.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>
        /// The modified string value.
        /// </returns>
        public static string SmartTrim(this string data, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                return string.Empty;
            }

            data = data.Trim();
            return data.Length > maxLength ? data[..maxLength] : data;
        }
        /// <summary>
        /// Returns the formatted date string if the date value is not a null
        /// date; otherwise, returns the specified no date value.
        /// </summary>
        /// <param name="value">
        /// The <see cref="DateTimeOffset"/> value to be evaluated.
        /// </param>
        /// <param name="noDateText">
        /// A string containing the text to return if the date is a null
        /// date value.
        /// </param>
        /// <returns>
        /// The <i>value</i> formatted as a date if the date is not null;
        /// otherwise, returns the specified <i>noDateText</i> content.
        /// </returns>
        public static string ToCustomTextIfNull(this DateTimeOffset value,
            string noDateText)
        {
            return value.ToUniversalTime().Year > 1901
                ? string.Format(CultureInfo.InvariantCulture, AiConstants.DateFormat, value.ToUniversalTime())
                : noDateText;
        }
        /// <summary>
        /// Returns the formatted date/time string if the date value is not a null
        /// date; otherwise, returns the specified no date value.
        /// </summary>
        /// <param name="value">
        /// The <see cref="DateTimeOffset"/> value to be evaluated.
        /// </param>
        /// <param name="noDateText">
        /// A string containing the text to return if the date is a null
        /// date value.
        /// </param>
        /// <returns>
        /// The <i>value</i> formatted as a date/time if the date is not null;
        /// otherwise, returns the specified <i>noDateText</i> content.
        /// </returns>
        public static string ToCustomTextIfNullWithTime(this DateTimeOffset value,
            string noDateText)
        {
            return value.ToUniversalTime().Year > 1900
                ? string.Format(CultureInfo.InvariantCulture, AiConstants.DateTimeFormat, value.ToUniversalTime())
                : noDateText;
        }
        /// <summary>
        /// Provides an extension method to change the string value to a
        /// default value if the string value is null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">
        /// The default value to substitute when the <i>value</i> is <b>null</b>.
        /// </param>
        /// <returns>
        /// The original string value if not null, or the specified default value if
        /// the original value is null.
        /// </returns>
        public static string ToDefaultStringIfNull(this string value, string defaultValue)
        {
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }
        /// <summary>
        /// Provides an extension method to change the string value to a
        /// <see cref="string.Empty"/> value if the string value is null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The original string value if not null, or an empty string if
        /// the original value is null.
        /// </returns>
        public static string ToEmptyStringIfNull(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }
        /// <summary>
        /// Returns the formatted date string if the date value is not a null
        /// date; otherwise, returns the specified no date value.
        /// </summary>
        /// <param name="value">
        /// The <see cref="DateTimeOffset"/> value to be evaluated.
        /// </param>
        /// <returns>
        /// The <i>value</i> formatted as a date if the date is not null;
        /// otherwise, returns an empty string.
        /// </returns>
        public static string ToNoDateTextIfNull(this DateTimeOffset value)
        {
            return value.ToUniversalTime().Year <= 1900
                ? string.Empty
                : string.Format(CultureInfo.InvariantCulture, AiConstants.DateFormat, value.ToUniversalTime());
        }
        /// <summary>
        /// Returns the formatted date/time string if the date value is not a null
        /// date; otherwise, returns the specified no date value.
        /// </summary>
        /// <param name="value">
        /// The <see cref="DateTimeOffset"/> value to be evaluated.
        /// </param>
        /// <returns>
        /// The <i>value</i> formatted as a date/time if the date is not null;
        /// otherwise, returns an empty string.
        /// </returns>
        public static string ToNoDateTextIfNull(this DateTime value)
        {
            return value.ToUniversalTime().Year == 1900
                ? string.Empty
                : string.Format(CultureInfo.InvariantCulture, AiConstants.DateTimeFormat, value.ToUniversalTime());
        }
        /// <summary>
        /// Translates the specified date/time value to the local user's time.
        /// </summary>
        /// <param name="value">
        /// The <see cref="DateTimeOffset"/> value to be translated.
        /// </param>
        /// <returns>
        /// The <see cref="DateTimeOffset"/> for the local time.
        /// </returns>
        public static DateTimeOffset ToUserTimeZone(this DateTimeOffset value)
        {
            return value.Offset.Hours == 0
                ? value.AddHours(TimeZoneInfo.Utc.GetUtcOffset(DateTime.Now).Hours)
                : value.AddHours(-1 * value.Offset.Hours);
        }
        /// <summary>
        /// Translates the specified date/time value to the local user's time.
        /// </summary>
        /// <param name="value">
        /// The <see cref="DateTimeOffset"/> value to be translated.
        /// </param>
        /// <param name="offset">
        /// An integer specifying the user's time zone offset in hours.
        /// </param>
        /// <returns>
        /// The <see cref="DateTimeOffset"/> for the local time.
        /// </returns>
        public static DateTimeOffset ToUserTimeZone(this DateTimeOffset value, int offset)
        {
            return value.AddHours(-1 * offset);
        }
        /// <summary>
        /// Translates the specified date/time value to the local user's time.
        /// </summary>
        /// <param name="value">
        /// The <see cref="DateTimeOffset"/> value to be translated.
        /// </param>
        /// <param name="offset">
        /// A string specifying the user's time zone offset in hours.
        /// </param>
        /// <returns>
        /// The <see cref="DateTimeOffset"/> for the local time.
        /// </returns>
        public static DateTimeOffset ToUserTimeZone(this DateTimeOffset value, string offset)
        {
            //bug fix 4394
            return value.AddHours(Convert.ToInt32(offset, CultureInfo.InvariantCulture));
        }
        /// <summary>
        /// Translates the specified list of string values for inclusion in a SQL query string.
        /// </summary>
        /// <param name="valueList">
        /// A <see cref="List{T}"/> of <see cref="string"/> containing the values to translate, or <b>null</b>.</param>
        /// <returns>
        /// The text "NULL" if the value is <b>null</b>, or a single-quoted character string
        /// containing a comma-delimited list of the string value items.
        /// </returns>
        public static string ToSqlListString(List<string>? valueList)
        {
            string sql = AiConstants.NullText;

            // If the list is null or empty, return "NULL",
            // else ...
            if (valueList != null && valueList.Count > 0)
            {
                StringBuilder builder = new(1000);

                // Append each element to the SQL string with single quotes surrounding
                // the entire string (instead of each element), where is element is followed
                // delimited by a comma, except for the last element.
                int length = valueList.Count;
                for (int count = 0; count < length - 1; count++)
                {
                    builder.Append(valueList[count]);
                    builder.AppendComma();
                }

                // Append the last element with a closing quote and no comma.
                builder.Append(valueList[length - 1]);
                sql = builder.ToString();
            }

            return sql;
        }
        /// <summary>
        /// Splits the larger list into smaller lists of the specified size.
        /// </summary>
        /// <typeparam name="T">
        /// The data type contained in the lists.
        /// </typeparam>
        /// <param name="originalList">The original list.</param>
        /// <param name="subListSize">
        /// An integer specifying the maximum size of the sub list.
        /// </param>
        /// <returns>
        /// A <see cref="List{T}"/> of <see cref="List{T}"/>s.
        /// </returns>
        public static List<List<T>> SplitList<T>(List<T> originalList, int subListSize)
        {
            List<List<T>> returnList = [];

            int index = 0;
            int length = originalList.Count;

            List<T>? currentSubList = null;
            if (originalList.Count > 0)
            {
                do
                {
                    currentSubList ??= [];

                    currentSubList.Add(originalList[index]);

                    if (currentSubList.Count == subListSize)
                    {

                        returnList.Add(currentSubList);
                        currentSubList = null;
                    }

                    index++;
                } while (index < length);

                if (currentSubList != null)
                {
                    returnList.Add(currentSubList);
                }
            }
            return returnList;
        }
        /// <summary>
        /// Converts the specified array to a <see cref="MemoryStream"/>.
        /// </summary>
        /// <param name="originalData">
        /// A byte array containing the original data.
        /// </param>
        /// <returns>
        /// A <see cref="MemoryStream"/> containing a copy of <i>originalData</i>
        /// </returns>
        public static MemoryStream ToStream(byte[] originalData)
        {
            return new MemoryStream(originalData);
        }
        /// <summary>
        /// Converts the specified array to a <see cref="MemoryStream"/>.
        /// </summary>
        /// <param name="originalData">
        /// A string containing the original data.
        /// </param>
        /// <returns>
        /// A <see cref="MemoryStream"/> containing a copy of <i>originalData</i>
        /// </returns>
        public static MemoryStream ToStream(string originalData)
        {
            MemoryStream newStream = new();
            StreamWriter writer = new(newStream);

            try
            {
                writer.Write(originalData);
                writer.Flush();
                newStream.Seek(0, SeekOrigin.Begin);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
                newStream.Dispose();
            }

            return newStream;
        }
        #endregion
    }
}