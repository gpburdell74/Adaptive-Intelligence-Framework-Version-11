using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Enumerations;
using System.Text.Json.Serialization;

namespace Adaptive.Intelligence.States
{
    /// <summary>
    /// Represents a State of the Union.
    /// </summary>
    public sealed class USState : DisposableObjectBase
    {
        #region Dispose Method
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            Abbreviation = null;
            Name = null;
            base.Dispose(disposing);
        }
        #endregion

        /// <summary>
        /// Gets or sets the postal abbreviation for the State.
        /// </summary>
        /// <value>
        /// The official 2-character postal abbreviation for the State.
        /// </value>
        public string? Abbreviation { get; set; }
        /// <summary>
        /// Gets the display name / text for the State.
        /// </summary>
        /// <value>
        /// A string containing the display name.
        /// </value>
        [JsonIgnore()]
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(Abbreviation) && !string.IsNullOrEmpty(Name))
                {
                    return Abbreviation + " - " + Name;
                }

                if (!string.IsNullOrEmpty(Name))
                {
                    return Name;
                }

                return !string.IsNullOrEmpty(Abbreviation) ? Abbreviation : string.Empty;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the item is a State.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the item represented is a State; otherwise, <c>false</c> if
        /// the item is a Territory.
        /// </value>
        public bool IsState { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the State is supported in the license scan.
        /// </summary>
        /// <value>
        /// <c>true</c> if the application can scan and process the State's driver's licenses; otherwise, <c>false</c>.
        /// </value>
        public bool IsSupported { get; set; }
        /// <summary>
        /// Gets or sets the name of the State.
        /// </summary>
        /// <value>
        /// The name of the State.
        /// </value>
        public string? Name { get; set; }
        /// <summary>
        /// Gets or sets the state code value.
        /// </summary>
        /// <value>
        /// A <see cref="USStates"/> enumerated value indicating the State; the integer value
        ///  of the enumeration represents the order of admission to the Union.
        /// </value>
        public USStates StateCode { get; set; }
    }
}