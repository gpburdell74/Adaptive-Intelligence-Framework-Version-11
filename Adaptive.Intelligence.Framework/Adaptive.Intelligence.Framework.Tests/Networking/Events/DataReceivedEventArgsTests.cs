using Adaptive.Intelligence.Networking.Events;

namespace Adaptive.Intelligence.Framework.Tests.Networking.Events
{
    /// <summary>
    /// Provides tests for the <see cref="DataReceivedEventArgs"/> class.
    /// </summary>
    public class DataReceivedEventArgsTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for DefaultConstructor_NoData_PropertiesAreNull.
        /// </summary>
        public void DefaultConstructor_NoData_PropertiesAreNull()
        {
            // Arrange

            // Act
            DataReceivedEventArgs args = new DataReceivedEventArgs();

            // Assert
            Assert.Null(args.Data);
            Assert.Null(args.DataAsString);
        }
        [Fact]
        /// <summary>
        /// Gets the definition for ConstructorWithRawData_ValidInput_CopiesDataAndReturnsAsciiString.
        /// </summary>
        public void ConstructorWithRawData_ValidInput_CopiesDataAndReturnsAsciiString()
        {
            // Arrange
            byte[] source = new byte[] { 65, 66, 67 };

            // Act
            DataReceivedEventArgs args = new DataReceivedEventArgs(source);
            source[0] = 90;

            // Assert
            Assert.NotNull(args.Data);
            Assert.Equal(new byte[] { 65, 66, 67 }, args.Data);
            Assert.Equal("ABC", args.DataAsString);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ConstructorWithSocketBuffer_PositiveLength_CopiesSpecifiedSegment.
        /// </summary>
        public void ConstructorWithSocketBuffer_PositiveLength_CopiesSpecifiedSegment()
        {
            // Arrange
            byte[] socketBuffer = new byte[] { 72, 73, 74, 75 };

            // Act
            DataReceivedEventArgs args = new DataReceivedEventArgs(socketBuffer, 3);
            socketBuffer[1] = 88;

            // Assert
            Assert.NotNull(args.Data);
            Assert.Equal(new byte[] { 72, 73, 74 }, args.Data);
            Assert.Equal("HIJ", args.DataAsString);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ConstructorWithSocketBuffer_ZeroLength_CreatesEmptyDataAndEmptyString.
        /// </summary>
        public void ConstructorWithSocketBuffer_ZeroLength_CreatesEmptyDataAndEmptyString()
        {
            // Arrange
            byte[] socketBuffer = new byte[] { 65, 66, 67 };

            // Act
            DataReceivedEventArgs args = new DataReceivedEventArgs(socketBuffer, 0);

            // Assert
            Assert.NotNull(args.Data);
            Assert.Empty(args.Data);
            Assert.Equal(string.Empty, args.DataAsString);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for DataAsUTF8String_DefaultInstance_ReturnsNull.
        /// </summary>
        public void DataAsUTF8String_DefaultInstance_ReturnsNull()
        {
            // Arrange
            DataReceivedEventArgs args = new DataReceivedEventArgs();

            // Act
            string? value = args.DataAsUTF8String;

            // Assert
            Assert.Null(value);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for DataAsUTF8String_WithBuffer_ReturnsUtf8DecodedText.
        /// </summary>
        public void DataAsUTF8String_WithBuffer_ReturnsUtf8DecodedText()
        {
            // Arrange
            string expected = "Hello π";
            byte[] data = System.Text.Encoding.UTF8.GetBytes(expected);
            DataReceivedEventArgs args = new DataReceivedEventArgs(data);

            // Act
            string? value = args.DataAsUTF8String;

            // Assert
            Assert.Equal(expected, value);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for DataAsUTF32String_DefaultInstance_ReturnsNull.
        /// </summary>
        public void DataAsUTF32String_DefaultInstance_ReturnsNull()
        {
            // Arrange
            DataReceivedEventArgs args = new DataReceivedEventArgs();

            // Act
            string? value = args.DataAsUTF32String;

            // Assert
            Assert.Null(value);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for DataAsUTF32String_WithBuffer_ReturnsUtf32DecodedText.
        /// </summary>
        public void DataAsUTF32String_WithBuffer_ReturnsUtf32DecodedText()
        {
            // Arrange
            string expected = "Hello Ω";
            byte[] data = System.Text.Encoding.UTF32.GetBytes(expected);
            DataReceivedEventArgs args = new DataReceivedEventArgs(data);

            // Act
            string? value = args.DataAsUTF32String;

            // Assert
            Assert.Equal(expected, value);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Length_DefaultInstance_ReturnsZero.
        /// </summary>
        public void Length_DefaultInstance_ReturnsZero()
        {
            // Arrange
            DataReceivedEventArgs args = new DataReceivedEventArgs();

            // Act
            int length = args.Length;

            // Assert
            Assert.Equal(0, length);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Length_WithBuffer_ReturnsBufferLength.
        /// </summary>
        public void Length_WithBuffer_ReturnsBufferLength()
        {
            // Arrange
            byte[] data = new byte[] { 10, 20, 30, 40 };
            DataReceivedEventArgs args = new DataReceivedEventArgs(data);

            // Act
            int length = args.Length;

            // Assert
            Assert.Equal(4, length);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ReceivedDate_DefaultInstance_ReturnsNull.
        /// </summary>
        public void ReceivedDate_DefaultInstance_ReturnsNull()
        {
            // Arrange
            DataReceivedEventArgs args = new DataReceivedEventArgs();

            // Act
            DateTime? receivedDate = args.ReceivedDate;

            // Assert
            Assert.Null(receivedDate);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ReceivedDate_ConstructedWithData_ReturnsValue.
        /// </summary>
        public void ReceivedDate_ConstructedWithData_ReturnsValue()
        {
            // Arrange
            DataReceivedEventArgs args = new DataReceivedEventArgs(new byte[] { 1 });

            // Act
            DateTime? receivedDate = args.ReceivedDate;

            // Assert
            Assert.True(receivedDate.HasValue);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for IsNullOrEmpty_NullArgs_ReturnsTrue.
        /// </summary>
        public void IsNullOrEmpty_NullArgs_ReturnsTrue()
        {
            // Arrange

            // Act
            bool isNullOrEmpty = DataReceivedEventArgs.IsNullOrEmpty(null);

            // Assert
            Assert.True(isNullOrEmpty);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for IsNullOrEmpty_DataIsNull_ReturnsTrue.
        /// </summary>
        public void IsNullOrEmpty_DataIsNull_ReturnsTrue()
        {
            // Arrange
            DataReceivedEventArgs args = new DataReceivedEventArgs();

            // Act
            bool isNullOrEmpty = DataReceivedEventArgs.IsNullOrEmpty(args);

            // Assert
            Assert.True(isNullOrEmpty);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for IsNullOrEmpty_DataLengthIsZero_ReturnsTrue.
        /// </summary>
        public void IsNullOrEmpty_DataLengthIsZero_ReturnsTrue()
        {
            // Arrange
            DataReceivedEventArgs args = new DataReceivedEventArgs(new byte[] { 7, 8 }, 0);

            // Act
            bool isNullOrEmpty = DataReceivedEventArgs.IsNullOrEmpty(args);

            // Assert
            Assert.True(isNullOrEmpty);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for IsNullOrEmpty_DataHasContent_ReturnsFalse.
        /// </summary>
        public void IsNullOrEmpty_DataHasContent_ReturnsFalse()
        {
            // Arrange
            DataReceivedEventArgs args = new DataReceivedEventArgs(new byte[] { 7, 8 }, 2);

            // Act
            bool isNullOrEmpty = DataReceivedEventArgs.IsNullOrEmpty(args);

            // Assert
            Assert.False(isNullOrEmpty);
        }


    }
}
