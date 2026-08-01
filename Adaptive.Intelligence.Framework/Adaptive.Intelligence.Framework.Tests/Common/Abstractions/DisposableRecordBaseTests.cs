using Adaptive.Intelligence.Framework.Tests.Mocks;

namespace Adaptive.Intelligence.Framework.Tests.Common.Abstractions;

/// <summary>
/// Provides the tests for the <see cref="DisposableObjectBase"/> abstract class.
/// </summary>
public class DisposableRecordBaseTests
{
    [Fact]
    public void Can_Create()
    {
        MockDisposableRecordBase mock = new();
        Assert.NotNull(mock);
    }

    [Fact]
    public void Can_Dispose()
    {
        MockDisposableRecordBase mock = new();
        Assert.False(mock.MockIsDisposed);

        mock.Dispose();
        Assert.True(mock.MockIsDisposed);
        mock.Dispose();
        Assert.True(mock.MockIsDisposed);
        mock.Dispose();
        Assert.True(mock.MockIsDisposed);

    }
    [Fact]
    public void Can_Dispose_Safely()
    {
        MockDisposableRecordBase mock = new();
        mock.Dispose();
        Assert.True(mock.MockIsDisposed);

        mock.Dispose();
        mock.Dispose();
        mock.Dispose();
        mock.Dispose();
        mock.Dispose();
        Assert.True(mock.MockIsDisposed);
    }
}
   