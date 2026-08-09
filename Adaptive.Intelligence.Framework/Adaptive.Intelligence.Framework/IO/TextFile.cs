using Adaptive.Intelligence.Abstractions;
using System.Security;

namespace Adaptive.Intelligence.IO
{
    /// <summary>
    /// Provides a simple class mechanism for reading from and writing to a text
    /// file on disk.
    /// </summary>
    public sealed class TextFile : DisposableObjectBase
    {
        #region Private Member Declarations		
        /// <summary>
        /// The underlying file stream instance.
        /// </summary>
        private FileStream? _stream;
        /// <summary>
        /// The underlying reader instance.
        /// </summary>
        private StreamReader? _reader;
        /// <summary>
        /// The underlying stream writer instance.
        /// </summary>
        private StreamWriter? _writer;
        /// <summary>
        /// The file name.
        /// </summary>
        private string? _fileName;
        #endregion

        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="TextFile"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public TextFile()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="TextFile"/> class.
        /// </summary>
        /// <param name="fileName">
        /// A string containing the fully-qualified path and name of the file.
        /// </param>
        public TextFile(string fileName)
        {
            _fileName = fileName;
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
                _reader?.Dispose();
                _writer?.Dispose();
                _stream?.Dispose();
            }

            _reader = null;
            _writer = null;
            _stream = null;
            _fileName = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets a value indicating whether the file can be read.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the file can be read; otherwise, <c>false</c>.
        /// </value>
        public bool CanRead => _stream != null && _reader != null && _stream.CanRead;
        /// <summary>
        /// Gets a value indicating whether the file can be written to.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the file can be written to; otherwise, <c>false</c>.
        /// </value>
        public bool CanWrite => _stream != null && _writer != null && _stream.CanWrite;
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <remarks>
        /// This property cannot be set if the file is currently open.
        /// </remarks>
        /// <value>
        /// A string containing the fully-qualified path and name of the file.
        /// </value>
        public string? FileName
        {
            get => _fileName;
            set
            {
                if (_stream == null)
                {
                    _fileName = value;
                }
            }
        }
        /// <summary>
        /// Gets a value indicating whether the file is open.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the file is open; otherwise, <c>false</c>.
        /// </value>
        public bool IsOpen => _stream != null;
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Closes the file, if open.
        /// </summary>
        public void Close()
        {
            _reader?.Close();
            _writer?.Close();
            _stream?.Dispose();

            _reader = null;
            _writer = null;
            _stream = null;
            _fileName = null;
            GC.Collect();
        }
        /// <summary>
        /// Attempts to create the file for writing.
        /// </summary>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        public bool Create()
        {
            bool success = false;
            if (!string.IsNullOrEmpty(_fileName))
            {
                success = Create(_fileName);
            }

            return success;
        }
        /// <summary>
        /// Attempts to create the file for writing.
        /// </summary>
        /// <param name="fileName">
        /// A string containing the fully-qualified path and name of the file.
        /// </param>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        public bool Create(string fileName)
        {
            _fileName = fileName;
            bool success = OpenStreamForWrite(true, fileName);
            if (success)
            {
                success = CreateWriter();
                if (!success)
                {
                    Close();
                }
            }
            return success;
        }
        /// <summary>
        /// Attempts to delete the file.
        /// </summary>
        /// <remarks>
        /// Calling this method will close the file, if currently open.
        /// </remarks>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        public bool Delete()
        {
            bool success = false;
            if (!string.IsNullOrEmpty(_fileName))
            {
                success = Delete(_fileName);
            }

            return success;
        }
        /// <summary>
        /// Attempts to delete the file.
        /// </summary>
        /// <remarks>
        /// Calling this method will close the file, if currently open.
        /// </remarks>
        /// <param name="fileName">
        /// A string containing the fully-qualified path and name of the file.
        /// </param>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        public bool Delete(string fileName)
        {
            bool success = true;
            bool exists = false;

            Close();
            try
            {
                exists = File.Exists(fileName);
            }
            catch (IOException ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message);
            }

