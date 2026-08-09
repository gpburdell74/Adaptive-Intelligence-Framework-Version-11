namespace Adaptive.Intelligence.Security.Password
{
    /// <summary>
    /// Lists the password strength categories that are currently supported.
    /// </summary>
    public enum PasswordScoreRange
    {
        /// <summary>
        /// Indicates the specified password value was not analyzed.
        /// </summary>
        NotAnalyzed = 0,
        /// <summary>
        /// Indicates the specified password value was invalid.
        /// </summary>
        Invalid = 1,
        /// <summary>
        /// Indicates the specified password value is a very weak password.
        /// </summary>
        VeryWeak = 2,
        /// <summary>
        /// Indicates the specified password value is a weak password.
        /// </summary>
        Weak = 3,
        /// <summary>
        /// Indicates the specified password value is a fair-to-middlin' password.
        /// </summary>
        Fair = 4,
        /// <summary>
        /// Indicates the specified password value is a strong password.
        /// </summary>
        Strong = 5,
        /// <summary>
        /// Indicates the specified password value is a very strong password.
        /// </summary>
        VeryStrong = 6
    }
}
