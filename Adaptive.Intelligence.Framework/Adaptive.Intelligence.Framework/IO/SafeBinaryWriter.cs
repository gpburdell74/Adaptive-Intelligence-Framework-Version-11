using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Abstractions.IO;

namespace Adaptive.Intelligence.IO
{
    /// <summary>
    /// Provides a mechanism for writing binary data to a stream where the instance tracks its own exceptions.	
    /// </summary>
    /// <seealso cref="ExceptionTrackingBase" />
    /// <seealso cref="ISafeBinaryWriter" />
    public sealed class SafeBinaryWriter : ExceptionTrackingBase, ISafeBinaryWriter
    {
        #region Private Member Declarations		
        /// <summary>
        /// The underlying writer instance.
        /// </summary>
        private BinaryWriter? _writer;
        /// <summary>
        /// A flag indicating whether the writer instance's scope is local.
        /// </summary>
        private readonly bool _writerLocal;
        /// <summary>
        /// The reference to the stream to be written to.
        /// </summary>
        private Stream? _outputStream;
        #endregion

        #region Constructor / Dispose Methods		
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeBinaryWriter"/> class.
        /// </summary>
        /// <param name="outputStream">
        /// The output <see cref="Stream"/> to be written to.
        /// </param>
        public SafeBinaryWriter(Stream outputStream)
        {
            _outputStream = outputStream;
            _writer = new BinaryWriter(outputStream);
            _writerLocal = true;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeBinaryWriter"/> class.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="BinaryWriter"/> instance to be used.
        /// </param>
        public SafeBinaryWriter(BinaryWriter writer)
        {
            _writer = writer;
            _outputStream = writer.BaseStream;
            _writerLocal = false;
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
                if (_writerLocal)
                {
                    _writer?.Dispose();
                }
            }

            _writer = null;
            _outputStream = null;
            base.Dispose(disposing);
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources asynchronously.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous dispose operation.
        /// </returns>
        public async ValueTask DisposeAsync()
        {
            Dispose(true);
        }
        #endregion

        #region Public Properties		
        /// <summary>
        /// Returns the stream associated with the writer. It flushes all pending
        /// writes before returning. All subclasses should override Flush to
        /// ensure that all buffered data is sent to the stream.
        /// </summary>
        /// <value>
        /// The underlying <see cref="Stream" /> that is being written to.
        /// </value>
        public Stream? BaseStream => _outputStream;
        /// <summary>
        /// Gets a value indicating whether this instance can write data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can write to an underlying stream; otherwise, <c>false</c>.
        /// </value>
        public bool CanWrite => _outputStream != null && _writer != null && _outputStream.CanWrite;

        /// <summary>
        /// Gets the reference to the underlying binary writer instance.
        /// </summary>
        /// <value>
        /// The <see cref="BinaryWriter" /> instance being used to do the writing.
        /// </value>
        public BinaryWriter? Writer => _writer;
        #endregion

        #region Public Methods / Functions		
        /// <summary>
        /// Closes this writer and releases any system resources associated with the
        /// writer. Following a call to Close, any operations on the writer
        /// may raise exceptions.
        /// </summary>
        public void Close()
        {
            _writer?.Close();
        }
        /// <summary>
        /// Clears all buffers for this writer and causes any buffered data to be
        /// written to the underlying device.
        /// </summary>
        public void Flush()
        {
            _writer?.Flush();
        }
        /// <summary>
        /// Moves the current position in the file to the specified location.
        /// </summary>
        /// <param name="offset">An integer specifying the byte offset index.</param>
        /// <param name="origin">A <see cref="SeekOrigin" /> enumerated value indicating the relative start position.</param>
        /// <returns>
        /// A <see cref="long" /> specifying the new position in the file.
        /// </returns>
        public long Seek(int offset, SeekOrigin origin)
        {
            long newPosition = -1;
            try
            {
                if (_writer != null)
                {
                    newPosition = _writer.Seek(offset, origin);
                }
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }

            return newPosition;
        }
        /// <summary>
        /// Writes a boolean to this stream. A single byte is written to the stream
        /// with the value 0 representing false or the value 1 representing true.
        /// </summary>
        /// <param name="value">
        /// The boolean value to be written.
        /// </param>
        public void Write(bool value)
        {
            try
            {
                _writer?.Write(value);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes a byte to this stream. The current position of the stream is
        /// advanced by one.
        /// </summary>
        /// <param name="value">
        /// The byte value to be written.
        /// </param>
        public void Write(byte value)
        {
            try
            {
                _writer?.Write(value);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes a signed byte to this stream. The current position of the stream
        /// is advanced by one.
        /// </summary>
        /// <param name="value">
        /// The <see cref="sbyte"/> value to be written.
        /// </param>
        public void Write(sbyte value)
        {
            try
            {
                _writer?.Write(value);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes a byte array to this stream.
        /// This default implementation calls the Write(Object, int, int)
        /// method to write the byte array.
        /// </summary>
        /// <param name="buffer">
        /// The byte array to be written to the stream.
        /// </param>
        public void Write(byte[] buffer)
        {
            try
            {
                _writer?.Write(buffer);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes a section of a byte array to this stream.
        /// This default implementation calls the Write(Object, int, int)
        /// method to write the byte array.
        /// </summary>
        /// <param name="buffer">
        /// The byte array to be written to the stream.
        /// </param>
        /// <param name="index">
        /// The ordinal index at which to begin reading from the byte array.
        /// </param>
        /// <param name="count">
        /// The number of bytes to be written.
        /// </param>
        public void Write(byte[] buffer, int index, int count)
        {
            try
            {
                _writer?.Write(buffer, index, count);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes a character to this stream. The current position of the stream is
        /// advanced by two.
        /// Note this method cannot handle surrogates properly in UTF-8.
        /// </summary>
        /// <param name="ch">
        /// The <see cref="char"/> value to be written to the stream.
        /// </param>
        public void Write(char ch)
        {
            try
            {
                _writer?.Write(ch);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes a character array to this stream.
        /// This default implementation calls the Write(Object, int, int)
        /// method to write the character array.
        /// </summary>
        /// <param name="chars">
        /// The <see cref="char"/> array to be written to the stream.
        /// </param>
        public void Write(char[] chars)
        {
            try
            {
                _writer?.Write(chars);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes a section of a character array to this stream.
        /// This default implementation calls the Write(Object, int, int)
        /// method to write the character array.
        /// </summary>
        /// <param name="chars">
        /// The <see cref="char"/> array to be written to the stream.
        /// </param>
        /// <param name="index">
        /// The ordinal index at which to begin reading from the character array.
        /// </param>
        /// <param name="count">
        /// The number of characters to be written.
        /// </param>
        public void Write(char[] chars, int index, int count)
        {
            try
            {
                _writer?.Write(chars, index, count);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes the specified double value to the stream.
        /// </summary>
        /// <param name="value">
        /// The <see cref="double"/> value to be written.
        /// </param>
        public void Write(double value)
        {
            try
            {
                _writer?.Write(value);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes the specified date/time value to the stream.  The value is converted to a filetime value and written
        /// as a long integer.
        /// </summary>
        /// <param name="dateTime">
        /// The <see cref="DateTime"/> value to be written.
        /// </param>
        public void Write(DateTime dateTime)
        {
            try
            {
                var value = dateTime.ToFileTime();
                _writer?.Write(value);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes the specified decimal value to the stream.
        /// </summary>
        /// <param name="value">
        /// The <see cref="decimal"/> value to be written.
        /// </param>
        public void Write(decimal value)
        {
            try
            {
                _writer?.Write(value);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes the specified short integer value to the stream.
        /// </summary>
        /// <param name="value">
        /// The <see cref="short"/> value to be written.
        /// </param>
        public void Write(short value)
        {
            try
            {
                _writer?.Write(value);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes the specified unsigned short integer value to the stream.
        /// </summary>
        /// <param name="value">
        /// The <see cref="ushort"/> value to be written.
        /// </param>
        public void Write(ushort value)
        {
            try
            {
                _writer?.Write(value);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes the specified integer value to the stream.
        /// </summary>
        /// <param name="value">
        /// The <see cref="int"/> value to be written.
        /// </param>
        public void Write(int value)
        {
            try
            {
                _writer?.Write(value);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }

        /// <summary>
        /// Writes the specified unsigned integer value to the stream.
        /// </summary>
        /// <param name="value">
        /// The <see cref="uint"/> value to be written.
        /// </param>
        public void Write(uint value)
        {
            try
            {
                _writer?.Write(value);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes an eight-byte signed integer to this stream. The current position
        /// of the stream is advanced by eight.
        /// </summary>
        /// <param name="value">
        /// The <see cref="long"/> value to be written.
        /// </param>
        public void Write(long value)
        {
            try
            {
                _writer?.Write(value);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes an eight-byte unsigned integer to this stream. The current
        /// position of the stream is advanced by eight.
        /// </summary>
        /// <param name="value">
        /// The <see cref="ulong"/> value to be written.
        /// </param>
        public void Write(ulong value)
        {
            try
            {
                _writer?.Write(value);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes a float to this stream. The current position of the stream is
        /// advanced by four.
        /// </summary>
        /// <param name="value">
        /// The <see cref="float"/> value to be written.
        /// </param>
        public void Write(float value)
        {
            try
            {
                _writer?.Write(value);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes a half to this stream. The current position of the stream is
        /// advanced by two.
        /// </summary>
        /// <param name="value">
        /// The <see cref="Half"/> value to be written.
        /// </param>
        public void Write(Half value)
        {
            try
            {
                _writer?.Write(value);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }

        /// <summary>
        /// Writes a length-prefixed string to this stream in the BinaryWriter's
        /// current Encoding. This method first writes the length of the string as
        /// an encoded unsigned integer with variable length, and then writes that many characters
        /// to the stream.
        /// </summary>
        /// <param name="value">
        /// The <see cref="string"/> value to be written.
        /// </param>
        public void Write(string? value)
        {
            try
            {
                if (value != null)
                {
                    _writer?.Write(value);
                }
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }


        /// <summary>
        /// Writes a length-prefixed string to this stream in the BinaryWriter's
        /// current Encoding. This method first writes the length of the string as
        /// an encoded unsigned integer with variable length, and then writes that many characters
        /// to the stream.
        /// </summary>
        /// <param name="value">
        /// The <see cref="string"/> value to be written.
        /// </param>
        public void WriteNullable(string? value)
        {
            try
            {
                if (value != null)
                {
                    _writer?.Write((byte)1);
                    _writer?.Write(value);
                }
                else
                {
                    _writer?.Write((byte)0);
                }
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }

        /// <summary>
        /// Writes the content of the read-only span of bytes to the stream.
        /// </summary>
        /// <param name="buffer">
        /// The <see cref="ReadOnlySpan{T}" /> of <see cref="byte" /> containing the data to be written
        /// </param>
        public void Write(ReadOnlySpan<byte> buffer)
        {
            try
            {
                _writer?.Write(buffer);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes the content of the read-only span of chars to the stream.
        /// </summary>
        /// <param name="chars">
        /// The <see cref="ReadOnlySpan{T}" /> of <see cref="char" /> containing the data to be written
        /// </param>
        public void Write(ReadOnlySpan<char> chars)
        {
            try
            {
                _writer?.Write(chars);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes the byte array to the stream.
        /// </summary>
        /// <remarks>
        /// This method writes a null/not null indicator, then, if the data is not <see langword="null"/>, then writes the <see cref="int"/> length indicator, and
        /// then, if present, writes the byte array.
        /// </remarks>
        /// <param name="data">
        /// A byte array containing the data to be written, or <b>null</b>.
        /// </param>
        public void WriteByteArray(byte[]? data)
        {
            if (_writer != null)
            {
                var isNull = data == null;
                var length = 0;
                if (data != null)
                {
                    length = data.Length;
                }

                try
                {
                    // Write the boolean null indicator.
                    _writer.Write(isNull);

                    // Continue if not null.
                    if (!isNull)
                    {
                        // Write the length indicator.
                        _writer.Write(length);

                        // Write the byte array, if data is present.
                        if (data != null && length > 0)
                        {
                            _writer.Write(data, 0, length);
                        }
                    }

                    _writer.Flush();
                }
                catch (Exception ex)
                {
                    Exceptions?.Add(ex);
                }
            }
        }
        /// <summary>
        /// Writes the integer to the stream as a 7-bit encoded value.
        /// </summary>
        /// <param name="value">
        /// The integer value to be written.
        /// </param>
        public void Write7BitEncodedInt(int value)
        {
            try
            {
                _writer?.Write7BitEncodedInt(value);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        /// <summary>
        /// Writes the long integer to the stream as a 7-bit encoded value.
        /// </summary>
        /// <param name="value">
        /// The long value to be written.
        /// </param>
        public void Write7BitEncodedInt64(long value)
        {
            try
            {
                _writer?.Write7BitEncodedInt64(value);
            }
            catch (Exception ex)
            {
                Exceptions?.Add(ex);
            }
        }
        #endregion
    }
}