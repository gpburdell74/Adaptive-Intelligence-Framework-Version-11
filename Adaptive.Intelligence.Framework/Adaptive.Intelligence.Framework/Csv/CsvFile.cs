using Adaptive.Intelligence.Abstractions.Logging;
using Adaptive.Intelligence.Csv.Exceptions;
using Adaptive.Intelligence.IO;
using System.IO.Compression;

namespace Adaptive.Intelligence.Csv;

/// <summary>
/// Represents and manages a CSV file.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CsvFile"/> class.
/// </remarks>
/// <param name="fileName">
/// A string containing the fully-qualified path and name of the file.
/// </param>
/// <param name="hasHeader">
/// An optional parameter indicating whether the CSV file has a header row.
/// </param>
public class CsvFile(string fileName, bool hasHeader = false) : LoggableBase, IComparable
{
    #region Private Member Declarations
    /// <summary>
    /// The default maximum cell size in bytes.
    /// </summary>
    private const int DefaultMaxCellSize = 65536;

    /// <summary>
    /// The maximum amount of memory to allocated for the data in a CSV cell.
    /// </summary>
    private int _maxCellSize = DefaultMaxCellSize;

    /// <summary>
    /// The file name.
    /// </summary>
    private string? _fileName = fileName;

    /// <summary>
    /// The has header flag.
    /// </summary>
    private bool _hasHeader = hasHeader;

    /// <summary>
    /// The header names list.
    /// </summary>
    private List<string>? _headerNames;

    /// <summary>
    /// The list of rows in the CSV file.
    /// </summary>
    private List<List<string>>? _dataRows;
    #endregion

    #region Constructor / Dispose Methods

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
    /// <b>false</b> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
        if (!IsDisposed && disposing)
        {
            _headerNames?.Clear();
            _dataRows?.Clear();
        }

