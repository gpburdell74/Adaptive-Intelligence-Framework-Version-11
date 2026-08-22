using Adaptive.Intelligence.Common;

namespace Adaptive.Intelligence.Framework.Tests
{
    /// <summary>
    /// Tests for the <see cref="USPhoneNumber"/> class.
    /// </summary>
    public class USPhoneNumberTests
    {
        [Fact]
        /// <summary>
        /// Verifies the string constructor parses a valid 10-digit number.
        /// </summary>
        public void Constructor_StringWithTenDigits_ParsesValuesAndSetsCountryCode()
        {
            // Arrange
            const string phoneValue = "5551234567";

            // Act
            using USPhoneNumber phoneNumber = new(phoneValue);

            // Assert
            Assert.Equal("1", phoneNumber.CountryCode);
            Assert.Equal("555", phoneNumber.AreaCode);
            Assert.Equal("123", phoneNumber.Prefix);
            Assert.Equal("4567", phoneNumber.Number);
            Assert.False(phoneNumber.IsNaPN);
        }

        [Fact]
        /// <summary>
        /// Verifies the string constructor keeps the value as NaPN for empty input.
        /// </summary>
        public void Constructor_EmptyString_SetsCountryCodeAndIsNaPN()
        {
            // Arrange
            const string phoneValue = "";

            // Act
            using USPhoneNumber phoneNumber = new(phoneValue);

            // Assert
            Assert.Equal("1", phoneNumber.CountryCode);
            Assert.Null(phoneNumber.AreaCode);
            Assert.Null(phoneNumber.Prefix);
            Assert.Null(phoneNumber.Number);
            Assert.True(phoneNumber.IsNaPN);
        }

        [Fact]
        /// <summary>
        /// Verifies the copy constructor copies all number components.
        /// </summary>
        public void Constructor_CopyConstructor_CopiesAreaPrefixAndNumber()
        {
            // Arrange
            using USPhoneNumber source = new("2125559876");

            // Act
            using USPhoneNumber copy = new(source);

            // Assert
            Assert.Equal("1", copy.CountryCode);
            Assert.Equal(source.AreaCode, copy.AreaCode);
            Assert.Equal(source.Prefix, copy.Prefix);
            Assert.Equal(source.Number, copy.Number);
            Assert.False(copy.IsNaPN);
        }

        [Fact]
        /// <summary>
        /// Verifies setting a valid area code stores the value.
        /// </summary>
        public void AreaCode_SetValidNumericThreeDigits_AssignsValue()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new();

            // Act
            phoneNumber.AreaCode = "404";

            // Assert
            Assert.Equal("404", phoneNumber.AreaCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("12")]
        [InlineData("1234")]
        [InlineData("A12")]
        /// <summary>
        /// Verifies invalid area code values clear the area code.
        /// </summary>
        /// <param name="areaCodeValue">The area code value.</param>
        public void AreaCode_SetInvalidValue_ClearsValue(string? areaCodeValue)
        {
            // Arrange
            using USPhoneNumber phoneNumber = new();
            phoneNumber.AreaCode = "303";

            // Act
            phoneNumber.AreaCode = areaCodeValue;

            // Assert
            Assert.Null(phoneNumber.AreaCode);
        }

        [Fact]
        /// <summary>
        /// Verifies IsNaPN is true when prefix is missing.
        /// </summary>
        public void IsNaPN_PrefixMissing_ReturnsTrue()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new();
            phoneNumber.Prefix = null;
            phoneNumber.Number = "1234";

            // Act
            bool isNaPN = phoneNumber.IsNaPN;

            // Assert
            Assert.True(isNaPN);
        }

        [Fact]
        /// <summary>
        /// Verifies IsNaPN is true when number is missing.
        /// </summary>
        public void IsNaPN_NumberMissing_ReturnsTrue()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new();
            phoneNumber.Prefix = "555";
            phoneNumber.Number = null;

            // Act
            bool isNaPN = phoneNumber.IsNaPN;

