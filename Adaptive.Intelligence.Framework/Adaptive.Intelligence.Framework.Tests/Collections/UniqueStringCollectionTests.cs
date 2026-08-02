using Adaptive.Intelligence.Collections;

namespace Adaptive.Intelligence.Framework.Tests.Collections
{
    /// <summary>
    /// Gets the definition for UniqueStringCollectionTests.
    /// </summary>
    public class UniqueStringCollectionTests
    {

        [Fact]
        /// <summary>
        /// Gets the definition for ConstructorTest.
        /// </summary>
        public void ConstructorTest()
        {
            UniqueStringCollection? list = [];
            Assert.NotNull(list);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Add_NewValue_AddsToCollection.
        /// </summary>
        public void Add_NewValue_AddsToCollection()
        {
            // Arrange
            var collection = new UniqueStringCollection();
            var newValue = "123";

            // Act
            var result = collection.Add(newValue);

            // Assert
            Assert.Equal(0, result);
            Assert.Contains(newValue, collection);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Add_DuplicateValue_DoesNotAddToCollection.
        /// </summary>
        public void Add_DuplicateValue_DoesNotAddToCollection()
        {
            // Arrange
            var collection = new UniqueStringCollection();
            var newValue = "123";
            collection.Add(newValue);

            // Act
            var result = collection.Add(newValue);

            // Assert
            Assert.Equal(-1, result);
            Assert.Single(collection);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for AddRange_NewValues_AddsToCollection.
        /// </summary>
        public void AddRange_NewValues_AddsToCollection()
        {
            // Arrange
            var collection = new UniqueStringCollection();
            var newValues = new List<string> { "123", "456", "789", "101", "112" };

            // Act
            collection.AddRange(newValues);

            // Assert
            foreach (var value in newValues)
            {
                Assert.Contains(value, collection);
            }
            Assert.Equal(5, collection.Count);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for AddRange_DuplicateValues_DoesNotAddDuplicates.
        /// </summary>
        public void AddRange_DuplicateValues_DoesNotAddDuplicates()
        {
            // Arrange
            var collection = new UniqueStringCollection();
            var newValues = new List<string> { "123", "456", "789", "101", "112" };
            collection.AddRange(newValues);

            // Act
            collection.AddRange(newValues);

            // Assert
            foreach (var value in newValues)
            {
                Assert.Contains(value, collection);
            }
            Assert.Equal(5, collection.Count);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Add_NullValue_DoesNotAddToCollection.
        /// </summary>
        public void Add_NullValue_DoesNotAddToCollection()
        {
            // Arrange
            var collection = new UniqueStringCollection();

            // Act
            var result = collection.Add(null);

            // Assert
            Assert.Equal(-1, result);
            Assert.Empty(collection);
        }
    }
}