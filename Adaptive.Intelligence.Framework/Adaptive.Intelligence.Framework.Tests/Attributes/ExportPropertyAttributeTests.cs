using Adaptive.Intelligence.Attributes;
using System.Reflection;

namespace Adaptive.Intelligence.Framework.Tests.Attributes
{
    /// <summary>
    /// Gets the definition for ExportPropertyAttributeTests.
    /// </summary>
    public class ExportPropertyAttributeTests
    {
        /// <summary>
        /// Gets the definition for TestClass.
        /// </summary>
        private sealed class TestClass
        {
            [ExportProperty]
            /// <summary>
            /// Gets the definition for ExportedProperty.
            /// </summary>
            public int ExportedProperty { get; set; }

            /// <summary>
            /// Gets the definition for NonDecoratedProperty.
            /// </summary>
            public int NonDecoratedProperty { get; set; }
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ExportPropertyAttribute_ShouldBeApplicableToProperty.
        /// </summary>
        public void ExportPropertyAttribute_ShouldBeApplicableToProperty()
        {
            // Arrange
            var property = typeof(TestClass).GetProperty(nameof(TestClass.ExportedProperty));

            // Act
            var attribute = property?.GetCustomAttribute(typeof(ExportPropertyAttribute));

            // Assert
            Assert.NotNull(attribute);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for ExportPropertyAttribute_ShouldNotBeFoundOnNonDecoratedProperty.
        /// </summary>
        public void ExportPropertyAttribute_ShouldNotBeFoundOnNonDecoratedProperty()
        {
            // Arrange
            var nonDecoratedProperty = typeof(TestClass).GetProperty(nameof(TestClass.NonDecoratedProperty));
            Assert.NotNull(nonDecoratedProperty);

            // Act
            var attribute = nonDecoratedProperty.GetCustomAttribute<ExportPropertyAttribute>();

            // Assert
            Assert.Null(attribute); // Assuming there's no ExportPropertyAttribute on the non-decorated property
        }
    }
}