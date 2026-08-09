namespace Adaptive.Intelligence.Abstractions.Security
{
    /// <summary>
    /// Provides the signature definition for a cryptographic Key table.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface IKeyTable : IDisposable
    {
        #region Properties		
        /// <summary>
        /// Gets the primary key data.
        /// </summary>
        /// <value>
        /// A byte array containing the primary (1st) key data.
        /// </value>
        byte[] Primary { get; }

        /// <summary>
        /// Gets the secondary key data.
        /// </summary>
        /// <value>
        /// A byte array containing the secondary (2nd) key data.
        /// </value>
        byte[] Secondary { get; }

        /// <summary>
        /// Gets the tertiary key data.
        /// </summary>
        /// <value>
        /// A byte array containing the tertiary (3rd) key data.
        /// </value>
        byte[] Tertiary { get; }

        /// <summary>
        /// Gets the quaternary key data.
        /// </summary>
        /// <value>
        /// A byte array containing the quaternary (4th) key data.
        /// </value>
        byte[] Quaternary { get; }
        /// <summary>
        /// Gets the quinary key data.
        /// </summary>
        /// <value>
        /// A byte array containing the quinary (5th) key data.
        /// </value>
        byte[] Quinary { get; }
        /// <summary>
        /// Gets the senary key data.
        /// </summary>
        /// <value>
        /// A byte array containing the senary (6th) key data.
        /// </value>
        byte[] Senary { get; }
        #endregion

        #region Public Methods / Functions		
        /// <summary>
        /// Clears the key data from memory.
        /// </summary>
        void ClearKeyData();

        /// <summary>
        /// Resets the internal key enumerator to the 1st key.
        /// </summary>
        void Reset();

        /// <summary>
        /// Gets the next key value in sequence, and increments the internal enumerator.
        /// </summary>
        /// <remarks>
        /// When the last key is reached, the method will return to the first key value.  Subsequent calls to
        /// the method will return the keys in sequential order.
        /// </remarks>
        /// <returns>
        /// A byte array containing the key data.
        /// </returns>
        byte[] NextKeyValue();
        #endregion
    }
}
