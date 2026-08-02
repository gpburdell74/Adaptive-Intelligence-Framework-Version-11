using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Collections;
using Adaptive.Intelligence.Common;

namespace Adaptive.Intelligence.Framework.Tests.Common
{
    /// <summary>
    /// Gets the definition for OperationResultTests.
    /// </summary>
    public class OperationResultTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_Default_InitializesEmptyExceptionCollection.
        /// </summary>
        public void Constructor_Default_InitializesEmptyExceptionCollection()
        {
            using OperationResult result = new();

            Assert.NotNull(result.Exceptions);
            Assert.Empty(result.Exceptions);
            Assert.False(result.Success);
            Assert.Null(result.Message);
            Assert.False(result.HasExceptions);
            Assert.Null(result.FirstException);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_WithSuccessAndMessage_SetsValues.
        /// </summary>
        public void Constructor_WithSuccessAndMessage_SetsValues()
        {
            using OperationResult result = new(true, "ok");

            Assert.True(result.Success);
            Assert.Equal("ok", result.Message);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_WithException_SetsFailureAndAddsException.
        /// </summary>
        public void Constructor_WithException_SetsFailureAndAddsException()
        {
            Exception ex = new InvalidOperationException("bad");
            using OperationResult result = new(ex, "failed");

            Assert.False(result.Success);
            Assert.Equal("failed", result.Message);
            Assert.True(result.HasExceptions);
            Assert.Same(ex, result.FirstException);
            Assert.Single(result.Exceptions!);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for AddException_WithNull_DoesNotAdd.
        /// </summary>
        public void AddException_WithNull_DoesNotAdd()
        {
            using OperationResult result = new();

            result.AddException(null);

            Assert.NotNull(result.Exceptions);
            Assert.Empty(result.Exceptions!);
            Assert.False(result.HasExceptions);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for AddException_WithValue_AddsException.
        /// </summary>
        public void AddException_WithValue_AddsException()
        {
            using OperationResult result = new();
            Exception ex = new ArgumentException("x");

            result.AddException(ex);

            Assert.True(result.HasExceptions);
            Assert.Same(ex, result.FirstException);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for AddExceptions_WithNull_DoesNotAdd.
        /// </summary>
        public void AddExceptions_WithNull_DoesNotAdd()
        {
            using OperationResult result = new();

            result.AddExceptions(null);

            Assert.NotNull(result.Exceptions);
            Assert.Empty(result.Exceptions!);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for AddExceptions_WithList_AddsAll.
        /// </summary>
        public void AddExceptions_WithList_AddsAll()
        {
            using OperationResult result = new();
            List<Exception> exceptions =
            [
                new InvalidOperationException("1"),
                new ArgumentException("2")
            ];

            result.AddExceptions(exceptions);

            Assert.Equal(2, result.Exceptions!.Count);
            Assert.Equal(exceptions[0], result.FirstException);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for CopyTo_WithTargetAndExceptions_CopiesAllData.
        /// </summary>
        public void CopyTo_WithTargetAndExceptions_CopiesAllData()
        {
            using OperationResult source = new(false, "message");
            source.AddException(new InvalidOperationException("1"));
            source.AddException(new ArgumentException("2"));
            TestOperationResult target = new();

            source.CopyTo(target);

            Assert.False(target.Success);
            Assert.Equal("message", target.Message);
            Assert.NotNull(target.Exceptions);
            Assert.Equal(2, target.Exceptions!.Count);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for CopyTo_WithTargetHavingNullExceptions_CopiesMessageAndSuccessOnly.
        /// </summary>
        public void CopyTo_WithTargetHavingNullExceptions_CopiesMessageAndSuccessOnly()
        {
            using OperationResult source = new(true, "done");
            source.AddException(new InvalidOperationException("1"));
            TestOperationResultWithNullExceptions target = new();

            source.CopyTo(target);

            Assert.True(target.Success);
            Assert.Equal("done", target.Message);
            Assert.Null(target.Exceptions);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for CopyTo_WithNullTarget_DoesNothing.
        /// </summary>
        public void CopyTo_WithNullTarget_DoesNothing()
        {
            using OperationResult source = new(true, "done");

            Exception? ex = Record.Exception(() => source.CopyTo(null));

            Assert.Null(ex);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for SetFailureMessage_SetsFailureAndMessage.
        /// </summary>
        public void SetFailureMessage_SetsFailureAndMessage()
        {
            using OperationResult result = new(true, "ok");

            result.SetFailureMessage("failed");

            Assert.False(result.Success);
            Assert.Equal("failed", result.Message);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Dispose_ClearsManagedState.
        /// </summary>
        public void Dispose_ClearsManagedState()
        {
            OperationResult result = new(true, "x");
            result.AddException(new InvalidOperationException("1"));

            result.Dispose();

            Assert.Null(result.Exceptions);
            Assert.Null(result.Message);
        }


        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_WithSuccessAndDefaultMessage_LeavesMessageNull.
        /// </summary>
        public void Constructor_WithSuccessAndDefaultMessage_LeavesMessageNull()
        {
            using OperationResult result = new(true);

            Assert.True(result.Success);
            Assert.Null(result.Message);
            Assert.NotNull(result.Exceptions);
            Assert.Empty(result.Exceptions!);
        }


        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_WithExceptionAndDefaultMessage_AddsExceptionAndKeepsMessageNull.
        /// </summary>
        public void Constructor_WithExceptionAndDefaultMessage_AddsExceptionAndKeepsMessageNull()
        {
            Exception ex = new InvalidOperationException("bad");
            using OperationResult result = new(ex);

            Assert.False(result.Success);
            Assert.Null(result.Message);
            Assert.NotNull(result.Exceptions);
            Assert.Single(result.Exceptions!);
            Assert.Same(ex, result.FirstException);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for FirstException_AfterDispose_ReturnsNull.
        /// </summary>
        public void FirstException_AfterDispose_ReturnsNull()
        {
            OperationResult result = new(new InvalidOperationException("bad"));

            result.Dispose();

            Assert.Null(result.FirstException);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Dispose_CalledMultipleTimes_DoesNotThrowAndKeepsStateCleared.
        /// </summary>
        public void Dispose_CalledMultipleTimes_DoesNotThrowAndKeepsStateCleared()
        {
            OperationResult result = new(false, "message");
            result.AddException(new InvalidOperationException("1"));

            result.Dispose();
            Exception? ex = Record.Exception(result.Dispose);

            Assert.Null(ex);
            Assert.Null(result.Exceptions);
            Assert.Null(result.Message);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Dispose_WithDisposingFalse_ClearsReferencesWithoutClearingCollection.
        /// </summary>
        public void Dispose_WithDisposingFalse_ClearsReferencesWithoutClearingCollection()
        {
            TestableOperationResult result = new(false, "message");
            result.AddException(new InvalidOperationException("1"));

            result.InvokeDispose(false);

            Assert.Null(result.Exceptions);
            Assert.Null(result.Message);
        }


        [Fact]
        /// <summary>
        /// Gets the definition for AddException_AfterDispose_InitializesCollectionAndAddsException.
        /// </summary>
        public void AddException_AfterDispose_InitializesCollectionAndAddsException()
        {
            OperationResult result = new();
            Exception ex = new InvalidOperationException("late");
            result.Dispose();

            result.AddException(ex);

            Assert.NotNull(result.Exceptions);
            Assert.True(result.HasExceptions);
            Assert.Single(result.Exceptions!);
            Assert.Same(ex, result.FirstException);
        }


        [Fact]
        /// <summary>
        /// Gets the definition for HasExceptions_AfterDispose_ReturnsFalse.
        /// </summary>
        public void HasExceptions_AfterDispose_ReturnsFalse()
        {
            OperationResult result = new();

            result.Dispose();

            Assert.False(result.HasExceptions);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for AddException_AfterDisposeWithNull_DoesNotInitializeCollection.
        /// </summary>
        public void AddException_AfterDisposeWithNull_DoesNotInitializeCollection()
        {
            OperationResult result = new();

            result.Dispose();
            result.AddException(null);

            Assert.Null(result.Exceptions);
            Assert.False(result.HasExceptions);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for AddExceptions_AfterDispose_InitializesCollectionAndAddsExceptions.
        /// </summary>
        public void AddExceptions_AfterDispose_InitializesCollectionAndAddsExceptions()
        {
            OperationResult result = new();
            List<Exception> exceptions =
            [
                new InvalidOperationException("1"),
                new ArgumentException("2")
            ];

            result.Dispose();
            result.AddExceptions(exceptions);

            Assert.NotNull(result.Exceptions);
            Assert.True(result.HasExceptions);
            Assert.Equal(2, result.Exceptions!.Count);
            Assert.Same(exceptions[0], result.FirstException);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for AddExceptions_AfterDisposeWithNull_DoesNotInitializeCollection.
        /// </summary>
        public void AddExceptions_AfterDisposeWithNull_DoesNotInitializeCollection()
        {
            OperationResult result = new();

            result.Dispose();
            result.AddExceptions(null);

            Assert.Null(result.Exceptions);
            Assert.False(result.HasExceptions);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for CopyTo_WithoutSourceExceptions_DoesNotAppendTargetExceptions.
        /// </summary>
        public void CopyTo_WithoutSourceExceptions_DoesNotAppendTargetExceptions()
        {
            using OperationResult source = new(true, "done");
            TestOperationResult target = new();
            Exception existing = new InvalidOperationException("existing");
            target.AddException(existing);

            source.CopyTo(target);

            Assert.True(target.Success);
            Assert.Equal("done", target.Message);
            Assert.Single(target.Exceptions!);
            Assert.Same(existing, target.FirstException);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for CopyTo_WithSourceExceptionsNull_DoesNotAppendTargetExceptions.
        /// </summary>
        public void CopyTo_WithSourceExceptionsNull_DoesNotAppendTargetExceptions()
        {
            OperationResult source = new(true, "done");
            TestOperationResult target = new();
            Exception existing = new InvalidOperationException("existing");
            target.AddException(existing);

            source.Dispose();
            source.CopyTo(target);

            Assert.True(target.Success);
            Assert.Null(target.Message);
            Assert.Single(target.Exceptions!);
            Assert.Same(existing, target.FirstException);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for SetFailureMessage_WithNull_SetsFailureAndClearsMessage.
        /// </summary>
        public void SetFailureMessage_WithNull_SetsFailureAndClearsMessage()
        {
            using OperationResult result = new(true, "ok");

            result.SetFailureMessage(null);

            Assert.False(result.Success);
            Assert.Null(result.Message);
        }

        /// <summary>
        /// Gets the definition for TestOperationResult.
        /// </summary>
        private sealed class TestOperationResult : IOperationResult
        {
            /// <summary>
            /// Gets the definition for Exceptions.
            /// </summary>
            public ExceptionCollection? Exceptions { get; } = [];
            /// <summary>
            /// Gets the definition for FirstException.
            /// </summary>
            public Exception? FirstException => Exceptions != null && Exceptions.Count > 0 ? Exceptions[0] : null;
            /// <summary>
            /// Gets the definition for HasExceptions.
            /// </summary>
            public bool HasExceptions => Exceptions != null && Exceptions.Count > 0;
            /// <summary>
            /// Gets the definition for Message.
            /// </summary>
            public string? Message { get; set; }
            /// <summary>
            /// Gets the definition for Success.
            /// </summary>
            public bool Success { get; set; }

            /// <summary>
            /// Gets the definition for AddException.
            /// </summary>
            public void AddException(Exception? exception)
            {
                if (exception != null)
                {
                    Exceptions!.Add(exception);
                }
            }

            /// <summary>
            /// Gets the definition for AddExceptions.
            /// </summary>
            public void AddExceptions(IEnumerable<Exception>? exceptions)
            {
                if (exceptions != null)
                {
                    Exceptions!.AddRange(exceptions);
                }
            }

            /// <summary>
            /// Gets the definition for CopyTo.
            /// </summary>
            public void CopyTo(IOperationResult? newResult)
            {
            }

            /// <summary>
            /// Gets the definition for SetFailureMessage.
            /// </summary>
            public void SetFailureMessage(string? message)
            {
                Success = false;
                Message = message;
            }

            /// <summary>
            /// Gets the definition for Dispose.
            /// </summary>
            public void Dispose()
            {
            }
        }

        /// <summary>
        /// Gets the definition for TestOperationResultWithNullExceptions.
        /// </summary>
        private sealed class TestOperationResultWithNullExceptions : IOperationResult
        {
            /// <summary>
            /// Gets the definition for Exceptions.
            /// </summary>
            public ExceptionCollection? Exceptions => null;
            /// <summary>
            /// Gets the definition for FirstException.
            /// </summary>
            public Exception? FirstException => null;
            /// <summary>
            /// Gets the definition for HasExceptions.
            /// </summary>
            public bool HasExceptions => false;
            /// <summary>
            /// Gets the definition for Message.
            /// </summary>
            public string? Message { get; set; }
            /// <summary>
            /// Gets the definition for Success.
            /// </summary>
            public bool Success { get; set; }

            /// <summary>
            /// Gets the definition for AddException.
            /// </summary>
            public void AddException(Exception? exception)
            {
            }

            /// <summary>
            /// Gets the definition for AddExceptions.
            /// </summary>
            public void AddExceptions(IEnumerable<Exception>? exceptions)
            {
            }

            /// <summary>
            /// Gets the definition for CopyTo.
            /// </summary>
            public void CopyTo(IOperationResult? newResult)
            {
            }

            /// <summary>
            /// Gets the definition for SetFailureMessage.
            /// </summary>
            public void SetFailureMessage(string? message)
            {
                Success = false;
                Message = message;
            }

            /// <summary>
            /// Gets the definition for Dispose.
            /// </summary>
            public void Dispose()
            {
            }
        }

        /// <summary>
        /// Gets the definition for TestableOperationResult.
        /// </summary>
        private sealed class TestableOperationResult(bool success, string? message) : OperationResult(success, message)
        {
            /// <summary>
            /// Gets the definition for InvokeDispose.
            /// </summary>
            public void InvokeDispose(bool disposing)
            {
                Dispose(disposing);
            }
        }
    }
}