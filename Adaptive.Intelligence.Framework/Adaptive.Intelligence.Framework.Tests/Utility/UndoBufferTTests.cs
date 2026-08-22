using Adaptive.Intelligence.Utility;

namespace Adaptive.Intelligence.Framework.Tests;

public class UndoBufferTTests
{
    [Fact]
    public void UndoBuffer_Constructor_InitializesEmptyBuffer()
    {
        // Arrange

        // Act
        using UndoBuffer<string> buffer = new UndoBuffer<string>();

        // Assert
        Assert.False(buffer.HasData);
    }

    [Fact]
    public void Add_ItemAdded_RaisesUndoBufferChangedAndSetsHasData()
    {
        // Arrange
        using UndoBuffer<string> buffer = new UndoBuffer<string>();
        int eventCount = 0;
        buffer.UndoBufferChanged += (_, _) => eventCount++;

        // Act
        buffer.Add("value");

        // Assert
        Assert.True(buffer.HasData);
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void IsSame_BufferIsEmpty_ReturnsFalse()
    {
        // Arrange
        using UndoBuffer<string> buffer = new UndoBuffer<string>();

        // Act
        bool result = buffer.IsSame("value");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSame_ItemMatchesTopOfStack_ReturnsTrue()
    {
        // Arrange
        using UndoBuffer<string> buffer = new UndoBuffer<string>();
        buffer.Add("value");

        // Act
        bool result = buffer.IsSame("value");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSame_ItemDoesNotMatchTopOfStack_ReturnsFalse()
    {
        // Arrange
        using UndoBuffer<string> buffer = new UndoBuffer<string>();
        buffer.Add("value");

        // Act
        bool result = buffer.IsSame("different");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Dispose_BufferDisposed_SetsInternalBufferToNullForPublicBehavior()
    {
        // Arrange
        UndoBuffer<string?> buffer = new UndoBuffer<string?>();
        buffer.Add("value");

        // Act
        buffer.Dispose();
        bool sameAsNull = buffer.IsSame(null);
        bool sameAsNonNull = buffer.IsSame("value");

        // Assert
        Assert.True(sameAsNull);
        Assert.False(sameAsNonNull);
        Assert.Throws<NullReferenceException>(() =>
        {
            _ = buffer.HasData;
        });
    }

    [Fact]
    public void Dispose_DisposeCalledTwice_DoesNotThrow()
    {
        // Arrange
        UndoBuffer<string> buffer = new UndoBuffer<string>();

        // Act
        buffer.Dispose();
        Exception? exception = Record.Exception(() => buffer.Dispose());

        // Assert
        Assert.Null(exception);
    }

    [Fact]
    public void Add_BufferDisposed_ThrowsNullReferenceException()
    {
        // Arrange
        UndoBuffer<string> buffer = new UndoBuffer<string>();
        buffer.Dispose();

        // Act
        Action action = () => buffer.Add("value");

        // Assert
        Assert.Throws<NullReferenceException>(action);
    }

    [Fact]
    public void GetLast_BufferHasData_ReturnsMostRecentItem_RaisesUndoBufferChangedAndUpdatesHasData()
    {
        // Arrange
        using UndoBuffer<string> buffer = new UndoBuffer<string>();
        int eventCount = 0;
        buffer.UndoBufferChanged += (_, _) => eventCount++;
        buffer.Add("first");
        buffer.Add("second");
        eventCount = 0;

        // Act
        string? result = buffer.GetLast();

        // Assert
        Assert.Equal("second", result);
        Assert.True(buffer.HasData);
        Assert.Equal(1, eventCount);
    }


    [Fact]
    public void GetLast_BufferIsEmpty_ReturnsDefaultAndDoesNotRaiseUndoBufferChanged()
    {
        // Arrange
        using UndoBuffer<string> buffer = new UndoBuffer<string>();
        int eventCount = 0;
        buffer.UndoBufferChanged += (_, _) => eventCount++;

        // Act
        string? result = buffer.GetLast();

        // Assert
        Assert.Null(result);
        Assert.False(buffer.HasData);
        Assert.Equal(0, eventCount);
    }

    [Fact]
    public void GetLast_BufferHasDataWithoutSubscribers_DoesNotThrowAndReturnsItem()
    {
        // Arrange
        using UndoBuffer<string> buffer = new UndoBuffer<string>();
        buffer.Add("value");

        // Act
        Exception? exception = Record.Exception(() =>
        {
            string? item = buffer.GetLast();
            Assert.Equal("value", item);
        });

        // Assert
        Assert.Null(exception);
        Assert.False(buffer.HasData);
    }


}
