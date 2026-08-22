namespace Adaptive.Intelligence.Financial
{
    /// <summary>
    /// Provides the static factory methods and functions for creating BIN table rules.
    /// </summary>
    public static class BINRuleFactory
    {
        #region Private Constants

        #region American Express
        private const string NameAmEx = "American Express";
        private const int AmexMax = 15;
        private const int AmexMin = 15;

        private const string AmexRangeAMin = "340000";
        private const string AmexRangeAMax = "349999";


        private const string AmexRangeBMin = "370000";
        private const string AmexRangeBMax = "379999";
        #endregion

        #region BankCard
        private const string NameBankCard = "BankCard";
        private const int BankCardMax = 16;
        private const int BankCardMin = 16;

        private const string BankCardRangeAMin = "561000";
        private const string BankCardRangeAMax = "561099";

        private const string BankCardRangeBMin = "560221";
        private const string BankCardRangeBMax = "560225";
        #endregion

        #region China Union Pay
        private const string NameChina = "China Union Pay";
        private const int ChinaMin = 16;
        private const int ChinaMax = 19;

        private const string ChinaRangeAMin = "622000";
        private const string ChinaRangeAMax = "622999";


        private const string ChinaRangeBMin = "624000";
        private const string ChinaRangeBMax = "624999";

        private const string ChinaRangeCMin = "626000";
        private const string ChinaRangeCMax = "626999";

        private const string ChinaRangeDMin = "628000";
        private const string ChinaRangeDMax = "628999";

        #endregion

        #region Dankort
        private const string DankortName = "Dankort";
        private const int DankortMax = 16;
        private const int DankortMin = 16;

        private const string DankortRangeAMin = "501900";
        private const string DankortRangeAMax = "501999";
        #endregion

        #region Diners Club
        private const string DinersClubName = "Diner's Club international";
        private const string DinersClubEnRouteName = "Diner's Club enRoute";

        private const int EnRouteMax = 16;
        private const int EnRouteMin = 15;

        private const int DinersClubMax = 18;
        private const int DinersClubMin = 14;

        private const string DinersClubRangeAMin = "201400";
        private const string DinersClubRangeAMax = "201499";

        private const string DinersClubRangeBMin = "360000";
        private const string DinersClubRangeBMax = "360999";

        private const string DinersClubRangeCMin = "300000";
        private const string DinersClubRangeCMax = "300599";

        private const string DinersClubRangeDMin = "309500";
        private const string DinersClubRangeDMax = "309599";

        private const string DinersClubRangeEMin = "380000";
        private const string DinersClubRangeEMax = "399999";
        #endregion

        #region Discover
        private const string DiscoverName = "Discover";
        private const int DiscoverMax = 19;
        private const int DiscoverMin = 16;

        private const string DiscoverRangeAMin = "601100";
        private const string DiscoverRangeAMax = "601199";
        private const string DiscoverRangeBMin = "640000";
        private const string DiscoverRangeBMax = "649999";
        private const string DiscoverRangeCMin = "650000";
        private const string DiscoverRangeCMax = "659999";
        #endregion

        #region Maestro
        private const string MaestroName = "Maestro";
        private const int MaestroMax = 19;
        private const int MaestroMin = 12;

        private const string MaestroRangeAMin = "500000";
        private const string MaestroRangeAMax = "509999";
        #endregion

        #region Master Card
        private const string MCName = "Master Card";
        private const int MCMax = 16;
        private const int MCMin = 16;

        private const string MCRangeAMin = "222100";
        private const string MCRangeAMax = "272099";
        private const string MCRangeBMin = "510000";
        private const string MCRangeBMax = "559999";
        #endregion

        #region VISA
        private const string VisaName = "VISA";
        private const int VisaMax = 19;
        private const int VisaMin = 16;

        private const string VisaRangeAMin = "400000";
        private const string VisaRangeAMax = "499999";
        #endregion

        #endregion

        #region Public Static Factory Methods
        /// <summary>
        /// Creates the list of BIN rules.
        /// </summary>
        /// <returns>
        /// A <see cref="BINRuleCollection"/> containing the standard instances.
        /// </returns>
        public static BINRuleCollection CreateBINRulesList()
        {
            BINRuleCollection ruleList = [];

            AddAmericanExpressCards(ruleList);
            AddBankCards(ruleList);
            AddChinaUnionPay(ruleList);
            AddDankort(ruleList);
            AddDinersClub(ruleList);
            AddDiscover(ruleList);
            AddMaestro(ruleList);
            AddMasterCard(ruleList);
            AddVisa(ruleList);

            return ruleList;
        }
        #endregion

        #region Private Static Methods / Functions
        /// <summary>
        /// Adds the rules for American Express cards.
        /// </summary>
        /// <param name="ruleList">
        /// The <see cref="List{T}"/> of <see cref="BINRule"/> instances to be appended to.
        /// </param>
        private static void AddAmericanExpressCards(List<BINRule> ruleList)
        {
            // American Express.
            Add(ruleList, AmexRangeAMin, AmexRangeAMax, AmexMin, AmexMax, NameAmEx, Resources.AmEx);
            Add(ruleList, AmexRangeBMin, AmexRangeBMax, AmexMin, AmexMax, NameAmEx, Resources.AmEx);
        }
        /// <summary>
        /// Adds the rules for BankCard cards.
        /// </summary>
        /// <param name="ruleList">
        /// The <see cref="List{T}"/> of <see cref="BINRule"/> instances to be appended to.
        /// </param>
        private static void AddBankCards(List<BINRule> ruleList)
        {
            // Bank Card
            Add(ruleList, BankCardRangeAMin, BankCardRangeAMax, BankCardMin, BankCardMax, NameBankCard,
                Resources.Generic_Card);
            Add(ruleList, BankCardRangeBMin, BankCardRangeBMax, BankCardMin, BankCardMax, NameBankCard,
                Resources.Generic_Card);
        }
        /// <summary>
        /// Adds the rules for China Union Pay cards.
        /// </summary>
        /// <param name="ruleList">
        /// The <see cref="List{T}"/> of <see cref="BINRule"/> instances to be appended to.
        /// </param>
        private static void AddChinaUnionPay(List<BINRule> ruleList)
        {
            // China Union Pay
            Add(ruleList, ChinaRangeAMin, ChinaRangeAMax, ChinaMin, ChinaMax, NameChina,
                Resources.China_Union_Pay);
            Add(ruleList, ChinaRangeBMin, ChinaRangeBMax, ChinaMin, ChinaMax, NameChina,
                Resources.China_Union_Pay);
            Add(ruleList, ChinaRangeCMin, ChinaRangeCMax, ChinaMin, ChinaMax, NameChina,
                Resources.China_Union_Pay);
            Add(ruleList, ChinaRangeDMin, ChinaRangeDMax, ChinaMin, ChinaMax, NameChina,
                Resources.China_Union_Pay);
        }
        /// <summary>
        /// Adds the rules for Dankort cards.
        /// </summary>
        /// <param name="ruleList">
        /// The <see cref="List{T}"/> of <see cref="BINRule"/> instances to be appended to.
        /// </param>
        private static void AddDankort(List<BINRule> ruleList)
        {
            // Dankort
            Add(ruleList, DankortRangeAMin, DankortRangeAMax, DankortMin, DankortMax, DankortName,
                Resources.Generic_Card);
        }
        /// <summary>
        /// Adds the rules for Diner's Club cards.
        /// </summary>
        /// <param name="ruleList">
        /// The <see cref="List{T}"/> of <see cref="BINRule"/> instances to be appended to.
        /// </param>
        private static void AddDinersClub(List<BINRule> ruleList)
        {
            // Diners Club
            Add(ruleList, DinersClubRangeAMin, DinersClubRangeAMax, EnRouteMin, EnRouteMax, DinersClubEnRouteName,
                Resources.Diners_Club);
            Add(ruleList, DinersClubRangeBMin, DinersClubRangeBMax, DinersClubMin, DinersClubMax, DinersClubName,
                Resources.Diners_Club);
            Add(ruleList, DinersClubRangeCMin, DinersClubRangeCMax, DinersClubMin, DinersClubMax, DinersClubName,
                Resources.Diners_Club);
            Add(ruleList, DinersClubRangeDMin, DinersClubRangeDMax, DinersClubMin, DinersClubMax, DinersClubName,
                Resources.Diners_Club);
            Add(ruleList, DinersClubRangeEMin, DinersClubRangeEMax, DinersClubMin, DinersClubMax, DinersClubName,
                Resources.Diners_Club);
        }
        /// <summary>
        /// Adds the rules for Discover cards.
        /// </summary>
        /// <param name="ruleList">
        /// The <see cref="List{T}"/> of <see cref="BINRule"/> instances to be appended to.
        /// </param>
        private static void AddDiscover(List<BINRule> ruleList)
        {
            // Discover Card
            Add(ruleList, DiscoverRangeAMin, DiscoverRangeAMax, DiscoverMin, DiscoverMax, DiscoverName,
                Resources.Discover);
            Add(ruleList, DiscoverRangeBMin, DiscoverRangeBMax, DiscoverMin, DiscoverMax, DiscoverName,
                Resources.Discover);
            Add(ruleList, DiscoverRangeCMin, DiscoverRangeCMax, DiscoverMin, DiscoverMax, DiscoverName,
                Resources.Discover);

        }
        /// <summary>
        /// Adds the rules for Maestro (European Master Card) cards.
        /// </summary>
        /// <param name="ruleList">
        /// The <see cref="List{T}"/> of <see cref="BINRule"/> instances to be appended to.
        /// </param>
        private static void AddMaestro(List<BINRule> ruleList)
        {
            // Maestro (European Master Card)
            Add(ruleList, MaestroRangeAMin, MaestroRangeAMax, MaestroMin, MaestroMax, MaestroName,
                Resources.Maestro);
        }

        /// <summary>
        /// Adds the rules for Master Card cards.
        /// </summary>
        /// <param name="ruleList">
        /// The <see cref="List{T}"/> of <see cref="BINRule"/> instances to be appended to.
        /// </param>
        private static void AddMasterCard(List<BINRule> ruleList)
        {
            // Master Card
            Add(ruleList, MCRangeAMin, MCRangeAMax, MCMin, MCMax, MCName, Resources.MasterCard);
            Add(ruleList, MCRangeBMin, MCRangeBMax, MCMin, MCMax, MCName, Resources.MasterCard);

        }
        /// <summary>
        /// Adds the rules for VISA cards.
        /// </summary>
        /// <param name="ruleList">
        /// The <see cref="List{T}"/> of <see cref="BINRule"/> instances to be appended to.
        /// </param>
        private static void AddVisa(List<BINRule> ruleList)
        {
            // VISA
            Add(ruleList, VisaRangeAMin, VisaRangeAMax, VisaMin, VisaMax, VisaName, Resources.Visa);

        }
        /// <summary>
        /// Adds the rule specifications to the provided list.
        /// </summary>
        /// <param name="list">The list to add to.</param>
        /// <param name="prefixMin">
        /// A string containing the minimum six digits to match for the card rule.
        /// </param>
        /// <param name="prefixMax">
        /// A string containing the maximum six digits to match for the card rule.
        /// </param>
        /// <param name="minLength">
        /// The minimum length of the card number for the rule.
        /// </param>
        /// <param name="maxLength">
        /// The maximum length of the card number for the rule.
        /// </param>
        /// <param name="name">
        /// A string containing the name of bank or issuer.
        /// </param>
        /// <param name="image">
        /// A byte array containing the image data to associate with the related card or bank.
        /// </param>
        private static void Add(List<BINRule> list, string prefixMin, string prefixMax, int minLength, int maxLength,
            string name, byte[] image)
        {
            list.Add(new BINRule
            {
                PrefixMin = prefixMin,
                PrefixMax = prefixMax,
                BankOrIssuerName = name,
                CardNumberMinLength = minLength,
                CardNumberMaxLength = maxLength,
                ImageData = image
            });
        }
        #endregion
    }
}
