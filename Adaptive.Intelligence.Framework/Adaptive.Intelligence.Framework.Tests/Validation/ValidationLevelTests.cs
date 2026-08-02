using Adaptive.Intelligence.Validation;

namespace Adaptive.Intelligence.Framework.Tests.Validation
{
    /// <summary>
    /// Provides tests for the <see cref="ValidationLevel"/> enum.
    /// </summary>
    public class ValidationLevelTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for Enum_Values_Match_Expected_Numeric_Definitions.
        /// </summary>
        public void Enum_Values_Match_Expected_Numeric_Definitions()
        {
            Assert.Equal(0, (int)ValidationLevel.NoneOrNotSpecified);
            Assert.Equal(1, (int)ValidationLevel.SuccessInformational);
            Assert.Equal(2, (int)ValidationLevel.Informational);
            Assert.Equal(3, (int)ValidationLevel.Warning);
            Assert.Equal(4, (int)ValidationLevel.Error);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Enum_Contains_All_Expected_Names.
        /// </summary>
        public void Enum_Contains_All_Expected_Names()
        {
            string[] names = Enum.GetNames<ValidationLevel>();

            Assert.Contains(nameof(ValidationLevel.NoneOrNotSpecified), names);
            Assert.Contains(nameof(ValidationLevel.SuccessInformational), names);
            Assert.Contains(nameof(ValidationLevel.Informational), names);
            Assert.Contains(nameof(ValidationLevel.Warning), names);
            Assert.Contains(nameof(ValidationLevel.Error), names);
            Assert.Equal(5, names.Length);
        }
    }
}