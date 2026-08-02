using Adaptive.Intelligence.Common;

namespace Adaptive.Intelligence.Framework.Tests.Common
{
    /// <summary>
    /// Gets the definition for NameValuePairTests.
    /// </summary>
    public class NameValuePairTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for TestConstructorAndProperties.
        /// </summary>
        public void TestConstructorAndProperties()
        {
            NameValuePair<int> item = new NameValuePair<int>();
            Assert.Null(item.Name);
            Assert.Equal(0, item.Value);

            item = new NameValuePair<int>("Test", 1);
            Assert.Equal("Test", item.Name);
            Assert.Equal(1, item.Value);

        }
        [Fact]
        /// <summary>
        /// Gets the definition for DisposeTest.
        /// </summary>
        public void DisposeTest()
        {
            NameValuePair<int> item = new NameValuePair<int>();
            item.Dispose();
            Assert.Null(item.Name);

            item.Dispose();
            item.Dispose();
            item.Dispose();

            item = new NameValuePair<int>("Test", 1);
            Assert.Equal("Test", item.Name);
            Assert.Equal(1, item.Value);

            item.Dispose();
            Assert.Null(item.Name);
            item.Dispose();
            item.Dispose();
        }
        [Fact]
        /// <summary>
        /// Gets the definition for PropertyTests.
        /// </summary>
        public void PropertyTests()
        {
            NameValuePair<int> item = new NameValuePair<int>();

            item.Name = "Test";
            item.Value = 3;
            Assert.Equal("Test", item.Name);
            Assert.Equal(3, item.Value);

            item.Name = null;
            item.Value = 32;
            Assert.Null(item.Name);
            Assert.Equal(32, item.Value);

            item.Dispose();
        }
        [Fact]
        public void NameValuePair_DefaultConstructorWithStringType_ValueAndNameAreNull()
        {
            // Arrange / Act
            NameValuePair<string> item = new NameValuePair<string>();

            // Assert
            Assert.Null(item.Name);
            Assert.Null(item.Value);
        }
        [Fact]
        public void NameValuePair_ConstructorWithParameters_SetsNameAndValue()
        {
            // Arrange
            const string expectedName = "Alpha";
            const int expectedValue = 42;

            // Act
            NameValuePair<int> item = new NameValuePair<int>(expectedName, expectedValue);

            // Assert
            Assert.Equal(expectedName, item.Name);
            Assert.Equal(expectedValue, item.Value);
        }

        [Fact]
        public void Name_SetterUpdatesName_GetterReturnsAssignedValue()
        {
            // Arrange
            NameValuePair<int> item = new NameValuePair<int>();

            // Act
            item.Name = "Updated";

            // Assert
            Assert.Equal("Updated", item.Name);
        }

        [Fact]
        public void Value_SetterUpdatesValue_GetterReturnsAssignedValue()
        {
            // Arrange
            NameValuePair<string> item = new NameValuePair<string>();

            // Act
            item.Value = "Payload";

            // Assert
            Assert.Equal("Payload", item.Value);
        }

        [Fact]
        public void Dispose_AfterSettingNameAndValue_ResetsNameAndValueToDefault()
        {
            // Arrange
            NameValuePair<int> item = new NameValuePair<int>("BeforeDispose", 100);

            // Act
            item.Dispose();

            // Assert
            Assert.Null(item.Name);
            Assert.Equal(default, item.Value);
        }


    }
}