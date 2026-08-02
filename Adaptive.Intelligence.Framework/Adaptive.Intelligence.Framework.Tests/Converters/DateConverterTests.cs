using Adaptive.Intelligence.Constants;
using Adaptive.Intelligence.Converters;

namespace Adaptive.Intelligence.Framework.Tests.Converters
{
    /// <summary>
    /// Gets the definition for DateConverterTests.
    /// </summary>
    public class DateConverterTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for Convert_EmptyString_ReturnsDefaultDate.
        /// </summary>
        public void Convert_EmptyString_ReturnsDefaultDate()
        {
            var converter = new DateConverter();
            var result = converter.Convert(string.Empty);
            Assert.Equal(new DateTime(1900, 1, 1), result);
        }

        [Theory]
        [InlineData("2023/04/01", 2023, 4, 1)]
        [InlineData("2023-04-01", 2023, 4, 1)]
        [InlineData("2023.04.01", 2023, 4, 1)]
        /// <summary>
        /// Gets the definition for Convert_ValidDateString_ReturnsCorrectDate.
        /// </summary>
        public void Convert_ValidDateString_ReturnsCorrectDate(string dateString, int year, int month, int day)
        {
            DateConverter converter = new DateConverter();
            DateTime expectedDate = new DateTime(year, month, day);
            DateTime result = converter.Convert(dateString);
            Assert.Equal(expectedDate, result);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Convert_InvalidDateString_ReturnsDefaultDate.
        /// </summary>
        public void Convert_InvalidDateString_ReturnsDefaultDate()
        {
            var converter = new DateConverter();
            var result = converter.Convert("InvalidDate");
            Assert.Equal(new DateTime(1900, 1, 1), result);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ConvertBack_DateTime_ReturnsUSFormattedString.
        /// </summary>
        public void ConvertBack_DateTime_ReturnsUSFormattedString()
        {
            var converter = new DateConverter();
            var date = new DateTime(2023, 4, 1);
            var result = converter.ConvertBack(date);
            Assert.Equal("04/01/2023", result); // Assuming Constants.USDateFormat is "MM/dd/yyyy"
        }

        [Theory]
        [InlineData("01-01-2026", 2026, 1, 1)]
        [InlineData("05-02-2025", 2025, 5, 2)]
        [InlineData("06-30-2024", 2024, 6, 30)]
        [InlineData("12-31-2020", 2020, 12, 31)]
        [InlineData("7-12-1974", 1974, 7, 12)]
        [InlineData("11-13-1962", 1962, 11, 13)]
        /// <summary>
        /// Gets the definition for Parse_With_Dashes_Happy_Path.
        /// </summary>
        public void Parse_With_Dashes_Happy_Path(string dateString, int year, int month, int day)
        {
            DateTime expected = new DateTime(year, month, day);
            var converter = new DateConverter();
            DateTime actual = converter.Convert(dateString);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData("/")]
        [InlineData("-")]
        [InlineData("--")]
        [InlineData("/-")]
        [InlineData("-/")]
        [InlineData("-,")]
        [InlineData("asdjahsdkjahsd-asdasd-asdasd")]
        [InlineData("12-32-1963")]
        [InlineData("00-99-1963")]
        /// <summary>
        /// Gets the definition for Parse_With_Dashes_Invalid_Input.
        /// </summary>
        public void Parse_With_Dashes_Invalid_Input(string text)
        {
            var converter = new DateConverter();
            var result = converter.Convert(text);

            Assert.Equal(new DateTime(1900, 1, 1), result);
        }
    }
}