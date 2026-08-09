using System.IO;
using Adaptive.Intelligence.IO;
using Xunit;

namespace Adaptive.Intelligence.Framework.Tests;

/// <summary>
/// Provides tests for the <see cref="SafeBinaryReader"/> class.
/// </summary>
public class SafeBinaryReaderTests
{
    [Fact]
    public void Constructor_Stream_SetsBaseStreamReaderAndCanRead()
    {
        // Arrange
        using MemoryStream sourceStream = new([1, 2, 3]);

        // Act
        using SafeBinaryReader reader = new(sourceStream);

        // Assert
        Assert.Same(sourceStream, reader.BaseStream);
        Assert.NotNull(reader.Reader);
        Assert.True(reader.CanRead);
    }

    [Fact]
    public void Constructor_BinaryReader_SetsBaseStreamReaderAndCanRead()
    {
        // Arrange
        using MemoryStream sourceStream = new([10, 20, 30]);
        using BinaryReader binaryReader = new(sourceStream);

        // Act
        using SafeBinaryReader reader = new(binaryReader);

        // Assert
        Assert.Same(sourceStream, reader.BaseStream);
        Assert.Same(binaryReader, reader.Reader);
        Assert.True(reader.CanRead);
    }

    [Fact]
    public async Task DisposeAsync_StreamConstructor_DisposesUnderlyingStreamAndClearsState()
    {
        // Arrange
        MemoryStream sourceStream = new([4, 5, 6]);
        SafeBinaryReader reader = new(sourceStream);

        // Act
        await reader.DisposeAsync();

        // Assert
        Assert.Null(reader.BaseStream);
        Assert.Null(reader.Reader);
        Assert.False(reader.CanRead);
        Assert.False(sourceStream.CanRead);
    }

    [Fact]
    public async Task DisposeAsync_BinaryReaderConstructor_DoesNotDisposeExternalReader()
    {
        // Arrange
        using MemoryStream sourceStream = new([7, 8, 9]);
        using BinaryReader binaryReader = new(sourceStream);
        SafeBinaryReader reader = new(binaryReader);

        // Act
        await reader.DisposeAsync();

        // Assert
        Assert.Null(reader.BaseStream);
        Assert.Null(reader.Reader);
        Assert.False(reader.CanRead);
        Assert.True(sourceStream.CanRead);
        Assert.Equal(7, binaryReader.ReadByte());
    }

    [Fact]
    public void Read_ValidBufferAndBytesAvailable_CopiesBytesAndReturnsLength()
    {
        // Arrange
        using MemoryStream sourceStream = new([1, 2, 3, 4]);
        using SafeBinaryReader reader = new(sourceStream);
        byte[] buffer = new byte[4];

        // Act
        int length = reader.Read(buffer, 0, 4);

        // Assert
        Assert.Equal(4, length);
        Assert.Equal(new byte[] { 1, 2, 3, 4 }, buffer);
    }

    [Fact]
    public async Task Reader_AfterDisposeAsync_ReturnsNull()
    {
        // Arrange
        using MemoryStream sourceStream = new([11, 12]);
        SafeBinaryReader reader = new(sourceStream);

        // Act
        await reader.DisposeAsync();

        // Assert
        Assert.Null(reader.Reader);
    }

    [Fact]
    public void Close_StreamConstructor_ClosesUnderlyingStream()
    {
        // Arrange
        using MemoryStream sourceStream = new([13, 14]);
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        reader.Close();

        // Assert
        Assert.False(sourceStream.CanRead);
    }

    [Fact]
    public async Task Close_AfterDisposeAsync_DoesNotThrow()
    {
        // Arrange
        using MemoryStream sourceStream = new([15, 16]);
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        Exception? exception = Record.Exception(reader.Close);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public async Task Read_AfterDisposeAsync_ReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new([1, 2, 3]);
        SafeBinaryReader reader = new(sourceStream);
        byte[] buffer = new byte[3];
        await reader.DisposeAsync();

        // Act
        int length = reader.Read(buffer, 0, 3);

        // Assert
        Assert.Equal(0, length);
    }

