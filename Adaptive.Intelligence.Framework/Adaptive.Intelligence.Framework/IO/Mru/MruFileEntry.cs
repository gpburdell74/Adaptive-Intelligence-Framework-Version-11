using Adaptive.Intelligence.Abstractions;

namespace Adaptive.Intelligence.IO.Mru
{
    /// <summary>
    /// Represents a most-recently-used file entry.
    /// </summary>
    /// <seealso cref="DisposableObjectBase" />
    /// <seealso cref="IMruEntry" />
    public class MruFileEntry : DisposableObjectBase, IMruEntry
    {
        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="MruFileEntry"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public MruFileEntry()
        {

        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            Id = 0;
            DisplayText = null;
            Permissions = 0;
            MruData = null;

            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties    
        /// <summary>
        /// Gets or sets the ID assigned to the instance.
        /// </summary>
        /// <value>
        /// An optional use property for assigning ID values if needed.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the display text.
        /// </summary>
        /// <value>
        /// A string containing the text to be displayed.
        /// </value>
        public string? DisplayText { get; set; }
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// A string containing the fully-qualified path and name of the file.
        /// </value>
        public string? FileName { get => MruData; set => MruData = value; }

        /// <summary>
        /// Gets the file name only.
        /// </summary>
        /// <value>
        /// A string containing just the file name without the path.
        /// </value>
        public string? FileNameOnly
        {
            get
            {
                if (string.IsNullOrEmpty(MruData))
                {
                    return null;
                }
                else
                {
                    return Path.GetFileName(MruData);
                }
            }
        }

        /// <summary>
        /// Gets or sets the MRU data.
        /// </summary>
        /// <value>
        /// A string containing the actual file name, document name, or other useful value being stored.
        /// </value>
        public string? MruData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating the permissions required for or assigned to this entry.
        /// </summary>
        /// <remarks>
        /// This is added for user implementation and use.
        /// </remarks>
        /// <value>
        /// An integer containing a value related to permissions, or zero (0) if not used.
        /// </value>
        public int Permissions { get; set; }
        #endregion

        #region Public Methods / Functions    
        /// <summary>
        /// Loads the content of the instance from the provided data sourrce.
        /// </summary>
        /// <param name="reader">The <see cref="SafeBinaryReader" /> instance used to read the data.</param>
        public void Load(SafeBinaryReader reader)
        {
            Id = reader.ReadInt32();
            MruData = reader.ReadNullableString();
            Permissions = reader.ReadInt32();
        }

        /// <summary>
        /// Saves the content of the instance to the provided data sourrce.
        /// </summary>
        /// <param name="writer">The <see cref="SafeBinaryWriter" /> instance used to write the data.</param>
        public void Save(SafeBinaryWriter writer)
        {
            writer.Write(Id);
            writer.WriteNullable(MruData);
            writer.Write(Permissions);
        }
        #endregion
    }
}
