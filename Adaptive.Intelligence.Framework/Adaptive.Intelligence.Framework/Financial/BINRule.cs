using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Converters;
using Adaptive.Intelligence.Utility;

namespace Adaptive.Intelligence.Financial
{
    /// <summary>
    /// Represents a Banking Identification Number rule for a range of card numbers
    /// and associated Bank.
    /// </summary>
    /// <seealso cref="DisposableObjectBase" />
    public sealed class BINRule : DisposableObjectBase
    {
        #region Private Member Declarations
        private string? _bankOrIssuerName;
        private int _cardNumberMaxLength;
        private int _cardNumberMinLength;
        private byte[]? _imageData;
        private string? _prefixMax;
        private string? _prefixMin;
        #endregion

        #region Constructor / Dispose Methods
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <b>true</b> to release both managed and unmanaged resources;
        /// <b>false</b> to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed && disposing && _imageData != null)
            {
                Array.Clear(_imageData, 0, _imageData.Length);
            }

            _bankOrIssuerName = null;
            _prefixMax = null;
            _prefixMin = null;
            _imageData = null;

            base.Dispose(disposing);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the name of the bank or issuer.
        /// </summary>
        /// <value>
        /// A string containing The name of the bank or issuer, or <b>null</b>.
        /// </value>
        public string? BankOrIssuerName
        {
            get => _bankOrIssuerName;
            set => _bankOrIssuerName = value;
        }
        /// <summary>
        /// Gets or sets the maximum length of the card number.
        /// </summary>
        /// <value>
        /// An integer specifying the maximum length of the card number.
        /// </value>
        public int CardNumberMaxLength
        {
            get => _cardNumberMaxLength;
            set => _cardNumberMaxLength = value;
        }
        /// <summary>
        /// Gets or sets the minimum length of the card number.
        /// </summary>
        /// <value>
        /// An integer specifying the minimum length of the card number.
        /// </value>
        public int CardNumberMinLength
        {
            get => _cardNumberMinLength;
            set => _cardNumberMinLength = value;
        }
        /// <summary>
        /// Gets or sets the reference to the byte array containing related image data.
        /// </summary>
        /// <value>
        /// A byte array containing the image data for the related card or bank, or <b>null</b>.
        /// </value>
        public byte[]? ImageData
        {
            get => ByteArrayUtil.CopyToNewArray(_imageData);
            set
            {
                // Clear the original.
                if (_imageData != null)
                {
                    Array.Clear(_imageData, 0, _imageData.Length);
                    _imageData = null;
                }

                if (value != null)
                {
                    _imageData = ByteArrayUtil.CopyToNewArray(value);
                }
            }
        }
        /// <summary>
        /// Gets or sets the maximum value for the number prefix.
        /// </summary>
        /// <value>
        /// A string containing the maximum first six digits for a match.
        /// </value>
        public string? PrefixMax
        {
            get => _prefixMax;
            set => _prefixMax = value;
        }
        /// <summary>
        /// Gets or sets the minimum value for the number prefix.
        /// </summary>
        /// <value>
        /// A string containing the minimum first six digits for a match.
        /// </value>
        public string? PrefixMin
        {
            get => _prefixMin;
            set => _prefixMin = value;
        }
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Determines if the specified card number matches this rule.
        /// </summary>
        /// <param name="cardNumber">
        /// A string containing the card number.
        /// </param>
        /// <returns>
        /// <b>true</b> if the card number matches this rule; otherwise, returns <b>null</b>.
        /// </returns>
        public bool Matches(string cardNumber)
        {
            bool matches = false;

            if (!string.IsNullOrEmpty(cardNumber))
            {

                int length = cardNumber.Length;

                // Ensure the card number length is in the correct range.
                if (length >= _cardNumberMinLength && length <= _cardNumberMaxLength)
                {
                    // Ensure the card number falls in the specified range.
                    int min = SafeConverter.ToInt32(_prefixMin);
                    int max = SafeConverter.ToInt32(_prefixMax);
                    int v = SafeConverter.ToInt32(cardNumber[..6]);

                    if (v == -1 || min == -1 || max == -1)
                    {
                        matches = false;
                    }
                    else
                    {
                        matches = (v >= min && v <= max);
                    }
                }
            }

            return matches;
        }
        #endregion
    }
}
