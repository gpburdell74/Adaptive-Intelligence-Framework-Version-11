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

}
