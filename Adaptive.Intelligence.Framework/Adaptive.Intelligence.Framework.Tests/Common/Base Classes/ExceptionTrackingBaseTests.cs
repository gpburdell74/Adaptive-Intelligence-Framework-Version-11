using Adaptive.Intelligence.Common;
using Adaptive.Intelligence.Framework.Tests.Mocks;

namespace Adaptive.Intelligence.Framework.Tests.Common;

/// <summary>
/// Provides the tests for the <see cref="ExceptionTrackingBase"/> abstract class.
/// </summary>
public class ExceptionTrackingBaseTests
{
    [Fact]
    public void Initial_State_Is_Empty()
    {
        MockExceptionTrackingBase mock = new();

        Assert.NotNull(mock.Exceptions);
        Assert.Empty(mock.Exceptions);
        Assert.False(mock.HasExceptions);
        Assert.Null(mock.FirstException);
        Assert.Equal(string.Empty, mock.ExceptionMessages);
    }

    [Fact]
    public void AddException_Tracks_Exception_And_Updates_Properties()
    {
        MockExceptionTrackingBase mock = new();
        InvalidOperationException ex = new("Operation failed.");

        mock.AddException(ex);
        Assert.NotNull(mock.Exceptions);
        Assert.True(mock.HasExceptions);
        Assert.Single(mock.Exceptions);
        Assert.Same(ex, mock.FirstException);
    }

    [Fact]
    public void ClearExceptions_Removes_All_Tracked_Exceptions()
    {
        MockExceptionTrackingBase mock = new();
        mock.AddException(new InvalidOperationException("One"));
        mock.AddException(new ApplicationException("Two"));
        Assert.True(mock.HasExceptions);

        mock.ClearExceptions();

        Assert.NotNull(mock.Exceptions);
        Assert.False(mock.HasExceptions);
        Assert.Empty(mock.Exceptions);
        Assert.Null(mock.FirstException);
        Assert.Equal(string.Empty, mock.ExceptionMessages);
    }

    [Fact]
    public void ExceptionMessages_Contains_All_Exception_Messages_In_Order()
    {
        MockExceptionTrackingBase mock = new MockExceptionTrackingBase();
        mock.AddException(new InvalidOperationException("First error"));
        mock.AddException(new ApplicationException("Second error"));

        string messages = mock.ExceptionMessages;

        Assert.Contains("First error", messages);
        Assert.Contains("Second error", messages);
        Assert.True(messages.IndexOf("First error", StringComparison.Ordinal) < messages.IndexOf("Second error", StringComparison.Ordinal));
    }

    [Fact]
    public void CopyExceptions_With_Null_Result_Does_Not_Change_State()
    {
        MockExceptionTrackingBase mock = new MockExceptionTrackingBase();
        mock.AddException(new InvalidOperationException("Existing"));

        mock.CopyExceptions(null);

        Assert.NotNull(mock.Exceptions);
        Assert.True(mock.HasExceptions);
        Assert.Single(mock.Exceptions);
        Assert.Equal("Existing", mock.FirstException?.Message);
    }

    [Fact]
    public void CopyExceptions_With_Result_That_Has_No_Exceptions_Does_Not_Change_State()
    {
        MockExceptionTrackingBase mock = new MockExceptionTrackingBase();
        mock.AddException(new InvalidOperationException("Existing"));
        FakeOperationalResult result = new FakeOperationalResult();

        mock.CopyExceptions(result);

        Assert.NotNull(mock.Exceptions);
        Assert.True(mock.HasExceptions);
        Assert.Single(mock.Exceptions);
        Assert.Equal("Existing", mock.FirstException?.Message);
    }

    [Fact]
    public void CopyExceptions_Appends_Exceptions_From_Result()
    {
        MockExceptionTrackingBase mock = new MockExceptionTrackingBase();
        mock.AddException(new InvalidOperationException("Existing"));

        FakeOperationalResult result = new FakeOperationalResult();
        result.AddException(new ApplicationException("Copied one"));
        result.AddException(new ArgumentException("Copied two"));

        mock.CopyExceptions(result);

        Assert.NotNull(mock.Exceptions);
        Assert.Equal(3, mock.Exceptions.Count);
        Assert.Equal("Existing", mock.Exceptions[0].Message);
        Assert.Equal("Copied one", mock.Exceptions[1].Message);
        Assert.Equal("Copied two", mock.Exceptions[2].Message);
    }

    [Fact]
    public void Dispose_Clears_Exceptions_And_Prevents_Further_Tracking()
    {
        MockExceptionTrackingBase mock = new MockExceptionTrackingBase();
        mock.AddException(new InvalidOperationException("Before dispose"));
        Assert.True(mock.HasExceptions);

        mock.Dispose();

        Assert.NotNull(mock.Exceptions);
        Assert.False(mock.HasExceptions);
        Assert.Null(mock.FirstException);
        Assert.Empty(mock.Exceptions);
        Assert.Equal(string.Empty, mock.ExceptionMessages);

        mock.AddException(new InvalidOperationException("After dispose"));
        Assert.False(mock.HasExceptions);
        Assert.Empty(mock.Exceptions);

        FakeOperationalResult result = new FakeOperationalResult();
        result.AddException(new InvalidOperationException("Copied after dispose"));
        mock.CopyExceptions(result);
        Assert.False(mock.HasExceptions);
        Assert.Empty(mock.Exceptions);
    }

    private sealed class FakeOperationalResult : IOperationalResult
    {
        public ExceptionCollection? Exceptions { get; } = [];

        public Exception? FirstException => Exceptions != null && Exceptions.Count > 0 ? Exceptions[0] : null;

        public bool HasExceptions => Exceptions != null && Exceptions.Count > 0;

        public string? Message { get; set; }

        public bool Success { get; set; }

        public void AddException(Exception? exception)
        {
            if (exception != null)
            {
                Exceptions?.Add(exception);
            }
        }

        public void AddExceptions(IEnumerable<Exception>? exceptions)
        {
            if (exceptions != null)
            {
                Exceptions?.AddRange(exceptions);
            }
        }

        public void CopyTo(IOperationalResult? newResult)
        {
            if (newResult == null)
            {
                return;
            }

            newResult.Message = Message;
            newResult.Success = Success;
            newResult.AddExceptions(Exceptions);
        }

        public void SetFailureMessage(string? message)
        {
            Message = message;
            Success = false;
        }

        public void Dispose()
        {
            Exceptions?.Dispose();
        }
    }
}