    [Fact]
    public void Read_BufferTooSmall_CatchesCopyExceptionAndReturnsSourceLength()
    {
        // Arrange
        using MemoryStream sourceStream = new([20, 21, 22, 23]);
        using SafeBinaryReader reader = new(sourceStream);
        byte[] buffer = new byte[2];

        // Act
        int length = reader.Read(buffer, 0, 4);

        // Assert
        Assert.Equal(4, length);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public void Seek_ValidOffset_ReturnsNewPosition()
    {
        // Arrange
        using MemoryStream sourceStream = new([30, 31, 32, 33]);
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        long position = reader.Seek(2, SeekOrigin.Begin);

        // Assert
        Assert.Equal(2, position);
    }

    [Fact]
    public void Seek_ClosedUnderlyingStream_CatchesExceptionAndReturnsNegativeOne()
    {
        // Arrange
        using MemoryStream sourceStream = new([40, 41, 42]);
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        long position = reader.Seek(0, SeekOrigin.Begin);

        // Assert
        Assert.Equal(-1, position);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public void ReadBoolean_ValueAvailable_ReturnsTrue()
    {
        // Arrange
        using MemoryStream sourceStream = new([1]);
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        bool value = reader.ReadBoolean();

        // Assert
        Assert.True(value);
    }

    [Fact]
    public void ReadBoolean_AfterClose_CatchesExceptionAndReturnsFalse()
    {
        // Arrange
        using MemoryStream sourceStream = new([1]);
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        bool value = reader.ReadBoolean();

        // Assert
        Assert.False(value);
        Assert.True(reader.HasExceptions);
    }


    [Fact]
    public void ReadByte_ValueAvailable_ReturnsByte()
    {
        // Arrange
        using MemoryStream sourceStream = new([0x2A]);
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        byte value = reader.ReadByte();

        // Assert
        Assert.Equal((byte)0x2A, value);
    }

    [Fact]
    public void ReadByte_AfterClose_CatchesExceptionAndReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new([0x2A]);
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        byte value = reader.ReadByte();

        // Assert
        Assert.Equal((byte)0, value);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadByte_AfterDisposeAsync_ReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new([0x2A]);
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        byte value = reader.ReadByte();

        // Assert
        Assert.Equal((byte)0, value);
    }

    [Fact]
    public void ReadSByte_ValueAvailable_ReturnsSByte()
    {
        // Arrange
        using MemoryStream sourceStream = new([0xFE]);
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        sbyte value = reader.ReadSByte();

        // Assert
        Assert.Equal((sbyte)-2, value);
    }

    [Fact]
    public void ReadSByte_AfterClose_CatchesExceptionAndReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new([0xFE]);
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        sbyte value = reader.ReadSByte();

        // Assert
        Assert.Equal((sbyte)0, value);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadSByte_AfterDisposeAsync_ReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new([0xFE]);
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        sbyte value = reader.ReadSByte();

        // Assert
        Assert.Equal((sbyte)0, value);
    }

    [Fact]
    public void ReadByteArray_PositiveLength_ReturnsData()
    {
        // Arrange
        using MemoryStream sourceStream = new([3, 0, 0, 0, 10, 11, 12]);
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        byte[]? data = reader.ReadByteArray();

        // Assert
        Assert.NotNull(data);
        Assert.Equal(new byte[] { 10, 11, 12 }, data);
    }

    [Fact]
    public void ReadByteArray_ZeroLength_ReturnsNull()
    {
        // Arrange
        using MemoryStream sourceStream = new([0, 0, 0, 0]);
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        byte[]? data = reader.ReadByteArray();

        // Assert
        Assert.Null(data);
    }

