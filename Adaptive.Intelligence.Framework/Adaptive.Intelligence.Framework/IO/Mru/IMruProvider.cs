namespace Adaptive.Intelligence.IO.Mru
{
    /// <summary>
    /// Provides the mechanism for creating, containing, reading, and writing a list of MRU entries.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface IMruProvider : IDisposable
    {
        /// <summary>
        /// Gets the reference to the list of entries.
        /// </summary>
        /// <value>
        /// The <see cref="IMruEntryList"/> containing the MRU entries.
        /// </value>
        IMruEntryList? Entries { get; }

        /// <summary>
        /// Gets the number of entries.
        /// </summary>
        /// <value>
        /// An integer specifying the number of items in the list.
        /// </value>
        int EntryCount { get; }

        /// <summary>
        /// Adds the MRU entry to the list.
        /// </summary>
        /// <param name="entry">
        /// The <see cref="IMruEntry"/> instance being added.
        /// </param>
        void AddMruEntry(IMruEntry entry);

        /// <summary>
        /// Removes the MRU entry from the list.
        /// </summary>
        /// <param name="entry">
        /// The <see cref="IMruEntry"/> instance being removed.
        /// </param>
        void DeleteMruEntry(IMruEntry entry);

        /// <summary>
        /// Gets the entry at the specified index.
        /// </summary>
        /// <param name="index">
        /// An integer specifying the ordinal index of the item to retrieve.
        /// </param>
        /// <returns>
        /// The <see cref="IMruEntry"/> instance at hte specified index, or <b>null</b> if the index
        /// is invalid.
        /// </returns>
        IMruEntry? GetEntry(int index);

        /// <summary>
        /// Performs any tasks related to initializing the provider.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Loads the content of the MRU list from a data source.
        /// </summary>
        void Load();

        /// <summary>
        /// Writes the content of the MRU list to a data source.
        /// </summary>
        void Save();
    }
}