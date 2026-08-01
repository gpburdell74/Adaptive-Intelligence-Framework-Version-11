using Adaptive.Intelligence.Framework.Tests.Mocks;

namespace Adaptive.Intelligence.Framework.Tests.Common.Abstractions.Repository;

/// <summary>
/// Provides tests for the <see cref="Adaptive.Intelligence.Common.Abstractions.Repository.RepositoryBase{TIdType, TEntityType}"/> abstract class.
/// </summary>
public class RepositoryBaseTTests
{
    [Fact]
    public void Add_With_Null_Item_Returns_False_And_Does_Not_Record_Error()
    {
        MockRepositoryBaseT mock = new();

        bool success = mock.Add(null);

        Assert.False(success);
        Assert.False(mock.LastOperationSuccess);
        Assert.Null(mock.LastOperationError);
    }

    [Fact]
    public void Add_Returns_PerformAdd_Result_And_Updates_LastOperationSuccess()
    {
        MockRepositoryBaseT mock = new()
        {
            AddResult = true
        };

        MockEntityBase item = new() { Id = 100 };

        bool success = mock.Add(item);

        Assert.True(success);
        Assert.True(mock.LastOperationSuccess);
        Assert.Null(mock.LastOperationError);
    }

    [Fact]
    public void Add_When_PerformAdd_Throws_Returns_False_And_Records_Error()
    {
        MockRepositoryBaseT mock = new()
        {
            ThrowOnAdd = true
        };

        MockEntityBase item = new() { Id = 100 };
        bool success = mock.Add(item);

        Assert.False(success);
        Assert.False(mock.LastOperationSuccess);
        Assert.Equal("Add failed.", mock.LastOperationError);
        Assert.True(mock.HasExceptions);
    }

    [Fact]
    public async Task AddAsync_Uses_Given_CancellationToken_And_Updates_LastOperationSuccess()
    {
        MockRepositoryBaseT mock = new()
        {
            AddAsyncResult = true
        };

        using CancellationTokenSource source = new();
        MockEntityBase item = new() { Id = 101 };

        bool success = await mock.AddAsync(item, source.Token);

        Assert.True(success);
        Assert.True(mock.LastOperationSuccess);
        Assert.Equal(source.Token, mock.LastAddAsyncToken);
    }

    [Fact]
    public async Task AddAsync_When_PerformAddAsync_Throws_Returns_False_And_Records_Error()
    {
        MockRepositoryBaseT mock = new()
        {
            ThrowOnAddAsync = true
        };

        using CancellationTokenSource source = new();
        MockEntityBase item = new() { Id = 102 };
        bool success = await mock.AddAsync(item, source.Token);

        Assert.False(success);
        Assert.False(mock.LastOperationSuccess);
        Assert.Equal("Add async failed.", mock.LastOperationError);
        Assert.True(mock.HasExceptions);
    }

    [Fact]
    public void Delete_With_Null_Item_Returns_False_And_Does_Not_Record_Error()
    {
        MockRepositoryBaseT mock = new();

        bool success = mock.Delete(null);

        Assert.False(success);
        Assert.False(mock.LastOperationSuccess);
        Assert.Null(mock.LastOperationError);
    }

    [Fact]
    public void Delete_Returns_PerformDelete_Result_And_Updates_LastOperationSuccess()
    {
        MockRepositoryBaseT mock = new()
        {
            DeleteResult = true
        };

        MockEntityBase item = new() { Id = 42 };

        bool success = mock.Delete(item);

        Assert.True(success);
        Assert.True(mock.LastOperationSuccess);
        Assert.Null(mock.LastOperationError);
    }

    [Fact]
    public void Delete_When_PerformDelete_Throws_Returns_False_And_Records_Error()
    {
        MockRepositoryBaseT mock = new()
        {
            ThrowOnDelete = true
        };

        MockEntityBase item = new() { Id = 7 };

        bool success = mock.Delete(item);

        Assert.False(success);
        Assert.False(mock.LastOperationSuccess);
        Assert.Equal("Delete failed.", mock.LastOperationError);
        Assert.True(mock.HasExceptions);
    }

    [Fact]
    public async Task DeleteAsync_Uses_Given_CancellationToken_And_Raises_Async_Events()
    {
        MockRepositoryBaseT mock = new()
        {
            DeleteAsyncResult = true
        };
        List<string> started = [];
        List<string> completed = [];

        mock.AsyncQueryStarted += (_, e) => started.Add(e.Content ?? string.Empty);
        mock.AsyncQueryCompleted += (_, e) => completed.Add(e.Content ?? string.Empty);

        using CancellationTokenSource source = new();
        MockEntityBase item = new() { Id = 5 };

        bool success = await mock.DeleteAsync(item, source.Token);

        Assert.True(success);
        Assert.True(mock.LastOperationSuccess);
        Assert.Equal(source.Token, mock.LastDeleteAsyncToken);
        Assert.Single(started);
        Assert.Single(completed);
        Assert.Equal("DeleteAsync", started[0]);
        Assert.Equal("DeleteAsync", completed[0]);
        Assert.Equal(0, mock.QueriesRunning);
    }

