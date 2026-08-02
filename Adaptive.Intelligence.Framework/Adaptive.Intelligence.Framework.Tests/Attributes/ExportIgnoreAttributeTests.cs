using Adaptive.Intelligence.Attributes;
using System.Reflection;

namespace Adaptive.Intelligence.Framework.Tests.Attributes
{
    /// <summary>
    /// Gets the definition for ExportIgnoreAttributeTests.
    /// </summary>
    public class ExportIgnoreAttributeTests
    {
        /// <summary>
        /// Gets the definition for TestClass.
        /// </summary>
        private sealed class TestClass
        {
            [ExportIgnore]
            /// <summary>
            /// Gets the definition for IgnoredProperty.
            /// </summary>
            public int IgnoredProperty { get; set; }

            /// <summary>
            /// Gets the definition for NotDecoratedProperty.
            /// </summary>
            public int NotDecoratedProperty { get; set; }
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ExportIgnoreAttribute_ShouldBeApplicableToProperty.
        /// </summary>
        public void ExportIgnoreAttribute_ShouldBeApplicableToProperty()
        {
            // Arrange
            var property = typeof(TestClass).GetProperty(nameof(TestClass.IgnoredProperty));

            // Act
            var attribute = property?.GetCustomAttribute(typeof(ExportIgnoreAttribute));

            // Assert
            Assert.NotNull(attribute);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ExportIgnoreAttribute_ShouldNotBeFoundOnNonDecoratedProperty.
        /// </summary>
        public void ExportIgnoreAttribute_ShouldNotBeFoundOnNonDecoratedProperty()
        {
            // Arrange
            var nonDecoratedProperty = typeof(TestClass).GetProperty(nameof(TestClass.NotDecoratedProperty));

            // Act
            var attribute = nonDecoratedProperty?.GetCustomAttribute(typeof(ExportIgnoreAttribute));

            // Assert
            Assert.Null(attribute); // Assuming there's no ExportIgnoreAttribute on the non-decorated property
        }
    }
}