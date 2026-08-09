using Adaptive.Intelligence.States;
using Xunit;

namespace Adaptive.Intelligence.Framework.Tests.States
{
    public class USStateTests
    {
        [Fact]
        public void DisplayName_AbbreviationAndNameSet_ReturnsCombinedDisplayName()
        {
            // Arrange
            using USState state = new USState
            {
                Abbreviation = "CA",
                Name = "California"
            };

            // Act
            string displayName = state.DisplayName;

            // Assert
            Assert.Equal("CA - California", displayName);
        }

        [Fact]
        public void DisplayName_OnlyNameSet_ReturnsName()
        {
            // Arrange
            using USState state = new USState
            {
                Abbreviation = null,
                Name = "California"
            };

            // Act
            string displayName = state.DisplayName;

            // Assert
            Assert.Equal("California", displayName);
        }

        [Fact]
        public void DisplayName_OnlyAbbreviationSet_ReturnsAbbreviation()
        {
            // Arrange
            using USState state = new USState
            {
                Abbreviation = "CA",
                Name = null
            };

            // Act
            string displayName = state.DisplayName;

            // Assert
            Assert.Equal("CA", displayName);
        }

        [Fact]
        public void DisplayName_AbbreviationAndNameMissing_ReturnsEmptyString()
        {
            // Arrange
            using USState state = new USState
            {
                Abbreviation = string.Empty,
                Name = null
            };

            // Act
            string displayName = state.DisplayName;

            // Assert
            Assert.Equal(string.Empty, displayName);
        }
    }
}
