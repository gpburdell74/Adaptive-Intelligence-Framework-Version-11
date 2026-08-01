using Adaptive.Intelligence.Common.Abstractions.Logging;
using Adaptive.Intelligence.Logging;
using Microsoft.Extensions.Logging;

namespace Adaptive.Intelligence.Framework.Tests.Logging;

public class StructuredLogRecordCollectionTests
{
    [Fact]
    public void Constructor_ShouldInitializeEmptyCollection()
    {
        // Arrange & Act
        var collection = new StructuredLogRecordCollection();

        // Assert
        Assert.NotNull(collection);
        Assert.Empty(collection);
    }

    [Fact]
    public void Constructor_Sets_Capacity()
    {
        // Arrange & Act
        var collection = new StructuredLogRecordCollection(10);

        // Assert
        Assert.NotNull(collection);
        Assert.Empty(collection);
        Assert.Equal(10, collection.Capacity);
    }

    [Fact]
    public void Constructor_Adds_Records()
    {
        List<SimpleLogRecord> list = new List<SimpleLogRecord>();
        list.Add(new SimpleLogRecord
        {
            InformationMessage = "Test message 1",
            Level = LogLevel.Information
        });

        list.Add(new SimpleLogRecord
        {
            InformationMessage = "Test message 2",
            Level = LogLevel.Information
        });

        list.Add(new SimpleLogRecord
        {
            InformationMessage = "Test message 3",
            Level = LogLevel.Information
        });
        // Arrange & Act
        var collection = new StructuredLogRecordCollection(list);

        // Assert
        Assert.NotNull(collection);
        Assert.NotEmpty(collection);
        Assert.Equal(3, collection.Count);

        Assert.NotNull(collection[0]);
        Assert.NotNull(collection[1]);
        Assert.NotNull(collection[2]);

        Assert.Equal("Test message 1", collection[0].InformationMessage);
        Assert.Equal("Test message 2", collection[1].InformationMessage);
        Assert.Equal("Test message 3", collection[2].InformationMessage);

    }

    [Fact]
    public void Indexer_Get_Works()
    {
        var newRecord = new SimpleLogRecord
        {
            InformationMessage = "Test message 3",
            Level = LogLevel.Information
        };

        var collection = new StructuredLogRecordCollection();
        collection.Add(newRecord);

        // Assert
        Assert.NotNull(collection);
        Assert.NotEmpty(collection);
        Assert.Single(collection);
        Assert.Equal(newRecord, collection[0]);
    }


    [Fact]
    public void Indexer_Set_Works()
    {
        var oldRecord = new SimpleLogRecord
        {
            InformationMessage = "To Be Replaced",
            Level = LogLevel.Information
        };
        var newRecord = new SimpleLogRecord
        {
            InformationMessage = "Is Replaced",
            Level = LogLevel.Information
        };

        var collection = new StructuredLogRecordCollection();
        collection.Add(oldRecord);

        collection[0] = newRecord;

        // Assert
        Assert.NotNull(collection);
        Assert.NotEmpty(collection);
        Assert.Single(collection);
        Assert.Equal(newRecord, collection[0]);
    }

    [Fact]
    public void Add_Range_Works()
    {
        List<SimpleLogRecord> list = new List<SimpleLogRecord>();
        list.Add(new SimpleLogRecord
        {
            InformationMessage = "Test message 1",
            Level = LogLevel.Information
        });

        list.Add(new SimpleLogRecord
        {
            InformationMessage = "Test message 2",
            Level = LogLevel.Information
        });

        list.Add(new SimpleLogRecord
        {
            InformationMessage = "Test message 3",
            Level = LogLevel.Information
        });
        // Arrange & Act
        var collection = new StructuredLogRecordCollection(3);

        collection.AddRange(list);

        // Assert
        Assert.NotNull(collection);
        Assert.NotEmpty(collection);
        Assert.Equal(3, collection.Count);

        Assert.NotNull(collection[0]);
        Assert.NotNull(collection[1]);
        Assert.NotNull(collection[2]);

        Assert.Equal("Test message 1", collection[0].InformationMessage);
        Assert.Equal("Test message 2", collection[1].InformationMessage);
        Assert.Equal("Test message 3", collection[2].InformationMessage);


    }

    [Fact]
    public void Remove_Works()
    {
        var record1 = new SimpleLogRecord
        {
            InformationMessage = "Test message 1",
            Level = LogLevel.Information
        };
        var record2 = new SimpleLogRecord
        {
            InformationMessage = "Test message 2",
            Level = LogLevel.Information
        };
        var collection = new StructuredLogRecordCollection();
        collection.Add(record1);
        collection.Add(record2);
        // Act
        collection.Remove(record1);
        bool hasInstance = collection.Contains(record1);

        // Assert
        Assert.False(hasInstance);
        Assert.Single(collection);
        Assert.Equal(record2, collection[0]);
    }

    [Fact]
    public void RemoveAll_Works()
    {
        var record1 = new SimpleLogRecord
        {
            InformationMessage = "Test message 1",
            Level = LogLevel.Information
        };
        var record2 = new SimpleLogRecord
        {
            InformationMessage = "Test message 2",
            Level = LogLevel.Information
        };
        var collection = new StructuredLogRecordCollection();
        collection.Add(record1);
        collection.Add(record2);
        // Act
        collection.RemoveAll(r => r.InformationMessage.Contains("Test message"));
        // Assert
        Assert.Empty(collection);
    }

    [Fact]
    public void RemoveAt_Works()
    {
        var record1 = new SimpleLogRecord
        {
            InformationMessage = "Test message 1",
            Level = LogLevel.Information
        };
        var record2 = new SimpleLogRecord
        {
            InformationMessage = "Test message 2",
            Level = LogLevel.Information
        };
        var collection = new StructuredLogRecordCollection();
        collection.Add(record1);
        collection.Add(record2);
        // Act
        collection.RemoveAt(0);
        // Assert
        Assert.Single(collection);
        Assert.Equal(record2, collection[0]);
    }

    [Fact]
    public void Remove_Range_Works()
    {
        var record1 = new SimpleLogRecord
        {
            InformationMessage = "Test message 1",
            Level = LogLevel.Information
        };
        var record2 = new SimpleLogRecord
        {
            InformationMessage = "Test message 2",
            Level = LogLevel.Information
        };
        var record3 = new SimpleLogRecord
        {
            InformationMessage = "Test message 3",
            Level = LogLevel.Information
        };
        var collection = new StructuredLogRecordCollection();
        collection.Add(record1);
        collection.Add(record2);
        collection.Add(record3);
        // Act
        collection.RemoveRange(0, 2);
        // Assert
        Assert.Single(collection);
        Assert.Equal(record3, collection[0]);
    }

    [Fact]
    public void Reverse_Works()
    {
        var record1 = new SimpleLogRecord
        {
            InformationMessage = "Test message 1",
            Level = LogLevel.Information
        };
        var record2 = new SimpleLogRecord
        {
            InformationMessage = "Test message 2",
            Level = LogLevel.Information
        };
        var record3 = new SimpleLogRecord
        {
            InformationMessage = "Test message 3",
            Level = LogLevel.Information
        };
        var collection = new StructuredLogRecordCollection();
        collection.Add(record1);
        collection.Add(record2);
        collection.Add(record3);
        // Act
        collection.Reverse();
        // Assert
        Assert.Equal(record3, collection[0]);
        Assert.Equal(record2, collection[1]);
        Assert.Equal(record1, collection[2]);
     }
}
