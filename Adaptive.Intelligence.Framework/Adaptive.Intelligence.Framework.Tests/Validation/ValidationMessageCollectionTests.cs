using Adaptive.Intelligence.Validation;

namespace Adaptive.Intelligence.Framework.Tests.Validation
{
    /// <summary>
    /// Gets the definition for ValidationMessageCollectionTests.
    /// </summary>
    public class ValidationMessageCollectionTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_ShouldInitializeEmptyCollection.
        /// </summary>
        public void Constructor_ShouldInitializeEmptyCollection()
        {
            // Arrange & Act
            var collection = new ValidationMessageCollection();
            // Assert
            if (collection.Count != 0)
            {
                throw new Exception("Collection should be initialized as empty.");
            }
        }

        [Fact]
        /// <summary>
        /// Gets the definition for AreAllValid_ShouldReturnTrueForEmptyCollection.
        /// </summary>
        public void AreAllValid_ShouldReturnTrueForEmptyCollection()
        {
            // Arrange
            var collection = new ValidationMessageCollection();
            // Act
            bool result = collection.AreAllValid();
            // Assert
            if (!result)
            {
                throw new Exception("AreAllValid should return true for an empty collection.");
            }
        }

        [Fact]
        /// <summary>
        /// Gets the definition for AreAllValid_ShouldReturnTrueWhenAllMessagesAreValid.
        /// </summary>
        public void AreAllValid_ShouldReturnTrueWhenAllMessagesAreValid()
        {
            // Arrange
            var collection = new ValidationMessageCollection();
            collection.Add(new ValidationMessage { IsValid = true });
            collection.Add(new ValidationMessage { IsValid = true });
            // Act
            bool result = collection.AreAllValid();
            // Assert
            if (!result)
            {
                throw new Exception("AreAllValid should return true when all messages are valid.");
            }
        }

        [Fact]
        /// <summary>
        /// Gets the definition for AreAllValid_ShouldReturnFalseWhenAnyMessageIsInvalid.
        /// </summary>
        public void AreAllValid_ShouldReturnFalseWhenAnyMessageIsInvalid()
        {
            // Arrange
            var collection = new ValidationMessageCollection();
            collection.Add(new ValidationMessage { IsValid = true });
            collection.Add(new ValidationMessage { IsValid = false });
            // Act
            bool result = collection.AreAllValid();
            // Assert
            if (result)
            {
                throw new Exception("AreAllValid should return false when any message is invalid.");
            }
        }
    }
}