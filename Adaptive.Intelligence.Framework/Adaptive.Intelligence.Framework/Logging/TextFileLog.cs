using Adaptive.Intelligence.Common.Abstractions;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text;

namespace Adaptive.Intelligence.Logging
{
    /// <summary>
    /// Provides a simple mechanism for logging information to a log file.
    /// </summary>
    public class TextFileLog : DisposableObjectBase, ILogger
    {
        #region Private Member Declarations
        private static object _syncRoot = new();

        private bool _traceEnabled = true;
        private bool _debugEnabled = true;
        private bool _informationEnabled = true;
        private bool _warningEnabled = true;
        private bool _errorEnabled = true;
        private bool _criticalEnabled = true;
        private bool _useDatePrefix = true;

        private string? _fileName;
        private FileStream? _destinationStream;
        private StreamWriter? _writer;
        #endregion

        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="TextFileLog"/> class with the specified log file name.
        /// </summary>
        /// <param name="fileName">
        /// A string containing the fully-qualified path and name of the log file to write to. If the file does not exist, 
        /// it will be created; if it does exist, new log entries will be appended to the end of the file.
        /// </param>
        public TextFileLog(string fileName)
        {
            _fileName = fileName;
            CreateStreamObjects();
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="TextFileLog"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <b>true</b> when called from <see cref="Dispose"/>; otherwise, <b>false</b>.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                CloseStreamObjects();
            }

            _fileName = null;
            _writer = null;
            _destinationStream = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets a value indicating whether the Critical log level is enabled.
        /// </summary>
        /// <value>
        /// <b>true</b> to write <see cref="LogLevel.Critical"/> messages; otherwise, <b>false</b>.
        /// </value>
        public bool CriticalEnabled { get => _criticalEnabled; set => _criticalEnabled = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the Debug log level is enabled.
        /// </summary>
        /// <value>
        /// <b>true</b> to write <see cref="LogLevel.Debug"/> messages; otherwise, <b>false</b>.
        /// </value>
        public bool DebugEnabled { get => _debugEnabled; set => _debugEnabled = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the Error log level is enabled.
        /// </summary>
        /// <value>
        /// <b>true</b> to write <see cref="LogLevel.Error"/> messages; otherwise, <b>false</b>.
        /// </value>
        public bool ErrorEnabled { get => _errorEnabled; set => _errorEnabled = value; }

        /// <summary>
        /// Gets the name of the log file.
        /// </summary>
        /// <value>
        /// A string containing the fully-qualified path and name of the log file to write to.
        /// </value>
        public string? FileName => _fileName;

        /// <summary>
        /// Gets or sets a value indicating whether the Information log level is enabled.
        /// </summary>
        /// <value>
        /// <b>true</b> to write <see cref="LogLevel.Information"/> messages; otherwise, <b>false</b>.
        /// </value>
        public bool InformationEnabled { get => _informationEnabled; set => _informationEnabled = value; }

        /// <summary>
        /// Gets a value indicating whether the specified <see cref="LogLevel"/> is currently enabled.
        /// </summary>
        /// <param name="logLevel">
        /// A <see cref="LogLevel"/> enumerated value to check.
        /// </param>
        /// <returns>
        /// <b>true</b> if the current log supports the specified log level; otherwise, returns <b>false</b>.
        /// </returns>
        public virtual bool IsEnabled(LogLevel logLevel)
        {
            bool isSupported = false;

            if (_writer == null || _destinationStream == null)
            {
                return false;
            }

            switch(logLevel)
            {
                case LogLevel.Critical:
                    isSupported = _criticalEnabled;
                    break;

                case LogLevel.Debug:
                    isSupported = _debugEnabled;
                    break;

                case LogLevel.Error:
                    isSupported = _errorEnabled;
                    break;

                case LogLevel.Information:
                    isSupported = _informationEnabled;
                    break;

                case LogLevel.Trace:
                    isSupported = _traceEnabled;
                    break;

                case LogLevel.Warning:
                    isSupported = _warningEnabled;
                    break;

                case LogLevel.None:
                    isSupported = false;
                    break;

            }
            return isSupported;

        }

        /// <summary>
        /// Gets or sets a value indicating whether the Trace log level is enabled.
        /// </summary>
        /// <value>
        /// <b>true</b> to write <see cref="LogLevel.Trace"/> messages; otherwise, <b>false</b>.
        /// </value>
        public bool TraceEnabled { get => _traceEnabled; set => _traceEnabled = value; }

        /// <summary>
        /// Gets or sets a value indicating whether to prefix each log entry in the file with
        /// a date/time value.
        /// </summary>
        /// <value>
        /// <b>true</b> to write a date/time prefix for each log entry; otherwise, <b>false</b>.
        /// </value>
        public bool UseDatePrefix { get => _useDatePrefix; set => _useDatePrefix = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the Warning log level is enabled.
        /// </summary>
        /// <value>
        /// <b>true</b> to write <see cref="LogLevel.Warning"/> messages; otherwise, <b>false</b>.
        /// </value>
        public bool WarningEnabled { get => _warningEnabled; set => _warningEnabled = value; }
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState">The type of the state to associate with the scope.</typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>
        /// A <see cref="NullScope"/> instance, since this logger does not support scopes. The returned <see cref="NullScope"/> 
        /// instance is a no-op and does not perform any operations.
        /// </returns>        
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return NullScope.Instance;
        }


        /// <summary>
        /// Logs a message with the specified log level, state, exception, and formatter.
        /// </summary>
        /// <typeparam name="TState">
        /// The type of the state to associate with the log entry.
        /// </typeparam>
        /// <param name="logLevel">The log level for the message.</param>
        /// <param name="state">The state to associate with the log entry.</param>
        /// <param name="exception">The exception to associate with the log entry, if any.</param>
        /// <param name="formatter">The function to format the log entry.</param>
        public void Log<TState>(LogLevel logLevel, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                string? text = formatter(state, exception);
                if (text != null)
                {
                    StringBuilder builder = new();
                    if (_useDatePrefix)
                    {
                        builder.Append(DateTime.Now.ToString(CultureInfo.CurrentCulture) + ": ");
                    }
                    if (text != null)
                    {
                        builder.Append(text);
                    }
                    WriteLine(builder);
                    builder.Clear();
                }
            }
        }

        /// <summary>
        /// Logs a message with the specified log level, event ID, state, exception, and formatter.
        /// </summary>
        /// <typeparam name="TState">
        /// The type of the state to associate with the log entry.
        /// </typeparam>
        /// <param name="logLevel">The log level for the message.</param>
        /// <param name="eventId">The event ID for the log entry.</param>
        /// <param name="state">The state to associate with the log entry.</param>
        /// <param name="exception">The exception to associate with the log entry, if any.</param>
        /// <param name="formatter">The function to format the log entry.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (IsEnabled(logLevel))
            {
                string? text = formatter(state, exception);
                if (text != null)
                {
                    StringBuilder builder = new();
                    if (_useDatePrefix)
                    {
                        builder.Append(DateTime.Now.ToString(CultureInfo.CurrentCulture) + ": ");
                    }
                    builder.Append(eventId.ToString() + ": ");
                    if (text != null)
                    {
                        builder.Append(text);
                    }
                    WriteLine(builder);
                    builder.Clear();
                }
            }
        }
        #endregion

