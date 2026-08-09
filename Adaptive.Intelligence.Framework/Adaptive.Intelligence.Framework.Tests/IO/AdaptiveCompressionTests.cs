using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Adaptive.Intelligence.IO;
using Xunit;

namespace Adaptive.Intelligence.Framework.Tests;

public class AdaptiveCompressionTests
{
    [Fact]
    public void Compress_StringIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        string sourceContent = null!;

        // Act
        Action action = () => AdaptiveCompression.Compress(sourceContent);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void Compress_StringHasValue_ReturnsSameOutputAsByteArrayOverload()
    {
        // Arrange
        const string sourceContent = "Hello Ω Compression";

        // Act
        byte[] fromString = AdaptiveCompression.Compress(sourceContent);
        byte[] fromBytes = AdaptiveCompression.Compress(Encoding.Unicode.GetBytes(sourceContent));

        // Assert
        Assert.Equal(fromBytes, fromString);
    }

    [Fact]
    public void Compress_MemoryStreamIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        MemoryStream sourceStream = null!;

        // Act
        Action action = () => AdaptiveCompression.Compress(sourceStream);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void Compress_MemoryStreamCannotRead_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        MemoryStream sourceStream = new([1, 2, 3]);
        sourceStream.Dispose();

        // Act
        Action action = () => AdaptiveCompression.Compress(sourceStream);

