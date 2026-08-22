using Adaptive.Intelligence.Extensions;
using System.Globalization;
using System.Text;

namespace Adaptive.Intelligence.Framework.Tests.Extensions
{
    /// <summary>
    /// Contains tests for <see cref="StringExtensions"/>.
    /// </summary>
    public class StringExtensionsTests
    {
        [Fact]
        public void CleanUpDollarText_Removes_DollarSigns()
        {
            string value = "$1,234.56 USD$";

            string result = value.CleanUpDollarText();

            Assert.Equal("1,234.56 USD", result);
        }

        [Theory]
        [InlineData("12345", false, -1)]
        [InlineData("123.45", true, -1)]
        [InlineData("123.45.67", true, 6)]
        [InlineData("123A45", false, 3)]
        public void FindFirstNonNumericCharacter_Returns_Expected_Position(string input, bool isFloatingPoint, int expected)
        {
            int result = input.FindFirstNonNumericCharacter(isFloatingPoint);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("cities", "city")]
        [InlineData("types", "type")]
        [InlineData("boxes", "box")]
        [InlineData("cars", "car")]
        [InlineData("glass", "glass")]
        public void Singularize_Returns_Expected_Result(string input, string expected)
        {
            string result = input.Singularize();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("day", "days")]
        [InlineData("city", "cities")]
        [InlineData("apple", "apples")]
        [InlineData("car", "cars")]
        public void Pluralize_Returns_Expected_Result(string input, string expected)
        {
            string result = input.Pluralize();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Properize_Converts_To_Title_Case()
        {
            string result = "hELLo woRLD".Properize();

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string expected = textInfo.ToTitleCase("hello world");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void SurroundWithQuotes_Adds_Quotes()
        {
            string result = "value".SurroundWithQuotes();

            Assert.Equal("\"value\"", result);
        }

        [Fact]
        public void ToStream_Returns_Stream_With_String_Content()
        {
            const string source = "stream-content";

            using MemoryStream stream = source.ToStream();
            using StreamReader reader = new(stream, Encoding.UTF8, true, leaveOpen: false);

            string text = reader.ReadToEnd();

            Assert.Equal(source, text);
        }
    }
}