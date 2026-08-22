namespace Adaptive.Intelligence.IO.Mru
{
    /// <summary>
    /// Provides the class for implementing a list of MRU file entries.
    /// </summary>
    /// <seealso cref="List{T}" />
    /// <seealso cref="IMruEntryList" />
    /// <seealso cref="IMruEntry"/>
    /// <seealso cref="MruFileEntry"/>
    public class MruFileEntryList : MruEntryList<MruFileEntry>
    {
        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="MruFileEntryList"/> class.
        /// </summary>
        public MruFileEntryList()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MruFileEntryList"/> class.
        /// </summary>
        /// <param name="sourceData">
        /// The <see cref="IEnumerable{MruFileEntry}"/> list containing the source data.
        /// </param>
        public MruFileEntryList(IEnumerable<MruFileEntry> sourceData) : base(sourceData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MruEntryList{T}"/> class.
        /// </summary>
        /// <param name="sourceStream">
        /// The source <see cref="Stream"/> used to read the data content of the list.
        /// </param>
        public MruFileEntryList(Stream sourceStream) : base(sourceStream)
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
        /// A new instance of <see cref="MruFileEntry"/>, populated from the data that was read.
        /// </returns>
        protected override MruFileEntry ReadRecord(SafeBinaryReader reader)
        {
            MruFileEntry entry = new();
            entry.Load(reader);
            return entry;
        }

        /// <summary>
        /// Writes the record to the data source.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="SafeBinaryWriter"/> instance used to write the data content.
        /// </param>
        /// <param name="recordToWrite">
        /// The <see cref="MruFileEntry"/> instance whose content is being saved.
        /// </param>
        protected override void WriteRecord(SafeBinaryWriter writer, MruFileEntry recordToWrite)
        {
            recordToWrite.Save(writer);
        }
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Adds the path and file name to the entries list.
        /// </summary>
        /// <param name="pathAndFileName">
        /// A string containing the fully-qualified path and name of the file.
        /// </param>
        /// <param name="displayText">
        /// An optional parameter containing user-friendly text for this entry.
        /// </param>
        public void AddFile(string pathAndFileName, string? displayText = null)
        {
            MruFileEntry entry = new()
            {
                FileName = pathAndFileName,
            };
            if (!string.IsNullOrEmpty(displayText))
            {
                entry.DisplayText = displayText;
            }
            Add(entry);
        }

        /// <summary>
        /// Determines whether an entry already exists for the specified path and file name.
        /// </summary>
        /// <param name="pathAndFileName">
        /// A string containing the fully-qualified path and name of the file.
        /// </param>
        /// <returns>
        ///   <c>true</c> if the file is already represented; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsFile(string pathAndFileName)
        {
            MruFileEntry? entry = this.FirstOrDefault(x => string.Compare(x.FileName, pathAndFileName, StringComparison.OrdinalIgnoreCase) == 0);
            return entry != null;
        }

        /// <summary>
        /// Removes the specified entry from the current list.
        /// </summary>
        /// <param name="pathAndFileName">
        /// A string containing the fully-qualified path and name of the file.
        /// </param>
        public void RemoveFile(string pathAndFileName)
        {
            MruFileEntry? entry = this.FirstOrDefault(x => string.Compare(x.FileName, pathAndFileName, StringComparison.OrdinalIgnoreCase) == 0);
            if (entry != null)
            {
                Remove(entry);
            }
        }
        #endregion
    }

}
