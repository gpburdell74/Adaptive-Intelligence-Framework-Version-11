#if DEBUG
using Adaptive.Intelligence.Constants;
using Adaptive.Intelligence.IO;
using System.Diagnostics;
using System.Globalization;
using System.Text;
#endif

namespace Adaptive.Intelligence.Logging;

#if DEBUG
/// <summary>
/// Provides a static shortcut class for printing debug and trace messages, with logging ability.  
/// </summary>
/// <remarks>
/// This class is intended solely for debug use, and not for any kind of production logging.
/// </remarks>
public static class AdaptiveDebug
{
    #region Private Constants
    /// <summary>
    /// A string describing the date format for creating a log file name.
    /// </summary>
    private const string DefaultFileNameDateFormat = "yyyyddMM-hhmmss";

    /// <summary>
    /// The default date format text.
    /// </summary>
    private const string DefaultDateFormat = "yyyy-MM-dd HH:mm:ss.fff";

    /// <summary>
    /// The default value for a file name prefix.
    /// </summary>
    private const string DefaultFileNamePrefix = @"Adaptive-Debug-Trace-Log - ";

    /// <summary>
    /// The default file extension for the file name.
    /// </summary>
    private const string DefaultFileNameExtension = @"csv";

    /// <summary>
    /// The default thread ID prefix for the output.
    /// </summary>
    private const string DefaultThreadIdPrefix = "TID: ";
    private const string ElapsedPrefix = "Elapsed: ";
    private const string ElapsedSuffix = " tick(s).";
    #endregion

    #region Private Static Members
    /// <summary>
    /// Thread synchronization instance.
    /// </summary>
    private static readonly Lock _syncRoot = new();

    /// <summary>
    /// An optional output file.
    /// </summary>
    private static TextFile? _outputFile;

    /// <summary>
    /// The last ticks value that was recorded.
    /// </summary>
    private static long _lastTicks;

    /// <summary>
    /// The date format for the output.
    /// </summary>
    private static string _dateFormat = DefaultDateFormat;

    /// <summary>
    /// The file name date format.
    /// </summary>
    private static string _fileNameDateFormat = DefaultFileNameDateFormat;

    /// <summary>
    /// The file name prefix value.
    /// </summary>
    private static string _fileNamePrefix = DefaultFileNamePrefix;

    /// <summary>
    /// Gets or sets the file name extension value.
    /// </summary>
    /// <value>
    /// A string containing the name extension value.
    /// </value>
    private static string _fileNameExtension = DefaultFileNameExtension;

    /// <summary>
    /// The mode text.
    /// </summary>
    private static string _mode = string.Empty;

    /// <summary>
    /// The text to print before a thread ID value.
    /// </summary>
    private static string _threadIdPrefix = DefaultThreadIdPrefix;

    /// <summary>
    /// A value indicating whether to track and write the number of elapsed milliseconds
    /// between each call.
    /// </summary>
    private static bool _useElapsedTime = true;
    #endregion

