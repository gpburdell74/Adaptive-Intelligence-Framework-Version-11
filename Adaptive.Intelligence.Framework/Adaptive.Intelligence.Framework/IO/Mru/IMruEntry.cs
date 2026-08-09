namespace Adaptive.Intelligence.IO.Mru
{
    /// <summary>
    /// Provides the signature definition for a Most Recently Used (MRU) entry.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface IMruEntry : IDisposable
    {
        /// <summary>
        /// Gets or sets the ID assigned to the instance.
        /// </summary>
        /// <value>
        /// An optional use property for assigning ID values if needed.
        /// </value>
        int Id { get; set; }

        /// <summary>
        /// Gets or sets the display text.
        /// </summary>
        /// <value>
        /// A string containing the text to be displayed.
        /// </value>
        string? DisplayText { get; set; }

        /// <summary>
        /// Gets or sets the MRU data.
        /// </summary>
        /// <value>
        /// A string containing the actual file name, document name, or other useful value being stored.
        /// </value>
        string? MruData { get; set; }

        /// <summary>
        /// Loads the content of the instance from the provided data sourrce.
        /// </summary>
        /// <param name="reader">
        /// The <see cref="SafeBinaryReader"/> instance used to read the data.
        /// </param>
        void Load(SafeBinaryReader reader);

        /// <summary>
        /// Saves the content of the instance to the provided data sourrce.
        /// </summary>
        /// <param name="writer">
        /// The <see cref="SafeBinaryWriter"/> instance used to write the data.
        /// </param>
        void Save(SafeBinaryWriter writer);
    }
}
