using Adaptive.Intelligence.Constants;
using Adaptive.Intelligence.Converters;

namespace Adaptive.Intelligence.Framework.Tests.Converters
{
    /// <summary>
    /// Gets the definition for BooleanConverterTests.
    /// </summary>
    public class BooleanConverterTests
    {
        /// <summary>
        /// Gets the definition for new.
        /// </summary>
        private readonly BooleanConverter _converter = new();

        [Fact]
        /// <summary>
        /// Gets the definition for Convert_WhenTrue_ReturnsYes.
        /// </summary>
        public void Convert_WhenTrue_ReturnsYes()
        {
            var result = _converter.Convert(true);
            Assert.Equal(BooleanConstants.TrueFormatted, result);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Convert_WhenFalse_ReturnsNo.
        /// </summary>
        public void Convert_WhenFalse_ReturnsNo()
        {
            var result = _converter.Convert(false);
            Assert.Equal(BooleanConstants.FalseFormatted, result);
        }

        [Theory]
        [InlineData(BooleanConstants.TrueValueYes)]
        [InlineData(BooleanConstants.TrueValueSi)]
        [InlineData(BooleanConstants.TrueValueTrue)]
        [InlineData(BooleanConstants.TrueValueBT)]
        [InlineData(BooleanConstants.TrueValueBY)]
        [InlineData(BooleanConstants.TrueValueMinus1)]
        [InlineData(BooleanConstants.TrueValueOK)]
        /// <summary>
        /// Gets the definition for ConvertBack_WhenTrueString_ReturnsTrue.
        /// </summary>
        public void ConvertBack_WhenTrueString_ReturnsTrue(string trueString)
        {
            var result = _converter.ConvertBack(trueString);
            Assert.True(result);
        }

        [Theory]
        [InlineData("")]
        [InlineData("no")]
        [InlineData("false")]
        [InlineData("0")]
        /// <summary>
        /// Gets the definition for ConvertBack_WhenNotTrueString_ReturnsFalse.
        /// </summary>
        public void ConvertBack_WhenNotTrueString_ReturnsFalse(string notTrueString)
        {
            var result = _converter.ConvertBack(notTrueString);
            Assert.False(result);
        }
        [Fact]
        /// <summary>
        /// Gets the definition for ConvertBack_WithVariousTrueRepresentations_ReturnsTrue.
        /// </summary>
        public void ConvertBack_WithVariousTrueRepresentations_ReturnsTrue()
        {
            // Testing various representations that should be interpreted as true
            var trueRepresentations = new string[] { "Yes", "Si", "True", ".T.", ".t.", ".Y.", ".y.", "-1", "OK" };
            foreach (var representation in trueRepresentations)
            {
                var result = _converter.ConvertBack(representation);
                Assert.True(result, $"Expected true for representation: {representation}");
            }
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ConvertBack_WithVariousFalseRepresentations_ReturnsFalse.
        /// </summary>
        public void ConvertBack_WithVariousFalseRepresentations_ReturnsFalse()
        {
            // Testing various representations that should be interpreted as false
            var falseRepresentations = new string[] { "No", "0", "False", ".n.", "N", ".N.", "Neg", "-2" };
            foreach (var representation in falseRepresentations)
            {
                var result = _converter.ConvertBack(representation);
                Assert.False(result, $"Expected false for representation: {representation}");
            }
        }

        [Theory]
        [InlineData("yes", true)] // Case insensitivity test
        [InlineData("YES", true)] // Upper case test
        [InlineData("si", true)]  // Different language (Spanish) for yes
        [InlineData("no", false)] // Negative case
        [InlineData("NO", false)] // Negative case with upper case
        [InlineData("nO", false)] // Negative case with mixed case
        /// <summary>
        /// Gets the definition for ConvertBack_CaseInsensitivityAndNegatives_ReturnsExpected.
        /// </summary>
        public void ConvertBack_CaseInsensitivityAndNegatives_ReturnsExpected(string input, bool expected)
        {
            var result = _converter.ConvertBack(input);
            Assert.Equal(expected, result);
        }

    }
}