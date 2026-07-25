using Adaptive.Intelligence.Framework.Tests.Mocks;

namespace Adaptive.Intelligence.Framework.Tests.Common;

/// <summary>
/// Provides the tests for the <see cref="DisposableObjectBase"/> abstract class.
/// </summary>
public class DisposableObjectBaseTests
{
    [Fact]
    public void Can_Create()
    {
        MockDisposableBase mock = new MockDisposableBase();
    }

    [Fact]
    public void Can_Dispose()
    {
        MockDisposableBase mock = new MockDisposableBase();
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
        MockDisposableBase mock = new MockDisposableBase();
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
