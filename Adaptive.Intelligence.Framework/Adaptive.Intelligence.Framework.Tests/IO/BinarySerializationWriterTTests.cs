using System.Reflection;

using Adaptive.Intelligence.IO;

namespace Adaptive.Intelligence.Framework.Tests.IO;

public class BinarySerializationWriterTTests
{
    [Fact]
    public void BinarySerializationWriter_NullDestinationStream_ThrowsArgumentNullException()
    {
        // Arrange
        Stream? destinationStream = null;
        _ = new TestItem();

        // Act
        Action action = () => _ = new BinarySerializationWriter<TestItem>(destinationStream!);

        // Assert
        ArgumentNullException exception = Assert.Throws<ArgumentNullException>(action);
        Assert.Equal("destinationStream", exception.ParamName);
    }

    [Fact]
    public void BinarySerializationWriter_ValidDestinationStream_WriteListSucceeds()
    {
        // Arrange
        using MemoryStream stream = new();
        using BinarySerializationWriter<TestItem> writer = new(stream);
        List<TestItem> list = [];

        // Act
        writer.WriteList(list);

        // Assert
        Assert.True(stream.Length > 0);
    }

    [Fact]
    public void Close_AfterCall_WriteListThrowsInvalidOperationException()
    {
        // Arrange
        using MemoryStream stream = new();
        using BinarySerializationWriter<TestItem> writer = new(stream);
        List<TestItem> list = [];
        writer.Close();

        // Act
        Action action = () => writer.WriteList(list);

        // Assert
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(action);
        Assert.Equal("Could not write to the specified stream.", exception.Message);
    }

    [Fact]
    public void WriteList_NullList_ThrowsArgumentNullException()
    {
        // Arrange
        using MemoryStream stream = new();
        using BinarySerializationWriter<TestItem> writer = new(stream);
        List<TestItem>? list = null;

        // Act
        Action action = () => writer.WriteList(list!);

        // Assert
        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void WriteList_ValidList_WritesPropertyMetadataCountAndValues()
    {
        // Arrange
        using MemoryStream stream = new();
        using BinarySerializationWriter<TestItem> writer = new(stream);
        List<TestItem> list =
        [
            new TestItem { Id = 10, OptionalNumber = 50 },
            new TestItem { Id = 20, OptionalNumber = null },
        ];
        PropertyInfo[] properties = typeof(TestItem).GetProperties();

        // Act
        writer.WriteList(list);

        // Assert
        stream.Position = 0;
        using BinaryReader reader = new(stream);

        int propertyCount = reader.ReadInt32();
        Assert.Equal(properties.Length, propertyCount);

        List<string> metadataEntries = [];
        for (int index = 0; index < propertyCount; index++)
        {
            metadataEntries.Add(reader.ReadString());
        }

        foreach (PropertyInfo property in properties)
        {
            string expectedEntry = $"{property.Name}:{property.PropertyType.FullName}";
            Assert.Contains(expectedEntry, metadataEntries);
        }

        int itemCount = reader.ReadInt32();
        Assert.Equal(list.Count, itemCount);

        foreach (TestItem item in list)
        {
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(int))
                {
                    Assert.Equal((int)property.GetValue(item)!, reader.ReadInt32());
                }
                else if (property.PropertyType == typeof(int?))
                {
                    int? value = (int?)property.GetValue(item);
                    byte isNotNull = reader.ReadByte();
                    Assert.Equal(value.HasValue ? (byte)1 : (byte)0, isNotNull);

                    if (value.HasValue)
                    {
                        Assert.Equal(value.Value, reader.ReadInt32());
                    }
                }
            }
        }

        Assert.Equal(stream.Length, stream.Position);
    }

    [Fact]
    public void WritePropertyValue_NullPropertyDefinition_DoesNotWrite()
    {
        // Arrange
        using MemoryStream stream = new();
        using BinarySerializationWriter<TestItem> writer = new(stream);
        TestItem item = new() { Id = 7, OptionalNumber = 9 };

        // Act
        writer.WritePropertyValue(item, null);

        // Assert
        Assert.Equal(0, stream.Length);
    }

    [Fact]
    public void WritePropertyValue_NullableProperty_WritesNullIndicatorAndValue()
    {
        // Arrange
        using MemoryStream stream = new();
        using BinarySerializationWriter<TestItem> writer = new(stream);
        TestItem item = new() { Id = 1, OptionalNumber = 123 };
        PropertyInfo property = typeof(TestItem).GetProperty(nameof(TestItem.OptionalNumber))!;

        // Act
        writer.WritePropertyValue(item, property);

        // Assert
        stream.Position = 0;
        using BinaryReader reader = new(stream);

        Assert.Equal((byte)1, reader.ReadByte());
        Assert.Equal(123, reader.ReadInt32());
    }

    [Fact]
    public void WritePropertyValue_NonNullableProperty_WritesValue()
    {
        // Arrange
        using MemoryStream stream = new();
        using BinarySerializationWriter<TestItem> writer = new(stream);
        TestItem item = new() { Id = 456, OptionalNumber = null };
        PropertyInfo property = typeof(TestItem).GetProperty(nameof(TestItem.Id))!;

        // Act
        writer.WritePropertyValue(item, property);

        // Assert
        stream.Position = 0;
        using BinaryReader reader = new(stream);

        Assert.Equal(456, reader.ReadInt32());
    }

    private sealed class TestItem
    {
        public int Id { get; set; }

        public int? OptionalNumber { get; set; }
    }
}
