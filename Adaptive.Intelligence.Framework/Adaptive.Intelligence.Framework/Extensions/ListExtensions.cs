using System.Collections;

namespace Adaptive.Intelligence.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="IList{T}"/> type.
    /// </summary>
    public static class ListExtensions
    {
        #region Public Static Methods / Functions
        /// <summary>
        /// Gets a value indicating whether the specified list is null or empty.
        /// </summary>
        /// <param name="listToCheck">
        /// The <see cref="IList"/> to be checked.
        /// </param>
        /// <returns>
        /// <b>true</b> if <i>listToCheck</i> is <b>null</b>, or <i>listToChecks</i> Count property is zero.
        /// Otherwise, returns <b>false</b>.
        /// </returns>
        public static bool IsNullOrEmpty(this IList? listToCheck)
        {
            return listToCheck == null || listToCheck.Count == 0;
        }
        #endregion
    }
}