using Adaptive.Intelligence.Abstractions.Logging;
using Adaptive.Intelligence.Csv.Attributes;
using Adaptive.Intelligence.Csv.Exceptions;
using Adaptive.Intelligence.Csv.Metadata;
using Adaptive.Intelligence.Csv.Reflection;
using System.Reflection;
using System.Text;

namespace Adaptive.Intelligence.Csv;

/// <summary>
/// Performs the task of writing CSV content.
/// </summary>
/// <seealso cref="LoggableBase" />
public class CsvWriter : LoggableBase
{
    #region Private Member Declarations
    /// <summary>
    /// The stream to be read from.
    /// </summary>
    private Stream? _stream;

    /// <summary>
    /// The text writer instance.
    /// </summary>
    private StreamWriter? _writer;

    /// <summary>
    /// The CSV delimiter to use.
    /// </summary>
    private char _delimiter = ',';

    /// <summary>
    /// The list of column metadata.
    /// </summary>
    private List<CsvColumnInfo>? _columns;

    /// <summary>
    /// The cached builder instance.
    /// </summary>
    private StringBuilder? _cachedBuilder;
    #endregion

    #region Constructor / Dispose Methods
    /// <summary>
    /// Initializes a new instance of the <see cref="CsvWriter"/> class.
    /// </summary>
    /// <param name="destinationStream">
    /// The reference to the destination <see cref="Stream"/> to write data to.
    /// </param>
    public CsvWriter(Stream destinationStream)
    {
        _stream = destinationStream;
        _stream.Seek(0, SeekOrigin.Begin);
        CreateWriter();
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
    /// <b>false</b> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
        if (!IsDisposed && disposing)
        {
            Close();
        }

        _columns = null;
        _writer = null;
        _stream = null;
        _cachedBuilder = null;

        base.Dispose(disposing);
    }
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets or sets the delimiter to use when separating cells of data.
    /// </summary>
    /// <value>
    /// A <see cref="char"/> to use as the delimiter.  Usually a comma.
    /// </value>
    public char Delimiter
    {
        get => _delimiter;
        set
        {
            if (value != _delimiter)
            {
                _delimiter = value;
            }
        }
    }

    /// <summary>
    /// Gets the current position in the underlying stream.
    /// </summary>
    /// <value>
    /// A <see cref="long"/> specifying the current postion.
    /// </value>
    public long Position
    {
        get
        {
            if (_stream == null)
            {
                return -1;
            }
            else
            {
                return _stream.Position;
            }
        }
    }
    #endregion

    #region Public Methods / Functions
    /// <summary>
    /// Closes and disposes of the underlying streams and reader instances.
    /// </summary>
    public void Close()
    {
        _columns?.Clear();
        _writer?.Dispose();
        _cachedBuilder?.Clear();
    }

    /// <summary>
    /// Writes the next line of text to the underlying stream.
    /// </summary>
    /// <remarks>
    /// Override this method to utilize a custom algorithm for writing the content.
    /// </remarks>
    /// <param name="writer">
    /// The reference to the <see cref="StreamWriter"/> used to write the text.
    /// </param>
    /// <param name="lineData">
    /// A <see cref="List{T}"/> of <see cref="string"/> containing the data elements to be writen.
    /// </param>
    public virtual void WriteCsvLine(StreamWriter writer, List<string>? lineData)
    {
        if (writer != null && lineData != null && lineData.Count > 0)
        {
            string lineText = string.Join(_delimiter, lineData);
            writer.WriteLine(lineText);
        }
    }

    /// <summary>
    /// Writes first line of text as a list of column header names.
    /// </summary>
    /// <param name="headerNames">
    /// A <see cref="List{T}"/> of <see cref="string"/> instances, or <b>null</b>.
    /// </param>
    public void WriteHeader(List<string>? headerNames)
    {
        if (_stream != null && _writer != null && headerNames != null)
        {
            _stream.Position = 0;
            string header = ConcatenateCsvValues(headerNames);
            _writer.WriteLine(header);
        }
    }

