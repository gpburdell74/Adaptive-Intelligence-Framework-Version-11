using Adaptive.Intelligence.IO;

namespace Adaptive.Intelligence.Framework.Tests.IO;

public class BinarySerializationReaderTTests
{
    [Fact]
    public void BinarySerializationReader_NullInputStream_ThrowsArgumentNullException()
    {
        // Arrange
        Stream? inputStream = null;
        _ = new TestItem();

        // Act
        Action action = () => _ = new BinarySerializationReader<TestItem>(inputStream!);

        // Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("inputStream", exception.ParamName);
    }

    [Fact]
    public void BinarySerializationReader_ValidStream_ReadListReturnsExpectedInstanceCount()
    {
        // Arrange
        using MemoryStream stream = CreateStreamForReadList(itemCount: 2);
        using BinarySerializationReader<TestItem> reader = new(stream);

        // Act
        List<TestItem> result = reader.ReadList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, item => Assert.NotNull(item));
    }

    [Fact]
    public void Close_AfterCallingClose_ReadListThrowsInvalidOperationException()
    {
        // Arrange
        using MemoryStream stream = CreateStreamForReadList(itemCount: 1);
        using BinarySerializationReader<TestItem> reader = new(stream);
        reader.Close();

        // Act
        Action action = () => _ = reader.ReadList();

        // Assert
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(action);
        Assert.Equal("Could not read from the specified stream.", exception.Message);
    }

    [Fact]
    public void ReadList_WhenSerializedCountIsZero_ReturnsEmptyList()
    {
        // Arrange
        using MemoryStream stream = CreateStreamForReadList(itemCount: 0);
        using BinarySerializationReader<TestItem> reader = new(stream);

        // Act
        List<TestItem> result = reader.ReadList();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    private static MemoryStream CreateStreamForReadList(int itemCount)
    {
        MemoryStream stream = new();
        using (BinaryWriter writer = new(stream, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write(0);
            writer.Write(itemCount);
        }

        stream.Position = 0;
        return stream;
    }


    private sealed class TestItem
    {
        public int Id { get; set; }
    }
}
