using Adaptive.Intelligence.Csv.Exceptions;
using System.Globalization;
using System.Reflection;

namespace Adaptive.Intelligence.Csv;

/// <summary>
/// Provides static methods and functions for converting data types when reading from or writing to CSV files.
/// </summary>
public static class CsvTypeConverter
{
    #region Main Conversion Methods / Functions
    /// <summary>
    /// Converts the specified source string to the specified data type.
    /// </summary>
    /// <typeparam name="T">
    /// The data type to convert the value to.
    /// </typeparam>
    /// <param name="sourceData">
    /// A string containing the source data to be converted.
    /// </param>
    /// <returns>
    /// A value of <typeparamref name="T"/> if the conversion is sucessful; or <b>null</b> of the parameter is 
    /// <b>null</b> or the conversion fails.
    /// </returns>
    public static T? Convert<T>(string sourceData)
    {
        T? returnValue;

        switch (Type.GetTypeCode(typeof(T)))
        {
            case TypeCode.String:
                returnValue = (T?)(object?)ToString(sourceData);
                break;

            case TypeCode.Object:
                returnValue = (T?)(object)sourceData;
                break;

            case TypeCode.Byte:
                returnValue = (T?)(object?)ToByte(sourceData);
                break;

            case TypeCode.SByte:
                returnValue = (T?)(object?)ToSByte(sourceData);
                break;

            case TypeCode.Boolean:
                returnValue = (T?)(object?)ToBoolean(sourceData);
                break;

            case TypeCode.Char:
                returnValue = (T?)(object?)ToChar(sourceData);
                break;

            case TypeCode.DateTime:
                returnValue = (T?)(object?)ToDateTime(sourceData);
                break;

            case TypeCode.DBNull:
                returnValue = default;
                break;

            case TypeCode.Decimal:
                returnValue = (T?)(object?)ToDecimal(sourceData);
                break;

            case TypeCode.Double:
                returnValue = (T?)(object?)ToDouble(sourceData);
                break;

            case TypeCode.Int16:
                returnValue = (T?)(object?)ToInt16(sourceData);
                break;

            case TypeCode.Int32:
                returnValue = (T?)(object?)ToInt32(sourceData);
                break;

            case TypeCode.Int64:
                returnValue = (T?)(object?)ToInt64(sourceData);
                break;

            case TypeCode.UInt16:
                returnValue = (T?)(object?)ToUInt16(sourceData);
                break;

            case TypeCode.UInt32:
                returnValue = (T?)(object?)ToUInt32(sourceData);
                break;

            case TypeCode.UInt64:
                returnValue = (T?)(object?)ToUInt64(sourceData);
                break;

            case TypeCode.Single:
                returnValue = (T?)(object?)ToSingle(sourceData);
                break;
            case TypeCode.Empty:
            default:
                throw new InvalidDataTypeException(typeof(T));
        }

        return returnValue;
    }

    /// <summary>
    /// Converts the specified source string to the data type of the specified property.
    /// </summary>
    /// <param name="property">
    /// A <see cref="PropertyInfo"/> containing the data type of the property a value is being converted to.
    /// </param>
    /// <param name="sourceData">
    /// A string containing the source data to be converted.
    /// </param>
    /// <returns>
    /// A boxed <see cref="object"/> if the conversion is sucessful; or <b>null</b> of the parameter is 
    /// <b>null</b> or the conversion fails.
    /// </returns>
    public static object? ConvertType(string sourceData, PropertyInfo property)
    {
        Type dataType = property.PropertyType;
        Type? actualType = Nullable.GetUnderlyingType(dataType);
        if (actualType != null)
        {
            dataType = actualType;
        }