    /// <summary>
    /// Writes the individual data row to to the underlying stream.
    /// </summary>
    /// <param name="contentToWrite">
    /// A <see cref="List{T}"/> of <see cref="string"/> values containing the content to write.
    /// </param>
    /// <exception cref="NullStreamException">
    /// Thrown if the underlying <see cref="Stream"/> is <b>null</b>.
    /// </exception>
    /// <exception cref="CantWriteStreamException">
    /// Thrown if the underlying <see cref="StreamWriter"/> is <b>null</b> or can't be written to.
    /// </exception>
    public void WriteDataRow(List<string>? contentToWrite)
    {
        if (contentToWrite != null)
        {
            if (_stream == null)
            {
                throw new NullStreamException();
            }
            else if (!_stream.CanWrite || _writer == null)
            {
                throw new CantWriteStreamException();
            }
            else
            {
                string dataLine = ConcatenateCsvValues(contentToWrite);
                _writer.WriteLine(dataLine);
                _writer.Flush();
            }
        }
    }
    /// <summary>
    /// Writes the data rows to the underlying CSV file.
    /// </summary>
    /// <param name="dataRows">
    /// A <see cref="List{T}"/> containing a <see cref="List{T}"/> of <see cref="string"/> for each data row.
    /// </param>
    /// <param name="hasHeader">
    /// <b>true</b> if the first line in the file is a list of column headers; otherwise, <b>false</b>.
    /// </param>
    /// <exception cref="NullStreamException">
    /// Thrown if the underlying <see cref="Stream"/> instance is <b>null</b>.
    /// </exception>
    /// <exception cref="CantWriteStreamException">
    /// Thrown if the underlying <see cref="Stream"/> cannot be written to.
    /// </exception>
    public void WriteRawDataRows(List<List<string>>? dataRows, bool hasHeader = false)
    {
        // Ensure we can read from the stream.
        if (_stream == null)
        {
            throw new NullStreamException();
        }
        else if (!_stream.CanWrite)
        {
            throw new CantWriteStreamException();
        }
        else
        {
            _stream.Seek(0, SeekOrigin.Begin);

            if (_writer != null)
            {
                if (dataRows != null && dataRows.Count > 0)
                {
                    int index = 0;
                    if (hasHeader)
                    {
                        WriteHeader(dataRows[0]);
                        index++;
                    }
                    int length = dataRows.Count;
                    for (int rowIndex = index; rowIndex < length; rowIndex++)
                    {
                        WriteDataRow(dataRows[rowIndex]);
                    }
                }
            }
            _writer?.Flush();
            _stream.Flush();
        }
    }

    /// <summary>
    /// Writes the provided object instances as CSV rows to the underlying stream.
    /// </summary>
    /// <typeparam name="T">
    /// The type of object to serialize to CSV rows.
    /// </typeparam>
    /// <param name="instances">
    /// A <see cref="List{T}"/> containing the objects for each data row.
    /// </param>
    /// <param name="hasHeader">
    /// <b>true</b> if the first line in the output is a list of column headers; otherwise, <b>false</b>.
    /// </param>
    /// <exception cref="NullStreamException">
    /// Thrown if the underlying <see cref="Stream"/> instance is <b>null</b>.
    /// </exception>
    /// <exception cref="CantWriteStreamException">
    /// Thrown if the underlying <see cref="Stream"/> cannot be written to.
    /// </exception>
    public void WriteDataRows<T>(List<T> instances, bool hasHeader = false)
    {
        // Read the raw CSV content.
        if (instances != null)
        {
            // Get the cached reflection data for the specified type, and if not 
            // yet created, create and cache it.
            UserTypeCacheInstance<T>? typeData = UserTypeCache.GetOrCreate<T>() ?? throw new ParserEngineException("Could not cache the specified data type.");

            // Update the meta data and order the columns as needed.
            CreateColumnProfiles(typeData.OrderedProperties);

            if (_columns != null)
            {
                typeData.UpdateCsvColumnInfo(_columns);
                List<CsvColumnInfo> orderedColumns = [.. _columns.OrderBy(x => x.Index)];

                if (hasHeader)
                {
                    List<string> headerNames = GetHeaderNamesList(orderedColumns);
                    WriteHeader(headerNames);
                    headerNames.Clear();
                }
                int length = instances.Count;

                // Copy the text into the appropriate property of the object.
                int index = 0;
                while (index < length)
                {
                    List<string>? lineData = ParseObject<T>(instances[index]);
                    if (lineData != null)
                    {
                        WriteDataRow(lineData);
                    }
                    index++;
                }
                _writer?.Flush();

            }
        }
    }
    #endregion

