using Adaptive.Intelligence.IO;
using Adaptive.Intelligence.IO.Mru;

namespace Adaptive.Intelligence.Framework.Tests;

public class MruFileProviderTests
{
    [Fact]
    public void MruFileProvider_DefaultConstructor_SetsExpectedDefaults()
    {
        // Arrange

        // Act
        using MruFileProvider provider = new MruFileProvider();

        // Assert
        Assert.NotNull(provider);
        Assert.Null(provider.FileName);
        Assert.False(provider.UseLocalExecutionPath);
        Assert.Null(provider.Entries);
    }

    [Fact]
    public void MruFileProvider_FileNameConstructor_SetsFileNameAndDisablesExecutionPath()
    {
        // Arrange
        const string localFileName = "recent-files.json";

        // Act
        using MruFileProvider provider = new MruFileProvider(localFileName);

        // Assert
        Assert.Equal(localFileName, provider.FileName);
        Assert.False(provider.UseLocalExecutionPath);
        Assert.Null(provider.Entries);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void MruFileProvider_FileNameAndExecutionPathConstructor_SetsProvidedValues(bool useExecutionPath)
    {
        // Arrange
        const string localFileName = "recent-files.json";

        // Act
        using MruFileProvider provider = new MruFileProvider(localFileName, useExecutionPath);

        // Assert
        Assert.Equal(localFileName, provider.FileName);
        Assert.Equal(useExecutionPath, provider.UseLocalExecutionPath);
        Assert.Null(provider.Entries);
    }

    [Fact]
    public void Entries_BeforeAndAfterInitialize_ReturnsExpectedValue()
    {
        // Arrange
        using MruFileProvider provider = new MruFileProvider();

        // Act
        IMruEntryList? entriesBeforeInitialize = provider.Entries;
        provider.Initialize();
        IMruEntryList? entriesAfterInitialize = provider.Entries;

        // Assert
        Assert.Null(entriesBeforeInitialize);
        Assert.NotNull(entriesAfterInitialize);
        Assert.Same(entriesAfterInitialize, provider.Entries);
    }

    [Fact]
    public void Dispose_AfterInitializeAndAddEntry_ClearsEntriesAndResetsState()
    {
        // Arrange
        MruFileProvider provider = new MruFileProvider("recent-files.json", true);
        provider.Initialize();
        provider.AddEntry("C:\\Temp\\file1.txt");

        // Assert
        Assert.NotNull(provider.Entries);
        Assert.Equal(1, provider.EntryCount);

        // Act
        provider.Dispose();

        // Assert
        Assert.Null(provider.Entries);
        Assert.Equal(0, provider.EntryCount);
        Assert.Null(provider.FileName);

        // Act
        provider.Dispose();

        // Assert
        Assert.Null(provider.Entries);
        Assert.Null(provider.FileName);
    }

    [Fact]
    public void Save_And_Load_Works()
    {
        MruFileEntryList mruList = new MruFileEntryList();
        mruList.Add(
            new MruFileEntry
            {
                DisplayText = "File No 1",
                FileName = "C:\\Temp\\file1.txt",
                MruData = "Some data",
                Permissions = 42,
                Id = 1
            });
        mruList.Add(
            new MruFileEntry
            {
                DisplayText = "File No 2",
                FileName = "C:\\Temp\\file2.txt",
                MruData = "Some data 2",
                Permissions = 43,
                Id = 2
            });
        mruList.Add(
            new MruFileEntry
            {
                DisplayText = "File No 3",
                FileName = "C:\\Temp\\file3.txt",
                MruData = "Some data 3",
                Permissions = 44,
                Id = 3
            });

        string tempFile = System.IO.Path.GetTempFileName();

        MruFileProvider provider = new MruFileProvider();
        provider.AddMruEntry(mruList[0]);
        provider.AddMruEntry(mruList[1]);
        provider.AddMruEntry(mruList[2]);
        provider.FileName = tempFile;
        provider.Save();
        provider.Dispose();

        Assert.True(SafeIO.FileExists(tempFile));
        Assert.True(SafeIO.GetFileSizeNative(tempFile) > 0);

        MruFileProvider provider2 = new MruFileProvider();
        provider2.FileName = tempFile;
        provider2.Load();
        var entry1 = provider2.GetEntry(0);
        var entry2 = provider2.GetEntry(1);
        var entry3 = provider2.GetEntry(2);

        Assert.NotNull(entry1);
        Assert.NotNull(entry2);
        Assert.NotNull(entry3);

        Assert.Equal("File No 1", entry1.DisplayText);
        Assert.Equal("C:\\Temp\\file1.txt", entry1.FileName);
        Assert.Equal("Some data", entry1.MruData);
        Assert.Equal(42, entry1.Permissions);

        Assert.Equal("File No 2", entry2.DisplayText);
        Assert.Equal("C:\\Temp\\file2.txt", entry2.FileName);
        Assert.Equal("Some data 2", entry2.MruData);
        Assert.Equal(43, entry2.Permissions);

        Assert.Equal("File No 3", entry3.DisplayText);
        Assert.Equal("C:\\Temp\\file3.txt", entry3.FileName);
        Assert.Equal("Some data 3", entry3.MruData);
        Assert.Equal(44, entry3.Permissions);

        provider2.Dispose();

    }
}
