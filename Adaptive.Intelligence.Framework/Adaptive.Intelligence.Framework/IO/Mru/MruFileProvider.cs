using Adaptive.Intelligence.Abstractions;

namespace Adaptive.Intelligence.IO.Mru
{
    /// <summary>
    /// Provides a management class for creating, reading, and writing a list of most recently used
    /// files.
    /// </summary>
    /// <seealso cref="DisposableObjectBase" />
    /// <seealso cref="IMruProvider" />
    public class MruFileProvider : DisposableObjectBase, IMruProvider
    {
        #region Private Member Declarations
        /// <summary>
        /// The entries list.
        /// </summary>
        private MruFileEntryList? _entries;
        /// <summary>
        /// The name of the file where the MRU content is stored.
        /// </summary>
        private string? _fileName;
        #endregion

        #region Constructor / Dispose Methods    
        /// <summary>
        /// Initializes a new instance of the <see cref="MruFileProvider"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public MruFileProvider()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MruFileProvider"/> class.
        /// </summary>
        /// <param name="localFileName">
        /// A string containing the path and name of the local file to use to store MRU data.
        /// </param>
        public MruFileProvider(string localFileName)
        {
            _fileName = localFileName;
            UseLocalExecutionPath = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MruFileProvider"/> class.
        /// </summary>
        /// <param name="localFileName">
        /// A string containing the path and name of the local file to use to store MRU data.
        /// </param>
        /// <param name="useExecutionPath">
        /// A value indicating whether to look for the file in the local execution path.
        /// </param>
        public MruFileProvider(string localFileName, bool useExecutionPath)
        {
            _fileName = localFileName;
            UseLocalExecutionPath = useExecutionPath;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing)
            {
                _entries?.Clear();
            }

            _entries = null;
            _fileName = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the reference to the list of entries.
        /// </summary>
        /// <value>
        /// The <see cref="IMruEntryList" /> containing the MRU entries.
        /// </value>
        public IMruEntryList? Entries => _entries;

        /// <summary>
        /// Gets the number of entries.
        /// </summary>
        /// <value>
        /// An integer specifying the number of items in the list.
        /// </value>
        public int EntryCount
        {
            get
            {
                if (_entries == null)
                {
                    return 0;
                }
                else
                {
                    return _entries.Count;
                }
            }
        }
        /// <summary>
        /// Gets or sets the name of the file to read from and write to.
        /// </summary>
        /// <value>
        /// A string containing the path and name of the file, or just the 
        /// file name value if <see cref="UseLocalExecutionPath"/> is <b>true</b>.
        /// </value>
        public string? FileName { get => _fileName; set => _fileName = value; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the local execution path when looking for
        /// or creating the MRU file.
        /// </summary>
        /// <value>
        ///   <c>true</c> to use the local execution path when looking for or creating the MRU file;
        ///   otherwise, <c>false</c>.
        /// </value>
        public bool UseLocalExecutionPath { get; set; }

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
        public void AddEntry(string pathAndFileName, string? displayText = null)
        {
            if (_entries != null)
            {
                if (!_entries.ContainsFile(pathAndFileName))
                {
                    _entries.AddFile(pathAndFileName, displayText);
                }
            }
        }

        /// <summary>
        /// Adds the MRU entry to the list.
        /// </summary>
        /// <param name="entry">
        /// The <see cref="IMruEntry" /> instance being added.
        /// </param>
        public void AddMruEntry(IMruEntry entry)
        {
            if (!IsDisposed)
            {
                _entries ??= new MruFileEntryList();
                if (entry is MruFileEntry fileEntry)
                {
                    _entries.Add(fileEntry);
                }
            }
        }

        /// <summary>
        /// Removes the MRU entry from the list.
        /// </summary>
        /// <param name="entry">
        /// The <see cref="IMruEntry" /> instance being removed.
        /// </param>
        public void DeleteMruEntry(IMruEntry entry)
        {
            if (_entries != null)
            {
                if (entry is MruFileEntry fileEntry)
                {
                    _entries.Remove(fileEntry);
                }
            }
        }

        /// <summary>
        /// Gets the entry at the specified index.
        /// </summary>
        /// <param name="index">An integer specifying the ordinal index of the item to retrieve.</param>
        /// <returns>
        /// The <see cref="IMruEntry" /> instance at hte specified index, or <b>null</b> if the index
        /// is invalid.
        /// </returns>
        IMruEntry? IMruProvider.GetEntry(int index)
        {
            return GetEntry(index);
        }

        /// <summary>
        /// Gets the entry at the specified index.
        /// </summary>
        /// <param name="index">An integer specifying the ordinal index of the item to retrieve.</param>
        /// <returns>
        /// The <see cref="MruFileEntry" /> instance at hte specified index, or <b>null</b> if the index
        /// is invalid.
        /// </returns>
        public MruFileEntry? GetEntry(int index)
        {
            if (_entries != null && index >= 0 && index < _entries.Count)
            {
                return _entries[index];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Performs any tasks related to initializing the provider.
        /// </summary>
        public void Initialize()
        {
            _entries = [];
        }

        /// <summary>
        /// Loads the content of the MRU list from a data source.
        /// </summary>
        public void Load()
        {
            // Determine the file to use.
            string fileToLoad = DetermineFileName();

            // Load the data.
            FileStream? sourceStream = SafeIO.OpenFileForExclusiveRead(fileToLoad);
            if (sourceStream != null)
            {
                _entries?.Clear();
                _entries = new MruFileEntryList(sourceStream);

                sourceStream.Close();
                sourceStream.Dispose();
            }
        }

        /// <summary>
        /// Writes the content of the MRU list to a data source.
        /// </summary>
        public void Save()
        {
            // Save to a temporary file first, then rename it to the target file name. This is to avoid data loss if the process is interrupted.
            bool success = false;
            string tempFileName = Path.GetTempFileName();

            // Write the data.
            FileStream? destinationStream = SafeIO.OpenFileForExclusiveWrite(tempFileName);
            if (destinationStream != null)
            {
                _entries?.SaveToStream(destinationStream);
                destinationStream.Close();
                destinationStream.Dispose();
                success = true;
            }

            // Determine the file to use.
            string fileToSave = DetermineFileName();
            if (success && !string.IsNullOrEmpty(fileToSave))
            {
                if (SafeIO.FileExists(fileToSave))
                {
                    SafeIO.DeleteFile(fileToSave);
                }

                // Move the temp file to the target file name.
                File.Move(tempFileName, fileToSave);
            }
        }
        #endregion

        #region Private Methods / Functions
        /// <summary>
        /// Determines the path and file name combination to use.
        /// </summary>
        /// <returns>
        /// A string containing hte concrete path and name of the file.
        /// </returns>
        private string DetermineFileName()
        {
            string file = string.Empty;

            if (!string.IsNullOrEmpty(_fileName))
            {
                if (UseLocalExecutionPath)
                {
                    file = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, _fileName);
                }
                else
                {
                    file = _fileName;
                }
            }
            return file;
        }

        #endregion
    }
}
