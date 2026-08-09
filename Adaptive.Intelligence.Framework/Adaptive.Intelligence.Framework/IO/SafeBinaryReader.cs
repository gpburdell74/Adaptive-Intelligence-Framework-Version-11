using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Abstractions.IO;

namespace Adaptive.Intelligence.IO
{
    /// <summary>
    /// Provides a mechanism for reading binary data from a stream where the instance tracks its own exceptions.	
    /// </summary>
    /// <seealso cref="ExceptionTrackingBase" />
    /// <seealso cref="ISafeBinaryReader" />
    public sealed class SafeBinaryReader : ExceptionTrackingBase, ISafeBinaryReader
    {
        #region Private Member Declarations		
        /// <summary>
        /// The underlying reader instance.
        /// </summary>
        private BinaryReader? _reader;
        /// <summary>
        /// A flag indicating whether the reader instance's scope is local.
        /// </summary>
        private readonly bool _readerLocal;
        /// <summary>
        /// The reference to the stream to be read from.
        /// </summary>
        private Stream? _sourceStream;
        #endregion

        #region Constructor / Dispose Methods		
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeBinaryReader"/> class.
        /// </summary>
        /// <param name="sourceStream">
        /// The <see cref="Stream"/> to be read from.
        /// </param>
        public SafeBinaryReader(Stream sourceStream)
        {
            _sourceStream = sourceStream;
            _reader = new BinaryReader(sourceStream);
            _readerLocal = true;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SafeBinaryReader"/> class.
        /// </summary>
        /// <param name="reader">
        /// The <see cref="BinaryReader"/> instance to be used.
        /// </param>
        public SafeBinaryReader(BinaryReader reader)
        {
            _reader = reader;
            _sourceStream = reader.BaseStream;
            _readerLocal = false;
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing && _readerLocal)
            {
                _reader?.Dispose();
            }

            _reader = null;
            _sourceStream = null;
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
            await Task.Yield();
            Dispose(true);
        }
        #endregion

        #region Public Properties		
        /// <summary>
        /// Returns the stream associated with the reader. 
        /// </summary>
        /// <value>
        /// The underlying <see cref="Stream" /> that is being read from.
        /// </value>
        public Stream? BaseStream => _sourceStream;
        /// <summary>
        /// Gets a value indicating whether this instance can read data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can read from an underlying stream; otherwise, <c>false</c>.
        /// </value>
        public bool CanRead => _sourceStream != null && _reader != null && _sourceStream.CanRead;

        /// <summary>
        /// Gets the reference to the underlying binary reader instance.
        /// </summary>
        /// <value>
        /// The <see cref="BinaryReader" /> instance being used to do the reading.
        /// </value>
        public BinaryReader? Reader => _reader;
        #endregion

