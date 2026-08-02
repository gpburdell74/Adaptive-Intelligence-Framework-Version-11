using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Constants;
using Adaptive.Intelligence.Extensions;
using System.Text;

namespace Adaptive.Intelligence.Common
{
    /// <summary>
    /// Represents a standard postal address in the United States.
    /// </summary>
    /// <seealso cref="IStandardPostalAddress" />
    public sealed class USAddress : DisposableObjectBase, IStandardPostalAddress
    {
        #region Dispose Method		
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            AddressLine1 = null;
            AddressLine2 = null;
            AddressLine3 = null;
            City = null;
            StateAbbreviation = null;
            StateName = null;
            ZipCode = null;
            ZipPlus4 = null;

            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties		
        /// <summary>
        /// Gets or sets the content of the first address line.
        /// </summary>
        /// <value>
        /// The content of the first address line.
        /// </value>
        public string? AddressLine1 { get; set; }
        /// <summary>
        /// Gets or sets the content of the second address line.
        /// </summary>
        /// <value>
        /// The content of the second address line.
        /// </value>
        public string? AddressLine2 { get; set; }
        /// <summary>
        /// Gets or sets the content of the third address line.
        /// </summary>
        /// <value>
        /// The content of the third address line.
        /// </value>
        public string? AddressLine3 { get; set; }
        /// <summary>
        /// Gets or sets the name of the city or town.
        /// </summary>
        /// <value>
        /// The name of the city or town.
        /// </value>
        public string? City { get; set; }
        /// <summary>
        /// Gets or sets the name of the State or territory.
        /// </summary>
        /// <value>
        /// The name of the State or territory.
        /// </value>
        public string? StateName { get; set; }
        /// <summary>
        /// Gets or sets the abbreviation of the State or territory.
        /// </summary>
        /// <value>
        /// The abbreviation of the State or territory.
        /// </value>
        public string? StateAbbreviation { get; set; }
        /// <summary>
        /// Gets or sets the ZIP Code value.
        /// </summary>
        /// <value>
        /// The first five digits of the ZIP code value.
        /// </value>
        public string? ZipCode { get; set; }
        /// <summary>
        /// Gets a value indicating whether the ZIP code value is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the ZIP code is valid; otherwise, <c>false</c>.
        /// </value>
        public bool ZipCodeIsValid => (!string.IsNullOrEmpty(ZipPlus4)) &&
                    (ZipPlus4.Length == 5) &&
                    char.IsNumber(ZipPlus4[0]) &&
                    char.IsNumber(ZipPlus4[1]) &&
                    char.IsNumber(ZipPlus4[2]) &&
                    char.IsNumber(ZipPlus4[3]) &&
                    char.IsNumber(ZipPlus4[4])
                    ;
        /// <summary>
        /// Gets or sets the ZIP Code plus 4 value.
        /// </summary>
        /// <value>
        /// ?
        /// The appended four digits of the extended ZIP code value.
        /// </value>
        public string? ZipPlus4 { get; set; }
        /// <summary>
        /// Gets a value indicating whether the ZIP  Plus 4 code value is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the ZIP Plus 4 code is valid; otherwise, <c>false</c>.
        /// </value>
        public bool ZipPlus4IsValid => (!string.IsNullOrEmpty(ZipPlus4)) &&
                    (ZipPlus4.Length == 4) &&
                    char.IsNumber(ZipPlus4[0]) &&
                    char.IsNumber(ZipPlus4[1]) &&
                    char.IsNumber(ZipPlus4[2]) &&
                    char.IsNumber(ZipPlus4[3])
                    ;
        #endregion

        #region Public Methods / Functions		
        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            StringBuilder addressBuilder = new();
            StringBuilder cityStateZipBuilder = new();

            if (!string.IsNullOrEmpty(AddressLine1))
            {
                addressBuilder.AppendLine(AddressLine1);
            }
            if (!string.IsNullOrEmpty(AddressLine2))
            {
                addressBuilder.AppendLine(AddressLine2);
            }
            if (!string.IsNullOrEmpty(AddressLine3))
            {
                addressBuilder.AppendLine(AddressLine3);
            }

            if (!string.IsNullOrEmpty(City))
            {
                cityStateZipBuilder.Append(City);
                cityStateZipBuilder.AppendComma();
                cityStateZipBuilder.AppendSpace();
            }
            if (!string.IsNullOrEmpty(StateAbbreviation))
            {
                cityStateZipBuilder.Append(StateAbbreviation);
                cityStateZipBuilder.AppendSpace();
            }
            else if (!string.IsNullOrEmpty(StateName))
            {
                cityStateZipBuilder.Append(StateName);
                cityStateZipBuilder.AppendSpace();
            }

            if (!string.IsNullOrEmpty(ZipCode))
            {
                cityStateZipBuilder.Append(ZipCode);
            }
            if (!string.IsNullOrEmpty(ZipPlus4))
            {
                if (!string.IsNullOrEmpty(ZipCode))
                {
                    cityStateZipBuilder.Append(CharacterConstants.DashChar);
                }

                cityStateZipBuilder.Append(ZipPlus4);
            }

            return addressBuilder.ToString() + cityStateZipBuilder.ToString().Trim();
        }
        #endregion
    }
}