    [Fact]
    public async Task DeleteAsync_When_PerformDeleteAsync_Throws_Returns_False_And_Records_Error()
    {
        MockRepositoryBaseT mock = new()
        {
            ThrowOnDeleteAsync = true
        };

        MockEntityBase item = new() { Id = 9 };

        bool success = await mock.DeleteAsync(item, CancellationToken.None);

        Assert.False(success);
        Assert.False(mock.LastOperationSuccess);
        Assert.Equal("Delete async failed.", mock.LastOperationError);
        Assert.True(mock.HasExceptions);
        Assert.Equal(0, mock.QueriesRunning);
    }

    [Fact]
    public void LoadItem_Returns_Item_And_Sets_LastOperationSuccess_True()
    {
        MockEntityBase expected = new() { Id = 88, Deleted = true };
        MockRepositoryBaseT mock = new()
        {
            LoadByIdResult = expected
        };

        MockEntityBase? item = mock.LoadItem(88);

        Assert.Same(expected, item);
        Assert.True(mock.LastOperationSuccess);
        Assert.Null(mock.LastOperationError);
    }

    [Fact]
    public void LoadItem_When_Result_Is_Null_Leaves_LastOperationSuccess_False()
    {
        MockRepositoryBaseT mock = new()
        {
            LoadByIdResult = null
        };

        MockEntityBase? item = mock.LoadItem(3);

        Assert.Null(item);
        Assert.False(mock.LastOperationSuccess);
        Assert.Null(mock.LastOperationError);
    }

    [Fact]
    public async Task LoadItemAsync_Uses_Given_CancellationToken_And_Updates_LastOperationSuccess()
    {
        MockEntityBase expected = new() { Id = 21 };
        MockRepositoryBaseT mock = new()
        {
            LoadAsyncResult = expected
        };

        using CancellationTokenSource source = new();
        MockEntityBase? item = await mock.LoadItemAsync(21, source.Token);

        Assert.Same(expected, item);
        Assert.True(mock.LastOperationSuccess);
        Assert.Equal(source.Token, mock.LastLoadAsyncToken);
        Assert.Equal(0, mock.QueriesRunning);
    }

    [Fact]
    public async Task LoadItemAsync_When_PerformLoadAsync_Throws_Returns_Null_And_Records_Error()
    {
        MockRepositoryBaseT mock = new()
        {
            ThrowOnLoadAsync = true
        };

        MockEntityBase? item = await mock.LoadItemAsync(44, CancellationToken.None);

        Assert.Null(item);
        Assert.False(mock.LastOperationSuccess);
        Assert.Equal("Load async failed.", mock.LastOperationError);
        Assert.True(mock.HasExceptions);
        Assert.Equal(0, mock.QueriesRunning);
    }

    [Fact]
    public void Save_Returns_PerformSave_Result_And_Updates_LastOperationSuccess()
    {
        MockRepositoryBaseT mock = new()
        {
            SaveResult = true
        };

        MockEntityBase item = new() { Id = 10 };
        bool success = mock.Save(item);

        Assert.True(success);
        Assert.True(mock.LastOperationSuccess);
        Assert.Null(mock.LastOperationError);
    }

    [Fact]
    public void Save_When_PerformSave_Throws_Returns_False_And_Records_Error()
    {
        MockRepositoryBaseT mock = new()
        {
            ThrowOnSave = true
        };

        MockEntityBase item = new() { Id = 10 };
        bool success = mock.Save(item);

        Assert.False(success);
        Assert.False(mock.LastOperationSuccess);
        Assert.Equal("Save failed.", mock.LastOperationError);
        Assert.True(mock.HasExceptions);
    }

    [Fact]
    public async Task SaveAsync_Uses_Given_CancellationToken_And_Updates_LastOperationSuccess()
    {
        MockRepositoryBaseT mock = new()
        {
            SaveAsyncResult = true
        };

        using CancellationTokenSource source = new();
        MockEntityBase item = new() { Id = 22 };

        bool success = await mock.SaveAsync(item, source.Token);

        Assert.True(success);
        Assert.True(mock.LastOperationSuccess);
        Assert.Equal(source.Token, mock.LastSaveAsyncToken);
        Assert.Equal(0, mock.QueriesRunning);
    }

    [Fact]
    public async Task SaveAsync_When_PerformSaveAsync_Throws_Returns_False_And_Records_Error()
    {
        MockRepositoryBaseT mock = new()
        {
            ThrowOnSaveAsync = true
        };

        MockEntityBase item = new() { Id = 31 };
        bool success = await mock.SaveAsync(item, CancellationToken.None);

        Assert.False(success);
        Assert.False(mock.LastOperationSuccess);
        Assert.Equal("Save async failed.", mock.LastOperationError);
        Assert.True(mock.HasExceptions);
        Assert.Equal(0, mock.QueriesRunning);
    }
}
