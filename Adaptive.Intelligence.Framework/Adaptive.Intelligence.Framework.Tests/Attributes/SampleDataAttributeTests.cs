using Adaptive.Intelligence.Attributes;

namespace Adaptive.Intelligence.Framework.Tests.Attributes
{
    /// <summary>
    /// Gets the definition for SampleDataAttributeTests.
    /// </summary>
    public class SampleDataAttributeTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_Default_ShouldInitializeWithNullSampleData.
        /// </summary>
        public void Constructor_Default_ShouldInitializeWithNullSampleData()
        {
            var attribute = new SampleDataAttribute();
            Assert.Null(attribute.SampleData);
        }

        [Theory]
        [InlineData("Example Data")]
        [InlineData("")]
        [InlineData(null)]
        /// <summary>
        /// Gets the definition for Constructor_WithSampleDataText_ShouldSetSampleDataProperty.
        /// </summary>
        public void Constructor_WithSampleDataText_ShouldSetSampleDataProperty(string? sampleData)
        {
            var attribute = new SampleDataAttribute(sampleData);
            Assert.Equal(sampleData, attribute.SampleData);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for SampleDataProperty_ShouldReturnCorrectSampleData.
        /// </summary>
        public void SampleDataProperty_ShouldReturnCorrectSampleData()
        {
            string sampleData = "Test Data";
            var attribute = new SampleDataAttribute(sampleData);
            Assert.Equal(sampleData, attribute.SampleData);
        }
    }
}