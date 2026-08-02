namespace Adaptive.Intelligence.Abstractions.IO
{
    /// <summary>
    /// Provides the signature definition for binary reader instances that safely track their own exceptions.
    /// </summary>
    /// <seealso cref="IDisposable" />
    /// <seealso cref="IAsyncDisposable" />
    /// <seealso cref="IExceptionTracking" />
    public interface ISafeBinaryReader : IDisposable, IAsyncDisposable, IExceptionTracking
    {
        #region Properties
        /// <summary>
        /// Returns the stream associated with the writer.
        /// </summary>
        /// <value>
        /// The underlying <see cref="Stream"/> that is being read from.
        /// </value>
        Stream? BaseStream { get; }

        /// <summary>
        /// Gets a value indicating whether this instance can read data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can read from an underlying stream; otherwise, <c>false</c>.
        /// </value>
        bool CanRead { get; }

        /// <summary>
        /// Gets the reference to the underlying binary reader instance.
        /// </summary>
        /// <value>
        /// The <see cref="BinaryReader"/> instance being used to do the reading.
        /// </value>
        BinaryReader? Reader { get; }
        #endregion

        #region Methods / Functions
        /// <summary>
        /// Closes this reader and releases any system resources associated with the
        /// reader. Following a call to Close, any operations on the reader
        /// may raise exceptions.
        /// </summary>
        void Close();

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
        /// Reads the byte array into the specified buffer.
        /// </summary>
        /// <param name="buffer">
        /// The byte array buffer to read data into.
        /// </param>
        /// <param name="startIndex">
        /// The ordinal index of the array at which to start writing.
        /// </param>
        /// <param name="numberOfBytes">
        /// An integer specifying the number of bytes to be read.
        /// </param>
        /// <returns>
        /// The actual number of bytest that were read.
        /// </returns>
        int Read(byte[] buffer, int startIndex, int numberOfBytes);

        /// <summary>
        /// Reads the next boolean value from the <see cref="Stream"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/> value that was read.
        /// </returns>
        bool ReadBoolean();
        /// <summary>
        /// Reads the next byte value from the <see cref="Stream"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="byte"/> value that was read.
        /// </returns>
        byte ReadByte();
        /// <summary>
        /// Reads the next signed byte value from the <see cref="Stream"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="sbyte"/> value that was read.
        /// </returns>
        sbyte ReadSByte();
        /// <summary>
        /// Reads the next byte array value from the <see cref="Stream"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="byte"/> array that was read.
        /// </returns>
        byte[]? ReadByteArray();
        /// <summary>
        /// Reads a byte array from the <see cref="Stream"/>.
        /// </summary>
        /// <param name="count">
        /// The number of bytes to be read.
        /// </param>
        /// <returns>
        /// The <see cref="byte"/> array that was read.
        /// </returns>
        byte[]? ReadBytes(int count);
        /// <summary>
        /// Reads a character from the <see cref="Stream"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="char"/> value that was read.
        /// </returns>
        char ReadChar();
        /// <summary>
        /// Reads a character array from the <see cref="Stream"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="char"/> array that was read.
        /// </returns>
        char[]? ReadCharArray();
        /// <summary>
        /// Reads a character array from the <see cref="Stream"/>.
        /// </summary>
        /// <param name="count">
        /// The number of characters to be read.
        /// </param>
        /// <returns>
        /// The <see cref="char"/> array that was read.
        /// </returns>
        char[]? ReadCharArray(int count);
        /// <summary>
        /// Reads the specified date/time returns from the stream.
        /// </summary>
        /// <returns>
        /// The <see cref="DateTime"/> returns that was read.
        /// </returns>
        DateTime ReadDateTime();
        /// <summary>
        /// Reads the specified double-precision returns from the stream.
        /// </summary>
        /// <returns>
        /// The <see cref="double"/> returns that was read.
        /// </returns>
        double ReadDouble();
        /// <summary>
        /// Reads the specified <see cref="decimal"/> returns from the stream.
        /// </summary>
        /// <returns>
        /// The <see cref="decimal"/> returns that was read.
        /// </returns>
        decimal ReadDecimal();
        /// <summary>
        /// Reads the specified short integer returns from the stream.
        /// </summary>
        /// <returns>
        /// The <see cref="short"/> returns that was read.
        /// </returns>
        short ReadInt16();
        /// <summary>
        /// Reads the specified unsigned short integer returns from the stream.
        /// </summary>
        /// <returns>
        /// The <see cref="ushort"/> returns that was read.
        /// </returns>
        ushort ReadUInt16();
        /// <summary>
        /// Reads the specified integer returns from the stream.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/> returns that was read.
        /// </returns>
        int ReadInt32();
        /// <summary>
        /// Reads the specified unsigned integer returns from the stream.
        /// </summary>
        /// <returns>
        /// The <see cref="uint"/> returns that was read.
        /// </returns>
        uint ReadUInt32();
        /// <summary>
        /// Reads the specified long integer returns from the stream.
        /// </summary>
        /// <returns>
        /// The <see cref="long"/> returns that was read.
        /// </returns>
        long ReadInt64();
        /// <summary>
        /// Reads the specified unsigned long integer returns from the stream.
        /// </summary>
        /// <returns>
        /// The <see cref="ulong"/> returns that was read.
        /// </returns>
        ulong ReadUInt64();
        /// <summary>
        /// Reads the single-precision returns from the stream.
        /// </summary>
        /// <returns>
        /// The <see cref="float"/> returns that was read.
        /// </returns>
        float ReadSingle();
        /// <summary>
        /// Reads the specified Half returns from the stream.
        /// </summary>
        /// <returns>
        /// The <see cref="Half"/> returns that was read.
        /// </returns>
        Half ReadHalf();
        /// <summary>
        /// Reads a length-prefixed string from this stream in the BinaryReader's
        /// current Encoding. 
        /// </summary>
        /// <returns>
        /// The string that was read, or <b>null</b>.
        /// </returns>
        string? ReadString();
        /// <summary>
        /// Reads a length-prefixed string from this stream in the BinaryReader's
        /// current Encoding. 
        /// </summary>
        /// <returns>
        /// The string that was read, or <b>null</b>.
        /// </returns>
        string? ReadNullableString();

        /// <summary>
        /// Reads the integer from the stream as a 7-bit encoded returns.
        /// </summary>
        /// <returns>
        /// The <see cref="int"/> returns that was read.
        /// </returns>
        int Read7BitEncodedInt32();
        /// <summary>
        /// Reads the long integer from the stream as a 7-bit encoded returns.
        /// </summary>
        /// <returns>
        /// The <see cref="long"/> returns that was read.
        /// </returns>
        long Read7BitEncodedInt64();
        #endregion
    }
}
