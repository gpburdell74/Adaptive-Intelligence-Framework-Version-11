namespace Adaptive.Intelligence.Abstractions.IO
{
    /// <summary>
    /// Provides the signature definition for binary writer instances that safely track their own exceptions.
    /// </summary>
    /// <seealso cref="IDisposable" />
    /// <seealso cref="IAsyncDisposable" />
    /// <seealso cref="IExceptionTracking" />
    public interface ISafeBinaryWriter : IDisposable, IAsyncDisposable, IExceptionTracking
    {
        #region Properties
        /// <summary>
        /// Returns the stream associated with the writer. It flushes all pending 
        /// writes before returning. All subclasses should override Flush to
        /// ensure that all buffered data is sent to the stream.
        /// </summary>
        /// <value>
        /// The underlying <see cref="Stream"/> that is being written to.
        /// </value>
        Stream? BaseStream { get; }

        /// <summary>
        /// Gets a value indicating whether this instance can write data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can write to an underlying stream; otherwise, <c>false</c>.
        /// </value>
        bool CanWrite { get; }

        /// <summary>
        /// Gets the reference to the underlying binary writer instance.
        /// </summary>
        /// <value>
        /// The <see cref="BinaryWriter"/> instance being used to do the writing.
        /// </value>
        BinaryWriter? Writer { get; }
        #endregion

        #region Methods / Functions
        /// <summary>
        /// Closes this writer and releases any system resources associated with the
        /// writer. Following a call to Close, any operations on the writer
        /// may raise exceptions.
        /// </summary>
        void Close();

        /// <summary>
        /// Clears all buffers for this writer and causes any buffered data to be
        /// written to the underlying device.
        /// </summary>
        void Flush();

        /// <summary>
        /// Moves the current position in the file to the specified location.
        /// </summary>
        /// <param name="offset">
        /// An integer specifying the byte offset index.
        /// </param>
        /// <param name="origin">
        /// A <see cref="SeekOrigin"/> enumerated value indicating the relative start position.
        /// </param>
        /// <returns>
        /// A <see cref="long"/> specifying the new position in the file.
        /// </returns>
        long Seek(int offset, SeekOrigin origin);

        /// <summary>
        /// Writes a boolean to this stream. A single byte is written to the stream
        /// with the value 0 representing false or the value 1 representing true.
        /// </summary>
        void Write(bool value);

        /// <summary>
        /// Writes a byte to this stream. The current position of the stream is
        /// advanced by one.
        /// </summary>
        void Write(byte value);

        /// <summary>
        /// Writes a signed byte to this stream. The current position of the stream
        /// is advanced by one.
        /// </summary>
        void Write(sbyte value);

        /// <summary>
        /// Writes a byte array to this stream.
        ///
        /// This default implementation calls the Write(Object, int, int)
        /// method to write the byte array.
        /// </summary>
        void Write(byte[] buffer);
        /// <summary>
        /// Writes a section of a byte array to this stream.
        ///
        /// This default implementation calls the Write(Object, int, int)
        /// method to write the byte array.
        /// </summary>
        void Write(byte[] buffer, int index, int count);
        /// <summary>
        /// Writes a character to this stream. The current position of the stream is
        /// advanced by two.
        /// Note this method cannot handle surrogates properly in UTF-8.
        /// </summary>
        void Write(char ch);
        /// <summary>
        /// Writes a character array to this stream.
        ///
        /// This default implementation calls the Write(Object, int, int)
        /// method to write the character array.
        /// </summary>
        void Write(char[] chars);
        /// <summary>
        /// Writes a section of a character array to this stream.
        ///
        /// This default implementation calls the Write(Object, int, int)
        /// method to write the character array.
        /// </summary>
        void Write(char[] chars, int index, int count);
        /// <summary>
        /// Writes the specified date/time value to the stream.
        /// </summary>
        /// <param name="dateTime">
        /// The <see cref="DateTime"/> value to be written.
        /// </param>
        void Write(DateTime dateTime);

        /// <summary>
        /// Writes a double to this stream. The current position of the stream is
        /// advanced by eight.
        /// </summary>
        void Write(double value);

        /// <summary>
        /// Writes a (16) sixteen-byte signed integer to this stream. The current position of
        /// the stream is advanced by sixteen (16).  
        /// </summary>
        void Write(decimal value);

        /// <summary>
        /// Writes a two-byte signed integer to this stream. The current position
        /// of the stream is advanced by two.
        /// </summary>
        void Write(short value);

        /// <summary>
        /// Writes a two-byte unsigned integer to this stream. The current position
        /// of the stream is advanced by two.
        /// </summary>
        void Write(ushort value);

        /// <summary>
        /// Writes a four-byte signed integer to this stream. The current position
        /// of the stream is advanced by four.
        /// </summary>
        void Write(int value);

        /// <summary>
        /// Writes a four-byte unsigned integer to this stream. The current position
        /// of the stream is advanced by four.
        /// </summary>
        void Write(uint value);

        /// <summary>
        /// Writes an eight-byte signed integer to this stream. The current position
        /// of the stream is advanced by eight.
        /// </summary>
        void Write(long value);

        /// <summary>
        /// Writes an eight-byte unsigned integer to this stream. The current
        /// position of the stream is advanced by eight.
        /// </summary>
        void Write(ulong value);

        /// <summary>
        /// Writes a float to this stream. The current position of the stream is
        /// advanced by four.
        /// </summary>
        void Write(float value);

        /// <summary>
        /// Writes a half to this stream. The current position of the stream is
        /// advanced by two.
        /// </summary>
        void Write(Half value);

        /// <summary>
        /// Writes a length-prefixed string to this stream in the BinaryWriter's
        /// current Encoding. This method first writes the length of the string as
        /// an encoded unsigned integer with variable length, and then writes that many characters
        /// to the stream.
        /// </summary>
        /// <param name="value">The value to be written.</param>
        void Write(string? value);

        /// <summary>
        /// Writes a boolean value indicating whether the data is null, and then, if not null,
        /// writes a length-prefixed string to this stream in the <see cref="BinaryWriter"/>'s
        /// current Encoding. This method first writes the length of the string as
        /// an encoded unsigned integer with variable length, and then writes that many characters
        /// to the stream.
        /// </summary>
        /// <param name="value">The value to be written.</param>
        void WriteNullable(string? value);

        /// <summary>
        /// Writes the content of the read-only span of bytes to the stream.
        /// </summary>
        /// <param name="buffer">
        /// The <see cref="ReadOnlySpan{T}"/> of <see cref="byte"/> containing the data to be written.
        /// </param>
        void Write(ReadOnlySpan<byte> buffer);

        /// <summary>
        /// Writes the content of the read-only span of chars to the stream.
        /// </summary>
        /// <param name="chars">
        /// The <see cref="ReadOnlySpan{T}"/> of <see cref="char"/> containing the data to be written.
        /// </param>
        void Write(ReadOnlySpan<char> chars);

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
        void WriteByteArray(byte[]? data);

        /// <summary>
        /// Writes the integer to the stream as a 7-bit encoded value.
        /// </summary>
        /// <param name="value">
        /// The <see cref="int"/> containing the data to be written.
        /// </param>
        void Write7BitEncodedInt(int value);

        /// <summary>
        /// Writes the long integer to the stream as a 7-bit encoded value.
        /// </summary>
        /// <param name="value">
        /// The <see cref="long"/> containing the data to be written.
        /// </param>
        void Write7BitEncodedInt64(long value);
        #endregion
    }

}