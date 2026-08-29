using Adaptive.Intelligence.Abstractions.Logging;
using Adaptive.Intelligence.Csv.Exceptions;
using Adaptive.Intelligence.Csv.Metadata;
using Adaptive.Intelligence.Csv.Parser;
using Adaptive.Intelligence.Csv.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace Adaptive.Intelligence.Csv;

/// <summary>
/// Provides methods and functions for reading CSV (comma-separated values) data.
/// </summary>
/// <seealso cref="LoggableBase" />
public class CsvReader : LoggableBase
{
    #region Private Member Declarations
    /// <summary>
    /// The default memory size for each cell of data.
    /// </summary>
    private const int DefaultMemorySize = 65536;

    /// <summary>
    /// The stream to be read from.
    /// </summary>
    private Stream? _stream;

    /// <summary>
    /// The text reader instance.
    /// </summary>
    private StreamReader? _reader;

    /// <summary>
    /// The header flag.
    /// </summary>
    private bool _hasHeader;

    /// <summary>
    /// The CSV delimiter to use.
    /// </summary>
    private char _delimiter = ',';

    /// <summary>
    /// The list of column metadata.
    /// </summary>
    private List<CsvColumnInfo>? _columns;

    /// <summary>
    /// The parser instance.
    /// </summary>
    private ICsvParser? _parser;

    /// <summary>
    /// The maximum size of the data in a cell.
    /// </summary>
    private readonly int _cellMemorySize;
    #endregion

