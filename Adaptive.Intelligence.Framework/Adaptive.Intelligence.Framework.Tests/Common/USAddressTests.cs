using Adaptive.Intelligence.Common;

namespace Adaptive.Intelligence.Framework.Tests
{
    /// <summary>
    /// Tests for the <see cref="USAddress"/> class.
    /// </summary>
    public class USAddressTests
    {
        [Fact]
        /// <summary>
        /// Verifies ZipCodeIsValid returns true when ZipPlus4 contains exactly five numeric characters.
        /// </summary>
        public void ZipCodeIsValid_ZipPlus4HasFiveDigits_ReturnsTrue()
        {
            // Arrange
            using USAddress address = new()
            {
                ZipPlus4 = "12345"
            };

            // Act
            bool isValid = address.ZipCodeIsValid;

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("1234")]
        [InlineData("123456")]
        /// <summary>
        /// Verifies ZipCodeIsValid returns false when ZipPlus4 is null, empty, or not five characters long.
        /// </summary>
        /// <param name="zipPlus4">The ZipPlus4 value to test.</param>
        public void ZipCodeIsValid_ZipPlus4IsNullEmptyOrWrongLength_ReturnsFalse(string? zipPlus4)
        {
            // Arrange
            using USAddress address = new()
            {
                ZipPlus4 = zipPlus4
            };

            // Act
            bool isValid = address.ZipCodeIsValid;

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData("A2345")]
        [InlineData("1A345")]
        [InlineData("12A45")]
        [InlineData("123A5")]
        [InlineData("1234A")]
        /// <summary>
        /// Verifies ZipCodeIsValid returns false when any character in ZipPlus4 is not numeric.
        /// </summary>
        /// <param name="zipPlus4">The ZipPlus4 value to test.</param>
        public void ZipCodeIsValid_ZipPlus4ContainsNonDigit_ReturnsFalse(string zipPlus4)
        {
            // Arrange
            using USAddress address = new()
            {
                ZipPlus4 = zipPlus4
            };

            // Act
            bool isValid = address.ZipCodeIsValid;

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        /// <summary>
        /// Verifies ZipPlus4IsValid returns true when ZipPlus4 contains exactly four numeric characters.
        /// </summary>
        public void ZipPlus4IsValid_ZipPlus4HasFourDigits_ReturnsTrue()
        {
            // Arrange
            using USAddress address = new()
            {
                ZipPlus4 = "6789"
            };

            // Act
            bool isValid = address.ZipPlus4IsValid;

            // Assert
            Assert.True(isValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("123")]
        [InlineData("12345")]
        /// <summary>
        /// Verifies ZipPlus4IsValid returns false when ZipPlus4 is null, empty, or not four characters long.
        /// </summary>
        /// <param name="zipPlus4">The ZipPlus4 value to test.</param>
        public void ZipPlus4IsValid_ZipPlus4IsNullEmptyOrWrongLength_ReturnsFalse(string? zipPlus4)
        {
            // Arrange
            using USAddress address = new()
            {
                ZipPlus4 = zipPlus4
            };

            // Act
            bool isValid = address.ZipPlus4IsValid;

            // Assert
            Assert.False(isValid);
        }

        [Theory]
        [InlineData("A789")]
        [InlineData("6A89")]
        [InlineData("67A9")]
        [InlineData("678A")]
        /// <summary>
        /// Verifies ZipPlus4IsValid returns false when any character in ZipPlus4 is not numeric.
        /// </summary>
        /// <param name="zipPlus4">The ZipPlus4 value to test.</param>
        public void ZipPlus4IsValid_ZipPlus4ContainsNonDigit_ReturnsFalse(string zipPlus4)
        {
            // Arrange
            using USAddress address = new()
            {
                ZipPlus4 = zipPlus4
            };

            // Act
            bool isValid = address.ZipPlus4IsValid;

            // Assert
            Assert.False(isValid);
        }

        [Fact]
        /// <summary>
        /// Verifies ToString returns all lines and city-state-zip data in expected mailing format.
        /// </summary>
        public void ToString_AllAddressFieldsPresent_ReturnsFormattedMultiLineAddress()
        {
            // Arrange
            using USAddress address = new()
            {
                AddressLine1 = "123 Main St",
                AddressLine2 = "Suite 200",
                AddressLine3 = "Building A",
                City = "Denver",
                StateAbbreviation = "CO",
                StateName = "Colorado",
                ZipCode = "80205",
                ZipPlus4 = "1234"
            };

            string expected = "123 Main St" + Environment.NewLine +
                              "Suite 200" + Environment.NewLine +
                              "Building A" + Environment.NewLine +
                              "Denver, CO 80205-1234";

            // Act
            string value = address.ToString();

            // Assert
            Assert.Equal(expected, value);
        }

        [Fact]
        /// <summary>
        /// Verifies ToString uses state name when abbreviation is missing and does not add a dash without ZipCode.
        /// </summary>
        public void ToString_StateAbbreviationMissing_UsesStateNameAndZipPlus4WithoutDash()
        {
            // Arrange
            using USAddress address = new()
            {
                City = "Aspen",
                StateName = "Colorado",
                ZipPlus4 = "6789"
            };

            // Act
            string value = address.ToString();

            // Assert
            Assert.Equal("Aspen, Colorado 6789", value);
        }

        [Fact]
        /// <summary>
        /// Verifies ToString returns an empty string when all address components are missing.
        /// </summary>
        public void ToString_AllFieldsMissing_ReturnsEmptyString()
        {
            // Arrange
            using USAddress address = new();

            // Act
            string value = address.ToString();

            // Assert
            Assert.Equal(string.Empty, value);
        }
    }
}