    [Fact]
    public void ReadByteArray_AfterClose_CatchesExceptionAndReturnsNull()
    {
        // Arrange
        using MemoryStream sourceStream = new([1, 0, 0, 0, 99]);
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        byte[]? data = reader.ReadByteArray();

        // Assert
        Assert.Null(data);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadByteArray_AfterDisposeAsync_ReturnsNull()
    {
        // Arrange
        using MemoryStream sourceStream = new([1, 0, 0, 0, 99]);
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        byte[]? data = reader.ReadByteArray();

        // Assert
        Assert.Null(data);
    }

    [Fact]
    public void ReadNullableByteArray_NullIndicatorTrue_ReturnsNull()
    {
        // Arrange
        using MemoryStream sourceStream = new([1]);
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        byte[]? data = reader.ReadNullableByteArray();

        // Assert
        Assert.Null(data);
    }

    [Fact]
    public void ReadNullableByteArray_NullIndicatorFalseAndPositiveLength_ReturnsData()
    {
        // Arrange
        using MemoryStream sourceStream = new([0, 3, 0, 0, 0, 1, 2, 3]);
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        byte[]? data = reader.ReadNullableByteArray();

        // Assert
        Assert.NotNull(data);
        Assert.Equal(new byte[] { 1, 2, 3 }, data);
    }

    [Fact]
    public void ReadNullableByteArray_NullIndicatorFalseAndZeroLength_ReturnsNull()
    {
        // Arrange
        using MemoryStream sourceStream = new([0, 0, 0, 0, 0]);
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        byte[]? data = reader.ReadNullableByteArray();

        // Assert
        Assert.Null(data);
    }

    [Fact]
    public void ReadNullableByteArray_AfterClose_CatchesExceptionAndReturnsNull()
    {
        // Arrange
        using MemoryStream sourceStream = new([0, 2, 0, 0, 0, 1, 2]);
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        byte[]? data = reader.ReadNullableByteArray();

        // Assert
        Assert.Null(data);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public void ReadBytes_CountSpecified_ReturnsRequestedBytes()
    {
        // Arrange
        using MemoryStream sourceStream = new([5, 6, 7]);
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        byte[]? data = reader.ReadBytes(2);

        // Assert
        Assert.NotNull(data);
        Assert.Equal(new byte[] { 5, 6 }, data);
    }

    [Fact]
    public void ReadBytes_AfterClose_CatchesExceptionAndReturnsNull()
    {
        // Arrange
        using MemoryStream sourceStream = new([5, 6, 7]);
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        byte[]? data = reader.ReadBytes(2);

        // Assert
        Assert.Null(data);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadBytes_AfterDisposeAsync_ReturnsNull()
    {
        // Arrange
        using MemoryStream sourceStream = new([5, 6, 7]);
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        byte[]? data = reader.ReadBytes(2);

        // Assert
        Assert.Null(data);
    }



    [Fact]
    public void ReadChar_ValueAvailable_ReturnsChar()
    {
        // Arrange
        using MemoryStream sourceStream = new([65]);
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        char value = reader.ReadChar();

        // Assert
        Assert.Equal('A', value);
    }
    [Fact]
    public void ReadChar_AfterClose_CatchesExceptionAndReturnsDefaultChar()
    {
        // Arrange
        using MemoryStream sourceStream = new([65]);
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        char value = reader.ReadChar();

        // Assert
        Assert.Equal('\0', value);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadChar_AfterDisposeAsync_ReturnsDefaultChar()
    {
        // Arrange
        using MemoryStream sourceStream = new([65]);
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        char value = reader.ReadChar();

        // Assert
        Assert.Equal('\0', value);
    }

    [Fact]
    public void ReadCharArray_LengthPrefixedData_ReturnsCharacters()
    {
        // Arrange
        using MemoryStream sourceStream = new([3, 0, 0, 0, 65, 66, 67]);
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        char[]? data = reader.ReadCharArray();

        // Assert
        Assert.NotNull(data);
        Assert.Equal(['A', 'B', 'C'], data);
    }

    [Fact]
    public void ReadCharArray_AfterClose_CatchesExceptionAndReturnsNull()
    {
        // Arrange
        using MemoryStream sourceStream = new([3, 0, 0, 0, 65, 66, 67]);
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        char[]? data = reader.ReadCharArray();

        // Assert
        Assert.Null(data);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadCharArray_AfterDisposeAsync_ReturnsNull()
    {
        // Arrange
        using MemoryStream sourceStream = new([3, 0, 0, 0, 65, 66, 67]);
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        char[]? data = reader.ReadCharArray();

        // Assert
        Assert.Null(data);
    }

    [Fact]
    public void ReadCharArray_CountSpecified_ReturnsCharacters()
    {
        // Arrange
        using MemoryStream sourceStream = new([88, 89, 90]);
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        char[]? data = reader.ReadCharArray(2);

        // Assert
        Assert.NotNull(data);
        Assert.Equal(['X', 'Y'], data);
    }

    [Fact]
    public void ReadCharArray_CountSpecifiedAfterClose_CatchesExceptionAndReturnsNull()
    {
        // Arrange
        using MemoryStream sourceStream = new([88, 89, 90]);
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        char[]? data = reader.ReadCharArray(2);

        // Assert
        Assert.Null(data);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadCharArray_CountSpecifiedAfterDisposeAsync_ReturnsNull()
    {
        // Arrange
        using MemoryStream sourceStream = new([88, 89, 90]);
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        char[]? data = reader.ReadCharArray(2);

        // Assert
        Assert.Null(data);
    }

    [Fact]
    public void ReadDateTime_FileTimeAvailable_ReturnsDateTime()
    {
        // Arrange
        long fileTime = new DateTime(2024, 1, 2, 3, 4, 5).ToFileTime();
        DateTime expected = DateTime.FromFileTime(fileTime);
        using MemoryStream sourceStream = new(BitConverter.GetBytes(fileTime));
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        DateTime value = reader.ReadDateTime();

        // Assert
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadDateTime_AfterClose_CatchesExceptionAndReturnsDateTimeMinValue()
    {
        // Arrange
        long fileTime = new DateTime(2024, 1, 2, 3, 4, 5).ToFileTime();
        using MemoryStream sourceStream = new(BitConverter.GetBytes(fileTime));
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        DateTime value = reader.ReadDateTime();

        // Assert
        Assert.Equal(DateTime.MinValue, value);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadDateTime_AfterDisposeAsync_ReturnsDateTimeMinValue()
    {
        // Arrange
        long fileTime = new DateTime(2024, 1, 2, 3, 4, 5).ToFileTime();
        using MemoryStream sourceStream = new(BitConverter.GetBytes(fileTime));
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        DateTime value = reader.ReadDateTime();

        // Assert
        Assert.Equal(DateTime.MinValue, value);
    }

    [Fact]
    public void ReadDouble_ValueAvailable_ReturnsDouble()
    {
        // Arrange
        using MemoryStream sourceStream = new(BitConverter.GetBytes(12.5d));
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        double value = reader.ReadDouble();

        // Assert
        Assert.Equal(12.5d, value);
    }

    [Fact]
    public void ReadDouble_AfterClose_CatchesExceptionAndReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new(BitConverter.GetBytes(12.5d));
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        double value = reader.ReadDouble();

        // Assert
        Assert.Equal(0d, value);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadDouble_AfterDisposeAsync_ReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new(BitConverter.GetBytes(12.5d));
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        double value = reader.ReadDouble();

        // Assert
        Assert.Equal(0d, value);
    }




    [Fact]
    public void ReadDecimal_ValueAvailable_ReturnsDecimal()
    {
        // Arrange
        const decimal expected = 1234.5678m;
        using MemoryStream sourceStream = new();
        using (BinaryWriter writer = new(sourceStream, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write(expected);
        }

        sourceStream.Position = 0;

        using SafeBinaryReader reader = new(sourceStream);

        // Act
        decimal value = reader.ReadDecimal();

        // Assert
        Assert.Equal(expected, value);
    }


    [Fact]
    public void ReadDecimal_AfterClose_CatchesExceptionAndReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new();
        using (BinaryWriter writer = new(sourceStream, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write(42.42m);
        }

        sourceStream.Position = 0;

        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        decimal value = reader.ReadDecimal();

        // Assert
        Assert.Equal(0m, value);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadDecimal_AfterDisposeAsync_ReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new();
        using (BinaryWriter writer = new(sourceStream, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write(42.42m);
        }

        sourceStream.Position = 0;

        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        decimal value = reader.ReadDecimal();

        // Assert
        Assert.Equal(0m, value);
    }

    [Fact]
    public void ReadInt16_ValueAvailable_ReturnsInt16()
    {
        // Arrange
        short expected = -12345;
        using MemoryStream sourceStream = new(BitConverter.GetBytes(expected));
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        short value = reader.ReadInt16();

        // Assert
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadInt16_AfterClose_CatchesExceptionAndReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new(BitConverter.GetBytes((short)100));
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        short value = reader.ReadInt16();

        // Assert
        Assert.Equal((short)0, value);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadInt16_AfterDisposeAsync_ReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new(BitConverter.GetBytes((short)100));
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        short value = reader.ReadInt16();

        // Assert
        Assert.Equal((short)0, value);
    }

    [Fact]
    public void ReadUInt16_ValueAvailable_ReturnsUInt16()
    {
        // Arrange
        ushort expected = 54321;
        using MemoryStream sourceStream = new(BitConverter.GetBytes(expected));
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        ushort value = reader.ReadUInt16();

        // Assert
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadUInt16_AfterClose_CatchesExceptionAndReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new(BitConverter.GetBytes((ushort)10));
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        ushort value = reader.ReadUInt16();

        // Assert
        Assert.Equal((ushort)0, value);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadUInt16_AfterDisposeAsync_ReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new(BitConverter.GetBytes((ushort)10));
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        ushort value = reader.ReadUInt16();

        // Assert
        Assert.Equal((ushort)0, value);
    }

    [Fact]
    public void ReadInt32_ValueAvailable_ReturnsInt32()
    {
        // Arrange
        const int expected = -123456789;
        using MemoryStream sourceStream = new(BitConverter.GetBytes(expected));
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        int value = reader.ReadInt32();

        // Assert
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadInt32_AfterClose_CatchesExceptionAndReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new(BitConverter.GetBytes(123));
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        int value = reader.ReadInt32();

        // Assert
        Assert.Equal(0, value);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadInt32_AfterDisposeAsync_ReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new(BitConverter.GetBytes(123));
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        int value = reader.ReadInt32();

        // Assert
        Assert.Equal(0, value);
    }

    [Fact]
    public void ReadUInt32_ValueAvailable_ReturnsUInt32()
    {
        // Arrange
        const uint expected = 3234567890;
        using MemoryStream sourceStream = new(BitConverter.GetBytes(expected));
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        uint value = reader.ReadUInt32();

        // Assert
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadUInt32_AfterClose_CatchesExceptionAndReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new(BitConverter.GetBytes((uint)123));
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        uint value = reader.ReadUInt32();

        // Assert
        Assert.Equal((uint)0, value);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadUInt32_AfterDisposeAsync_ReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new(BitConverter.GetBytes((uint)123));
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        uint value = reader.ReadUInt32();

        // Assert
        Assert.Equal((uint)0, value);
    }

    [Fact]
    public void ReadInt64_ValueAvailable_ReturnsInt64()
    {
        // Arrange
        const long expected = -1234567890123456789;
        using MemoryStream sourceStream = new(BitConverter.GetBytes(expected));
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        long value = reader.ReadInt64();

        // Assert
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadInt64_AfterClose_CatchesExceptionAndReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new(BitConverter.GetBytes(123L));
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        long value = reader.ReadInt64();

        // Assert
        Assert.Equal(0L, value);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadInt64_AfterDisposeAsync_ReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new(BitConverter.GetBytes(123L));
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        long value = reader.ReadInt64();

        // Assert
        Assert.Equal(0L, value);
    }

    [Fact]
    public void ReadUInt64_ValueAvailable_ReturnsUInt64()
    {
        // Arrange
        const ulong expected = 12345678901234567890UL;
        using MemoryStream sourceStream = new(BitConverter.GetBytes(expected));
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        ulong value = reader.ReadUInt64();

        // Assert
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadUInt64_AfterClose_CatchesExceptionAndReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new(BitConverter.GetBytes(123UL));
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        ulong value = reader.ReadUInt64();

        // Assert
        Assert.Equal(0UL, value);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadUInt64_AfterDisposeAsync_ReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new(BitConverter.GetBytes(123UL));
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        ulong value = reader.ReadUInt64();

        // Assert
        Assert.Equal(0UL, value);
    }

    [Fact]
    public void ReadSingle_ValueAvailable_ReturnsSingle()
    {
        // Arrange
        const float expected = 123.5f;
        using MemoryStream sourceStream = new(BitConverter.GetBytes(expected));
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        float value = reader.ReadSingle();

        // Assert
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadSingle_AfterClose_CatchesExceptionAndReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new(BitConverter.GetBytes(123.5f));
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        float value = reader.ReadSingle();

        // Assert
        Assert.Equal(0f, value);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadSingle_AfterDisposeAsync_ReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new(BitConverter.GetBytes(123.5f));
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        float value = reader.ReadSingle();

        // Assert
        Assert.Equal(0f, value);
    }

    [Fact]
    public void ReadHalf_ValueAvailable_ReturnsHalf()
    {
        // Arrange
        Half expected = (Half)12.5f;
        short halfBits = BitConverter.HalfToInt16Bits(expected);
        using MemoryStream sourceStream = new(BitConverter.GetBytes(halfBits));
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        Half value = reader.ReadHalf();

        // Assert
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadHalf_AfterClose_CatchesExceptionAndReturnsHalfMinValue()
    {
        // Arrange
        Half expected = (Half)6.25f;
        short halfBits = BitConverter.HalfToInt16Bits(expected);
        using MemoryStream sourceStream = new(BitConverter.GetBytes(halfBits));
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        Half value = reader.ReadHalf();

        // Assert
        Assert.Equal(Half.MinValue, value);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadHalf_AfterDisposeAsync_ReturnsHalfMinValue()
    {
        // Arrange
        Half expected = (Half)6.25f;
        short halfBits = BitConverter.HalfToInt16Bits(expected);
        using MemoryStream sourceStream = new(BitConverter.GetBytes(halfBits));
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        Half value = reader.ReadHalf();

        // Assert
        Assert.Equal(Half.MinValue, value);
    }

    [Fact]
    public void ReadString_ValueAvailable_ReturnsString()
    {
        // Arrange
        const string expected = "Safe Reader";
        using MemoryStream sourceStream = new();
        using (BinaryWriter writer = new(sourceStream, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write(expected);
        }

        sourceStream.Position = 0;
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        string? value = reader.ReadString();

        // Assert
        Assert.Equal(expected, value);
    }

    [Fact]
    public void ReadString_AfterClose_CatchesExceptionAndReturnsNull()
    {
        // Arrange
        using MemoryStream sourceStream = new();
        using (BinaryWriter writer = new(sourceStream, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write("Safe Reader");
        }

        sourceStream.Position = 0;
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        string? value = reader.ReadString();

        // Assert
        Assert.Null(value);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadString_AfterDisposeAsync_ReturnsNull()
    {
        // Arrange
        using MemoryStream sourceStream = new();
        using (BinaryWriter writer = new(sourceStream, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write("Safe Reader");
        }

        sourceStream.Position = 0;
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        string? value = reader.ReadString();

        // Assert
        Assert.Null(value);
    }


    [Fact]
    public void ReadNullableString_HasDataIndicatorOne_ReturnsString()
    {
        // Arrange
        const string expected = "Nullable text";
        using MemoryStream sourceStream = new();
        using (BinaryWriter writer = new(sourceStream, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write((byte)1);
            writer.Write(expected);
        }

        sourceStream.Position = 0;
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        string? value = reader.ReadNullableString();

        // Assert
        Assert.Equal(expected, value);
    }


    [Fact]
    public void ReadNullableString_HasDataIndicatorZero_ReturnsNull()
    {
        // Arrange
        using MemoryStream sourceStream = new([0]);
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        string? value = reader.ReadNullableString();

        // Assert
        Assert.Null(value);
    }

    [Fact]
    public void ReadNullableString_AfterClose_CatchesExceptionAndReturnsNull()
    {
        // Arrange
        using MemoryStream sourceStream = new([1]);
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        string? value = reader.ReadNullableString();

        // Assert
        Assert.Null(value);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task ReadNullableString_AfterDisposeAsync_ReturnsNull()
    {
        // Arrange
        using MemoryStream sourceStream = new([1]);
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        string? value = reader.ReadNullableString();

        // Assert
        Assert.Null(value);
    }

    [Fact]
    public void Read7BitEncodedInt32_ValueAvailable_ReturnsInt32()
    {
        // Arrange
        const int expected = 123456;
        using MemoryStream sourceStream = new();
        using (BinaryWriter writer = new(sourceStream, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write7BitEncodedInt(expected);
        }

        sourceStream.Position = 0;
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        int value = reader.Read7BitEncodedInt32();

        // Assert
        Assert.Equal(expected, value);
    }

    [Fact]
    public void Read7BitEncodedInt32_AfterClose_CatchesExceptionAndReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new();
        using (BinaryWriter writer = new(sourceStream, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write7BitEncodedInt(123456);
        }

        sourceStream.Position = 0;
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        int value = reader.Read7BitEncodedInt32();

        // Assert
        Assert.Equal(0, value);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task Read7BitEncodedInt32_AfterDisposeAsync_ReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new();
        using (BinaryWriter writer = new(sourceStream, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write7BitEncodedInt(123456);
        }

        sourceStream.Position = 0;
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        int value = reader.Read7BitEncodedInt32();

        // Assert
        Assert.Equal(0, value);
    }

    [Fact]
    public void Read7BitEncodedInt64_ValueAvailable_ReturnsInt64()
    {
        // Arrange
        const long expected = 9876543210;
        using MemoryStream sourceStream = new();
        using (BinaryWriter writer = new(sourceStream, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write7BitEncodedInt64(expected);
        }

        sourceStream.Position = 0;
        using SafeBinaryReader reader = new(sourceStream);

        // Act
        long value = reader.Read7BitEncodedInt64();

        // Assert
        Assert.Equal(expected, value);
    }

    [Fact]
    public void Read7BitEncodedInt64_AfterClose_CatchesExceptionAndReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new();
        using (BinaryWriter writer = new(sourceStream, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write7BitEncodedInt64(9876543210);
        }

        sourceStream.Position = 0;
        using SafeBinaryReader reader = new(sourceStream);
        reader.Close();

        // Act
        long value = reader.Read7BitEncodedInt64();

        // Assert
        Assert.Equal(0L, value);
        Assert.True(reader.HasExceptions);
    }

    [Fact]
    public async Task Read7BitEncodedInt64_AfterDisposeAsync_ReturnsZero()
    {
        // Arrange
        using MemoryStream sourceStream = new();
        using (BinaryWriter writer = new(sourceStream, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write7BitEncodedInt64(9876543210);
        }

        sourceStream.Position = 0;
        SafeBinaryReader reader = new(sourceStream);
        await reader.DisposeAsync();

        // Act
        long value = reader.Read7BitEncodedInt64();

        // Assert
        Assert.Equal(0L, value);
    }



}
