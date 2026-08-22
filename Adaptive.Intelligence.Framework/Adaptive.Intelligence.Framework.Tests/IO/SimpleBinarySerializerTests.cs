using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Adaptive.Intelligence.IO;

namespace Adaptive.Intelligence.Framework.Tests.IO
{
    /// <summary>
    /// Provides tests for the <see cref="SimpleBinarySerializer"/> class.
    /// </summary>
    public class SimpleBinarySerializerTests
    {
        [Fact]
        public void Serialize_ValidInstance_ReturnsByteArray()
        {
            // Arrange
            const string instance = "Widget";

            // Act
            byte[]? result = SimpleBinarySerializer.Serialize(instance);

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Serialize_WriteXmlThrows_ReturnsNull()
        {
            // Arrange
            ThrowingXmlSerializableModel instance = new ThrowingXmlSerializableModel();

            // Act
            byte[]? result = SimpleBinarySerializer.Serialize(instance);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Deserialize_SerializedDataIsNull_ReturnsDefault()
        {
            // Arrange
            byte[]? serializedData = null;

            // Act
            string? result = SimpleBinarySerializer.Deserialize<string>(serializedData);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Deserialize_SerializedDataIsEmpty_ReturnsDefault()
        {
            // Arrange
            byte[] serializedData = Array.Empty<byte>();

            // Act
            string? result = SimpleBinarySerializer.Deserialize<string>(serializedData);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Deserialize_ValidSerializedData_ReturnsInstance()
        {
            // Arrange
            const string value = "RoundTripValue";
            byte[] serializedData = SimpleBinarySerializer.Serialize(value)!;

            // Act
            string? result = SimpleBinarySerializer.Deserialize<string>(serializedData);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void Deserialize_InvalidSerializedData_ReturnsDefault()
        {
            // Arrange
            byte[] serializedData = [0x01, 0x02, 0x03, 0x04];

            // Act
            string? result = SimpleBinarySerializer.Deserialize<string>(serializedData);

            // Assert
            Assert.Null(result);
        }

        [SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "XmlSerializer requires a public type.")]
        public sealed class ThrowingXmlSerializableModel : IXmlSerializable
        {
            public XmlSchema? GetSchema()
            {
                return null;
            }

            public void ReadXml(XmlReader reader)
            {
                _ = reader;
            }

            public void WriteXml(XmlWriter writer)
            {
                _ = writer;
                throw new InvalidOperationException("Serialization failed.");
            }
        }
    }
}
