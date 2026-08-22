using Adaptive.Intelligence.IO;
using Adaptive.Intelligence.IO.Mru;

namespace Adaptive.Intelligence.Framework.Tests;

public class MruFileEntryListTests
{
    [Fact]
    public void AddFile_DisplayTextProvided_AddsEntryWithDisplayText()
    {
        // Arrange
        MruFileEntryList list = new MruFileEntryList();
        const string fileName = "C:\\Temp\\document.txt";
        const string displayText = "Document";
        // Act
        list.AddFile(fileName, displayText);

        // Assert
        Assert.Single(list);
        Assert.Equal(fileName, list[0].FileName);
        Assert.Equal(displayText, list[0].DisplayText);
        
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void AddFile_DisplayTextNullOrEmpty_AddsEntryWithoutDisplayText(string? displayText)
    {
        // Arrange
        MruFileEntryList list = new MruFileEntryList();
        const string fileName = "C:\\Temp\\document.txt";

        // Act
        list.AddFile(fileName, displayText);

        // Assert
        Assert.Single(list);
        Assert.Equal(fileName, list[0].FileName);
        Assert.Null(list[0].DisplayText);
    }

    [Fact]
    public void Constructor_SourceData_CopiesEntriesToList()
    {
        // Arrange
        MruFileEntry first = new MruFileEntry { Id = 1, FileName = "C:\\Temp\\first.txt", Permissions = 7 };
        MruFileEntry second = new MruFileEntry { Id = 2, FileName = "C:\\Temp\\second.txt", Permissions = 9 };
        IEnumerable<MruFileEntry> sourceData = new[] { first, second };

        // Act
        MruFileEntryList list = new MruFileEntryList(sourceData);

        // Assert
        Assert.Equal(2, list.Count);
        Assert.Same(first, list[0]);
        Assert.Same(second, list[1]);
    }

    [Fact]
    public void Constructor_SourceStream_LoadsEntriesFromStream()
    {
        // Arrange
        MruFileEntryList sourceList = new MruFileEntryList
        {
            new MruFileEntry { Id = 10, FileName = "C:\\Temp\\alpha.txt", Permissions = 3 },
            new MruFileEntry { Id = 20, FileName = "C:\\Temp\\beta.txt", Permissions = 5 },
        };

        using MemoryStream stream = new MemoryStream();
        sourceList.SaveToStream(stream);
        stream.Position = 0;

        // Act
        MruFileEntryList list = new MruFileEntryList(stream);

        // Assert
        Assert.Equal(2, list.Count);
        Assert.Equal(10, list[0].Id);
        Assert.Equal("C:\\Temp\\alpha.txt", list[0].FileName);
        Assert.Equal(3, list[0].Permissions);
        Assert.Equal(20, list[1].Id);
        Assert.Equal("C:\\Temp\\beta.txt", list[1].FileName);
        Assert.Equal(5, list[1].Permissions);
    }

    [Fact]
    public void ReadRecord_EntryDataInReader_ReturnsPopulatedEntry()
    {
        // Arrange
        TestableMruFileEntryList list = new TestableMruFileEntryList();
        using MruFileEntry expected = new MruFileEntry
        {
            Id = 42,
            FileName = "C:\\Temp\\read.txt",
            Permissions = 12,
        };

        using MemoryStream stream = new MemoryStream();
        using SafeBinaryWriter writer = new SafeBinaryWriter(stream);
        expected.Save(writer);
        writer.Flush();
        stream.Position = 0;

        using SafeBinaryReader reader = new SafeBinaryReader(stream);

        // Act
        using MruFileEntry actual = list.InvokeReadRecord(reader);

        // Assert
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.FileName, actual.FileName);
        Assert.Equal(expected.Permissions, actual.Permissions);
    }

    [Fact]
    public void WriteRecord_ValidEntry_WritesEntryContent()
    {
        // Arrange
        TestableMruFileEntryList list = new TestableMruFileEntryList();
        using MruFileEntry entry = new MruFileEntry
        {
            Id = 77,
            FileName = "C:\\Temp\\write.txt",
            Permissions = 14,
        };

        using MemoryStream stream = new MemoryStream();
        using SafeBinaryWriter writer = new SafeBinaryWriter(stream);

        // Act
        list.InvokeWriteRecord(writer, entry);
        writer.Flush();
        stream.Position = 0;

        using SafeBinaryReader reader = new SafeBinaryReader(stream);
        using MruFileEntry actual = new MruFileEntry();
        actual.Load(reader);

        // Assert
        Assert.Equal(entry.Id, actual.Id);
        Assert.Equal(entry.FileName, actual.FileName);
        Assert.Equal(entry.Permissions, actual.Permissions);
    }


    [Fact]
    public void ContainsFile_FileExists_ReturnsTrue()
    {
        // Arrange
        MruFileEntryList list = new MruFileEntryList();
        const string fileName = "C:\\Temp\\exists.txt";
        list.AddFile(fileName);

        // Act
        bool result = list.ContainsFile(fileName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ContainsFile_FileDoesNotExist_ReturnsFalse()
    {
        // Arrange
        MruFileEntryList list = new MruFileEntryList();
        list.AddFile("C:\\Temp\\other.txt");

        // Act
        bool result = list.ContainsFile("C:\\Temp\\missing.txt");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void RemoveFile_FileExists_RemovesMatchingEntry()
    {
        // Arrange
        MruFileEntryList list = new MruFileEntryList();
        const string existingFile = "C:\\Temp\\remove-me.txt";
        const string otherFile = "C:\\Temp\\keep-me.txt";
        list.AddFile(existingFile);
        list.AddFile(otherFile);

        // Act
        list.RemoveFile(existingFile);

        // Assert
        Assert.Single(list);
        Assert.Equal(otherFile, list[0].FileName);
    }

    [Fact]
    public void RemoveFile_FileDoesNotExist_DoesNotChangeList()
    {
        // Arrange
        MruFileEntryList list = new MruFileEntryList();
        const string existingFile = "C:\\Temp\\existing.txt";
        list.AddFile(existingFile);

        // Act
        list.RemoveFile("C:\\Temp\\missing.txt");

        // Assert
        Assert.Single(list);
        Assert.Equal(existingFile, list[0].FileName);
    }


    private sealed class TestableMruFileEntryList : MruFileEntryList
    {
        public MruFileEntry InvokeReadRecord(SafeBinaryReader reader)
        {
            return ReadRecord(reader);
        }

        public void InvokeWriteRecord(SafeBinaryWriter writer, MruFileEntry entry)
        {
            WriteRecord(writer, entry);
        }
    }
}

