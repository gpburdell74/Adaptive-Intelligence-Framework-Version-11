using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Csv.Attributes;
using System.Reflection;

namespace Adaptive.Intelligence.Csv.Metadata
{
    /// <summary>
    /// Represents and contains meta data for a CSV column.
    /// </summary>
    /// <seealso cref="DisposableObjectBase" />
    public sealed class CsvColumnInfo : DisposableObjectBase
    {
        #region Private Member Declarations
        private const int NotIndexed = -1;
        #endregion

        #region Constructor / Dispose Methods    
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvColumnInfo"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public CsvColumnInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvColumnInfo"/> class.
        /// </summary>
        /// <param name="columnName">
        /// A string containing the name of the column, property, or field the CSV column is mapped to.
        /// </param>
        /// <param name="index">
        /// An integer specifying the ordinal index of the column.
        /// </param>
        public CsvColumnInfo(string? columnName, int index)
        {
            ColumnName = columnName;
            Index = index;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CsvColumnInfo"/> class.
        /// </summary>
        /// <param name="columnName">
        /// A string containing the name of the column, property, or field the CSV column is mapped to.
        /// </param>
        /// <param name="headerText">
        /// A string containing the text of the CSV column header, if specified.
        /// </param>
        /// <param name="index">
        /// An optional parameter of integer specifying the ordinal index of the column.
        /// </param>
        public CsvColumnInfo(string? columnName, string? headerText, int index = NotIndexed)
        {
            ColumnName = columnName;
            HeaderText = headerText;
            Index = index;
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            ColumnName = null;
            HeaderText = null;
            PropertyData = null;
            Index = NotIndexed;

            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties    
        /// <summary>
        /// Gets or sets the name of the column for the CSV file.
        /// </summary>
        /// <remarks>
        /// This text may be used if <see cref="HeaderText"/> is null or empty.  This is often used to correspond
        /// to a related database column or property name that the CSV column maps to.
        /// </remarks>
        /// <value>
        /// A string containing the name of the column.
        /// </value>
        public string? ColumnName { get; set; }

        /// <summary>
        /// Gets or sets the header text for the column for the CSV file.
        /// </summary>
        /// <remarks>
        /// This text is used as the "friendly" column header, if the CSV file has column headers.  If this
        /// value is <b>null</b> or empty, the <see cref="ColumnName"/> value may be used instread. This
        /// value is not related to any other object or used for any kind of mapping.
        /// </remarks>
        /// <value>
        /// A string containing the display / header text for the column.
        /// </value>
        public string? HeaderText { get; set; }

        /// <summary>
        /// Gets or sets the index of the column in the CSV file.
        /// </summary>
        /// <value>
        /// An integer with an initial value from an <see cref="IndexAttribute"/> decoration, or defaults
        /// to <see cref="NotIndexed"/>.
        /// </value>
        public int Index { get; set; } = NotIndexed;

        /// <summary>
        /// Gets or sets the reference to the <see cref="PropertyInfo"/> instance representing the property
        /// this CSV column maps to (or is mapped from).
        /// </summary>
        /// <value>
        /// The <see cref="PropertyInfo"/> read from the source data type, if provided; otherwise, <b>null</b>.
        /// </value>
        public PropertyInfo? PropertyData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property, field, or column to map to is missing in the
        /// source or destination.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the property, field, or column is missing; otherwise, <c>false</c>.
        /// </value>
        public bool PropertyMissing { get; set; }
        #endregion
    }
}
