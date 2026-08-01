using Adaptive.Intelligence.Logging;

namespace Adaptive.Intelligence.Framework.Tests.Logging;

/// <summary>
/// Provides tests for the <see cref="NullScope"/> class.
/// </summary>
public class NullScopeTests
{
    [Fact]
    public void Instance_Returns_A_New_NullScope_Reference_Each_Time()
    {
        NullScope first = NullScope.Instance;
        NullScope second = NullScope.Instance;

        Assert.NotSame(first, second);
    }

    [Fact]
    public void Dispose_Can_Be_Called_Without_Throwing()
    {
        NullScope scope = NullScope.Instance;

        Exception? ex = Record.Exception(scope.Dispose);

        Assert.Null(ex);
    }
}
