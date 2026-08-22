using Adaptive.Intelligence.Abstractions;

namespace Adaptive.Intelligence.Financial
{
    /// <summary>
    /// Provides a simple implementation for a banking identification number (BIN) table.
    /// </summary>
    /// <seealso cref="DisposableObjectBase" />
    public sealed class BINTable : DisposableObjectBase
    {
        #region Private Member Declarations
        /// <summary>
        /// The rule list.
        /// </summary>
        private BINRuleCollection? _ruleList;
        #endregion

        #region Constructor / Dispose Methods
        /// <summary>
        /// Initializes a new instance of the <see cref="BINTable"/> class.
        /// </summary>
        /// <remarks>
        /// This is the default constructor.
        /// </remarks>
        public BINTable()
        {
            _ruleList = BINRuleFactory.CreateBINRulesList();
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
            _ruleList?.Clear();
            _ruleList = null;
            base.Dispose(disposing);
        }
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Gets the BIN rule that the specified card number matches.
        /// </summary>
        /// <param name="cardNumber">
        /// A string containing the card number to check.
        /// </param>
        /// <returns>
        /// The matching <see cref="BINRule"/>, if found; otherwise, returns <b>null</b>
        /// </returns>
        public BINRule? GetMatchingRule(string cardNumber)
        {
            BINRule? matching = null;

            // Card Number must be at least 6 digits.
            if (_ruleList != null && !string.IsNullOrEmpty(cardNumber) && cardNumber.Length >= 6)
            {
                int count = _ruleList.Count;
                int pos = 0;

                do
                {
                    BINRule rule = _ruleList[pos];
                    if (rule.Matches(cardNumber))
                    {
                        matching = rule;
                    }

                    pos++;

                } while (pos < count && matching == null);
            }

            return matching;
        }
        #endregion
    }
}
