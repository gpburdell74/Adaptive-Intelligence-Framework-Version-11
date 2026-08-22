namespace Adaptive.Intelligence.IO.Mru
{
    /// <summary>
    /// Provides a base class for implementing a list of MRU entries.
    /// </summary>
    /// <typeparam name="T">
    /// The data type of the entry being stored in the list.
    /// </typeparam>
    /// <seealso cref="List{T}" />
    /// <seealso cref="IMruEntryList" />
    /// <seealso cref="IMruEntry"/>
    public abstract class MruEntryList<T> : List<T>, IMruEntryList
        where T : IMruEntry
    {
        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="MruEntryList{T}"/> class.
        /// </summary>
        protected MruEntryList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MruEntryList{T}"/> class.
        /// </summary>
        /// <param name="sourceData">
        /// The <see cref="IEnumerable{T}"/> list containing the source data.
        /// </param>
        protected MruEntryList(IEnumerable<T> sourceData)
        {
            AddRange(sourceData);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MruEntryList{T}"/> class.
        /// </summary>
        /// <param name="sourceStream">
        /// The source <see cref="Stream"/> used to read the data content of the list.
        /// </param>
        protected MruEntryList(Stream sourceStream)
        {
            PopulateFromStream(sourceStream);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Creates and reads the record from the data source to be added to the current collection.
        /// </summary>
        /// <param name="reader">
        /// The <see cref="SafeBinaryReader"/> instance used to read the data content.
        /// </param>
        /// <returns>
        /// A new instance of <typeparamref name="T"/>, populated from the data that was read.
        /// </returns>
        protected abstract T ReadRecord(SafeBinaryReader reader);

        /// <summary>
        /// Writes the record to the data source.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="SafeBinaryWriter"/> instance used to write the data content.
        /// </param>
        /// <param name="recordToWrite">
        /// The <typeparamref name="T"/> instance whose content is being saved.
        /// </param>
        protected abstract void WriteRecord(SafeBinaryWriter writer, T recordToWrite);
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Populates the list from  the provided stream instance.
        /// </summary>
        /// <param name="sourceStream">
        /// The source <see cref="Stream" /> that contains the data source.
        /// </param>
        /// <exception cref="Exception">
        /// Thrown when the input stream cannot be read from.
        /// </exception>
        public void PopulateFromStream(Stream sourceStream)
        {
            if (sourceStream is null || !sourceStream.CanRead)
            {
                throw new ArgumentException("The input stream cannot be read from.", nameof(sourceStream));
            }

            // Remove old content.
            Clear();

            // Read the record count.
            sourceStream.Seek(0, SeekOrigin.Begin);
            SafeBinaryReader reader = new(sourceStream);
            int recordCount = reader.ReadInt32();
            if (recordCount > -1 && recordCount < 25000)
            {
                // Load each record.
                for (int count = 0; count < recordCount; count++)
                {
                    Add(ReadRecord(reader));
                }
            }
        }

        /// <summary>
        /// Writes the list content to the provided stream instance.
        /// </summary>
        /// <param name="destinationStream">
        /// The source <see cref="Stream" /> to write the content of the list to.
        /// </param>
        /// <exception cref="Exception">
        /// Thrown when the provided stream cannot be written to.
        /// </exception>
        public void SaveToStream(Stream destinationStream)
        {
            if (destinationStream is null || !destinationStream.CanWrite)
            {
                throw new ArgumentException("The input stream cannot be written to.", nameof(destinationStream));
            }

            // Write the record count.
            SafeBinaryWriter writer = new(destinationStream);
            writer.Write(Count);

            // Load each record.
            for (int count = 0; count < Count; count++)
            {
                WriteRecord(writer, this[count]);
            }
            writer.Flush();
        }
        #endregion
    }
}
