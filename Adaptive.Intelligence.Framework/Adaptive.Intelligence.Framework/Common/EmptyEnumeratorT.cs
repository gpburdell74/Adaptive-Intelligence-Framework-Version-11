using System.Collections;

namespace Adaptive.Intelligence.Common
{
    /// <summary>
    /// Provides an enumerator implementation for the cases when a collection is empty or otherwise, the GenEnumerator method
    /// would result in an exception or NULL value.
    /// </summary>
    /// <typeparam name="T">
    /// The data type of the parent collection.
    /// </typeparam>
    /// <seealso cref="IEnumerator{T}" />
    public sealed class EmptyEnumerator<T> : IEnumerator<T>
    {
        /// <summary>
        /// Gets the element in the collection at the current position of the enumerator.
        /// </summary>
        /// <value>
        /// Returns <b>null</b>.
        /// </value>
        public T Current => default!;
        object IEnumerator.Current => Current!;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }
        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns>
        ///   Always returns <see langword="false" />.
        /// </returns>
        public bool MoveNext()
        {
            return false;
        }
        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        public void Reset()
        {
        }
    }
}