    #region Public Static Properties
    /// <summary>
    /// Gets or sets the date format for the output.
    /// </summary>
    /// <value>
    /// A string specifying the format for the date value in the output.
    /// </value>
    public static string DateFormat
    {
        get => _dateFormat;
        set
        {
            lock (_syncRoot)
            {
                _dateFormat = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the file name date format.
    /// </summary>
    /// <value>
    /// A string specifying the format for the date value in the file name.
    /// </value>
    public static string FileNameDateFormat
    {
        get => _fileNameDateFormat;
        set
        {
            lock (_syncRoot)
            {
                _fileNameDateFormat = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the file name prefix value.
    /// </summary>
    /// <value>
    /// A string containing the name prefix value.
    /// </value>
    public static string FileNamePrefix
    {
        get => _fileNamePrefix;
        set
        {
            lock (_syncRoot)
            {
                _fileNamePrefix = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the file name extension value.
    /// </summary>
    /// <value>
    /// A string containing the name extension value.
    /// </value>
    public static string FileNameExtension
    {
        get => _fileNameExtension;
        set
        {
            lock (_syncRoot)
            {
                _fileNameExtension = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the mode text.
    /// </summary>
    /// <value>
    /// A string describing the operation mode for the debugger, or <see cref="string.Empty"/>.
    /// </value>
    public static string Mode
    {
        get => _mode;
        set
        {
            lock (_syncRoot)
            {
                _mode = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets the text to print before a thread ID value.
    /// </summary>
    /// <value>
    /// A string containing the prefix text.
    /// </value>
    public static string ThreadIdPrefix
    {
        get => _threadIdPrefix;
        set
        {
            lock (_syncRoot)
            {
                _threadIdPrefix = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether to track and write the number of elapsed milliseconds
    /// between each call.
    /// </summary>
    /// <value>
    /// <b>true</b> to track the elapsed time; otherwise, <b>false</b>.
    /// </value>
    public static bool UseElapsedTime
    {
        get => _useElapsedTime;
        set
        {
            lock (_syncRoot)
            {
                if (value != _useElapsedTime)
                {
                    if (!value)
                    {
                        _lastTicks = 0;
                    }
                    else
                    {
                        _lastTicks = Environment.TickCount;
                    }
                }
                _useElapsedTime = value;
            }
        }
    }

    #endregion

    #region Public Static Methods / Functions
    /// <summary>
    /// Signals a break point to an attached debugger.
    /// </summary>
    public static void Break()
    {
        Debugger.Break();
    }

    /// <summary>
    /// Closes the text output file, if open.
    /// </summary>
    public static void CloseLogFile()
    {
        lock (_syncRoot)
        {
            _outputFile?.Close();
            _outputFile?.Dispose();
            _outputFile = null;
        }
    }

    /// <summary>
    /// Creates a logging output file name.
    /// </summary>
    /// <returns>
    /// A string containing the fully-qualified path and name of the file.
    /// </returns>
    public static string CreateOutputFileName()
    {
        // Format:
        //		<path>\<filename prefix><date time>.<filename extension>
        return
            Directory.GetCurrentDirectory() +
            FileNamePrefix +
            DateTime.Now.ToString(FileNameDateFormat, CultureInfo.CurrentCulture) + "." +
            FileNameExtension;
    }

    /// <summary>
    /// Increases the current trace indention level by one.
    /// </summary>
    public static void Indent()
    {
        lock (_syncRoot)
        {
            Trace.Indent();
        }
    }

    /// <summary>
    /// Writes the current thread ID value to the trace and output window.
    /// </summary>
    public static void ShowThread()
    {
        lock (_syncRoot)
        {
            var currentThreadId = Environment.CurrentManagedThreadId.ToString(CultureInfo.CurrentCulture);
            string threadIdText = ThreadIdPrefix + currentThreadId;
            if (Debugger.IsAttached)
            {
                Debug.WriteLine(threadIdText);
            }

            Trace.TraceInformation(threadIdText);
            _outputFile?.WriteLine(threadIdText);
        }
    }

    /// <summary>
    /// Decreases the current trace indention level by one.
    /// </summary>
    public static void Unindent()
    {
        lock (_syncRoot)
        {
            Trace.Unindent();
        }
    }

    /// <summary>
    /// Initializes the use of an output file for writing the debug/trace content to
    /// a text file.
    /// </summary>
    /// <param name="fileName">
    /// A string containing the fully-qualified path and name of the file.
    /// </param>
    public static void UseOutputFile(string fileName)
    {
        lock (_syncRoot)
        {
            _outputFile = new TextFile(fileName);
            _outputFile.Create();

            string line = "Mode,Date/Time,Thread Id,Elapsed Ticks,Content";
            _outputFile.WriteLine(line);
        }
    }

    /// <summary>
    /// Writes the specified content to the trace and output window as a
    /// single line.
    /// </summary>
    /// <param name="content">
    /// A string containing the content to be written.
    /// </param>
    public static void WriteLine(string content)
    {
        lock (_syncRoot)
        {
            AdaptiveDebugLogRecord log = new AdaptiveDebugLogRecord
            {
                DateTime = RenderDate(),
                ElapsedTime = RenderElapsedTime(),
                LogText = content,
                Mode = _mode,
                ThreadId = RenderThreadId()
            };

            string output = RenderLogRecord(log);

            // Debug window.
            if (Debugger.IsAttached)
            {
                Debug.WriteLine(output);
            }
            else
            {
                // Trace
                Trace.WriteLine(output);
            }

            // Output file, if present.
            if (_outputFile is not null)
            {
                WriteToFile(content);
            }
        }
    }
    #endregion

    #region Private Methods / Functions

    #region Rendering Methods
    /// <summary>
    /// Renders the currnet date and time in the user-specified format.
    /// </summary>
    private static string RenderDate()
    {
        return DateTime.Now.ToString(DateFormat, CultureInfo.CurrentCulture);
    }

    /// <summary>
    /// Renders the output prefix string to print before the intended output.
    /// </summary>
    private static string RenderThreadId()
    {
        return ThreadIdPrefix + Environment.CurrentManagedThreadId;
    }

    /// <summary>
    /// Renders the elapsed time string, if the flag is <b>true</b>.
    /// </summary>
    private static string RenderElapsedTime()
    {
        string result = string.Empty;

        if (_useElapsedTime)
        {
            var difference = Environment.TickCount64 - _lastTicks;
            _lastTicks = Environment.TickCount64;

            result = ElapsedPrefix + difference + ElapsedSuffix;
        }
        return result;
    }
    /// <summary>
    /// Renders the log record as a line of text.
    /// </summary>
    /// <param name="record">
    /// The <see cref="AdaptiveDebugLogRecord"/> instance/
    /// </param>
    private static string RenderLogRecord(AdaptiveDebugLogRecord record)
    {
        StringBuilder builder = new StringBuilder(100);

        builder.Append(record.DateTime);
        builder.Append(CharacterConstants.TabChar);

        builder.Append(record.Mode);
        builder.Append(CharacterConstants.TabChar);

        if (_useElapsedTime)
        {
            builder.Append(record.DateTime);
            builder.Append(CharacterConstants.TabChar);
        }

        builder.Append(record.ThreadId);
        builder.Append(CharacterConstants.TabChar);

        builder.Append(record.Mode);
        builder.Append(CharacterConstants.TabChar);


        builder.Append(record.LogText);

        return builder.ToString();
    }
    #endregion

    /// <summary>
    /// Writes the content to an output file.
    /// </summary>
    /// <param name="content">
    /// A string containing the content to be written.</param>
    private static void WriteToFile(string content)
    {
        _outputFile?.WriteLine(content);
    }

    #endregion
}
#else
/// <summary>
/// Provides a static shortcut class for printing debug and trace messages, with logging ability.  
/// </summary>
/// <remarks>
/// This class is compiled when in release mode, and does nothing.
/// </remarks>
public static class AdaptiveDebug
{
    #region Public Static Properties
    /// <summary>
    /// Gets or sets the date format for the output.
    /// </summary>
    /// <value>
    /// A string specifying the format for the date value in the output.
    /// </value>
    public static string DateFormat { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file name date format.
    /// </summary>
    /// <value>
    /// A string specifying the format for the date value in the file name.
    /// </value>
    public static string FileNameDateFormat { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file name prefix value.
    /// </summary>
    /// <value>
    /// A string containing the name prefix value.
    /// </value>
    public static string FileNamePrefix { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the file name extension value.
    /// </summary>
    /// <value>
    /// A string containing the name extension value.
    /// </value>
    public static string FileNameExtension { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the mode text.
    /// </summary>
    /// <value>
    /// A string describing the operation mode for the debugger, or <see cref="string.Empty"/>.
    /// </value>
    public static string Mode { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the text to print before a thread ID value.
    /// </summary>
    /// <value>
    /// A string containing the prefix text.
    /// </value>
    public static string ThreadIdPrefix { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether to track and write the number of elapsed milliseconds
    /// between each call.
    /// </summary>
    /// <value>
    /// <b>true</b> to track the elapsed time; otherwise, <b>false</b>.
    /// </value>
    public static bool UseElapsedTime { get; set; }
    #endregion

    #region Public Static Methods / Functions
    /// <summary>
    /// Signals a break point to an attached debugger.
    /// </summary>
    public static void Break()
    {
        return;
    }

    /// <summary>
    /// Closes the text output file, if open.
    /// </summary>
    public static void CloseLogFile()
    {
        return;
    }

    /// <summary>
    /// Creates a logging output file name.
    /// </summary>
    /// <returns>
    /// A string containing the fully-qualified path and name of the file.
    /// </returns>
    public static string CreateOutputFileName()
    {
        return string.Empty;
    }

    /// <summary>
    /// Increases the current trace indention level by one.
    /// </summary>
    public static void Indent()
    {
        return;
    }

    /// <summary>
    /// Writes the current thread ID value to the trace and output window.
    /// </summary>
    public static void ShowThread()
    {
        return;
    }

    /// <summary>
    /// Decreases the current trace indention level by one.
    /// </summary>
    public static void Unindent()
    {
        return;
    }

    /// <summary>
    /// Initializes the use of an output file for writing the debug/trace content to
    /// a text file.
    /// </summary>
    /// <param name="fileName">
    /// A string containing the fully-qualified path and name of the file.
    /// </param>
    public static void UseOutputFile(string fileName)
    {
        return;
    }

    /// <summary>
    /// Writes the specified content to the trace and output window as a
    /// single line.
    /// </summary>
    /// <param name="content">
    /// A string containing the content to be written.
    /// </param>
    public static void WriteLine(string content)
    {
        return;
    }
    #endregion
}
#endif