        // Assert
        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(action);
        Assert.Equal("sourceStream", exception.ParamName);
    }

    [Fact]
    public void Compress_MemoryStreamReadable_CompressesSourceContent()
    {
        // Arrange
        byte[] sourceContent = Encoding.UTF8.GetBytes("Readable stream content");
        using MemoryStream sourceStream = new(sourceContent);

        // Act
        byte[] compressedFromStream = AdaptiveCompression.Compress(sourceStream);
        byte[] compressedFromBytes = AdaptiveCompression.Compress(sourceContent);

        // Assert
        Assert.Equal(compressedFromBytes, compressedFromStream);
    }

    [Fact]
    public void Compress_ByteArrayIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        byte[] sourceContent = null!;

        // Act
        Action action = () => AdaptiveCompression.Compress(sourceContent);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void Compress_ByteArrayHasContent_ReturnsDataThatCanBeDecompressed()
    {
        // Arrange
        byte[] sourceContent = Encoding.UTF8.GetBytes("Byte[] compression payload");

        // Act
        byte[] compressed = AdaptiveCompression.Compress(sourceContent);
        byte[]? decompressed = AdaptiveCompression.Decompress(compressed);

        // Assert
        Assert.NotEmpty(compressed);
        Assert.NotNull(decompressed);
        Assert.Equal(sourceContent, decompressed);
    }

    [Fact]
    public void Compress_StreamInputAndOutputValid_WritesCompressedDataToOutput()
    {
        // Arrange
        byte[] sourceContent = Encoding.UTF8.GetBytes("Stream compression payload");
        using MemoryStream inputStream = new(sourceContent);
        using MemoryStream outputStream = new();

        // Act
        AdaptiveCompression.Compress(inputStream, outputStream);
        byte[] outputBytes = outputStream.ToArray();
        byte[]? decompressed = AdaptiveCompression.Decompress(outputBytes);

        // Assert
        Assert.NotEmpty(outputBytes);
        Assert.NotNull(decompressed);
        Assert.Equal(sourceContent, decompressed);
    }

    [Fact]
    public void Compress_StreamInputDisposed_DoesNotWriteSourceContentToOutput()
    {
        // Arrange
        MemoryStream inputStream = new(Encoding.UTF8.GetBytes("disposed stream"));
        inputStream.Dispose();
        using MemoryStream outputStream = new();

        // Act
        AdaptiveCompression.Compress(inputStream, outputStream);
        byte[] outputBytes = outputStream.ToArray();
        byte[]? decompressed = AdaptiveCompression.Decompress(outputBytes);

        // Assert
        Assert.NotEmpty(outputBytes);
        Assert.NotNull(decompressed);
        Assert.Empty(decompressed);
    }

    [Fact]
    public void Compress_StreamOutputIsNull_DoesNotThrow()
    {
        // Arrange
        using MemoryStream inputStream = new(Encoding.UTF8.GetBytes("null output"));
        Stream outputStream = null!;

        // Act
        Exception? exception = Record.Exception(() => AdaptiveCompression.Compress(inputStream, outputStream));

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public async Task CompressAsync_StreamInputAndOutputValid_WritesCompressedDataToOutput()
    {
        // Arrange
        byte[] sourceContent = Encoding.UTF8.GetBytes("Async stream compression payload");
        using MemoryStream inputStream = new(sourceContent);
        using MemoryStream outputStream = new();

        // Act
        await AdaptiveCompression.CompressAsync(inputStream, outputStream);
        byte[] outputBytes = outputStream.ToArray();
        byte[]? decompressed = AdaptiveCompression.Decompress(outputBytes);

        // Assert
        Assert.NotEmpty(outputBytes);
        Assert.NotNull(decompressed);
        Assert.Equal(sourceContent, decompressed);
    }

    [Fact]
    public async Task CompressAsync_StreamInputDisposed_DoesNotWriteSourceContentToOutput()
    {
        // Arrange
        MemoryStream inputStream = new(Encoding.UTF8.GetBytes("disposed async stream"));
        inputStream.Dispose();
        using MemoryStream outputStream = new();

        // Act
        await AdaptiveCompression.CompressAsync(inputStream, outputStream);
        byte[] outputBytes = outputStream.ToArray();
        byte[]? decompressed = AdaptiveCompression.Decompress(outputBytes);

        // Assert
        Assert.NotEmpty(outputBytes);
        Assert.NotNull(decompressed);
        Assert.Empty(decompressed);
    }

    [Fact]
    public async Task CompressAsync_StreamOutputIsNull_CompletesWithoutException()
    {
        // Arrange
        using MemoryStream inputStream = new(Encoding.UTF8.GetBytes("null async output"));
        Stream outputStream = null!;

        // Act
        Exception? exception = await Record.ExceptionAsync(() => AdaptiveCompression.CompressAsync(inputStream, outputStream));

        // Assert
        Assert.Null(exception);
    }


    [Fact]
    public void Decompress_ByteArrayHasCompressedContent_ReturnsOriginalData()
    {
        // Arrange
        byte[] sourceContent = Encoding.UTF8.GetBytes("decompression payload");
        byte[] compressed = AdaptiveCompression.Compress(sourceContent);

        // Act
        byte[]? decompressed = AdaptiveCompression.Decompress(compressed);

        // Assert
        Assert.NotNull(decompressed);
        Assert.Equal(sourceContent, decompressed);
    }

    [Fact]
    public void Decompress_MemoryStreamIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        MemoryStream sourceContent = null!;

        // Act
        Action action = () => AdaptiveCompression.Decompress(sourceContent);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void Decompress_MemoryStreamCannotRead_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        MemoryStream sourceContent = new([1, 2, 3]);
        sourceContent.Dispose();

        // Act
        Action action = () => AdaptiveCompression.Decompress(sourceContent);

        // Assert
        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(action);
        Assert.Equal("sourceContent", exception.ParamName);
    }

    [Fact]
    public void Decompress_MemoryStreamHasCompressedContent_ReturnsOriginalData()
    {
        // Arrange
        byte[] original = Encoding.UTF8.GetBytes("memory-stream decompression payload");
        byte[] compressed = AdaptiveCompression.Compress(original);
        using MemoryStream sourceContent = new(compressed);

        // Act
        byte[]? decompressed = AdaptiveCompression.Decompress(sourceContent);

        // Assert
        Assert.NotNull(decompressed);
        Assert.Equal(original, decompressed);
    }

    [Fact]
    public void Decompress_ByteArrayIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        byte[] sourceContent = null!;

        // Act
        Action action = () => AdaptiveCompression.Decompress(sourceContent);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void Decompress_ByteArrayInvalidCompressedData_ReturnsNull()
    {
        // Arrange
        byte[] sourceContent = Encoding.UTF8.GetBytes("not a gzip payload");

        // Act
        byte[]? decompressed = AdaptiveCompression.Decompress(sourceContent);

        // Assert
        Assert.Null(decompressed);
    }

    [Fact]
    public void Decompress_ByteArrayAndDestinationSourceIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        byte[] sourceContent = null!;
        using MemoryStream destinationStream = new();

        // Act
        Action action = () => AdaptiveCompression.Decompress(sourceContent, destinationStream);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void Decompress_ByteArrayAndDestinationValidCompressedData_ThrowsObjectDisposedExceptionAfterWritingContent()
    {
        // Arrange
        byte[] original = Encoding.UTF8.GetBytes("destination stream payload");
        byte[] compressed = AdaptiveCompression.Compress(original);
        using MemoryStream destinationStream = new();

        // Act
        Action action = () => AdaptiveCompression.Decompress(compressed, destinationStream);

        // Assert
        Assert.Throws<ObjectDisposedException>(action);
        Assert.Equal(original, destinationStream.ToArray());
    }

    [Fact]
    public void Decompress_StreamSourceCannotRead_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        MemoryStream sourceStream = new([1, 2, 3]);
        sourceStream.Dispose();
        using MemoryStream destinationStream = new();

        // Act
        Action action = () => AdaptiveCompression.Decompress(sourceStream, destinationStream);

        // Assert
        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(action);
        Assert.Equal("sourceStream", exception.ParamName);
    }

    [Fact]
    public void Decompress_StreamDestinationCannotWrite_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        byte[] original = Encoding.UTF8.GetBytes("destination cannot write");
        byte[] compressed = AdaptiveCompression.Compress(original);
        using MemoryStream sourceStream = new(compressed);
        MemoryStream destinationStream = new();
        destinationStream.Dispose();

        // Act
        Action action = () => AdaptiveCompression.Decompress(sourceStream, destinationStream);

        // Assert
        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(action);
        Assert.Equal("destinationStream", exception.ParamName);
    }

    [Fact]
    public void Decompress_StreamValidStreams_WritesDecompressedData()
    {
        // Arrange
        byte[] original = Encoding.UTF8.GetBytes("stream-to-stream decompression payload");
        byte[] compressed = AdaptiveCompression.Compress(original);
        using MemoryStream sourceStream = new(compressed);
        sourceStream.Seek(sourceStream.Length, SeekOrigin.Begin);
        using MemoryStream destinationStream = new();

        // Act
        AdaptiveCompression.Decompress(sourceStream, destinationStream);

        // Assert
        Assert.Equal(original, destinationStream.ToArray());
    }

    [Fact]
    public async Task DecompressAsync_StreamSourceCannotRead_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        MemoryStream sourceStream = new([1, 2, 3]);
        sourceStream.Dispose();
        using MemoryStream destinationStream = new();

        // Act
        Func<Task> action = () => AdaptiveCompression.DecompressAsync(sourceStream, destinationStream);

        // Assert
        ArgumentOutOfRangeException exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(action);
        Assert.Equal("sourceStream", exception.ParamName);
    }

    [Fact]
    public async Task DecompressAsync_StreamDestinationCannotWrite_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        byte[] original = Encoding.UTF8.GetBytes("async destination cannot write");
        byte[] compressed = AdaptiveCompression.Compress(original);
        using MemoryStream sourceStream = new(compressed);
        MemoryStream destinationStream = new();
        destinationStream.Dispose();

        // Act
        Func<Task> action = () => AdaptiveCompression.DecompressAsync(sourceStream, destinationStream);

        // Assert
        ArgumentOutOfRangeException exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(action);
        Assert.Equal("destinationStream", exception.ParamName);
    }

    [Fact]
    public async Task DecompressAsync_StreamValidStreams_WritesDecompressedData()
    {
        // Arrange
        byte[] original = Encoding.UTF8.GetBytes("async stream decompression payload");
        byte[] compressed = AdaptiveCompression.Compress(original);
        using MemoryStream sourceStream = new(compressed);
        sourceStream.Seek(sourceStream.Length, SeekOrigin.Begin);
        using MemoryStream destinationStream = new();

        // Act
        await AdaptiveCompression.DecompressAsync(sourceStream, destinationStream);

        // Assert
        Assert.Equal(original, destinationStream.ToArray());
    }


}
