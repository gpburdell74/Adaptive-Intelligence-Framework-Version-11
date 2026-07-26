using Adaptive.Intelligence.IO;
using System.Diagnostics;
using System.Text;

namespace Adaptive.Intelligence.Common.Abstractions;

/// <summary>
/// Provides a simple logging implementation for statically logging data and exceptions that occur during
/// operation of the application.
/// </summary>
public abstract class SimpleLogBase : DisposableObjectBase
{
    #region Public Events
    /// <summary>
    /// Occurs when the log encounters an exception.
    /// </summary>
    public event UnhandledExceptionEventHandler? LoggerException;
    #endregion

    #region Private Member Declarations
    /// <summary>
    /// The default file name to use when one is not specified.
    /// </summary>
    private const string DefaultFileName = "Adaptive.Intelligence.Common.Log.log";
    /// <summary>
    /// Thread synchronization object for writing to the log file.
    /// </summary>
    private readonly object _syncRoot = new();
    /// <summary>
    /// The file stream to write to.
    /// </summary>
    private Stream? _stream;
    /// <summary>
    /// The text writer instance.
    /// </summary>
    private StreamWriter? _writer;
    /// <summary>
    /// The file name.
    /// </summary>
    private string? _fileName;
    /// <summary>
    /// A value indicating whether the current instance created and opened the stream for writing.
    /// </summary>
    private bool _local;
    /// <summary>
    /// Flag to suspend writing to the log.
    /// </summary>
    private bool _suspend;
    #endregion

