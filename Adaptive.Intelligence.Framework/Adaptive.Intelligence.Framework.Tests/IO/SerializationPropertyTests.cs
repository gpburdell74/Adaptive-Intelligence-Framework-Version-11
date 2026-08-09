using System;
using Adaptive.Intelligence.IO;

namespace Adaptive.Intelligence.Framework.Tests.IO
{
    /// <summary>
    /// Provides tests for the <see cref="SerializationProperty"/> class.
    /// </summary>
    public class SerializationPropertyTests
    {
        [Fact]
        public void SerializationProperty_DataContainsDefinition_ParsesValues()
        {
            // Arrange
            string data = "CustomerName:System.String";

            // Act
            using SerializationProperty property = new SerializationProperty(data);

            // Assert
            Assert.Equal("CustomerName", property.PropertyName);
            Assert.Equal("String", property.DataType);
            Assert.Equal(typeof(string), property.PropertyType);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void SerializationProperty_DataIsNullOrEmpty_LeavesPropertiesUnset(string? data)
        {
            // Act
            using SerializationProperty property = new SerializationProperty(data!);

            // Assert
            Assert.Null(property.PropertyName);
            Assert.Null(property.DataType);
            Assert.Null(property.PropertyType);
        }

        [Fact]
        public void SerializationProperty_DataWithoutDelimiter_LeavesPropertiesUnset()
        {
            // Arrange
            string data = "CustomerNameOnly";

            // Act
            using SerializationProperty property = new SerializationProperty(data);

            // Assert
            Assert.Null(property.PropertyName);
            Assert.Null(property.DataType);
            Assert.Null(property.PropertyType);
        }
    }
}
