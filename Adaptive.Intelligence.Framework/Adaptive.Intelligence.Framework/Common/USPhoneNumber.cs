using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Constants;
using System.Text;

namespace Adaptive.Intelligence.Common
{
    /// <summary>
    /// Represents a telephone number in the United States.
    /// </summary>
    public sealed class USPhoneNumber : DisposableObjectBase, ICloneable, IEquatable<USPhoneNumber>
    {
        #region Private Member Declarations		
        /// <summary>
        /// The country code value.
        /// </summary>
        private string? _countryCode;
        /// <summary>
        /// The area code value.
        /// </summary>
        private string? _areaCode;
        /// <summary>
        /// The prefix value.
        /// </summary>
        private string? _prefix;
        /// <summary>
        /// The number value.
        /// </summary>
        private string? _number;

        /// <summary>
        /// The county code for the US.
        /// </summary>
        private const string CountryCodeUS = "1";
        #endregion

        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="USPhoneNumber"/> class.
        /// </summary>
        public USPhoneNumber()
        {
            _countryCode = CountryCodeUS;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="USPhoneNumber"/> class.
        /// </summary>
        /// <param name="number">The number.</param>
        public USPhoneNumber(string number)
        {
            _countryCode = CountryCodeUS;
            Parse(number);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="USPhoneNumber"/> class.
        /// </summary>
        /// <param name="itemToCopy">The item to copy.</param>
        public USPhoneNumber(USPhoneNumber itemToCopy)
        {
            _countryCode = CountryCodeUS;
            _areaCode = itemToCopy.AreaCode;
            _prefix = itemToCopy.Prefix;
            _number = itemToCopy.Number;
        }
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            _countryCode = null;
            _areaCode = null;
            _prefix = null;
            _number = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the area code value for the phone number.
        /// </summary>
        /// <value>
        /// A string containing a 3-digit US area code, or <b>null</b>.
        /// </value>
        public string? AreaCode
        {
            get => _areaCode;
            set
            {
                if (value != null && value.Length == 3 && IsNumeric(value))
                {
                    _areaCode = value;
                }
                else
                {
                    _areaCode = null;
                }
            }
        }
        /// <summary>
        /// Gets or sets the area code value for the phone number.
        /// </summary>
        /// <value>
        /// A string containing a 3-digit US area code, or <b>null</b>.
        /// </value>
        public string? CountryCode => _countryCode;
        /// <summary>
        /// Gets a value indicating whether this instance is not a phone number (NaPN).
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is not a phone number; otherwise, <c>false</c>.
        /// </value>
        public bool IsNaPN => _prefix == null || _number == null;
        /// <summary>
        /// Gets the not-a-phone-number representation.
        /// </summary>
        /// <value>
        /// The <see cref="USPhoneNumber"/> instance that represents the not-a-phone-number
        /// value.
        /// </value>
        public static USPhoneNumber NaPN => new(string.Empty);
        /// <summary>
        /// Gets or sets the post-prefix number value for the phone number.
        /// </summary>
        /// <value>
        /// A string containing a 4-digit US number, or <b>null</b>.
        /// </value>
        public string? Number
        {
            get => _number;
            set
            {
                if (value != null && value.Length == 4 && IsNumeric(value))
                {
                    _number = value;
                }
                else
                {
                    _number = null;
                }
            }
        }
        /// <summary>
        /// Gets or sets the prefix code value for the phone number.
        /// </summary>
        /// <value>
        /// A string containing a 3-digit US prefix code, or <b>null</b>.
        /// </value>
        public string? Prefix
        {
            get => _prefix;
            set
            {
                if (value != null && value.Length == 3 && IsNumeric(value))
                {
                    _prefix = value;
                }
                else
                {
                    _prefix = null;
                }
            }
        }

        private static readonly char[] separator = ['.'];
        #endregion

        #region Public Methods / Functions		
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new <see cref="USPhoneNumber"/> that is a copy of this instance.
        /// </returns>
        public USPhoneNumber Clone()
        {
            return new USPhoneNumber(this);
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            StringBuilder item = new();
            if (_countryCode != null)
            {
                item.Append(_countryCode);
            }

            if (_areaCode != null)
            {
                item.Append(_areaCode);
            }

            if (_prefix != null)
            {
                item.Append(_prefix);
            }

            if (_number != null)
            {
                item.Append(_number);
            }

            return item.ToString().GetHashCode();
        }
        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///   <see langword="true" /> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(USPhoneNumber? other)
        {
            if (other == null)
            {
                return false;
            }
            else
            {
                return (IsNaPN && other.IsNaPN) ||
                       (_areaCode == other.AreaCode &&
                        _prefix == other.Prefix &&
                        _number == other.Number);
            }
        }
        /// <summary>
        /// Determines whether the specified <see cref="Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">
        /// The <see cref="System.Object" /> to compare with this instance.
        /// </param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object? obj)
        {
            if (obj is not USPhoneNumber rightSide)
            {
                return false;
            }
            else
            {
                return Equals(rightSide);
            }
        }
        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (IsNaPN)
            {
                return string.Empty;
            }
            else
            {
                StringBuilder builder = new();

                if (_areaCode != null)
                {
                    builder.Append(_areaCode);
                }

                if (_prefix != null)
                {
                    builder.Append(_prefix);
                }

                if (_number != null)
                {
                    builder.Append(_number);
                }

                return builder.ToString();
            }
        }
        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <param name="formatted">
        /// A value indicating whether to format the string.
        /// </param>
        /// <param name="withCountryCode">
        /// A value indicating whether to include the country code.
        /// </param>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public string ToString(bool formatted, bool withCountryCode)
        {
            string returnValue;
            if (IsNaPN)
            {
                returnValue = string.Empty;
            }
            else
            {
                if (!formatted)
                {
                    if (withCountryCode)
                    {
                        returnValue = _countryCode + ToString();
                    }
                    else
                    {
                        returnValue = ToString();
                    }
                }
                else
                {
                    StringBuilder builder = new();

                    if (withCountryCode)
                    {
                        builder.Append("+" + _countryCode + " ");
                    }

                    if (_areaCode != null)
                    {
                        builder.Append("(" + _areaCode + ") ");
                    }

                    if (_prefix != null)
                    {
                        builder.Append(_prefix + " - ");
                    }

                    if (_number != null)
                    {
                        builder.Append(_number);
                    }

                    returnValue = builder.ToString().Trim();
                }
            }
            return returnValue;
        }
        #endregion

        #region Private Methods / Functions
        /// <summary>
        /// Parses the specified original data.
        /// </summary>
        /// <param name="originalData">The original data.</param>
        private void Parse(string? originalData)
        {
            if (!string.IsNullOrEmpty(originalData))
            {
                originalData = originalData.Trim();


                // Remove all spaces.
                originalData = originalData.Replace(" ", "");

                // Parse in specific format if all numbers.
                if (IsNumeric(originalData))
                {
                    ParseAsNumbers(originalData);
                }
                else
                {
                    // Standardize the tokens.
                    originalData = StandardizeTokens(originalData);
                    ParseWithTokens(originalData);
                }
            }
        }
        #endregion

        #region Private Static Methods / Functions
        /// <summary>
        /// Determines whether the specified string data contains only numbers.
        /// </summary>
        /// <param name="data">
        /// The string value to be checked.
        /// </param>
        /// <returns>
        ///   <c>true</c> if the specified string contains only numbers; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsNumeric(string data)
        {
            bool isNumeric = true;

            foreach (char c in data)
            {
                if (!char.IsNumber(c))
                {
                    isNumeric = false;
                }
            }
            return isNumeric;
        }

        private void ParseAsNumbers(string data)
        {
            switch (data.Length)
            {
                case 11:
                    _areaCode = data.Substring(1, 3);
                    _prefix = data.Substring(4, 3);
                    _number = data.Substring(7, 4);
                    break;

                case 10:
                    _areaCode = data[..3];
                    _prefix = data.Substring(3, 3);
                    _number = data.Substring(6, 4);
                    break;

                case 7:
                    _areaCode = null;
                    _prefix = data[..3];
                    _number = data.Substring(3, 4);
                    break;

                default:
                    _areaCode = null;
                    _prefix = null;
                    _number = null;
                    break;
            }

        }
        /// <summary>
        /// Standardizes the tokens from the original string.
        /// </summary>
        /// <param name="original">
        /// The original string value to be modified.
        /// </param>
        /// <returns>
        /// A string containing the original number with the tokens standardized.
        /// </returns>
        private static string? StandardizeTokens(string? original)
        {
            // Replace the ( ) and - delimiters with .
            if (original != null)
            {
                original = original
                    .Replace(CharacterConstants.OpenParen, string.Empty)
                    .Replace(CharacterConstants.CloseParen, CharacterConstants.Dot)
                    .Replace(CharacterConstants.Dash, CharacterConstants.Dot);
            }

            return original;
        }
        /// <summary>
        /// Parses the phone number value with tokens.
        /// </summary>
        /// <param name="data">
        /// A string containing the phone number to be parsed.
        /// </param>
        private void ParseWithTokens(string? data)
        {
            if (data != null)
            {
                string[] parts = data.Split(separator);

                if (parts.Length == 1)
                {
                    string number = data.Replace("+", "");
                    if (IsNumeric(number))
                    {
                        ParseAsNumbers(number);
                    }
                    else
                    {
                        _areaCode = null;
                        _prefix = null;
                        _number = null;
                    }
                }
                else if (parts.Length == 2)
                {
                    _prefix = parts[0];
                    _number = parts[1];
                }
                else if (parts.Length == 3)
                {
                    _areaCode = parts[0];
                    _prefix = parts[1];
                    _number = parts[2];
                }
                else if (parts.Length > 3)
                {
                    int last = parts.Length - 1;
                    _number = parts[last];
                    _prefix = parts[last - 1];
                    _areaCode = parts[last - 2];
                }
                else
                {
                    _areaCode = null;
                    _prefix = null;
                    _number = null;
                }
            }
            else
            {
                _areaCode = null;
                _prefix = null;
                _number = null;
            }
        }

        #endregion
    }
}