    #region Constructor / Dispose Methods
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleLogBase"/> class.
    /// </summary>
    protected SimpleLogBase()
    {
        _fileName = DefaultFileName;
        CreateStream(DefaultFileName);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleLogBase"/> class.
    /// </summary>
    /// <param name="fileName">
    /// A string containing the fully-qualified path and name of the log file.
    /// </param>
    protected SimpleLogBase(string fileName)
    {
        _fileName = fileName;
        CreateStream(fileName);
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="SimpleLogBase"/> class.
    /// </summary>
    /// <param name="destinationStream">
    /// The <see cref="Stream"/> instance to which the log will be written.
    /// </param>
    protected SimpleLogBase(Stream destinationStream)
    {
        CreateStream(destinationStream);
    }

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.
    /// </summary>
    /// <param name="disposing">
    /// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
    /// </param>
    protected override void Dispose(bool disposing)
    {
        if (!IsDisposed && disposing)
        {
            CloseStream();
        }

        _fileName = null;
        _stream = null;
        _writer = null;
        _local = false;
        base.Dispose(disposing);
    }
    #endregion

    #region Public Properties
    /// <summary>
    /// Gets the log file name, if provided.
    /// </summary>
    /// <value>
    /// A string containing the name of the log file, or <b>null</b> if not specified.
    /// </value>
    public string? FileName => _fileName;
    #endregion

    #region Public Abstract Methods / Functions
    /// <summary>
    /// Formats the exception content into the log output to be written.
    /// </summary>
    /// <param name="exception">
    /// The <see cref="Exception"/> instance to be logged.
    /// </param>
    /// <returns>
    /// A string containing the content to be written, or <b>null</b>.
    /// </returns>
    protected abstract string? FormatException(Exception exception);

    /// <summary>
    /// Creates and formats the content of the file footer, to be written when the log file is closed.
    /// </summary>
    /// <returns>
    /// A string containing the content to be written, or <b>null</b>.
    /// </returns>
    protected abstract string? FormatFileFooter();

    /// <summary>
    /// Creates and formats the content of the file header, to be written when the log file is opened.
    /// </summary>
    /// <returns>
    /// A string containing the content to be written, or <b>null</b>.
    /// </returns>
    protected abstract string? FormatFileHeader();
    #endregion

    #region Event Methods
    /// <summary>
    /// Raises the <see cref="LoggerException"/> event.
    /// </summary>
    /// <param name="ex">
    /// The <see cref="Exception"/> that occurred during logging.
    /// </param>
    protected virtual void OnLoggerException(Exception ex)
    {
        LoggerException?.Invoke(this, new UnhandledExceptionEventArgs(ex, false));
    }
    #endregion

    #region Public Methods / Functions
    /// <summary>
    /// Logs the specified text to the output stream.
    /// </summary>
    /// <param name="textToLog">
    /// A string containing the text to be written.  Null values are ignored.
    /// </param>
    public void Log(string? textToLog)
    {
        if (textToLog != null)
        {
            WriteLine(textToLog);
        }
    }

    /// <summary>
    /// Logs the exception.
    /// </summary>
    /// <param name="ex">
    /// The <see cref="Exception"/> instance to be logged.
    /// </param>
    public void LogException(Exception ex)
    {
        if (ex != null)
        {
            StackTrace st = new();
            WriteException(ex, st.ToString());
        }
    }

    /// <summary>
    /// Logs the exception.
    /// </summary>
    /// <param name="ex">
    /// The <see cref="Exception" /> instance to be logged.
    /// </param>
    /// <param name="member">
    /// A string describing the class/struct member in which the exception occurred.
    /// </param>
    /// <param name="file">
    /// A string containing the file name of the source code file.
    /// </param>
    /// <param name="line">
    /// An integer specifying the line number on which the exception occurred.
    /// </param>
    public void LogException(Exception ex, string member, string file, int line)
    {
        StackTrace st = new();

        StringBuilder builder = new();
        builder.AppendLine("Member: " + member);
        builder.AppendLine("File: " + file);
        builder.AppendLine("Line No: " + line);

        WriteLine(builder.ToString());
        builder.Clear();
        if (ex != null)
        {
            WriteException(ex, st.ToString());
        }
    }
    #endregion

    #region Private Static Methods / Functions
    /// <summary>
    /// Writes the exception to the output stream.
    /// </summary>
    /// <param name="ex">
    /// The <see cref="Exception"/> instance to be recorded.
    /// </param>
    /// <param name="currentCallStack">
    /// An optional string parameter specifying the current call stack from which the WriteException
    /// method was called.
    /// </param>
    private void WriteException(Exception ex, string? currentCallStack = null)
    {
        string? exceptionText = FormatException(ex);
        if (exceptionText != null)
        {
            WriteLine(exceptionText);
        }
        if (currentCallStack != null)
        {
            WriteLine(currentCallStack);
        }
    }

    /// <summary>
    /// Suspends writing to the log.
    /// </summary>
    public void Suspend()
    {
        _suspend = true;
    }

    /// <summary>
    /// Resumes writing to the log.
    /// </summary>
    public void Resume()
    {
        _suspend = false;
    }
    #endregion

    #region Private Instance Methods / Functions
    /// <summary>
    /// Initializes the underlying stream objects for writing to the log.
    /// </summary>
    /// <param name="destinationStream">
    /// A <see cref="Stream"/> instance to be written to.
    /// </param>
    private void CreateStream(Stream destinationStream)
    {
        _stream = destinationStream;
        _local = false;
        _writer = new StreamWriter(_stream);
        string? header = FormatFileHeader();
        if (header != null)
        {
            WriteLine(header); 
        }
    }

    /// <summary>
    /// Attempts to create the output steam to write to.
    /// </summary>
    private void CreateStream(string fileName)
    {
        try
        {
            // Ensure the file name is unique.
            fileName = FileUtility.EnsureUniqueFileName(fileName);

            _stream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            CreateStream(_stream);
            _local = true;
        }
        catch (Exception ex)
        {
            OnLoggerException(ex);
        }
    }

    /// <summary>
    /// Closes the output stream.
    /// </summary>
    private void CloseStream()
    {
        string? footer = FormatFileFooter();
        if (footer != null)
        {
            WriteLine(footer);
        }

        // Do not close or dispose either object if the stream was created externally and passed in to the constructor.
        // Closing or disposing of the writer will affect the parent stream instance.
        if (_local)
        {
            _writer?.Dispose();
            _stream?.Dispose();
        }

        _writer = null;
        _stream = null;
    }
    /// <summary>
    /// Writes the line to the log.
    /// </summary>
    /// <param name="content">
    /// A string containing the content to be written.
    /// </param>
    private void WriteLine(string content)
    {
        if ((_writer != null) && (content != null) && (!_suspend))
        {
            lock (_syncRoot)
            {
                try
                {
                    _writer.WriteLine(content);
                    _writer.Flush();
                }
                catch (Exception ex)
                {
                    OnLoggerException(ex);
                }
            }
        }
    }
    #endregion
}