        #region Public Methods / Functions		
        /// <summary>
        /// Closes this reader and releases any system resources associated with the
        /// reader. Following a call to Close, any operations on the reader
        /// may raise exceptions.
        /// </summary>
        public void Close()
        {
            _reader?.Close();
        }
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
        public int Read(byte[] buffer, int startIndex, int numberOfBytes)
        {
            int length = 0;
            // Load the source content.
            byte[]? sourceData = ReadBytes(numberOfBytes);

            if (sourceData != null && sourceData.Length > 0)
            {
                // Copy to the provided array.
                try
                {
                    length = sourceData.Length;
                    Array.Copy(sourceData, 0, buffer, startIndex, length);
                }
                catch (Exception ex)
                {
                    AddException(ex);
                }
            }
            return length;
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
                if (_sourceStream != null)
                {
                    newPosition = _sourceStream.Seek(offset, origin);
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return newPosition;
        }
        /// <summary>
        /// Reads the next boolean value from the <see cref="Stream" />.
        /// </summary>
        /// <returns>
        /// The <see cref="bool" /> value that was read.
        /// </returns>
        public bool ReadBoolean()
        {
            bool value = false;
            try
            {
                if (_reader != null)
                {
                    value = _reader.ReadBoolean();
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return value;
        }
        /// <summary>
        /// Reads the next byte value from the <see cref="Stream" />.
        /// </summary>
        /// <returns>
        /// The <see cref="byte" /> value that was read.
        /// </returns>
        public byte ReadByte()
        {
            byte value = 0;
            try
            {
                if (_reader != null)
                {
                    value = _reader.ReadByte();
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return value;
        }
        /// <summary>
        /// Reads the next signed byte value from the <see cref="Stream" />.
        /// </summary>
        /// <returns>
        /// The <see cref="sbyte" /> value that was read.
        /// </returns>
        public sbyte ReadSByte()
        {
            sbyte value = 0;
            try
            {
                if (_reader != null)
                {
                    value = _reader.ReadSByte();
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return value;
        }
        /// <summary>
        /// Reads the next byte array value from the <see cref="Stream" />.
        /// </summary>
        /// <remarks>
        /// This method assumes a <see cref="int"/> length indicator precedes the byte array.  If the length indicator is zero (0),
        /// <b>null</b> is returned.
        /// </remarks>
        /// <returns>
        /// The <see cref="byte" /> array that was read, or <b>null</b>.
        /// </returns>
        public byte[]? ReadByteArray()
        {
            byte[]? data = null;
            int length;
            try
            {
                if (_reader != null)
                {
                    // Read the length indicator.
                    length = ReadInt32();

                    if (length > 0)
                    {
                        data = _reader.ReadBytes(length);
                    }
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return data;
        }
        /// <summary>
        /// Reads the next byte array value from the <see cref="Stream"/>.
        /// </summary>
        /// <remarks>
        /// This method expects a null indicator value, then a length indicator, and then the data.
        /// </remarks>
        /// <returns>
        /// The <see cref="byte"/> array that was read.
        /// </returns>
        public byte[]? ReadNullableByteArray()
        {
            byte[]? data = null;
            try
            {
                if (_reader != null)
                {
                    // Read the null indicator.
                    bool isNull = _reader.ReadBoolean();

                    if (!isNull)
                    {
                        // Read the length indicator.
                        int length = _reader.ReadInt32();

                        if (length > 0)
                        {
                            data = _reader.ReadBytes(length);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return data;
        }
        /// <summary>
        /// Reads a byte array from the <see cref="Stream"/>.
        /// </summary>
        /// <param name="count">
        /// The number of bytes to be read.
        /// </param>
        /// <returns>
        /// The <see cref="byte"/> array that was read.
        /// </returns>
        public byte[]? ReadBytes(int count)
        {
            byte[]? data = null;
            try
            {
                if (_reader != null)
                {
                    data = _reader.ReadBytes(count);
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return data;
        }
        /// <summary>
        /// Reads a character from the <see cref="Stream"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="char"/> value that was read.
        /// </returns>
        public char ReadChar()
        {
            char value = (char)0;
            try
            {
                if (_reader != null)
                {
                    value = _reader.ReadChar();
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return value;
        }
        /// <summary>
        /// Reads a character array from the <see cref="Stream"/>.
        /// </summary>
        /// <remarks>
        /// This method assumes a <see cref="int"/> length indicator preceeds the byte array.  If the length indicator is zero (0),
        /// <b>null</b> is returned.
        /// </remarks>
        /// <returns>
        /// The <see cref="char"/> array that was read.
        /// </returns>
        public char[]? ReadCharArray()
        {
            char[]? data = null;
            try
            {
                if (_reader != null)
                {
                    int length = _reader.ReadInt32();
                    data = _reader.ReadChars(length);
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return data;
        }
        /// <summary>
        /// Reads a character array from the <see cref="Stream"/>.
        /// </summary>
        /// <param name="count">
        /// The number of characters to be read.
        /// </param>
        /// <returns>
        /// The <see cref="char"/> array that was read.
        /// </returns>
        public char[]? ReadCharArray(int count)
        {
            char[]? data = null;
            try
            {
                if (_reader != null)
                {
                    data = _reader.ReadChars(count);
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return data;
        }
        /// <summary>
        /// Reads the specified date/time value from the stream.
        /// </summary>
        /// <remarks>
        /// This method assumes the date/time value is stored as a long integer containing the filetime value.
        /// </remarks>
        /// <returns>
        /// The <see cref="DateTime" /> value that was read.
        /// </returns>
        public DateTime ReadDateTime()
        {
            DateTime dateValue = DateTime.MinValue;

            try
            {
                if (_reader != null)
                {
                    long fileTime = _reader.ReadInt64();
                    dateValue = DateTime.FromFileTime(fileTime);
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return dateValue;
        }
        /// <summary>
        /// Reads the specified double-precision value from the stream.
        /// </summary>
        /// <returns>
        /// The <see cref="double" /> value that was read.
        /// </returns>
        public double ReadDouble()
        {
            double value = 0;
            try
            {
                if (_reader != null)
                {
                    value = _reader.ReadDouble();
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return value;
        }
        /// <summary>
        /// Reads the specified <see cref="decimal" /> value from the stream.
        /// </summary>
        /// <returns>
        /// The <see cref="decimal" /> value that was read.
        /// </returns>
        public decimal ReadDecimal()
        {
            decimal value = 0;
            try
            {
                if (_reader != null)
                {
                    value = _reader.ReadDecimal();
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return value;
        }
        /// <summary>
        /// Reads the specified short integer value from the stream.
        /// </summary>
        /// <returns>
        /// The <see cref="short" /> returns that was read.
        /// </returns>
        public short ReadInt16()
        {
            short returns = 0;
            try
            {
                if (_reader != null)
                {
                    returns = _reader.ReadInt16();
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return returns;
        }
        /// <summary>
        /// Reads the specified unsigned short integer returns from the stream.
        /// </summary>
        /// <returns></returns>
        /// <returns>
        /// The <see cref="ushort" /> returns that was read.
        /// </returns>
        public ushort ReadUInt16()
        {
            ushort returns = 0;
            try
            {
                if (_reader != null)
                {
                    returns = _reader.ReadUInt16();
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return returns;
        }
        /// <summary>
        /// Reads the specified integer returns from the stream.
        /// </summary>
        /// <returns></returns>
        /// <returns>
        /// The <see cref="int" /> returns that was read.
        /// </returns>
        public int ReadInt32()
        {
            int returns = 0;
            try
            {
                if (_reader != null)
                {
                    returns = _reader.ReadInt32();
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return returns;
        }
        /// <summary>
        /// Reads the specified unsigned integer returns from the stream.
        /// </summary>
        /// <returns></returns>
        /// <returns>
        /// The <see cref="uint" /> returns that was read.
        /// </returns>
        public uint ReadUInt32()
        {
            uint returns = 0;
            try
            {
                if (_reader != null)
                {
                    returns = _reader.ReadUInt32();
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return returns;
        }
        /// <summary>
        /// Reads the specified long integer returns from the stream.
        /// </summary>
        /// <returns></returns>
        /// <returns>
        /// The <see cref="long" /> returns that was read.
        /// </returns>
        public long ReadInt64()
        {
            long returns = 0;
            try
            {
                if (_reader != null)
                {
                    returns = _reader.ReadInt64();
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return returns;
        }
        /// <summary>
        /// Reads the specified unsigned long integer returns from the stream.
        /// </summary>
        /// <returns></returns>
        /// <returns>
        /// The <see cref="ulong" /> returns that was read.
        /// </returns>
        public ulong ReadUInt64()
        {
            ulong returns = 0;
            try
            {
                if (_reader != null)
                {
                    returns = _reader.ReadUInt64();
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return returns;
        }
        /// <summary>
        /// Reads the single-precision returns from the stream.
        /// </summary>
        /// <returns></returns>
        /// <returns>
        /// The <see cref="float" /> returns that was read.
        /// </returns>
        public float ReadSingle()
        {
            float returns = 0;
            try
            {
                if (_reader != null)
                {
                    returns = _reader.ReadSingle();
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return returns;
        }
        /// <summary>
        /// Reads the specified Half returns from the stream.
        /// </summary>
        /// <returns></returns>
        /// <returns>
        /// The <see cref="Half" /> returns that was read.
        /// </returns>
        public Half ReadHalf()
        {
            Half returns = Half.MinValue;
            try
            {
                if (_reader != null)
                {
                    returns = _reader.ReadHalf();
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return returns;
        }
        /// <summary>
        /// Reads the string.
        /// </summary>
        /// <returns></returns>
        public string? ReadString()
        {
            string? returns = null;
            try
            {
                if (_reader != null)
                {
                    returns = _reader.ReadString();
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return returns;
        }
        /// <summary>
        /// Reads the string.
        /// </summary>
        /// <returns></returns>
        public string? ReadNullableString()
        {
            string? returns = null;
            try
            {
                if (_reader != null)
                {
                    int hasData = _reader.ReadByte();
                    if (hasData == 1)
                    {
                        returns = _reader.ReadString();
                    }
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return returns;
        }
        /// <summary>
        /// Reads the integer from the stream as a 7-bit encoded returns.
        /// </summary>
        /// <returns>
        /// The <see cref="int" /> returns that was read.
        /// </returns>
        public int Read7BitEncodedInt32()
        {
            int returns = 0;
            try
            {
                if (_reader != null)
                {
                    returns = _reader.Read7BitEncodedInt();
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return returns;
        }
        /// <summary>
        /// Reads the long integer from the stream as a 7-bit encoded returns.
        /// </summary>
        /// <returns>
        /// The <see cref="long" /> returns that was read.
        /// </returns>
        public long Read7BitEncodedInt64()
        {
            long returns = 0;
            try
            {
                if (_reader != null)
                {
                    returns = _reader.Read7BitEncodedInt64();
                }
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
            return returns;
        }
        #endregion
    }
}