            if (exists)
            {
                try
                {
                    File.Delete(fileName);
                }
                catch (IOException ioEx)
                {
                    success = false;
                    System.Diagnostics.Trace.TraceError(ioEx.Message);
                }
                catch (SecurityException secEx)
                {
                    success = false;
                    System.Diagnostics.Trace.TraceError(secEx.Message);
                }
            }
            return success;
        }
        /// <summary>
        /// Attempts to open the file for reading.
        /// </summary>
        /// <remarks>
        /// The <see cref="FileName"/> property must be set to a valid file name to use
        /// this method overload.
        /// </remarks>
        /// <returns>
        /// <b>true</b> if the file is opened successfully; otherwise, returns
        /// <b>false</b>.
        /// </returns>
        public bool OpenForRead()
        {
            bool success = false;
            if (!string.IsNullOrEmpty(_fileName))
            {
                success = OpenForRead(_fileName);
            }

            return success;
        }
        /// <summary>
        /// Attempts to open the file for reading.
        /// </summary>
        /// <param name="fileName">
        /// A string containing the fully-qualified path and name of the file.
        /// </param>
        /// <returns>
        /// <b>true</b> if the file is opened successfully; otherwise, returns
        /// <b>false</b>.
        /// </returns>
        public bool OpenForRead(string fileName)
        {
            _fileName = fileName;

            bool success = OpenStreamForRead(fileName);
            if (success)
            {
                success = CreateReader();
                if (!success)
                {
                    Close();
                }
            }
            return success;
        }
        /// <summary>
        /// Attempts to open the file for writing.
        /// </summary>
        /// <remarks>
        /// The <see cref="FileName"/> property must be set to a valid file name to use
        /// this method overload.
        ///
        /// This method is for appending to an existing text file.  The <see cref="Create()"/>
        /// and <see cref="Create(string)"/> methods are used to create new text files.
        /// </remarks>
        /// <returns>
        /// <b>true</b> if the file is opened successfully; otherwise, returns
        /// <b>false</b>.
        /// </returns>
        public bool OpenForWrite()
        {
            bool success = false;
            if (!string.IsNullOrEmpty(_fileName))
            {
                success = OpenForWrite(_fileName);
            }

            return success;
        }
        /// <summary>
        /// Attempts to open the file for writing.
        /// </summary>
        /// <remarks>
        /// This method is for appending to an existing text file.  The <see cref="Create()"/>
        /// and <see cref="Create(string)"/> methods are used to create new text files.
        /// </remarks>
        /// <param name="fileName">
        /// A string containing the fully-qualified path and name of the file.
        /// </param>
        /// <returns>
        /// <b>true</b> if the file is opened successfully; otherwise, returns
        /// <b>false</b>.
        /// </returns>
        public bool OpenForWrite(string fileName)
        {
            _fileName = fileName;
            bool success = OpenStreamForWrite(false, fileName);
            if (success)
            {
                success = CreateWriter();
                if (!success)
                {
                    Close();
                }
            }
            return success;
        }
        /// <summary>
        /// Attempts to read a line of text from an open file.
        /// </summary>
        /// <returns>
        /// A string containing the line of text, or <b>null</b> if the
        /// file is not open, or no more text can be read.
        /// </returns>
        public string? ReadLine()
        {
            string? line = null;

            if (_stream != null && _reader != null)
            {
                try
                {
                    line = _reader?.ReadLine();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }
            return line;
        }
        /// <summary>
        /// Reads all the content of the text file as a single string.
        /// </summary>
        /// <returns>
        /// A string containing the content of the text file, or <b>null</b> if the
        /// file is not open, or no text can be read.
        /// </returns>
        public string? ReadAll()
        {
            string? text = null;

            if (_stream != null && _reader != null)
            {
                try
                {
                    text = _reader.ReadToEnd();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }
            return text;
        }
        /// <summary>
        /// Writes the specified text to the open file..
        /// </summary>
        /// <param name="text">
        /// A string containing the text to be written.</param>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        public bool Write(string text)
        {
            bool success = false;

            if (_stream != null && _writer != null)
            {
                try
                {
                    _writer.Write(text);
                    _writer.Flush();
                    success = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }

            }
            return success;
        }
        /// <summary>
        /// Writes the specified text as a single line to the open file..
        /// </summary>
        /// <param name="text">
        /// A string containing the line of text to be written.</param>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        public bool WriteLine(string text)
        {
            bool success = false;

            if (_stream != null && _writer != null)
            {
                try
                {
                    _writer.WriteLine(text);
                    _writer.Flush();
                    success = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }
            return success;
        }
        #endregion

        #region Private Methods / Functions
        /// <summary>
        /// Attempts to create the internal <see cref="StreamReader"/> instance.
        /// </summary>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        private bool CreateReader()
        {
            bool success = false;
            if (_stream != null && _reader == null && _stream.CanRead)
            {
                try
                {
                    _stream.Seek(0, SeekOrigin.Begin);
                    _reader = new StreamReader(_stream);
                    success = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }
            return success;
        }
        /// <summary>
        /// Attempts to create the internal <see cref="StreamWriter"/> instance.
        /// </summary>
        /// <returns>
        /// <b>true</b> if the operation is successful; otherwise, returns <b>false</b>.
        /// </returns>
        private bool CreateWriter()
        {
            bool success = false;
            if (_stream != null && _writer == null && _stream.CanWrite)
            {
                try
                {
                    _writer = new StreamWriter(_stream);
                    success = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                }
            }
            return success;
        }
        /// <summary>
        /// Opens the stream for reading.
        /// </summary>
        /// <param name="fileName">
        /// A string containing the fully-qualified path and name of the file.</param>
        /// <returns>
        /// <b>true</b> if the file is opened successfully; otherwise,
        /// <b>false</b>.
        /// </returns>
        private bool OpenStreamForRead(string fileName)
        {
            bool success = false;

            if (_stream == null)
            {
                if (File.Exists(fileName))
                {
                    try
                    {
                        _stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError(ex.Message);
                    }

                }
            }
            return success;
        }
        /// <summary>
        /// Opens the stream for writing.
        /// </summary>
        /// <param name="createNew">
        /// <b>true</b> to create a new file, or <b>false</b> to append to the end of an
        /// existing file.
        /// </param>
        /// <param name="fileName">
        /// A string containing the fully-qualified path and name of the file.</param>
        /// <returns>
        /// <b>true</b> if the file is created or opened successfully; otherwise,
        /// <b>false</b>.
        /// </returns>
        private bool OpenStreamForWrite(bool createNew, string fileName)
        {
            bool success = false;

            if (_stream == null)
            {
                // Remove the old file, if present.
                if (createNew)
                {
                    success = Delete(fileName);
                }

                // Set the file mode.
                FileMode mode = FileMode.Append;
                if (createNew)
                {
                    mode = FileMode.CreateNew;
                }

                try
                {
                    _stream = new FileStream(fileName, mode, FileAccess.Write);
                    success = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.Message);
                    _stream = null;
                }
            }
            return success;
        }
        #endregion
    }
}