    #region Private Methods / Functions
    /// <summary>
    /// Extracts the metadata for the columns.
    /// </summary>
    /// <param name="propertyList">
    /// An <see cref="List{T}"/> of <see cref="PropertyInfo"/> instances describing the columns in the 
    /// CSV file.
    /// </param>
    private void CreateColumnProfiles(List<PropertyInfo>? propertyList)
    {
        _columns?.Clear();
        _columns = [];

        if (propertyList != null)
        {
            int index = 0;
            foreach (PropertyInfo property in propertyList)
            {
                CsvColumnInfo info = new()
                {
                    ColumnName = property.Name,
                    HeaderText = property.Name,
                    PropertyData = property,
                    Index = index
                };
                HeaderNameAttribute? nameAttribute = property.GetCustomAttribute<HeaderNameAttribute>();
                if (nameAttribute != null)
                {
                    info.HeaderText = nameAttribute.HeaderName;
                }
                _columns.Add(info);
                index++;
            }
        }
    }
    /// <summary>
    /// Concatenates the values in the provided list into a single string separated by the specified delimiter.
    /// </summary>
    /// <remarks>
    /// This method encloses non-standard data elements (for example those containing the delimiter,
    /// quote characters, carriage return, or line feed) in double quotes and escapes internal quotes.
    /// </remarks>
    /// <param name="dataElements">
    /// A <see cref="List{T}"/> of <see cref="string"/> containing the data elements to combine.
    /// </param>
    /// <returns>
    /// A CSV-formatted line representing the provided values.
    /// </returns>
    private string ConcatenateCsvValues(List<string> dataElements)
    {
        StringBuilder builder = CreateCachedBuilder(dataElements.Count);

        for (int index = 0; index < dataElements.Count; index++)
        {
            string element = dataElements[index];

            // Single SIMD-friendly scan: delimiter, quote, CR, LF.
            bool mustQuote = element.AsSpan().IndexOfAny(_delimiter, '"', '\n') >= 0 || element.Contains('\r');

            if (mustQuote)
            {
                builder.Append('"');
                // Keep existing escape semantics.
                builder.Append(element.Replace("\"", "\"\"", StringComparison.Ordinal));
                builder.Append('"');
            }
            else
            {
                builder.Append(element);
            }

            if (index < dataElements.Count - 1)
            {
                builder.Append(_delimiter);
            }
        }

        return builder.ToString();
    }

    /// <summary>
    /// Concatenates the values in the provided list into a single string separated by the specified delimiter.
    /// </summary>
    /// <remarks>
    /// This is the legacy implementation retained for compatibility/reference.
    /// </remarks>
    /// <param name="dataElements">
    /// A <see cref="List{T}"/> of <see cref="string"/> containing the data elements to combine.
    /// </param>
    /// <returns>
    /// A CSV-formatted line representing the provided values.
    /// </returns>
    private string OldConcatenateCsvValues(List<string> dataElements)
    {
        // Create a buffer large enough for the whole line.
        StringBuilder builder = CreateCachedBuilder(dataElements.Count);

        int length = dataElements.Count;
        for (int index = 0; index < length; index++)
        {
            string element = dataElements[index];
            if (element.Contains(_delimiter) || element.Contains('"'))
            {
                // Enclose the element in quotes and escape any existing quotes.
                string escapedElement = element.Replace("\"", "\"\"", StringComparison.Ordinal);
                builder.Append('"');
                builder.Append(escapedElement);
                builder.Append('"');
            }
            else
            {
                builder.Append(element);
            }
            if (index < length - 1)
            {
                builder.Append(_delimiter);
            }
        }
        string line = builder.ToString();
        builder.Clear();
        return line;
    }
    /// <summary>
    /// Extracts the metadata for the columns.
    /// </summary>
    /// <param name="headerNames">
    /// An array of strings containing the column header names.
    /// </param>
    private void xCreateColumnProfiles(string[] headerNames)
    {
        _columns?.Clear();
        _columns = [];

        int index = 0;
        foreach (string headerText in headerNames)
        {
            _columns.Add(new CsvColumnInfo()
            {
                ColumnName = headerText.Replace(" ", string.Empty, StringComparison.Ordinal),
                HeaderText = headerText,
                Index = index
            });
            index++;
        }
    }