        _fileName = null;
        _dataRows = null;
        _headerNames = null;
        base.Dispose(disposing);
    }
    #endregion

    #region Public Properties    
    /// <summary>
    /// Gets the number of columns defined in the CSV file.
    /// </summary>
    /// <value>
    /// An integer specifying the number of columns, or -1 if the data is not loaded.
    /// </value>
    public int ColumnCount
    {
        get
        {
            int count = -1;
            if (_headerNames != null)
            {
                count = _headerNames.Count;
            }
            else if (_dataRows != null && _dataRows.Count > 0)
            {
                count = _dataRows[0].Count;
            }

            return count;
        }
    }
    /// <summary>
    /// Gets or sets the fully-qualified path and name of the CSV file.
    /// </summary>
    /// <value>
    /// A string containing the path and file name.
    /// </value>
    public string? FileName
    {
        get => _fileName;
        set => _fileName = value;
    }

    /// <summary>
    /// Gets the length or size of the file as specified in <see cref="FileName"/>.
    /// </summary>
    /// <value>
    /// A <see cref="long"/> indicating the length of the file, or -1 if the file does not exist or 
    /// cannot be accessed.
    /// </value>
    public long Length
    {
        get
        {
            long size = -1;
            if (File.Exists(_fileName))
            {
                size = SafeIO.GetFileSizeNative(_fileName);
            }
            return size;
        }
    }

    /// <summary>
    /// Gets or sets the maximum size of a data cell in the CSV file, in bytes.
    /// </summary>
    /// <value>
    /// An integer indicating the maximum size of a data cell, in bytes.
    /// </value>
    public int MaximumCellSize
    {
        get => _maxCellSize;
        set => _maxCellSize = value;
    }

    /// <summary>
    /// Gets the number of rows in the CSV file, when loaded into memory.
    /// </summary>
    /// <value>
    /// An integer specifying the row count, or -1 if the data is not loaded.
    /// </value>
    public int RowCount
    {
        get
        {
            if (_dataRows == null)
            {
                return -1;
            }
            else
            {
                return _dataRows.Count;
            }
        }
    }
    #endregion

    #region Public Methods / Functions

    /// <summary>
    /// Closes any open streams and unloads any data content that was loaded.
    /// </summary>
    public void Close()
    {
        _dataRows?.Clear();
        _headerNames?.Clear();

        _dataRows = null;
        _headerNames = null;
        GC.Collect();
    }

    /// <summary>
    /// Compresses the content of the currently referenced CSV file to the specified compressed file name.
    /// </summary>
    /// <remarks>
    /// This method will read the original file from disk and compress it using the GZIP format.
    /// The name of the new file will be the same as the original with ".zip" appended to it.
    /// </remarks>
    public void Compress()
    {
        Compress(GenerateCompressedName());
    }

    /// <summary>
    /// Compresses the content of the currently referenced CSV file to the specified compressed file name.
    /// </summary>
    /// <remarks>
    /// This method will read the original file from disk and compress it using the GZIP format.
    /// The name of the new file will be the same as the original with ".zip" appended to it.
    /// </remarks>
    public Task CompressAsync()
    {
        return CompressAsync(GenerateCompressedName());
    }

    /// <summary>
    /// Compresses the content of the currently referenced CSV file to the specified compressed file name.
    /// </summary>
    /// <remarks>
    /// This method will read the original file from disk and compress it using the GZIP format.
    /// </remarks>
    /// <param name="compressedFileName">
    /// A string containing the name of the compressed file to create.
    /// </param>
    public void Compress(string compressedFileName)
    {
        if (_fileName != null && SafeIO.FileExists(_fileName))
        {
            if (SafeIO.FileExists(compressedFileName))
            {
                SafeIO.DeleteFile(compressedFileName);
            }

            FileStream? inputFile = SafeIO.OpenFileForExclusiveRead(_fileName);
            FileStream? outputFile = SafeIO.OpenFileForExclusiveWrite(compressedFileName);

            if (inputFile == null)
            {
                throw new NullStreamException("Could not open the specified file for reading.");
            }

            if (outputFile == null)
            {
                throw new NullStreamException("Could not open the specified file for writing.");
            }

            AdaptiveCompression.Compress(inputFile, outputFile);

            outputFile.Flush();
            outputFile.Close();
            outputFile.Dispose();

            inputFile.Close();
            inputFile.Dispose();
        }
    }

    /// <summary>
    /// Compresses the content of the currently referenced CSV file to the specified compressed file name.
    /// </summary>
    /// <remarks>
    /// This method will read the original file from disk and compress it using the GZIP format.
    /// </remarks>
    /// <param name="compressedFileName">
    /// A string containing the name of the compressed file to create.
    /// </param>
    public async Task CompressAsync(string compressedFileName)
    {
        if (_fileName != null && SafeIO.FileExists(_fileName))
        {
            if (SafeIO.FileExists(compressedFileName))
            {
                SafeIO.DeleteFile(compressedFileName);
            }

            FileStream? inputFile = SafeIO.OpenFileForExclusiveRead(_fileName);
            FileStream? outputFile = SafeIO.OpenFileForExclusiveWrite(compressedFileName);

            if (inputFile == null)
            {
                throw new NullStreamException("Could not open the specified file for reading.");
            }

            if (outputFile == null)
            {
                throw new NullStreamException("Could not open the specified file for writing.");
            }

            await AdaptiveCompression.CompressAsync(inputFile, outputFile).ConfigureAwait(false);

            await outputFile.FlushAsync().ConfigureAwait(false);
            outputFile.Close();
            await outputFile.DisposeAsync().ConfigureAwait(false);

            inputFile.Close();
            await inputFile.DisposeAsync().ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates 
    /// whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="fileToCompareTo">
    /// An object to compare with this instance.
    /// </param>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. 
    /// The return value has these meanings:
    ///     Less than zero: This instance precedes <paramref name="fileToCompareTo" /> in the sort order.
    ///     Zero: The files are identical.
    ///     Greater than zero: This instance follows <paramref name="fileToCompareTo" /> in the sort order.
    /// </returns>
    int IComparable.CompareTo(object? fileToCompareTo)
    {
        return CompareTo((CsvFile?)fileToCompareTo);
    }

    /// <summary>
    /// Compares the current instance with another CSV file and returns an integer that indicates 
    /// whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
    /// </summary>
    /// <param name="fileToCompareTo">
    /// A <see cref="CsvFile"/> instance to compare with this instance.
    /// </param>
    /// <remarks>
    /// This method will compare the two files' data content, event if not loaded into memory.
    /// </remarks>
    /// <returns>
    /// A value that indicates the relative order of the objects being compared. 
    /// The return value has these meanings:
    ///     Less than zero: This instance precedes <paramref name="fileToCompareTo" /> in the sort order.
    ///     Zero: The files are identical.
    ///     Greater than zero: This instance follows <paramref name="fileToCompareTo" /> in the sort order.
    /// </returns>
#pragma warning disable CA1822 // Mark members as static
    public int CompareTo(CsvFile? fileToCompareTo)
#pragma warning restore CA1822 // Mark members as static
    {
        if (fileToCompareTo is null)
        {
            return 1;
        }

        return 0;
    }

    /// <summary>
    /// Decompresses the content of the specified source file into an expanded CSV File.
    /// </summary>
    /// <remarks>
    /// This method expects the compression algorithm to use the GZIP standard.  (For use with
    /// the <see cref="GZipStream"/> class.)
    /// </remarks>
    /// <param name="sourceFileName">
    /// A string containing the fully-qualified path and name of the source file.
    /// </param>
    /// <param name="destinationFileName">
    /// A string containing the fully-qualified path and name of the destination file.
    /// </param>
    public static void Decompress(string sourceFileName, string destinationFileName)
    {
        if (!SafeIO.FileExists(sourceFileName))
        {
            throw new FileNotFoundException("The specified source file does not exist.", sourceFileName);
        }

        if (SafeIO.FileExists(destinationFileName))
        {
            SafeIO.DeleteFile(destinationFileName);
        }

        FileStream? inputFile = SafeIO.OpenFileForExclusiveRead(sourceFileName);
        FileStream? outputFile = SafeIO.OpenFileForExclusiveWrite(destinationFileName);
        if (inputFile == null)
        {
            throw new NullStreamException("Could not open the specified file for reading.");
        }

        if (outputFile == null)
        {
            throw new NullStreamException("Could not open the specified file for writing.");
        }

        AdaptiveCompression.Decompress(inputFile, outputFile);

        outputFile.Flush();
        outputFile.Close();
        outputFile.Dispose();

        inputFile.Close();
        inputFile.Dispose();
    }

    /// <summary>
    /// Decompresses the content of the specified source file into an expanded CSV File.
    /// </summary>
    /// <remarks>
    /// This method expects the compression algorithm to use the GZIP standard.  (For use with
    /// the <see cref="GZipStream"/> class.)
    /// </remarks>
    /// <param name="sourceFileName">
    /// A string containing the fully-qualified path and name of the source file.
    /// </param>
    /// <param name="destinationFileName">
    /// A string containing the fully-qualified path and name of the destination file.
    /// </param>
    public static async Task DecompressAsync(string sourceFileName, string destinationFileName)
    {
        if (!SafeIO.FileExists(sourceFileName))
        {
            throw new FileNotFoundException("The specified source file does not exist.", sourceFileName);
        }

        if (SafeIO.FileExists(destinationFileName))
        {
            SafeIO.DeleteFile(destinationFileName);
        }

        FileStream? inputFile = SafeIO.OpenFileForExclusiveRead(sourceFileName);
        FileStream? outputFile = SafeIO.OpenFileForExclusiveWrite(destinationFileName);
        if (inputFile == null)
        {
            throw new NullStreamException("Could not open the specified file for reading.");
        }

        if (outputFile == null)
        {
            throw new NullStreamException("Could not open the specified file for writing.");
        }

        await AdaptiveCompression.DecompressAsync(inputFile, outputFile).ConfigureAwait(false);

        await outputFile.FlushAsync().ConfigureAwait(false);
        await outputFile.DisposeAsync().ConfigureAwait(false);

        inputFile.Close();
        await inputFile.DisposeAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Loads the content of the specified CSV file into memory.
    /// </summary>
    /// <param name="fileName">
    /// A string containing the fully-qualified path and name of the file to be read.
    /// </param>
    /// <param name="hasHeader">
    /// <b>true</b> if the first row of the CSV file contains header names; otherwise, <b>false</b>.
    /// </param>
    public void LoadContent(string fileName, bool hasHeader = false)
    {
        _fileName = fileName;
        _hasHeader = hasHeader;

        FileStream? sourceStream = SafeIO.OpenFileForExclusiveRead(fileName);
        NullStreamException.ThrowIfNull(sourceStream, nameof(sourceStream), "Could not open the specified file for reading.");
        CsvReader reader = new(sourceStream!, _maxCellSize, hasHeader);
        if (hasHeader)
        {
            _headerNames = reader.ReadHeader();
        }

        _dataRows = reader.ReadRawDataRows(hasHeader);
        reader.Dispose();
        sourceStream?.Dispose();
    }

    /// <summary>
    /// Loads the content of the specified CSV file into memory.
    /// </summary>
    /// <param name="fileName">
    /// A string containing the fully-qualified path and name of the file to be read.
    /// </param>
    /// <param name="hasHeader">
    /// <b>true</b> if the first row of the CSV file contains header names; otherwise, <b>false</b>.
    /// </param>
    public async Task LoadContentAsync(string fileName, bool hasHeader = false)
    {
        _fileName = fileName;
        _hasHeader = hasHeader;

        FileStream? sourceStream = SafeIO.OpenFileForExclusiveRead(fileName) ?? throw new NullStreamException("Could not open the specified file for reading.");
        CsvReader reader = new(sourceStream, _maxCellSize, hasHeader);
        if (hasHeader)
        {
            _headerNames = await reader.ReadHeaderAsync().ConfigureAwait(false);
        }

        _dataRows = await reader.ReadRawDataRowsAsync(hasHeader).ConfigureAwait(false);
        reader.Dispose();
        await sourceStream.DisposeAsync().ConfigureAwait(false);
    }

    /// <summary>
    /// Saves the currently loaded content to a new file.
    /// </summary>
    /// <param name="newFileName">
    /// A string containing the fully-qualified path and name of the file.
    /// </param>
    public void SaveAs(string newFileName)
    {
        if (_dataRows != null)
        {
            if (SafeIO.FileExists(newFileName))
            {
                SafeIO.DeleteFile(newFileName);
            }

            using FileStream? outStream = SafeIO.OpenFileForExclusiveWrite(newFileName);
            NullStreamException.ThrowIfNull(outStream, newFileName);
            CantWriteStreamException.ThrowIfStreamCantWrite(outStream, newFileName);

            using CsvWriter writer = new(outStream!);
            writer.WriteRawDataRows(_dataRows, _hasHeader);

            outStream?.Flush();
        }
    }
    /// <summary>
    /// Validates that every row of the CSv data as the same number of columns as the header or first row.
    /// </summary>
    /// <returns>
    /// A <see cref="List{T}"/> of integers indicating which row index does not match the expected column count.
    /// </returns>
    public List<int> ValidateColumnCounts()
    {
        int expectedCount = 0;
        if (_headerNames != null)
        {
            expectedCount = _headerNames.Count;
        }
        else if (_dataRows != null && _dataRows.Count > 0)
        {
            expectedCount = _dataRows[0].Count;
        }
        else if (_dataRows == null || _dataRows.Count == 0)
        {
            expectedCount = 0;
        }

        return ValidateColumnCounts(expectedCount);

    }

    /// <summary>
    /// Validates that every row of the CSV data as the same number of columns as the header or first row.
    /// </summary>
    /// <param name="expectedCount">
    /// An integer specifying the expected number of columns.
    /// </param>
    /// <returns>
    /// A <see cref="List{T}"/> of integers indicating which row index does not match the expected column count.
    /// </returns>
    public List<int> ValidateColumnCounts(int expectedCount)
    {
        List<int> rowIndexValues = [];

        if (expectedCount > 0 && _dataRows != null)
        {
            int length = _dataRows.Count;
            for (int rowIndex = 0; rowIndex < length; rowIndex++)
            {
                List<string> row = _dataRows[rowIndex];
                if (row.Count != expectedCount)
                {
                    rowIndexValues.Add(rowIndex);
                }
            }
        }

        return rowIndexValues;
    }
    #endregion

    #region Private Methods / Functions
    /// <summary>
    /// Generates the default compressed file name for the current CSV file.
    /// </summary>
    /// <returns>
    /// The compressed file name derived from <see cref="FileName"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when <see cref="FileName"/> property value is <b>null</b> or empty.
    /// </exception>
    private string GenerateCompressedName()
    {
        if (string.IsNullOrEmpty(_fileName))
        {
            throw new InvalidOperationException("There is no file to compress.");
        }

        return _fileName + ".zip";
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current <see cref="CsvFile"/>.
    /// </summary>
    /// <param name="obj">
    /// The object to compare with the current instance.
    /// </param>
    /// <returns>
    /// <b>true</b> if the specified object is equal to the current instance; otherwise, <b>false</b>.
    /// </returns>
    public override bool Equals(object obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (ReferenceEquals(obj, null))
        {
            return false;
        }

        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance.
    /// </returns>
    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// Determines whether two <see cref="CsvFile"/> instances are equal.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns><b>true</b> if both operands are equal; otherwise, <b>false</b>.</returns>
    public static bool operator ==(CsvFile left, CsvFile right)
    {
        if (ReferenceEquals(left, null))
        {
            return ReferenceEquals(right, null);
        }

        return left.Equals(right);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(CsvFile left, CsvFile right)
    {
        return !(left == right);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator <(CsvFile left, CsvFile right)
    {
        return ReferenceEquals(left, null) ? !ReferenceEquals(right, null) : left.CompareTo(right) < 0;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator <=(CsvFile left, CsvFile right)
    {
        return ReferenceEquals(left, null) || left.CompareTo(right) <= 0;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator >(CsvFile left, CsvFile right)
    {
        return !ReferenceEquals(left, null) && left.CompareTo(right) > 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator >=(CsvFile left, CsvFile right)
    {
        return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.CompareTo(right) >= 0;
    }

    #endregion

}
