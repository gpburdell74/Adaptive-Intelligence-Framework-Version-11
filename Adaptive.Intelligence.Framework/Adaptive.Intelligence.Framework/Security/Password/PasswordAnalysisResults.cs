namespace Adaptive.Intelligence.Security.Password
{
    /// <summary>
    /// Contains the results of a password analysis.
    /// </summary>
    public sealed class PasswordAnalysisResults
    {
        /// <summary>
        /// Gets or sets a value indicating whether the password value has the required
        /// minimum length.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the password value has the required minimum length; otherwise, <c>false</c>.
        /// </value>
        public bool HasRequiredLength { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the password value has at least one upper-
        /// case character.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the password value has at least one upper-case character; otherwise, <c>false</c>.
        /// </value>
        public bool HasUpperCaseCharacter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the password value has at least one lower-
        /// case character.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the password value has at least one lower-case character; otherwise, <c>false</c>.
        /// </value>
        public bool HasLowerCaseCharacter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the password value has at least one number
        /// character.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the password value has at least one number character; otherwise, <c>false</c>.
        /// </value>
        public bool HasNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the password value has at least one special
        /// character.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the password value has at least one special character; otherwise, <c>false</c>.
        /// </value>
        public bool HasSpecialCharacter { get; set; }

        /// <summary>
        /// Gets a value indicating whether the specified password is valid according to the
        /// basic password rules.
        /// </summary>
        /// <value>
        ///   <c>true</c> if  the specified password is valid according to the basic password rules;
        ///   otherwise, <c>false</c>.
        /// </value>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the password strength score for the analyzed password.
        /// </summary>
        /// <value>
        /// An integer containing the password strength score for the analyzed password.
        /// </value>
        public int Score { get; set; }

        /// <summary>
        /// Gets or sets the general score category for the analyzed password.
        /// </summary>
        /// <value>
        /// A <see cref="PasswordScoreRange"/> enumerated value indicating the score category.
        /// </value>
        public PasswordScoreRange ScoreCategory { get; set; }
    }
}
