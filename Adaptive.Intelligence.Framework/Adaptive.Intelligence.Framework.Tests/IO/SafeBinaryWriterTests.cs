using System.IO;
using System.Security.Cryptography;

using Adaptive.Intelligence.IO;

namespace Adaptive.Intelligence.Framework.Tests.IO;

public class SafeBinaryWriterTests
{
    [Fact]
    public void SafeBinaryWriter_StreamConstructor_InitializesWriterAndBaseStream()
    {
        // Arrange
        using MemoryStream stream = new();

        // Act
        using SafeBinaryWriter writer = new(stream);

        // Assert
        Assert.Same(stream, writer.BaseStream);
        Assert.NotNull(writer.Writer);
        Assert.True(writer.CanWrite);
    }

    [Fact]
    public void SafeBinaryWriter_BinaryWriterConstructor_UsesProvidedWriterAndStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using BinaryWriter binaryWriter = new(stream);

        // Act
        using SafeBinaryWriter writer = new(binaryWriter);

        // Assert
        Assert.Same(binaryWriter, writer.Writer);
        Assert.Same(stream, writer.BaseStream);
        Assert.True(writer.CanWrite);
    }

    [Fact]
    public async Task DisposeAsync_StreamConstructor_DisposesWriterAndClearsState()
    {
        // Arrange
        MemoryStream stream = new();
        SafeBinaryWriter writer = new(stream);

        // Act
        await writer.DisposeAsync();

        // Assert
        Assert.Null(writer.BaseStream);
        Assert.Null(writer.Writer);
        Assert.False(writer.CanWrite);
        Assert.False(stream.CanWrite);
    }

    [Fact]
    public async Task DisposeAsync_BinaryWriterConstructor_DoesNotDisposeProvidedWriter()
    {
        // Arrange
        using MemoryStream stream = new();
        using BinaryWriter binaryWriter = new(stream);
        SafeBinaryWriter writer = new(binaryWriter);

        // Act
        await writer.DisposeAsync();
        binaryWriter.Write((byte)123);
        binaryWriter.Flush();

        // Assert
        Assert.Null(writer.BaseStream);
        Assert.Null(writer.Writer);
        Assert.Equal(1, stream.Length);
    }

    [Fact]
    public void CanWrite_UnderlyingStreamDisposed_ReturnsFalse()
    {
        // Arrange
        MemoryStream stream = new();
        using BinaryWriter binaryWriter = new(stream);
        using SafeBinaryWriter writer = new(binaryWriter);
        stream.Dispose();

        // Act
        bool canWrite = writer.CanWrite;

        // Assert
        Assert.False(canWrite);
    }

    [Fact]
    public void Close_WriterCreatedFromStream_DisposesUnderlyingStream()
    {
        // Arrange
        MemoryStream stream = new();
        SafeBinaryWriter writer = new(stream);

        // Act
        writer.Close();

        // Assert
        Assert.False(stream.CanWrite);
    }


    [Fact]
    public async Task Close_WriterDisposed_DoesNotThrow()
    {
        // Arrange
        SafeBinaryWriter writer = new(new MemoryStream());
        await writer.DisposeAsync();

        // Act
        Exception? exception = Record.Exception(writer.Close);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Flush_WriterAvailable_DoesNotThrow()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Write(true);

        // Act
        writer.Flush();

        // Assert
        Assert.Equal(1, stream.Length);
    }

    [Fact]
    public async Task Flush_WriterDisposed_DoesNotThrow()
    {
        // Arrange
        SafeBinaryWriter writer = new(new MemoryStream());
        await writer.DisposeAsync();

        // Act
        Exception? exception = Record.Exception(writer.Flush);

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Seek_WriterAvailable_ReturnsNewPosition()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Write(true);

        // Act
        long newPosition = writer.Seek(0, SeekOrigin.Begin);

        // Assert
        Assert.Equal(0, newPosition);
    }

    [Fact]
    public async Task Seek_WriterDisposed_ReturnsMinusOne()
    {
        // Arrange
        SafeBinaryWriter writer = new(new MemoryStream());
        await writer.DisposeAsync();

        // Act
        long newPosition = writer.Seek(0, SeekOrigin.Begin);

        // Assert
        Assert.Equal(-1, newPosition);
    }

    [Fact]
    public void Seek_UnderlyingStreamNotSeekable_AddsExceptionAndReturnsMinusOne()
    {
        // Arrange
        using Aes aes = Aes.Create();
        using ICryptoTransform transform = aes.CreateEncryptor();
        using MemoryStream baseStream = new();
        using CryptoStream cryptoStream = new(baseStream, transform, CryptoStreamMode.Write);
        using BinaryWriter binaryWriter = new(cryptoStream);
        using SafeBinaryWriter writer = new(binaryWriter);

        // Act
        long newPosition = writer.Seek(0, SeekOrigin.Begin);

        // Assert
        Assert.Equal(-1, newPosition);
        Assert.True(writer.HasExceptions);
        Assert.IsType<NotSupportedException>(writer.FirstException);
    }

    [Fact]
    public void Write_ValueProvided_WritesBooleanByteToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write(true);
        writer.Flush();

        // Assert
        Assert.Equal(1, stream.Length);
        Assert.Equal(1, stream.ToArray()[0]);
    }

    [Fact]
    public void Write_UnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.Write(true);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }
    [Fact]
    public void Write_ByteValueProvided_WritesByteToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write((byte)123);
        writer.Flush();

        // Assert
        Assert.Equal(1, stream.Length);
        Assert.Equal(123, stream.ToArray()[0]);
    }

    [Fact]
    public void Write_ByteUnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.Write((byte)12);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }

    [Fact]
    public void Write_SByteValueProvided_WritesSByteToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write((sbyte)-5);
        writer.Flush();

        // Assert
        Assert.Equal(1, stream.Length);
        Assert.Equal(unchecked((byte)-5), stream.ToArray()[0]);
    }

    [Fact]
    public void Write_SByteUnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.Write((sbyte)-1);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }

    [Fact]
    public void Write_ByteArrayValueProvided_WritesBytesToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        byte[] buffer = [1, 2, 3, 4];

        // Act
        writer.Write(buffer);
        writer.Flush();

        // Assert
        Assert.Equal(buffer, stream.ToArray());
    }

    [Fact]
    public void Write_ByteArrayNull_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write((byte[]?)null!);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ArgumentNullException>(writer.FirstException);
    }

    [Fact]
    public void Write_ByteArrayIndexCountProvided_WritesSelectedRangeToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        byte[] buffer = [9, 8, 7, 6];

        // Act
        writer.Write(buffer, 1, 2);
        writer.Flush();

        // Assert
        Assert.Equal([8, 7], stream.ToArray());
    }

    [Fact]
    public void Write_ByteArrayIndexCountNullBuffer_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write((byte[]?)null!, 0, 1);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ArgumentNullException>(writer.FirstException);
    }

    [Fact]
    public void Write_CharProvided_WritesCharDataToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write('A');
        writer.Flush();

        // Assert
        Assert.NotEmpty(stream.ToArray());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void Write_CharUnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.Write('Z');

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }



    [Fact]
    public void Write_CharArrayValueProvided_WritesCharDataToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        char[] chars = ['A', 'B', 'C'];

        // Act
        writer.Write(chars);
        writer.Flush();

        // Assert
        Assert.NotEmpty(stream.ToArray());
        Assert.False(writer.HasExceptions);
    }


    [Fact]
    public void Write_CharArrayNull_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write((char[]?)null!);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ArgumentNullException>(writer.FirstException);
    }

    [Fact]
    public void Write_CharArrayIndexCountProvided_WritesSelectedCharsToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        char[] chars = ['A', 'B', 'C', 'D'];

        // Act
        writer.Write(chars, 1, 2);
        writer.Flush();

        // Assert
        Assert.Equal([66, 67], stream.ToArray());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void Write_CharArrayIndexCountNullChars_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write((char[]?)null!, 0, 1);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ArgumentNullException>(writer.FirstException);
    }

    [Fact]
    public void Write_DoubleValueProvided_WritesDoubleToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write(123.5d);
        writer.Flush();
        stream.Position = 0;

        // Assert
        using BinaryReader reader = new(stream);
        Assert.Equal(123.5d, reader.ReadDouble());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void Write_DoubleUnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.Write(15.25d);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }

    [Fact]
    public void Write_DateTimeValueProvided_WritesFileTimeToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        DateTime value = new(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);

        // Act
        writer.Write(value);
        writer.Flush();
        stream.Position = 0;

        // Assert
        using BinaryReader reader = new(stream);
        Assert.Equal(value.ToFileTime(), reader.ReadInt64());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void Write_DateTimeMinValue_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write(DateTime.MinValue);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ArgumentOutOfRangeException>(writer.FirstException);
    }

    [Fact]
    public void Write_DecimalValueProvided_WritesDecimalToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write(79228162514264337593543950335m);
        writer.Flush();
        stream.Position = 0;

        // Assert
        using BinaryReader reader = new(stream);
        Assert.Equal(79228162514264337593543950335m, reader.ReadDecimal());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void Write_DecimalUnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.Write(10.5m);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }


    [Fact]
    public void Write_ShortValueProvided_WritesInt16ToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write((short)-12345);
        writer.Flush();
        stream.Position = 0;

        // Assert
        using BinaryReader reader = new(stream);
        Assert.Equal((short)-12345, reader.ReadInt16());
        Assert.False(writer.HasExceptions);
    }
    [Fact]
    public void Write_ShortUnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.Write((short)-1);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }

    [Fact]
    public void Write_UShortValueProvided_WritesUInt16ToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write((ushort)54321);
        writer.Flush();
        stream.Position = 0;

        // Assert
        using BinaryReader reader = new(stream);
        Assert.Equal((ushort)54321, reader.ReadUInt16());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void Write_UShortUnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.Write((ushort)1);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }

    [Fact]
    public void Write_IntValueProvided_WritesInt32ToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write(-123456789);
        writer.Flush();
        stream.Position = 0;

        // Assert
        using BinaryReader reader = new(stream);
        Assert.Equal(-123456789, reader.ReadInt32());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void Write_IntUnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.Write(1);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }

    [Fact]
    public void Write_UIntValueProvided_WritesUInt32ToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write(1234567890u);
        writer.Flush();
        stream.Position = 0;

        // Assert
        using BinaryReader reader = new(stream);
        Assert.Equal(1234567890u, reader.ReadUInt32());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void Write_UIntUnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.Write(1u);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }

    [Fact]
    public void Write_LongValueProvided_WritesInt64ToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write(-1234567890123456789L);
        writer.Flush();
        stream.Position = 0;

        // Assert
        using BinaryReader reader = new(stream);
        Assert.Equal(-1234567890123456789L, reader.ReadInt64());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void Write_LongUnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.Write(1L);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }




    [Fact]
    public void Write_ULongValueProvided_WritesUInt64ToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write(12345678901234567890UL);
        writer.Flush();
        stream.Position = 0;

        // Assert
        using BinaryReader reader = new(stream);
        Assert.Equal(12345678901234567890UL, reader.ReadUInt64());
        Assert.False(writer.HasExceptions);
    }
    [Fact]
    public void Write_ULongUnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.Write(1UL);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }

    [Fact]
    public void Write_FloatValueProvided_WritesSingleToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write(123.25f);
        writer.Flush();
        stream.Position = 0;

        // Assert
        using BinaryReader reader = new(stream);
        Assert.Equal(123.25f, reader.ReadSingle());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void Write_FloatUnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.Write(1.25f);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }

    [Fact]
    public void Write_HalfValueProvided_WritesHalfToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write((Half)42.5f);
        writer.Flush();
        stream.Position = 0;

        // Assert
        using BinaryReader reader = new(stream);
        Assert.Equal((Half)42.5f, reader.ReadHalf());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void Write_HalfUnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.Write((Half)1.5f);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }

    [Fact]
    public void Write_StringValueProvided_WritesStringToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write("hello");
        writer.Flush();
        stream.Position = 0;

        // Assert
        using BinaryReader reader = new(stream);
        Assert.Equal("hello", reader.ReadString());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void Write_StringNull_DoesNotWriteToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write((string?)null);
        writer.Flush();

        // Assert
        Assert.Equal(0, stream.Length);
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void Write_StringUnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.Write("value");

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }

    [Fact]
    public void WriteNullable_StringValueProvided_WritesPresenceByteAndString()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.WriteNullable("hello");
        writer.Flush();
        stream.Position = 0;

        // Assert
        using BinaryReader reader = new(stream);
        Assert.Equal(1, reader.ReadByte());
        Assert.Equal("hello", reader.ReadString());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void WriteNullable_Null_WritesZeroPresenceByte()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.WriteNullable(null);
        writer.Flush();
        stream.Position = 0;

        // Assert
        using BinaryReader reader = new(stream);
        Assert.Equal(0, reader.ReadByte());
        Assert.Equal(1, stream.Length);
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void WriteNullable_UnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.WriteNullable("value");

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }


    [Fact]
    public void Write_ReadOnlySpanByteProvided_WritesBytesToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        byte[] bytes = [5, 10, 15];

        // Act
        writer.Write(bytes.AsSpan());
        writer.Flush();

        // Assert
        Assert.Equal(bytes, stream.ToArray());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void Write_ReadOnlySpanByteUnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();
        byte[] bytes = [1, 2, 3];

        // Act
        writer.Write(bytes.AsSpan());

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }

    [Fact]
    public void Write_ReadOnlySpanCharProvided_WritesEncodedCharsToStream()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        char[] chars = ['O', 'K'];

        // Act
        writer.Write(chars.AsSpan());
        writer.Flush();

        // Assert
        Assert.NotEmpty(stream.ToArray());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void Write_ReadOnlySpanCharUnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();
        char[] chars = ['X'];

        // Act
        writer.Write(chars.AsSpan());

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }

    [Fact]
    public void WriteByteArray_NullData_WritesNullIndicatorOnly()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.WriteByteArray(null);

        // Assert
        Assert.Equal([1], stream.ToArray());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void WriteByteArray_EmptyData_WritesNotNullIndicatorAndZeroLength()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.WriteByteArray([]);
        stream.Position = 0;

        // Assert
        using BinaryReader reader = new(stream);
        Assert.False(reader.ReadBoolean());
        Assert.Equal(0, reader.ReadInt32());
        Assert.Equal(stream.Length, stream.Position);
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void WriteByteArray_DataProvided_WritesIndicatorLengthAndBytes()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        byte[] data = [4, 5, 6];

        // Act
        writer.WriteByteArray(data);
        stream.Position = 0;

        // Assert
        using BinaryReader reader = new(stream);
        Assert.False(reader.ReadBoolean());
        Assert.Equal(data.Length, reader.ReadInt32());
        Assert.Equal(data, reader.ReadBytes(data.Length));
        Assert.Equal(stream.Length, stream.Position);
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void WriteByteArray_UnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.WriteByteArray([7]);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }

    [Fact]
    public async Task WriteByteArray_WriterDisposed_DoesNothing()
    {
        // Arrange
        using MemoryStream stream = new();
        SafeBinaryWriter writer = new(stream);
        await writer.DisposeAsync();

        // Act
        writer.WriteByteArray([9, 9]);

        // Assert
        Assert.Empty(stream.ToArray());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void Write7BitEncodedInt_ValueProvided_WritesEncodedInt()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write7BitEncodedInt(300);
        writer.Flush();
        stream.Position = 0;

        // Assert
        using BinaryReader reader = new(stream);
        Assert.Equal(300, reader.Read7BitEncodedInt());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void Write7BitEncodedInt_UnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.Write7BitEncodedInt(1);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }

    [Fact]
    public void Write7BitEncodedInt64_ValueProvided_WritesEncodedLong()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);

        // Act
        writer.Write7BitEncodedInt64(9876543210L);
        writer.Flush();
        stream.Position = 0;

        // Assert
        using BinaryReader reader = new(stream);
        Assert.Equal(9876543210L, reader.Read7BitEncodedInt64());
        Assert.False(writer.HasExceptions);
    }

    [Fact]
    public void Write7BitEncodedInt64_UnderlyingWriterClosed_AddsException()
    {
        // Arrange
        using MemoryStream stream = new();
        using SafeBinaryWriter writer = new(stream);
        writer.Close();

        // Act
        writer.Write7BitEncodedInt64(1L);

        // Assert
        Assert.True(writer.HasExceptions);
        Assert.IsType<ObjectDisposedException>(writer.FirstException);
    }



}