        #region Private Methods / Functions
        /// <summary>
        /// Creates the file stream and writer instances.
        /// </summary>
        private void CreateStreamObjects()
        {
            if (_fileName != null)
            {
                // Either create the file, or append to it.
                FileMode mode = FileMode.CreateNew;
                if (File.Exists(_fileName))
                {
                    mode = FileMode.Append;
                }

                try
                {
                    _destinationStream = new FileStream(_fileName, mode, FileAccess.Write, FileShare.Read);
                    _writer = new StreamWriter(_destinationStream);
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message);
                    _writer = null;
                    _destinationStream = null;
                }
            }
        }

        /// <summary>
        /// Closes the file stream and writer instances.
        /// </summary>
        private void CloseStreamObjects()
        {
            if (_writer != null)
            {
                try
                {
                    _writer.Flush();
                    _writer.Close();
                    _writer.Dispose();
                }
                catch
                {
                }
            }
            if (_destinationStream != null)
            {
                try
                {
                    _destinationStream.Flush();
                    _destinationStream.Close();
                    _destinationStream.Dispose();
                }
                catch
                {
                }
            }

            _writer = null;
            _destinationStream = null;
        }

        /// <summary>
        /// Writes the provided content to the log.
        /// </summary>
        /// <param name="builder">
        /// A <see cref="StringBuilder"/> containing the text to be written.
        /// </param>
        private void WriteLine(StringBuilder builder)
        {
            if (_writer != null)
            {
                try
                {
                    lock (_syncRoot)
                    {
                        _writer.WriteLine(builder.ToString());
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.Message);
                }
            }
        }
        #endregion
    }
}