        object? returnValue;
        switch (Type.GetTypeCode(dataType))
        {
            case TypeCode.String:
                returnValue = (object?)ToString(sourceData);
                break;

            case TypeCode.Byte:
                returnValue = (object?)ToByte(sourceData);
                break;

            case TypeCode.SByte:
                returnValue = (object?)ToSByte(sourceData);
                break;

            case TypeCode.Boolean:
                returnValue = (object?)ToBoolean(sourceData);
                break;

            case TypeCode.Char:
                returnValue = (object?)ToChar(sourceData);
                break;

            case TypeCode.DateTime:
                returnValue = (object?)ToDateTime(sourceData);
                break;

            case TypeCode.DBNull:
                returnValue = null;
                break;

            case TypeCode.Decimal:
                returnValue = (object?)ToDecimal(sourceData);
                break;

            case TypeCode.Double:
                returnValue = (object?)ToDouble(sourceData);
                break;

            case TypeCode.Int16:
                returnValue = (object?)ToInt16(sourceData);
                break;

            case TypeCode.Int32:
                returnValue = (object?)ToInt32(sourceData);
                break;

            case TypeCode.Int64:
                returnValue = (object?)ToInt64(sourceData);
                break;

            case TypeCode.UInt16:
                returnValue = (object?)ToUInt16(sourceData);
                break;

            case TypeCode.UInt32:
                returnValue = (object?)ToUInt32(sourceData);
                break;

            case TypeCode.UInt64:
                returnValue = (object?)ToUInt64(sourceData);
                break;

            case TypeCode.Single:
                returnValue = (object?)ToSingle(sourceData);
                break;

            case TypeCode.Object:
                returnValue = (object?)sourceData;
                break;

            default:
                throw new InvalidDataTypeException(dataType);
        }
        return returnValue;

    }
    #endregion

    #region Public Typed Conversion Methods

