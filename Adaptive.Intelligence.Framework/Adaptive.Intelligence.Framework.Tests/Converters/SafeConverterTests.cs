using Adaptive.Intelligence.Converters;

namespace Adaptive.Intelligence.Framework.Tests.Converters
{
    /// <summary>
    /// Gets the definition for SafeConverterTests.
    /// </summary>
    public class SafeConverterTests
    {
        [Theory]
        [InlineData("path", "path\\")]
        [InlineData("path\\", "path\\")]
        /// <summary>
        /// Gets the definition for AddBackslash_ShouldAddBackslashWhenNeeded.
        /// </summary>
        public void AddBackslash_ShouldAddBackslashWhenNeeded(string input, string expected)
        {
            var result = SafeConverter.AddBackslash(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for HexToBytes_ToHex_ShouldRoundTrip.
        /// </summary>
        public void HexToBytes_ToHex_ShouldRoundTrip()
        {
            string hexString = "4A6F686E";
            var bytes = SafeConverter.HexToBytes(hexString);
            var resultHexString = SafeConverter.ToHex(bytes);
            Assert.Equal(hexString, resultHexString);
        }

        [Theory]
        [InlineData("123", true)]
        [InlineData("abc", false)]
        [InlineData("123abc", false)]
        /// <summary>
        /// Gets the definition for IsNumeric_ShouldIdentifyNumericStrings.
        /// </summary>
        public void IsNumeric_ShouldIdentifyNumericStrings(string input, bool expected)
        {
            var result = SafeConverter.IsNumeric(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("100", 100)]
        [InlineData("invalid", 0)]
        /// <summary>
        /// Gets the definition for ToInt32_ShouldConvertSafely.
        /// </summary>
        public void ToInt32_ShouldConvertSafely(string input, int expected)
        {
            var result = SafeConverter.ToInt32(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for DecimalToArrray_DecimalFromArrray_ShouldRoundTrip.
        /// </summary>
        public void DecimalToArrray_DecimalFromArrray_ShouldRoundTrip()
        {
            decimal originalValue = 123.456m;
            var byteArray = SafeConverter.DecimalToArray(originalValue);
            var resultValue = SafeConverter.DecimalFromArray(byteArray);
            Assert.Equal(originalValue, resultValue);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for SafeToSingleTest.
        /// </summary>
        public void SafeToSingleTest()
        {
            float cf = SafeConverter.SafeToSingle("1.2322");
            Assert.Equal(1.2322f, cf);
        }
        [Fact]
        /// <summary>
        /// Gets the definition for SafeToSingleEmptyTest.
        /// </summary>
        public void SafeToSingleEmptyTest()
        {
            float cf = SafeConverter.SafeToSingle(string.Empty);
            Assert.Equal(0f, cf);
        }
        [Fact]
        /// <summary>
        /// Gets the definition for SafeToSingleNullTest.
        /// </summary>
        public void SafeToSingleNullTest()
        {
            float cf = SafeConverter.SafeToSingle(null);
            Assert.Equal(0f, cf);
        }
    }
}