    #region Constructor / Dispose Methods    
    /// <summary>
    /// Initializes a new instance of the <see cref="CsvReader"/> class.
    /// </summary>
    /// <param name="sourceStream">
    /// The reference to the source <see cref="Stream"/> to read data from.
    /// </param>
    /// <param name="hasHeader">
    /// An optional parameter indicating whether the first row of the CSV file contains header names.
    /// </param>
    public CsvReader(Stream sourceStream, bool hasHeader = false) : this(sourceStream, DefaultMemorySize, hasHeader)
    {
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="CsvReader"/> class.
    /// </summary>
    /// <param name="sourceStream">
    /// The reference to the source <see cref="Stream"/> to read data from.
    /// </param>
    /// <param name="cellMemorySize">
    /// An integer specifying the maximum size of a cell of data.
    /// </param>
    /// <param name="hasHeader">
    /// An optional parameter indicating whether the first row of the CSV file contains header names.
    /// </param>
    public CsvReader(Stream sourceStream, int cellMemorySize, bool hasHeader = false)
    {
        _stream = sourceStream;
        _stream.Seek(0, SeekOrigin.Begin);
        _cellMemorySize = cellMemorySize;
        _hasHeader = hasHeader;
        _parser = CreateParser();
        CreateReader();
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
        _reader = null;
        _stream = null;
        _parser = null;

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
            // Re-create the parser if the delimiter changed.
            if (value != _delimiter)
            {
                _delimiter = value;
                _parser?.Dispose();
                _parser = new CsvParser(_delimiter, _cellMemorySize);
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether the first row of the CSV file contains header names.
    /// </summary>
    /// <value>
    /// <b>true</b> if the file has a header, otherwise <b>false</b>.
    /// </value>
    public bool HasHeader => _hasHeader;

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
        _reader?.Dispose();
        _parser?.Dispose();
    }

    /// <summary>
    /// Creates the CSV parser instance.
    /// </summary>
    /// <remarks>
    /// Override this method to substitue a DI-based implmentation of <see cref="ICsvParser"/>.
    /// </remarks>
    /// <returns>
    /// An <see cref="ICsvParser"/> instance.
    /// </returns>
    public virtual ICsvParser CreateParser()
    {
        return new CsvParser(_delimiter, _cellMemorySize);
    }

    /// <summary>
    /// Reads the next line of text from the underlying stream, and attempts to parse the content.
    /// </summary>
    /// <remarks>
    /// Override this method to utilize a custom algorithm for reading and parsing the content.
    /// </remarks>
    /// <param name="reader">
    /// The reference to the <see cref="StreamReader"/> used to read the text.
    /// </param>
    /// <param name="parser">
    /// The refernece to the <see cref="CsvParser"/> instance to use.
    /// </param>
    /// <returns>
    /// A <see cref="List{T}"/> of <see cref="string"/> containing the parsed cell values.
    /// </returns>
    public virtual List<string>? ReadAndParseCsvLine(StreamReader reader, ICsvParser parser)
    {
        List<string>? rowData = null;

        ParserEngineException.ThrowIfNull(parser);

        string? lineText = SafeReadLine(reader);
        if (lineText != null)
        {
            if (lineText == string.Empty)
            {
                rowData = [];
            }
            else
            {
                // Parse the data "row" to individual cells.
                rowData = parser.ParseLine(lineText);
            }
        }
        return rowData;
    }


    /// <summary>
    /// Reads the next line of text from the underlying stream, and attempts to parse the content.
    /// </summary>
    /// <remarks>
    /// Override this method to utilize a custom algorithm for reading and parsing the content.
    /// </remarks>
    /// <param name="reader">
    /// The reference to the <see cref="StreamReader"/> used to read the text.
    /// </param>
    /// <param name="parser">
    /// The refernece to the <see cref="CsvParser"/> instance to use.
    /// </param>
    /// <returns>
    /// A <see cref="List{T}"/> of <see cref="string"/> containing the parsed cell values.
    /// </returns>
    public virtual async Task<List<string>?> ReadAndParseCsvLineAsync(StreamReader reader, ICsvParser parser)
    {
        List<string>? rowData = null;

        _reader = reader;
        _parser = parser;
        string? lineText = await SafeReadLineAsync().ConfigureAwait(false);
        if (lineText != null)
        {
            if (lineText == string.Empty)
            {
                rowData = [];
            }
            else
            {
                // Parse the data "row" to individual cells.
                if (_parser != null)
                {
                    rowData = _parser.ParseLine(lineText);
                }
            }
        }
        return rowData;
    }

    /// <summary>
    /// Reads first line of text as a list of column header names.
    /// </summary>
    /// <returns>
    /// A <see cref="List{T}"/> of <see cref="string"/> instances, or <b>null</b>.
    /// </returns>
    public List<string>? ReadHeader()
    {
        List<string>? headerNames = null;

        if (_stream != null && _reader != null && _parser != null)
        {
            _stream.Position = 0;
            ReadAndParseHeader();

            if (_columns != null)
            {
                headerNames = new List<string>(_columns.Count);
                foreach (CsvColumnInfo info in _columns)
                {
                    headerNames.Add(info.HeaderText ?? string.Empty);
                }
            }
        }
        return headerNames;
    }

    /// <summary>
    /// Reads first line of text as a list of column header names.
    /// </summary>
    /// <returns>
    /// A <see cref="List{T}"/> of <see cref="string"/> instances, or <b>null</b>.
    /// </returns>
    public async Task<List<string>?> ReadHeaderAsync()
    {
        List<string>? headerNames = null;

        if (_stream != null && _reader != null && _parser != null)
        {
            _stream.Position = 0;
            await ReadAndParseHeaderAsync().ConfigureAwait(false);

            if (_columns != null)
            {
                headerNames = new List<string>(_columns.Count);
                foreach (CsvColumnInfo info in _columns)
                {
                    headerNames.Add(info.HeaderText ?? string.Empty);
                }
            }
        }
        return headerNames;
    }

    /// <summary>
    /// Reads the data rows from the underlying CSV file.
    /// </summary>
    /// <param name="hasHeader">
    /// <b>true</b> if the first line in the file is a list of column headers; otherwise, <b>false</b>.
    /// </param>
    /// <returns>
    /// A <see cref="List{T}"/> containing a <see cref="List{T}"/> of <see cref="string"/> for each data row.
    /// </returns>
    /// <exception cref="NullStreamException">
    /// Thrown if the underlying <see cref="Stream"/> instance is <b>null</b>.
    /// </exception>
    /// <exception cref="CantReadStreamException">
    /// Thrown if the underlying <see cref="Stream"/> cannot be read from.
    /// </exception>
    public List<List<string>>? ReadRawDataRows(bool hasHeader = false)
    {
        List<List<string>>? dataRows = null;

        // Ensure we can read from the stream and parse the text.
        NullStreamException.ThrowIfNull(_stream);
        CantReadStreamException.ThrowIfStreamCannotBeRead(_stream!);
        ParserEngineException.ThrowIfNull(_parser);

        _stream.Seek(0, SeekOrigin.Begin);
        _hasHeader = hasHeader;

        if (_reader != null)
        {
            dataRows = InnerReadCsvContent(hasHeader);
        }
        return dataRows;
    }


    /// <summary>
    /// Reads the data rows from the underlying CSV file.
    /// </summary>
    /// <param name="hasHeader">
    /// <b>true</b> if the first line in the file is a list of column headers; otherwise, <b>false</b>.
    /// </param>
    /// <returns>
    /// A <see cref="List{T}"/> containing a <see cref="List{T}"/> of <see cref="string"/> for each data row.
    /// </returns>
    /// <exception cref="NullStreamException">
    /// Thrown if the underlying <see cref="Stream"/> instance is <b>null</b>.
    /// </exception>
    /// <exception cref="CantReadStreamException">
    /// Thrown if the underlying <see cref="Stream"/> cannot be read from.
    /// </exception>
    public async Task<List<List<string>>?> ReadRawDataRowsAsync(bool hasHeader = false)
    {
        List<List<string>>? dataRows = null;

        // Ensure we can read from the stream.
        if (_stream == null)
        {
            throw new NullStreamException();
        }
        else if (!_stream.CanRead)
        {
            throw new CantReadStreamException();
        }
        else if (_parser == null)
        {
            throw new ParserEngineException();
        }
        else
        {
            _stream.Seek(0, SeekOrigin.Begin);
            _hasHeader = hasHeader;

            if (_reader != null)
            {
                dataRows = await InnerReadCsvContentAsync(hasHeader).ConfigureAwait(false);
            }
        }
        return dataRows;
    }

    /// <summary>
    /// Reads the data rows from the underlying CSV file into the specified object types.
    /// </summary>
    /// <param name="hasHeader">
    /// <b>true</b> if the first line in the file is a list of column headers; otherwise, <b>false</b>.
    /// </param>
    /// <returns>
    /// A <see cref="List{T}"/> containing the objects for each data row.
    /// </returns>
    /// <exception cref="NullStreamException">
    /// Thrown if the underlying <see cref="Stream"/> instance is <b>null</b>.
    /// </exception>
    /// <exception cref="CantReadStreamException">
    /// Thrown if the underlying <see cref="Stream"/> cannot be read from.
    /// </exception>
    public List<T>? ReadDataRows<T>(bool hasHeader = false)
    {
        List<T>? dataRows = null;

        NullStreamException.ThrowIfNull(_stream);

        // Read the raw CSV content.
        List<List<string>>? rawDataRows = InnerReadCsvContent(hasHeader);
        if (rawDataRows != null)
        {
            dataRows = new List<T>(rawDataRows.Count);

            // Get the cached reflection data for the specified type, and if not 
            // yet created, create and cache it.
            UserTypeCacheInstance<T>? typeData = UserTypeCache.GetOrCreate<T>() ?? throw new ParserEngineException("Could not cache the specified data type.");
            if (_columns != null)
            {
                // Update the meta data and order the columns as needed.
                typeData.UpdateCsvColumnInfo(_columns);
                List<CsvColumnInfo> orderedColumns = [.. _columns.OrderBy(x => x.Index)];

                int index = 0;
                int length = rawDataRows.Count;

                // Copy the text into the appropriate property of the object.
                while (index < length)
                {
                    List<string> lineData = rawDataRows[index];
                    T? instance = ProcessLineToObject<T>(orderedColumns, lineData);
                    if (instance != null)
                    {
                        dataRows.Add(instance);
                    }
                    index++;
                }
            }
        }

        return dataRows;
    }
    #endregion

    #region Private Methods / Functions
    /// <summary>
    /// Attempts to read and parse the first line of text to extract the column header names.
    /// </summary>
    /// <exception cref="ReadHeaderException">
    /// Thrown if an unexpected error occurs when reading and parsing the content.
    /// </exception>
    private void ReadAndParseHeader()
    {
        ParserEngineException.ThrowIfNull(_parser);
        string? line;
        try
        {
            line = _reader?.ReadLine();
        }
        catch (Exception ex)
        {
            throw new ReadHeaderException(ex);
        }

        if (line != null)
        {
            string[]? headerNames;
            try
            {
                ParserEngineException.ThrowIfNull(_parser);
                headerNames = _parser!.ParseLine(line)?.ToArray();
            }
            catch (Exception ex)
            {
                throw new ReadHeaderException(ex);
            }

            if (headerNames != null)
            {
                CreateColumnProfiles(headerNames);
            }
        }
    }

    /// <summary>
    /// Attempts to read and parse the first line of text to extract the column header names.
    /// </summary>
    /// <exception cref="ReadHeaderException">
    /// Thrown if an unexpected error occurs when reading and parsing the content.
    /// </exception>
    private async Task ReadAndParseHeaderAsync()
    {
        string? line;

        ParserEngineException.ThrowIfNull(_parser);
        if (_reader != null)
        {
            try
            {
                line = await _reader.ReadLineAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new ReadHeaderException(ex);
            }
            if (line != null)
            {
                string[]? headerNames;
                try
                {
                    headerNames = _parser?.ParseLine(line)?.ToArray();
                }
                catch (Exception ex)
                {
                    throw new ReadHeaderException(ex);
                }
                CreateColumnProfiles(headerNames);
            }
        }
    }

    /// <summary>
    /// Extracts the metadata for the columns.
    /// </summary>
    /// <param name="headerNames">
    /// An array of strings containing the column header names.
    /// </param>
    private void CreateColumnProfiles(string[] headerNames)
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
    /// Creates the text reader instance used to read the data content.
    /// </summary>
    /// <exception cref="CreateReaderFailedException">
    /// Thrown if the attempt to create the reader instance fails.
    /// </exception>
    private void CreateReader()
    {
        if (_stream != null)
        {
            try
            {
                // Open the reader from the source stream, using auto-detection of the
                // text encoding, and leave the underlying stream open and ready for use
                // when the reader is closed/disposed.
                _reader = new StreamReader(
                    _stream,
                    detectEncodingFromByteOrderMarks: true,
                    leaveOpen: true);
            }
            catch (Exception ex)
            {
                _reader = null;
                throw new CreateReaderFailedException(ex);
            }
        }
    }

    /// <summary>
    /// Reads the content from the underlying stream instance.
    /// </summary>
    /// <param name="hasHeader">
    /// <b>true</b> if the CSV content contains header rows; otherwise, <b>false</b>.
    /// </param>
    private List<List<string>>? InnerReadCsvContent(bool hasHeader)
    {
        List<List<string>>? dataRows = null;

        if (_stream != null && _reader != null && _parser != null)
        {
            _stream.Seek(0, SeekOrigin.Begin);
            _reader?.Close();
            CreateReader();

            if (hasHeader)
            {
                // Read the header column text.
                ReadAndParseHeader();
            }

            // Prepare the "row" container.
            dataRows = [];

            // Read each line of text, and parse the line into cells.
            List<string>? rowData = null;
            do
            {
                // Parse the data "row" to individual cells.
                if (_reader != null)
                {
                    rowData = ReadAndParseCsvLine(_reader, _parser);
                    if (rowData != null && rowData.Count > 0)
                    {
                        dataRows.Add(rowData);
                    }
                }
            } while (rowData != null);

        }
        return dataRows;
    }

    /// <summary>
    /// Reads the content from the underlying stream instance.
    /// </summary>
    /// <param name="hasHeader">
    /// <b>true</b> if the CSV content contains header rows; otherwise, <b>false</b>.
    /// </param>
    private async Task<List<List<string>>?> InnerReadCsvContentAsync(bool hasHeader)
    {
        List<List<string>>? dataRows = null;

        if (_stream != null && _reader != null && _parser != null)
        {
            _stream.Seek(0, SeekOrigin.Begin);
            _reader?.Close();
            CreateReader();

            if (hasHeader)
            {
                // Read the header column text.
                await ReadAndParseHeaderAsync().ConfigureAwait(false);
            }

            // Prepare the "row" container.
            dataRows = [];

            // Read each line of text, and parse the line into cells.
            if (_reader != null)
            {
                List<string>? rowData;
                do
                {
                    // Parse the data "row" to individual cells.
                    rowData = await ReadAndParseCsvLineAsync(_reader, _parser).ConfigureAwait(false);
                    if (rowData != null && rowData.Count > 0)
                    {
                        dataRows.Add(rowData);
                    }
                } while (rowData is not null);
            }

        }
        return dataRows;
    }

    /// <summary>
    /// Translates the line of CSV elements to the appropriate properties of the specified data type.
    /// </summary>
    /// <typeparam name="T">
    /// The data type of the object to create and populate.
    /// </typeparam>
    /// <param name="orderedColumns">
    /// A <see cref="List{T}"/> of <see cref="CsvColumnInfo"/> containing the ordered column metadata.
    /// </param>
    /// <param name="lineData">
    /// A <see cref="List{T}"/> of <see cref="string"/> containing the CSV cell values.
    /// </param>
    /// <returns>
    /// An instance of <typeparamref name="T"/> if successful; otherwise, returns <b>null</b>.
    /// </returns>
    private T? ProcessLineToObject<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(
            List<CsvColumnInfo> orderedColumns, List<string>? lineData)
    {
        T? dataObject = default;
        if (lineData != null)
        {
            // Create the object.
            dataObject = Activator.CreateInstance<T>();

            // Iterate over each cell applying the valeu to the property that matches the ordered column.
            int length = orderedColumns.Count;
            for (int columnIndex = 0; columnIndex < length; columnIndex++)
            {
                // Don't read past the end of the data.
                if (columnIndex < lineData.Count)
                {
                    CsvColumnInfo colProfile = orderedColumns[columnIndex];
                    if (!colProfile.PropertyMissing && colProfile.PropertyData != null)
                    {
                        // Read the cell data and covert to the appropriate data type.
                        string cellData = lineData[columnIndex];
                        object? convertedValue = CsvTypeConverter.ConvertType(cellData, colProfile.PropertyData);

                        // Set the property value, if converted successfully.
                        if (convertedValue != null)
                        {
                            try
                            {
                                colProfile.PropertyData.SetValue(dataObject, convertedValue);
                            }
                            catch (Exception ex)
                            {
                                LogError(ex);
                            }
                        }
                    }
                }
            }
        }
        return dataObject;
    }

    /// <summary>
    /// Safely reads a line of text from the source stream.
    /// </summary>
    /// <returns>
    /// The line of text that was read, or <b>null</b>.
    /// </returns>
    private string? SafeReadLine()
    {
        string? lineText = null;
        try
        {
            lineText = _reader?.ReadLine();
        }
        catch (Exception ex)
        {
            LogError(ex);
        }

        return lineText;
    }

    /// <summary>
    /// Safely reads a line of text from the source stream.
    /// </summary>
    /// <returns>
    /// The line of text that was read, or <b>null</b>.
    /// </returns>
    private string? SafeReadLine(StreamReader reader)
    {
        string? lineText = null;
        try
        {
            lineText = reader?.ReadLine();
        }
        catch (Exception ex)
        {
            LogError(ex);
        }

        return lineText;
    }

    /// <summary>
    /// Safely reads a line of text from the source stream.
    /// </summary>
    /// <returns>
    /// The line of text that was read, or <b>null</b>.
    /// </returns>
    private async Task<string?> SafeReadLineAsync()
    {
        if (_reader is null)
        {
            return null;
        }

        return await _reader.ReadLineAsync().ConfigureAwait(false);
    }
    #endregion
}