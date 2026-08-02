using Adaptive.Intelligence.Attributes;
using System.Reflection;

namespace Adaptive.Intelligence.Framework.Tests.Attributes
{
    /// <summary>
    /// Gets the definition for ImportIgnoreAttributeTests.
    /// </summary>
    public class ImportIgnoreAttributeTests
    {
        /// <summary>
        /// Gets the definition for TestClass.
        /// </summary>
        private sealed class TestClass
        {
            [ImportIgnore]
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
        /// Gets the definition for ImportIgnoreAttribute_ShouldBeApplicableToProperty.
        /// </summary>
        public void ImportIgnoreAttribute_ShouldBeApplicableToProperty()
        {
            // Arrange
            var property = typeof(TestClass).GetProperty(nameof(TestClass.IgnoredProperty));

            // Act
            Assert.NotNull(property); // Ensure the property exists);
            var attribute = property.GetCustomAttribute<ImportIgnoreAttribute>();

            // Assert
            Assert.NotNull(attribute);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ImportIgnoreAttribute_ShouldNotBeFoundOnNonDecoratedProperty.
        /// </summary>
        public void ImportIgnoreAttribute_ShouldNotBeFoundOnNonDecoratedProperty()
        {
            // Arrange
            var property = typeof(TestClass).GetProperty(nameof(TestClass.NotDecoratedProperty));
            Assert.NotNull(property);

            // Adding a non-decorated property for comparison
            var nonDecoratedProperty = typeof(TestClass).GetProperty(nameof(TestClass.NotDecoratedProperty));

            // Act
            Assert.NotNull(nonDecoratedProperty); // Ensure the property exists
            var attribute = nonDecoratedProperty.GetCustomAttribute<ImportIgnoreAttribute>();

            // Assert
            Assert.Null(attribute);
        }
    }
}