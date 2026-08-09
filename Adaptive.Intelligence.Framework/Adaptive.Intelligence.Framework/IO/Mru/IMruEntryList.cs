namespace Adaptive.Intelligence.IO.Mru
{
    /// <summary>
    /// Provides the signature definition for a list or collection containing a list of MRU entries.
    /// </summary>
    /// <seealso cref="IList{T}" />
    /// <seealso cref="IMruEntry"/>
    public interface IMruEntryList
    {
        /// <summary>
        /// Populates the list from  the provided stream instance.
        /// </summary>
        /// <param name="sourceStream">
        /// The source <see cref="Stream"/> that contains the data source.
        /// </param>
        void PopulateFromStream(Stream sourceStream);

        /// <summary>
        /// Writes the list content to the provided stream instance.
        /// </summary>
        /// <param name="destinationStream">
        /// The source <see cref="Stream"/> to write the content of the list to.
        /// </param>
        void SaveToStream(Stream destinationStream);
    }
}
