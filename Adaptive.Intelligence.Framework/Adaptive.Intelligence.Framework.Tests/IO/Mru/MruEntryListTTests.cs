using System.Collections.Generic;
using System.IO;
using Adaptive.Intelligence.IO;
using Adaptive.Intelligence.IO.Mru;
using Moq;

namespace Adaptive.Intelligence.Framework.Tests;

public class MruEntryListTTests
{
    [Fact]
    public void MruEntryList_SourceDataConstructor_AddsEntriesToList()
    {
        // Arrange
        Mock<IMruEntry> firstEntryMock = new();
        Mock<IMruEntry> secondEntryMock = new();
        List<IMruEntry> source =
        [
            firstEntryMock.Object,
            secondEntryMock.Object
        ];

        // Act
        TestMruEntryList sut = new(source);

        // Assert
        Assert.Equal(2, sut.Count);
        Assert.Same(firstEntryMock.Object, sut[0]);
        Assert.Same(secondEntryMock.Object, sut[1]);
    }

    [Fact]
    public void MruEntryList_StreamConstructor_PopulatesEntriesFromStream()
    {
        // Arrange
        MemoryStream stream = CreateStreamWithRecordIds(10, 20);

        // Act
        TestMruEntryList sut = new(stream);

        // Assert
        Assert.Equal(2, sut.Count);
        Assert.Equal(10, sut[0].Id);
        Assert.Equal(20, sut[1].Id);
    }

    [Fact]
    public void PopulateFromStream_WhenStreamCannotRead_ThrowsArgumentNullException()
    {
        // Arrange
        TestMruEntryList sut = new();
        Mock<Stream> streamMock = new();
        streamMock.SetupGet(stream => stream.CanRead).Returns(false);

        // Act
        ArgumentException exception = Assert.Throws<ArgumentException>(() => sut.PopulateFromStream(streamMock.Object));

        // Assert
        Assert.Equal("sourceStream", exception.ParamName);
        Assert.Contains("cannot be read", exception.Message, System.StringComparison.Ordinal);
    }

    [Fact]
    public void PopulateFromStream_WhenListHasExistingEntries_ClearsAndLoadsFromStream()
    {
        // Arrange
        TestMruEntryList sut = new();
        Mock<IMruEntry> existingEntryMock = new();
        existingEntryMock.SetupProperty(entry => entry.Id, 999);
        sut.Add(existingEntryMock.Object);

        MemoryStream stream = CreateStreamWithRecordIds(3, 4);

        // Act
        sut.PopulateFromStream(stream);

        // Assert
        Assert.Equal(2, sut.Count);
        Assert.Equal(3, sut[0].Id);
        Assert.Equal(4, sut[1].Id);
    }

    [Fact]
    public void SaveToStream_WhenStreamCannotWrite_ThrowsArgumentNullException()
    {
        // Arrange
        TestMruEntryList sut = new();
        Mock<Stream> streamMock = new();
        streamMock.SetupGet(stream => stream.CanWrite).Returns(false);

        // Act
        ArgumentException exception = Assert.Throws<ArgumentException>(() => sut.SaveToStream(streamMock.Object));

        // Assert
        Assert.Equal("destinationStream", exception.ParamName);
        Assert.Contains("cannot be written", exception.Message, System.StringComparison.Ordinal);
    }

    [Fact]
    public void SaveToStream_WithEntries_WritesRecordCountAndRecords()
    {
        // Arrange
        TestMruEntryList sut = new();
        Mock<IMruEntry> firstEntryMock = new();
        firstEntryMock.SetupProperty(entry => entry.Id, 7);

        Mock<IMruEntry> secondEntryMock = new();
        secondEntryMock.SetupProperty(entry => entry.Id, 8);

        sut.Add(firstEntryMock.Object);
        sut.Add(secondEntryMock.Object);

        using MemoryStream stream = new();

        // Act
        sut.SaveToStream(stream);

        // Assert
        stream.Position = 0;
        using BinaryReader reader = new(stream);
        Assert.Equal(2, reader.ReadInt32());
        Assert.Equal(7, reader.ReadInt32());
        Assert.Equal(8, reader.ReadInt32());
    }

    private static MemoryStream CreateStreamWithRecordIds(params int[] ids)
    {
        MemoryStream stream = new();
        using (BinaryWriter writer = new(stream, System.Text.Encoding.UTF8, true))
        {
            writer.Write(ids.Length);
            foreach (int id in ids)
            {
                writer.Write(id);
            }
        }

        stream.Position = 0;
        return stream;
    }


    private sealed class TestMruEntryList : MruEntryList<IMruEntry>
    {
        public TestMruEntryList()
        {
        }

        public TestMruEntryList(IEnumerable<IMruEntry> sourceData)
            : base(sourceData)
        {
        }

        public TestMruEntryList(Stream sourceStream)
            : base(sourceStream)
        {
        }

        protected override IMruEntry ReadRecord(SafeBinaryReader reader)
        {
            int id = reader.ReadInt32();
            Mock<IMruEntry> entryMock = new();
            entryMock.SetupProperty(entry => entry.Id, id);
            return entryMock.Object;
        }

        protected override void WriteRecord(SafeBinaryWriter writer, IMruEntry recordToWrite)
        {
            writer.Write(recordToWrite.Id);
        }
    }
}