            // Assert
            Assert.True(isNaPN);
        }

        [Fact]
        /// <summary>
        /// Verifies IsNaPN is false when both prefix and number are present.
        /// </summary>
        public void IsNaPN_PrefixAndNumberPresent_ReturnsFalse()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new();
            phoneNumber.Prefix = "555";
            phoneNumber.Number = "1234";

            // Act
            bool isNaPN = phoneNumber.IsNaPN;

            // Assert
            Assert.False(isNaPN);
        }

        [Fact]
        /// <summary>
        /// Verifies CountryCode always returns the US country code.
        /// </summary>
        public void CountryCode_DefaultConstructor_ReturnsOne()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new();

            // Act
            string? countryCode = phoneNumber.CountryCode;

            // Assert
            Assert.Equal("1", countryCode);
        }

        [Fact]
        /// <summary>
        /// Verifies NaPN returns a non-null not-a-phone-number instance.
        /// </summary>
        public void NaPN_Get_ReturnsNotAPhoneNumberInstance()
        {
            // Act
            using USPhoneNumber napn = USPhoneNumber.NaPN;

            // Assert
            Assert.NotNull(napn);
            Assert.True(napn.IsNaPN);
            Assert.Equal("1", napn.CountryCode);
            Assert.Null(napn.Prefix);
            Assert.Null(napn.Number);
        }


        [Fact]
        /// <summary>
        /// Verifies NaPN returns a new instance each time.
        /// </summary>
        public void NaPN_GetMultipleTimes_ReturnsDifferentInstances()
        {
            // Act
            using USPhoneNumber first = USPhoneNumber.NaPN;
            using USPhoneNumber second = USPhoneNumber.NaPN;

            // Assert
            Assert.NotSame(first, second);
            Assert.True(first.Equals(second));
        }

        [Fact]
        /// <summary>
        /// Verifies setting a valid number stores the value.
        /// </summary>
        public void Number_SetValidNumericFourDigits_AssignsValue()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new();

            // Act
            phoneNumber.Number = "4567";

            // Assert
            Assert.Equal("4567", phoneNumber.Number);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("123")]
        [InlineData("12345")]
        [InlineData("12A4")]
        /// <summary>
        /// Verifies invalid number values clear the number.
        /// </summary>
        /// <param name="numberValue">The number value.</param>
        public void Number_SetInvalidValue_ClearsValue(string? numberValue)
        {
            // Arrange
            using USPhoneNumber phoneNumber = new();
            phoneNumber.Number = "9876";

            // Act
            phoneNumber.Number = numberValue;

            // Assert
            Assert.Null(phoneNumber.Number);
        }

        [Fact]
        /// <summary>
        /// Verifies setting a valid prefix stores the value.
        /// </summary>
        public void Prefix_SetValidNumericThreeDigits_AssignsValue()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new();

            // Act
            phoneNumber.Prefix = "212";

            // Assert
            Assert.Equal("212", phoneNumber.Prefix);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("12")]
        [InlineData("1234")]
        [InlineData("A12")]
        /// <summary>
        /// Verifies invalid prefix values clear the prefix.
        /// </summary>
        /// <param name="prefixValue">The prefix value.</param>
        public void Prefix_SetInvalidValue_ClearsValue(string? prefixValue)
        {
            // Arrange
            using USPhoneNumber phoneNumber = new();
            phoneNumber.Prefix = "777";

            // Act
            phoneNumber.Prefix = prefixValue;

            // Assert
            Assert.Null(phoneNumber.Prefix);
        }

        [Fact]
        /// <summary>
        /// Verifies Clone creates a separate copy with the same values.
        /// </summary>
        public void Clone_PublicMethod_ReturnsDeepCopy()
        {
            // Arrange
            using USPhoneNumber source = new();
            source.AreaCode = "303";
            source.Prefix = "555";
            source.Number = "1234";

            // Act
            using USPhoneNumber clone = source.Clone();

            // Assert
            Assert.NotSame(source, clone);
            Assert.Equal(source.CountryCode, clone.CountryCode);
            Assert.Equal(source.AreaCode, clone.AreaCode);
            Assert.Equal(source.Prefix, clone.Prefix);
            Assert.Equal(source.Number, clone.Number);
        }

        [Fact]
        /// <summary>
        /// Verifies ICloneable.Clone delegates to the strongly typed Clone method.
        /// </summary>
        public void Clone_ExplicitInterface_ReturnsEquivalentUSPhoneNumberCopy()
        {
            // Arrange
            using USPhoneNumber source = new();
            source.AreaCode = "404";
            source.Prefix = "222";
            source.Number = "9999";
            ICloneable cloneable = source;

            // Act
            object cloneObject = cloneable.Clone();

            // Assert
            USPhoneNumber? clone = Assert.IsType<USPhoneNumber>(cloneObject);
            Assert.NotSame(source, clone);
            Assert.Equal(source.CountryCode, clone.CountryCode);
            Assert.Equal(source.AreaCode, clone.AreaCode);
            Assert.Equal(source.Prefix, clone.Prefix);
            Assert.Equal(source.Number, clone.Number);
            clone.Dispose();
        }


        [Fact]
        /// <summary>
        /// Verifies GetHashCode uses country code, area code, prefix, and number values.
        /// </summary>
        public void GetHashCode_AllValuesPresent_ReturnsHashCodeForConcatenatedValue()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new("2125557890");
            int expected = "12125557890".GetHashCode(StringComparison.Ordinal);

            // Act
            int hashCode = phoneNumber.GetHashCode();

            // Assert
            Assert.Equal(expected, hashCode);
        }

        [Fact]
        /// <summary>
        /// Verifies GetHashCode includes only non-null values.
        /// </summary>
        public void GetHashCode_AreaCodeOnly_ReturnsHashCodeForCountryAndAreaCode()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new();
            phoneNumber.AreaCode = "212";
            int expected = "1212".GetHashCode(StringComparison.Ordinal);

            // Act
            int hashCode = phoneNumber.GetHashCode();

            // Assert
            Assert.Equal(expected, hashCode);
        }

        [Fact]
        /// <summary>
        /// Verifies Equals returns false when comparing to null.
        /// </summary>
        public void Equals_TypedOverloadWithNull_ReturnsFalse()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new("2125557890");

            // Act
            USPhoneNumber? other = CreateNullablePhoneNumber();
            bool isEqual = phoneNumber.Equals(other);

            other?.Dispose();

            // Assert
            Assert.False(isEqual);
        }

        [Fact]
        /// <summary>
        /// Verifies Equals returns true when both values are NaPN.
        /// </summary>
        public void Equals_TypedOverloadBothNaPN_ReturnsTrue()
        {
            // Arrange
            using USPhoneNumber left = new();
            left.AreaCode = "212";
            using USPhoneNumber right = new();

            // Act
            bool isEqual = left.Equals(right);

            // Assert
            Assert.True(isEqual);
        }

        [Fact]
        /// <summary>
        /// Verifies Equals returns true when all components are equal.
        /// </summary>
        public void Equals_TypedOverloadMatchingValues_ReturnsTrue()
        {
            // Arrange
            using USPhoneNumber left = new("2125557890");
            using USPhoneNumber right = new("2125557890");

            // Act
            bool isEqual = left.Equals(right);

            // Assert
            Assert.True(isEqual);
        }

        [Fact]
        /// <summary>
        /// Verifies Equals returns false when components differ.
        /// </summary>
        public void Equals_TypedOverloadDifferentValues_ReturnsFalse()
        {
            // Arrange
            using USPhoneNumber left = new("2125557890");
            using USPhoneNumber right = new("3035557890");

            // Act
            bool isEqual = left.Equals(right);

            // Assert
            Assert.False(isEqual);
        }

        [Fact]
        /// <summary>
        /// Verifies object Equals returns false for non-USPhoneNumber values.
        /// </summary>
        public void Equals_ObjectOverloadWithDifferentType_ReturnsFalse()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new("2125557890");

            // Act
            bool isEqual = phoneNumber.Equals("2125557890");

            // Assert
            Assert.False(isEqual);
        }

        [Fact]
        /// <summary>
        /// Verifies object Equals returns true for equivalent USPhoneNumber values.
        /// </summary>
        public void Equals_ObjectOverloadWithUSPhoneNumber_ReturnsTypedEqualsResult()
        {
            // Arrange
            using USPhoneNumber left = new("2125557890");
            object right = new USPhoneNumber("2125557890");

            // Act
            bool isEqual = left.Equals(right);

            // Assert
            Assert.True(isEqual);
            ((USPhoneNumber)right).Dispose();
        }

        [Fact]
        /// <summary>
        /// Verifies ToString returns empty when the value is NaPN.
        /// </summary>
        public void ToString_OverrideForNaPN_ReturnsEmptyString()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new();

            // Act
            string result = phoneNumber.ToString();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        /// <summary>
        /// Verifies ToString omits a missing area code.
        /// </summary>
        public void ToString_OverrideWithNoAreaCode_ReturnsPrefixAndNumber()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new();
            phoneNumber.Prefix = "555";
            phoneNumber.Number = "7890";

            // Act
            string result = phoneNumber.ToString();

            // Assert
            Assert.Equal("5557890", result);
        }

        [Fact]
        /// <summary>
        /// Verifies ToString concatenates area code, prefix, and number.
        /// </summary>
        public void ToString_OverrideWithAllComponents_ReturnsUnformattedDigits()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new("2125557890");

            // Act
            string result = phoneNumber.ToString();

            // Assert
            Assert.Equal("2125557890", result);
        }

        [Fact]
        /// <summary>
        /// Verifies ToString with formatting options returns empty for NaPN values.
        /// </summary>
        public void ToString_WithFormattingForNaPN_ReturnsEmptyString()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new();

            // Act
            string result = phoneNumber.ToString(formatted: true, withCountryCode: true);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        /// <summary>
        /// Verifies ToString returns unformatted output with country code when requested.
        /// </summary>
        public void ToString_UnformattedWithCountryCode_ReturnsCountryCodeAndDigits()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new("2125557890");

            // Act
            string result = phoneNumber.ToString(formatted: false, withCountryCode: true);

            // Assert
            Assert.Equal("12125557890", result);
        }

        [Fact]
        /// <summary>
        /// Verifies ToString returns unformatted output without country code.
        /// </summary>
        public void ToString_UnformattedWithoutCountryCode_ReturnsDigits()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new("2125557890");

            // Act
            string result = phoneNumber.ToString(formatted: false, withCountryCode: false);

            // Assert
            Assert.Equal("2125557890", result);
        }

        [Fact]
        /// <summary>
        /// Verifies ToString returns formatted output with country code.
        /// </summary>
        public void ToString_FormattedWithCountryCode_ReturnsFormattedPhoneNumber()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new("2125557890");

            // Act
            string result = phoneNumber.ToString(formatted: true, withCountryCode: true);

            // Assert
            Assert.Equal("+1 (212) 555 - 7890", result);
        }

        [Fact]
        /// <summary>
        /// Verifies ToString trims and omits missing components in formatted output.
        /// </summary>
        public void ToString_FormattedWithoutCountryCodeAndAreaCode_ReturnsTrimmedFormattedValue()
        {
            // Arrange
            using USPhoneNumber phoneNumber = new();
            phoneNumber.Prefix = "555";
            phoneNumber.Number = "7890";

            // Act
            string result = phoneNumber.ToString(formatted: true, withCountryCode: false);

            // Assert
            Assert.Equal("555 - 7890", result);
        }


        /// <summary>
        /// Creates a nullable phone number for null-branch equality testing.
        /// </summary>
        /// <returns>
        /// A <see cref="USPhoneNumber"/> instance or <b>null</b>.
        /// </returns>
        private static USPhoneNumber? CreateNullablePhoneNumber()
        {
            if (Environment.TickCount == int.MinValue)
            {
                return new USPhoneNumber("2125557890");
            }

            return null;
        }


    }
}
