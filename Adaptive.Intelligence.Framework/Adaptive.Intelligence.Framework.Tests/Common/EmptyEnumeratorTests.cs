using Adaptive.Intelligence.Common;
using System.Collections;

namespace Adaptive.Intelligence.Framework.Tests.Common
{
    /// <summary>
    /// Gets the definition for EmptyEnumeratorTests.
    /// </summary>
    public class EmptyEnumeratorTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for MoveNext_AlwaysReturnsFalse.
        /// </summary>
        public void MoveNext_AlwaysReturnsFalse()
        {
            EmptyEnumerator<int> enumerator = new();

            Assert.False(enumerator.MoveNext());
            Assert.False(enumerator.MoveNext());
            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Current_ForReferenceType_ReturnsNull.
        /// </summary>
        public void Current_ForReferenceType_ReturnsNull()
        {
            EmptyEnumerator<string> enumerator = new();

            string current = enumerator.Current;

            Assert.Null(current);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Current_ForValueType_ReturnsDefaultValue.
        /// </summary>
        public void Current_ForValueType_ReturnsDefaultValue()
        {
            EmptyEnumerator<int> enumerator = new();

            int current = enumerator.Current;

            Assert.Equal(default, current);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for NonGenericCurrent_ReturnsDefaultValue.
        /// </summary>
        public void NonGenericCurrent_ReturnsDefaultValue()
        {
            EmptyEnumerator<int> enumerator = new();
            IEnumerator nonGeneric = enumerator;

            object? current = nonGeneric.Current;

            Assert.Equal(0, current);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Reset_DoesNotThrow.
        /// </summary>
        public void Reset_DoesNotThrow()
        {
            EmptyEnumerator<Guid> enumerator = new();

            Exception? ex = Record.Exception(() => enumerator.Reset());

            Assert.Null(ex);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Dispose_DoesNotThrow.
        /// </summary>
        public void Dispose_DoesNotThrow()
        {
            EmptyEnumerator<DateTime> enumerator = new();

            Exception? ex = Record.Exception(() => enumerator.Dispose());

            Assert.Null(ex);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ResetAndDispose_AfterMultipleCalls_DoesNotChangeBehavior.
        /// </summary>
        public void ResetAndDispose_AfterMultipleCalls_DoesNotChangeBehavior()
        {
            EmptyEnumerator<int> enumerator = new();

            enumerator.Reset();
            enumerator.Dispose();
            enumerator.Reset();
            enumerator.Dispose();

            Assert.False(enumerator.MoveNext());
            Assert.Equal(0, enumerator.Current);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for NonGenericCurrent_ForReferenceType_ReturnsNull.
        /// </summary>
        public void NonGenericCurrent_ForReferenceType_ReturnsNull()
        {
            EmptyEnumerator<string> enumerator = new();
            IEnumerator nonGeneric = enumerator;

            object? current = nonGeneric.Current;

            Assert.Null(current);
        }


        [Fact]
        /// <summary>
        /// Gets the definition for ResetAndDispose_ThroughInterfaces_DoNotThrowAndEnumeratorRemainsEmpty.
        /// </summary>
        public void ResetAndDispose_ThroughInterfaces_DoNotThrowAndEnumeratorRemainsEmpty()
        {
            EmptyEnumerator<int> enumerator = new();
            IEnumerator nonGeneric = enumerator;
            EmptyEnumerator<int> disposable = enumerator;

            Exception? resetException = Record.Exception(nonGeneric.Reset);
            Exception? disposeException = Record.Exception(disposable.Dispose);

            Assert.Null(resetException);
            Assert.Null(disposeException);
            Assert.False(nonGeneric.MoveNext());
            Assert.Equal(0, nonGeneric.Current);
        }

    }
}