    /// <summary>
    /// Converts the original value to a byte value.
    /// </summary>
    /// <param name="sourceData">
    /// A string containing the source data read from the CSV cell.
    /// </param>
    /// <returns>
    /// A byte value representing the original data, or <b>null</b>.
    /// </returns>
    public static byte? ToByte(string? sourceData)
    {
        byte? result = null;

        if (!string.IsNullOrEmpty(sourceData))
        {
            if (byte.TryParse(sourceData, out byte parseResult))
            {
                result = parseResult;
            }
            else
            {
                try
                {
                    result = System.Convert.ToByte(sourceData, CultureInfo.CurrentCulture);
                }
                catch
                {
                    result = null;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Converts the original value to a boolean value.
    /// </summary>
    /// <param name="sourceData">
    /// A string containing the source data read from the CSV cell.
    /// </param>
    /// <returns>
    /// A boolean value representing the original data, or <b>null</b>.
    /// </returns>
    public static bool? ToBoolean(string? sourceData)
    {
        bool? result = null;

        if (!string.IsNullOrEmpty(sourceData))
        {
            if (bool.TryParse(sourceData, out bool parseResult))
            {
                result = parseResult;
            }
            else
            {
                try
                {
                    result = System.Convert.ToBoolean(sourceData);
                }
                catch
                {
                    result = null;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Converts the original value to a character value.
    /// </summary>
    /// <param name="sourceData">
    /// A string containing the source data read from the CSV cell.
    /// </param>
    /// <returns>
    /// A character value representing the original data, or <b>null</b>.
    /// </returns>
    public static char? ToChar(string? sourceData)
    {
        char? result = null;

        if (!string.IsNullOrEmpty(sourceData))
        {
            if (char.TryParse(sourceData, out char newChar))
            {
                result = newChar;
            }
            else
            {
                try
                {
                    result = System.Convert.ToChar(sourceData);
                }
                catch
                {
                    result = null;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Converts the original value to a date/time value.
    /// </summary>
    /// <param name="sourceData">
    /// A string containing the source data read from the CSV cell.
    /// </param>
    /// <returns>
    /// A <see cref="DateTime"/> value representing the original data, or <b>null</b>.
    /// </returns>
    public static DateTime? ToDateTime(string? sourceData)
    {
        DateTime? result = null;

        if (!string.IsNullOrEmpty(sourceData))
        {
            if (DateTime.TryParse(sourceData, out DateTime parseResult))
            {
                result = parseResult;
            }
            else
            {
                try
                {
                    result = System.Convert.ToDateTime(sourceData, CultureInfo.CurrentCulture);
                }
                catch
                {
                    result = null;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Converts the original value to a Decimal value.
    /// </summary>
    /// <param name="sourceData">
    /// A string containing the source data read from the CSV cell.
    /// </param>
    /// <returns>
    /// A <see cref="decimal"/> value representing the original data, or <b>null</b>.
    /// </returns>
    public static decimal? ToDecimal(string? sourceData)
    {
        decimal? result = null;

        if (!string.IsNullOrEmpty(sourceData))
        {
            if (decimal.TryParse(sourceData, out decimal parseResult))
            {
                result = parseResult;
            }
            else
            {
                try
                {
                    result = System.Convert.ToDecimal(sourceData, CultureInfo.CurrentCulture);
                }
                catch
                {
                    result = null;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Converts the original value to a double-precision floating point value.
    /// </summary>
    /// <param name="sourceData">
    /// A string containing the source data read from the CSV cell.
    /// </param>
    /// <returns>
    /// A <see cref="double"/> value representing the original data, or <b>null</b>.
    /// </returns>
    public static double? ToDouble(string? sourceData)
    {
        double? result = null;

        if (!string.IsNullOrEmpty(sourceData))
        {
            if (double.TryParse(sourceData, out double parseResult))
            {
                result = parseResult;
            }
            else
            {
                try
                {
                    result = System.Convert.ToDouble(sourceData, CultureInfo.CurrentCulture);
                }
                catch
                {
                    result = null;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Converts the original value to a short integer value.
    /// </summary>
    /// <param name="sourceData">
    /// A string containing the source data read from the CSV cell.
    /// </param>
    /// <returns>
    /// A <see cref="short"/> value representing the original data, or <b>null</b>.
    /// </returns>
    public static short? ToInt16(string? sourceData)
    {
        short? result = null;

        if (!string.IsNullOrEmpty(sourceData))
        {
            if (short.TryParse(sourceData, out short parseResult))
            {
                result = parseResult;
            }
            else
            {
                try
                {
                    result = System.Convert.ToInt16(sourceData, CultureInfo.CurrentCulture);
                }
                catch
                {
                    result = null;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Converts the original value to an integer value.
    /// </summary>
    /// <param name="sourceData">
    /// A string containing the source data read from the CSV cell.
    /// </param>
    /// <returns>
    /// A <see cref="int"/> value representing the original data, or <b>null</b>.
    /// </returns>
    public static int? ToInt32(string? sourceData)
    {
        int? result = null;

        if (!string.IsNullOrEmpty(sourceData))
        {
            if (int.TryParse(sourceData, out int parseResult))
            {
                result = parseResult;
            }
            else
            {
                try
                {
                    result = System.Convert.ToInt32(sourceData, CultureInfo.CurrentCulture);
                }
                catch
                {
                    result = null;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Converts the original value to a long integer value.
    /// </summary>
    /// <param name="sourceData">
    /// A string containing the source data read from the CSV cell.
    /// </param>
    /// <returns>
    /// A <see cref="long"/> value representing the original data, or <b>null</b>.
    /// </returns>
    public static long? ToInt64(string? sourceData)
    {
        long? result = null;

        if (!string.IsNullOrEmpty(sourceData))
        {
            if (long.TryParse(sourceData, out long parseResult))
            {
                result = parseResult;
            }
            else
            {
                try
                {
                    result = System.Convert.ToInt64(sourceData, CultureInfo.CurrentCulture);
                }
                catch
                {
                    result = null;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Converts the original value to an unsigned short integer value.
    /// </summary>
    /// <param name="sourceData">
    /// A string containing the source data read from the CSV cell.
    /// </param>
    /// <returns>
    /// A <see cref="ushort"/> value representing the original data, or <b>null</b>.
    /// </returns>
    public static ushort? ToUInt16(string? sourceData)
    {
        ushort? result = null;

        if (!string.IsNullOrEmpty(sourceData))
        {
            if (ushort.TryParse(sourceData, out ushort parseResult))
            {
                result = parseResult;
            }
            else
            {
                try
                {
                    result = System.Convert.ToUInt16(sourceData, CultureInfo.CurrentCulture);
                }
                catch
                {
                    result = null;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Converts the original value to an unsigned integer value.
    /// </summary>
    /// <param name="sourceData">
    /// A string containing the source data read from the CSV cell.
    /// </param>
    /// <returns>
    /// A <see cref="uint"/> value representing the original data, or <b>null</b>.
    /// </returns>
    public static uint? ToUInt32(string? sourceData)
    {
        uint? result = null;

        if (!string.IsNullOrEmpty(sourceData))
        {
            if (uint.TryParse(sourceData, out uint parseResult))
            {
                result = parseResult;
            }
            else
            {
                try
                {
                    result = System.Convert.ToUInt32(sourceData, CultureInfo.CurrentCulture);
                }
                catch
                {
                    result = null;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Converts the original value to an unsigned long integer value.
    /// </summary>
    /// <param name="sourceData">
    /// A string containing the source data read from the CSV cell.
    /// </param>
    /// <returns>
    /// A <see cref="ulong"/> value representing the original data, or <b>null</b>.
    /// </returns>
    public static ulong? ToUInt64(string? sourceData)
    {
        ulong? result = null;

        if (!string.IsNullOrEmpty(sourceData))
        {
            if (ulong.TryParse(sourceData, out ulong parseResult))
            {
                result = parseResult;
            }
            else
            {
                try
                {
                    result = System.Convert.ToUInt64(sourceData, CultureInfo.CurrentCulture);
                }
                catch
                {
                    result = null;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Converts the original value to a signed byte value.
    /// </summary>
    /// <param name="sourceData">
    /// A string containing the source data read from the CSV cell.
    /// </param>
    /// <returns>
    /// A signed byte value representing the original data, or <b>null</b>.
    /// </returns>
    public static sbyte? ToSByte(string? sourceData)
    {
        sbyte? result = null;

        if (!string.IsNullOrEmpty(sourceData))
        {
            if (sbyte.TryParse(sourceData, out sbyte parseResult))
            {
                result = parseResult;
            }
            else
            {
                try
                {
                    result = System.Convert.ToSByte(sourceData, CultureInfo.CurrentCulture);
                }
                catch
                {
                    result = null;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Converts the original value to a single-precision floating point value.
    /// </summary>
    /// <param name="sourceData">
    /// A string containing the source data read from the CSV cell.
    /// </param>
    /// <returns>
    /// A <see cref="float"/> value representing the original data, or <b>null</b>.
    /// </returns>
    public static float? ToSingle(string? sourceData)
    {
        float? result = null;

        if (!string.IsNullOrEmpty(sourceData))
        {
            if (float.TryParse(sourceData, out float parseResult))
            {
                result = parseResult;
            }
            else
            {
                try
                {
                    result = System.Convert.ToSingle(sourceData, CultureInfo.CurrentCulture);
                }
                catch
                {
                    result = null;
                }
            }
        }
        return result;
    }

    /// <summary>
    /// Converts the original value to a string.
    /// </summary>
    /// <param name="sourceData">
    /// A string containing the source data read from the CSV cell.
    /// </param>
    /// <returns>
    /// A string representing the original data, or <b>null</b>.
    /// </returns>
    public static string? ToString(string? sourceData)
    {
        return sourceData;
    }
    #endregion

}
