namespace Adaptive.Intelligence.Abstractions
{
    /// <summary>
    /// Provides the signature definition for a standard United States postal address.
    /// </summary>
    public interface IStandardPostalAddress : IDisposable
    {
        #region Properties
        /// <summary>
        /// Gets or sets the content of the first address line.
        /// </summary>
        /// <value>
        /// The content of the first address line.
        /// </value>
        string? AddressLine1 { get; set; }
        /// <summary>
        /// Gets or sets the content of the second address line.
        /// </summary>
        /// <value>
        /// The content of the second address line.
        /// </value>
        string? AddressLine2 { get; set; }
        /// <summary>
        /// Gets or sets the content of the third address line.
        /// </summary>
        /// <value>
        /// The content of the third address line.
        /// </value>
        string? AddressLine3 { get; set; }
        /// <summary>
        /// Gets or sets the name of the city or town.
        /// </summary>
        /// <value>
        /// The name of the city or town.
        /// </value>
        string? City { get; set; }
        /// <summary>
        /// Gets or sets the name of the State or territory.
        /// </summary>
        /// <value>
        /// The name of the State or territory.
        /// </value>
        string? StateName { get; set; }
        /// <summary>
        /// Gets or sets the abbreviation of the State or territory.
        /// </summary>
        /// <value>
        /// The abbreviation of the State or territory.
        /// </value>
        string? StateAbbreviation { get; set; }
        /// <summary>
        /// Gets or sets the ZIP Code value.
        /// </summary>
        /// <value>
        /// The first five digits of the ZIP code value.
        /// </value>
        string? ZipCode { get; set; }
        /// <summary>
        /// Gets a value indicating whether the ZIP code value is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the ZIP code is valid; otherwise, <c>false</c>.
        /// </value>
        bool ZipCodeIsValid { get; }
        /// <summary>
        /// Gets or sets the ZIP Code plus 4 value.
        /// </summary>
        /// <value>?
        /// The appended four digits of the extended ZIP code value.
        /// </value>
        string? ZipPlus4 { get; set; }
        /// <summary>
        /// Gets a value indicating whether the ZIP  Plus 4 code value is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the ZIP Plus 4 code is valid; otherwise, <c>false</c>.
        /// </value>
        bool ZipPlus4IsValid { get; }
        #endregion
    }
}