    /// <summary>
    /// Creates the text writer instance used to writer the data content.
    /// </summary>
    /// <exception cref="CreateWriterFailedException">
    /// Thrown if the attempt to create the writer instance fails.
    /// </exception>
    private void CreateWriter()
    {
        if (_stream != null)
        {
            try
            {
                // Open the reader from the source stream, and leave the underlying stream open and ready for use
                // when the reader is closed/disposed.
                _writer = new StreamWriter(
                    _stream,
                    leaveOpen: true);
            }
            catch (Exception ex)
            {
                _writer = null;
                throw new CreateWriterFailedException(ex);
            }
        }
    }

    /// <summary>
    /// Creates or references the cached builder instance.
    /// </summary>
    /// <param name="itemsCount">
    /// AN integer specifying the number of columns / items in the CSV data.
    /// </param>
    /// <returns>
    /// The refernece to the <see cref="StringBuilder"/> instance to use.
    /// </returns>
    private StringBuilder CreateCachedBuilder(int itemsCount)
    {
        if (_cachedBuilder == null)
        {
            _cachedBuilder = new StringBuilder(itemsCount * 65536);
        }
        else
        {
            _cachedBuilder.Clear();
        }

        return _cachedBuilder;
    }

    /// <summary>
    /// Gets the list of column header names in order.
    /// </summary>
    /// <param name="orderedList">
    /// A <see cref="List{T}"/> of <see cref="CsvColumnInfo"/> instances containing the column data.
    /// </param>
    /// <returns>
    /// A <see cref="List{T}"/> of <see cref="string"/> instances containing the header names.
    /// </returns>
    private static List<string> GetHeaderNamesList(List<CsvColumnInfo> orderedList)
    {
        List<string> namesList = new(orderedList.Count);
        for (int index = 0; index < orderedList.Count; index++)
        {
            string? value = (orderedList[index].HeaderText ?? orderedList[index].ColumnName) ?? string.Empty;
            namesList.Add(value);
        }
        return namesList;
    }

    /// <summary>
    /// Parses the provided object instance into a list of string values.
    /// </summary>
    /// <typeparam name="T">
    /// The data type of the object to be parsed into CSV values.
    /// </typeparam>
    /// <param name="objectInstance">
    /// The object instance of <typeparamref name="T"/> to be written.
    /// </param>
    /// <returns>
    /// A <see cref="List{T}"/> of <see cref="string"/> values, or <b>null</b>.
    /// </returns>
    private static List<string>? ParseObject<T>(T? objectInstance)
    {
        List<string>? valueList = null;

        // Get the cached reflection data for the specified type, and if not 
        // yet created, create and cache it.
        UserTypeCacheInstance<T>? typeData = UserTypeCache.GetOrCreate<T>();
        if (typeData == null)
        {
            throw new ParserEngineException("Could read the specified data type from the cache.");
        }

        if (typeData != null && typeData.OrderedProperties != null)
        {
            valueList = new List<string>(typeData.OrderedProperties.Count);
            foreach (PropertyInfo propData in typeData.OrderedProperties)
            {
                object? value = propData.GetValue(objectInstance);
                if (value == null)
                {
                    valueList.Add(string.Empty);
                }
                else
                {
                    valueList.Add(value.ToString() ?? string.Empty);
                }
            }
        }

        return valueList;
    }
    #endregion
}