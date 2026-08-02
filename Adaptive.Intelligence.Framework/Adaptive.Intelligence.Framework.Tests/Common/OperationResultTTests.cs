using Adaptive.Intelligence.Common;

namespace Adaptive.Intelligence.Framework.Tests.Common
{
    /// <summary>
    /// Gets the definition for OperationResultTTests.
    /// </summary>
    public class OperationResultTTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_Default_InitializesBaseAndDataDefaults.
        /// </summary>
        public void Constructor_Default_InitializesBaseAndDataDefaults()
        {
            using OperationResult<string> result = new();

            Assert.NotNull(result.Exceptions);
            Assert.False(result.Success);
            Assert.Null(result.Message);
            Assert.Null(result.DataContent);
            Assert.False(result.HasData);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_WithSuccessOnly_SetsSuccess.
        /// </summary>
        public void Constructor_WithSuccessOnly_SetsSuccess()
        {
            using OperationResult<int> result = new(true);

            Assert.True(result.Success);
            Assert.Null(result.Message);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_WithSuccessAndMessage_SetsProperties.
        /// </summary>
        public void Constructor_WithSuccessAndMessage_SetsProperties()
        {
            using OperationResult<int> result = new(false, "not ok");

            Assert.False(result.Success);
            Assert.Equal("not ok", result.Message);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_WithException_SetsFailureAndAddsException.
        /// </summary>
        public void Constructor_WithException_SetsFailureAndAddsException()
        {
            Exception ex = new InvalidOperationException("bad");
            using OperationResult<int> result = new(ex);

            Assert.False(result.Success);
            Assert.True(result.HasExceptions);
            Assert.Same(ex, result.FirstException);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_WithExceptionAndMessage_SetsFailureMessageAndException.
        /// </summary>
        public void Constructor_WithExceptionAndMessage_SetsFailureMessageAndException()
        {
            Exception ex = new ArgumentException("bad");
            using OperationResult<int> result = new(ex, "failed");

            Assert.False(result.Success);
            Assert.Equal("failed", result.Message);
            Assert.True(result.HasExceptions);
            Assert.Same(ex, result.FirstException);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_WithDataContent_SetsDataAndSuccess.
        /// </summary>
        public void Constructor_WithDataContent_SetsDataAndSuccess()
        {
            using OperationResult<string> result = new("payload");

            Assert.True(result.Success);
            Assert.Equal("payload", result.DataContent);
            Assert.True(result.HasData);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for HasData_WhenReferenceTypeContentIsNull_ReturnsFalse.
        /// </summary>
        public void HasData_WhenReferenceTypeContentIsNull_ReturnsFalse()
        {
            using OperationResult<string> result = new()
            {
                DataContent = null
            };

            Assert.False(result.HasData);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for HasData_WhenReferenceTypeContentIsSet_ReturnsTrue.
        /// </summary>
        public void HasData_WhenReferenceTypeContentIsSet_ReturnsTrue()
        {
            using OperationResult<string> result = new()
            {
                DataContent = "x"
            };

            Assert.True(result.HasData);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for HasData_WhenValueTypeIsDefault_ReturnsTrue.
        /// </summary>
        public void HasData_WhenValueTypeIsDefault_ReturnsTrue()
        {
            using OperationResult<int> result = new();

            Assert.True(result.HasData);
            Assert.Equal(0, result.DataContent);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for HasData_WhenNullableValueTypeIsSet_ReturnsTrue.
        /// </summary>
        public void HasData_WhenNullableValueTypeIsSet_ReturnsTrue()
        {
            using OperationResult<int> result = new()
            {
                DataContent = 0
            };

            Assert.True(result.HasData);
            Assert.Equal(0, result.DataContent);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Dispose_ResetsDataContentToDefault.
        /// </summary>
        public void Dispose_ResetsDataContentToDefault()
        {
            OperationResult<string> result = new("payload");

            result.Dispose();

            Assert.Null(result.DataContent);
            Assert.Null(result.Exceptions);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_WithSuccessOnlyFalse_SetsFailureAndNullMessage.
        /// </summary>
        public void Constructor_WithSuccessOnlyFalse_SetsFailureAndNullMessage()
        {
            using OperationResult<int> result = new(false);

            Assert.False(result.Success);
            Assert.Null(result.Message);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_WithNullReferenceData_SetsSuccessAndHasDataFalse.
        /// </summary>
        public void Constructor_WithNullReferenceData_SetsSuccessAndHasDataFalse()
        {
            using OperationResult<string> result = new((string?)null);

            Assert.True(result.Success);
            Assert.Null(result.DataContent);
            Assert.False(result.HasData);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_WithNullNullableValueData_SetsSuccessAndHasDataFalse.
        /// </summary>
        public void Constructor_WithNullNullableValueData_SetsSuccessAndHasDataFalse()
        {
            using OperationResult<int?> result = new((int?)null);

            Assert.True(result.Success);
            Assert.Null(result.DataContent);
            Assert.False(result.HasData);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Dispose_WhenNullableValueTypeContentIsSet_ResetsDataContentToNull.
        /// </summary>
        public void Dispose_WhenNullableValueTypeContentIsSet_ResetsDataContentToNull()
        {
            OperationResult<int?> result = new(42);

            result.Dispose();

            Assert.Null(result.DataContent);
            Assert.False(result.HasData);
        }



    }
}