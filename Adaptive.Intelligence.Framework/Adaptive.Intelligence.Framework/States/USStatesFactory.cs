using Adaptive.Intelligence.Enumerations;

namespace Adaptive.Intelligence.States
{
    /// <summary>
    /// Provides static methods/functions for translating <see cref="USStates"/> enumeration values.
    /// </summary>
    public static class USStatesFactory
    {
        #region Private Constants Declarations
        /// <summary>
        /// The list of States.
        /// </summary>
        private static readonly USStateCollection _states;

        #region State Names
        private const string StateNameAlabama = "Alabama";
        private const string StateNameAlaska = "Alaska";
        private const string StateNameAmericanSamoa = "the Territory of American Samoa";
        private const string StateNameArizona = "Arizona";
        private const string StateNameArkansas = "Arkansas";
        private const string StateNameCalifornia = "California";
        private const string StateNameColorado = "Colorado";
        private const string StateNameConnecticut = "Connecticut";
        private const string StateNameDelaware = "Delaware";
        private const string StateNameDistrictOfColumbia = "the District of Columbia";
        private const string StateNameFlorida = "Florida";
        private const string StateNameGeorgia = "Georgia";
        private const string StateNameGuam = "Guam";
        private const string StateNameHawaii = "Hawaii";
        private const string StateNameIdaho = "Idaho";
        private const string StateNameIllinois = "Illinois";
        private const string StateNameIndiana = "Indiana";
        private const string StateNameIowa = "Iowa";
        private const string StateNameKansas = "Kansas";
        private const string StateNameKentucky = "Kentucky";
        private const string StateNameLouisiana = "Louisiana";
        private const string StateNameMaine = "Maine";
        private const string StateNameMaryland = "Maryland";
        private const string StateNameMassachusetts = "Massachusetts";
        private const string StateNameMichigan = "Michigan";
        private const string StateNameMinnesota = "Minnesota";
        private const string StateNameMississippi = "Mississippi";
        private const string StateNameMissouri = "Missouri";
        private const string StateNameMontana = "Montana";
        private const string StateNameNebraska = "Nebraska";
        private const string StateNameNevada = "Nevada";
        private const string StateNameNewHampshire = "New Hampshire";
        private const string StateNameNewJersey = "New Jersey";
        private const string StateNameNewMexico = "New Mexico";
        private const string StateNameNewYork = "New York";
        private const string StateNameNorthCarolina = "North Carolina";
        private const string StateNameNorthDakota = "North Dakota";
        private const string StateNameOhio = "Ohio";
        private const string StateNameOklahoma = "Oklahoma";
        private const string StateNameOregon = "Oregon";
        private const string StateNamePennsylvania = "Pennsylvania";
        private const string StateNameRhodeIsland = "Rhode Island";
        private const string StateNameSouthCarolina = "South Carolina";
        private const string StateNameTennessee = "Tennessee";
        private const string StateNameTexas = "Texas";
        private const string StateNameUSVirginIslands = "the Territory of the US Virgin Islands";
        private const string StateNameUtah = "Utah";
        private const string StateNameVermont = "Vermont";
        private const string StateNameVirginia = "Virginia";
        private const string StateNameWashington = "Washington";
        private const string StateNameWestVirginia = "West Virginia";
        private const string StateNameWisconsin = "Wisconsin";
        private const string StateNameWyoming = "Wyoming";
        #endregion

        #region State Abbreviations
        private const string StateAbbrevAlabama = "AL";
        private const string StateAbbrevAlaska = "AK";
        private const string StateAbbrevAmericanSamoa = "AS";
        private const string StateAbbrevArizona = "AZ";
        private const string StateAbbrevArkansas = "AR";
        private const string StateAbbrevCalifornia = "CA";
        private const string StateAbbrevColorado = "CO";
        private const string StateAbbrevConnecticut = "CT";
        private const string StateAbbrevDelaware = "DE";
        private const string StateAbbrevDistrictOfColumbia = "DC";
        private const string StateAbbrevFlorida = "FL";
        private const string StateAbbrevGeorgia = "GA";
        private const string StateAbbrevGuam = "GU";
        private const string StateAbbrevHawaii = "HI";
        private const string StateAbbrevIdaho = "ID";
        private const string StateAbbrevIllinois = "IL";
        private const string StateAbbrevIndiana = "IN";
        private const string StateAbbrevIowa = "IA";
        private const string StateAbbrevKansas = "KS";
        private const string StateAbbrevKentucky = "KY";
        private const string StateAbbrevLouisiana = "LA";
        private const string StateAbbrevMaine = "ME";
        private const string StateAbbrevMaryland = "MD";
        private const string StateAbbrevMassachusetts = "MA";
        private const string StateAbbrevMichigan = "MI";
        private const string StateAbbrevMinnesota = "MN";
        private const string StateAbbrevMississippi = "MS";
        private const string StateAbbrevMissouri = "MO";
        private const string StateAbbrevMontana = "MT";
        private const string StateAbbrevNebraska = "NE";
        private const string StateAbbrevNevada = "NV";
        private const string StateAbbrevNewHampshire = "NH";
        private const string StateAbbrevNewJersey = "NJ";
        private const string StateAbbrevNewMexico = "NM";
        private const string StateAbbrevNewYork = "NY";
        private const string StateAbbrevNorthCarolina = "NC";
        private const string StateAbbrevNorthDakota = "ND";
        private const string StateAbbrevOhio = "OH";
        private const string StateAbbrevOklahoma = "OK";
        private const string StateAbbrevOregon = "OR";
        private const string StateAbbrevPennsylvania = "PA";
        private const string StateAbbrevRhodeIsland = "RI";
        private const string StateAbbrevSouthCarolina = "SC";
        private const string StateAbbrevTennessee = "TN";
        private const string StateAbbrevTexas = "TX";
        private const string StateAbbrevUSVirginIslands = "VI";
        private const string StateAbbrevUtah = "UT";
        private const string StateAbbrevVermont = "VT";
        private const string StateAbbrevVirginia = "VA";
        private const string StateAbbrevWashington = "WA";
        private const string StateAbbrevWestVirginia = "WV";
        private const string StateAbbrevWisconsin = "WI";
        private const string StateAbbrevWyoming = "WY";
        #endregion

        /// <summary>
        /// The list of State names.
        /// </summary>
        private static readonly string[] _names =
        [
                StateNameAlabama,
                StateNameAlaska,
                StateNameAmericanSamoa,
                StateNameArizona,
                StateNameArkansas,
                StateNameCalifornia,
                StateNameColorado ,
                StateNameConnecticut,
                StateNameDelaware ,
                StateNameDistrictOfColumbia,
                StateNameFlorida,
                StateNameGeorgia,
                StateNameGuam ,
                StateNameHawaii,
                StateNameIdaho ,
                StateNameIllinois ,
                StateNameIndiana ,
                StateNameIowa ,
                StateNameKansas,
                StateNameKentucky ,
                StateNameLouisiana ,
                StateNameMaine ,
                StateNameMaryland ,
                StateNameMassachusetts ,
                StateNameMichigan ,
                StateNameMinnesota ,
                StateNameMississippi,
                StateNameMissouri ,
                StateNameMontana ,
                StateNameNebraska ,
                StateNameNevada ,
                StateNameNewHampshire ,
                StateNameNewJersey,
                StateNameNewMexico,
                StateNameNewYork ,
                StateNameNorthCarolina ,
                StateNameNorthDakota,
                StateNameOhio,
                StateNameOklahoma ,
                StateNameOregon,
                StateNamePennsylvania,
                StateNameRhodeIsland ,
                StateNameSouthCarolina,
                StateNameTennessee ,
                StateNameTexas ,
                StateNameUSVirginIslands ,
                StateNameUtah,
                StateNameVermont ,
                StateNameVirginia ,
                StateNameWashington,
                StateNameWestVirginia ,
                StateNameWisconsin ,
                StateNameWyoming
        ];
        /// <summary>
        /// The list of State postal abbreviations.
        /// </summary>
        private static readonly string[] _abbreviations =
            [
                StateAbbrevAlabama,
                StateAbbrevAlaska,
                StateAbbrevAmericanSamoa,
                StateAbbrevArizona,
                StateAbbrevArkansas,
                StateAbbrevCalifornia,
                StateAbbrevColorado ,
                StateAbbrevConnecticut,
                StateAbbrevDelaware ,
                StateAbbrevDistrictOfColumbia,
                StateAbbrevFlorida,
                StateAbbrevGeorgia,
                StateAbbrevGuam ,
                StateAbbrevHawaii,
                StateAbbrevIdaho ,
                StateAbbrevIllinois ,
                StateAbbrevIndiana ,
                StateAbbrevIowa ,
                StateAbbrevKansas,
                StateAbbrevKentucky ,
                StateAbbrevLouisiana ,
                StateAbbrevMaine ,
                StateAbbrevMaryland ,
                StateAbbrevMassachusetts ,
                StateAbbrevMichigan ,
                StateAbbrevMinnesota ,
                StateAbbrevMississippi,
                StateAbbrevMissouri ,
                StateAbbrevMontana ,
                StateAbbrevNebraska ,
                StateAbbrevNevada ,
                StateAbbrevNewHampshire ,
                StateAbbrevNewJersey,
                StateAbbrevNewMexico,
                StateAbbrevNewYork ,
                StateAbbrevNorthCarolina ,
                StateAbbrevNorthDakota,
                StateAbbrevOhio,
                StateAbbrevOklahoma ,
                StateAbbrevOregon,
                StateAbbrevPennsylvania,
                StateAbbrevRhodeIsland ,
                StateAbbrevSouthCarolina,
                StateAbbrevTennessee ,
                StateAbbrevTexas ,
                StateAbbrevUSVirginIslands ,
                StateAbbrevUtah,
                StateAbbrevVermont ,
                StateAbbrevVirginia ,
                StateAbbrevWashington,
                StateAbbrevWestVirginia ,
                StateAbbrevWisconsin ,
                StateAbbrevWyoming
            ];
        /// <summary>
        /// The list of State code values.
        /// </summary>
        private static readonly USStates[] _codes =
        [
                USStates.Alabama,
                USStates.Alaska,
                USStates.AmericanSamoa,
                USStates.Arizona,
                USStates.Arkansas,
                USStates.California,
                USStates.Colorado ,
                USStates.Connecticut,
                USStates.Delaware ,
                USStates.DistrictOfColumbia,
                USStates.Florida,
                USStates.Georgia,
                USStates.Guam ,
                USStates.Hawaii,
                USStates.Idaho ,
                USStates.Illinois ,
                USStates.Indiana ,
                USStates.Iowa ,
                USStates.Kansas,
                USStates.Kentucky ,
                USStates.Louisiana ,
                USStates.Maine ,
                USStates.Maryland ,
                USStates.Massachusetts ,
                USStates.Michigan ,
                USStates.Minnesota ,
                USStates.Mississippi,
                USStates.Missouri ,
                USStates.Montana ,
                USStates.Nebraska ,
                USStates.Nevada ,
                USStates.NewHampshire ,
                USStates.NewJersey,
                USStates.NewMexico,
                USStates.NewYork ,
                USStates.NorthCarolina ,
                USStates.NorthDakota,
                USStates.Ohio,
                USStates.Oklahoma ,
                USStates.Oregon,
                USStates.Pennsylvania,
                USStates.RhodeIsland ,
                USStates.SouthCarolina,
                USStates.Tennessee ,
                USStates.Texas ,
                USStates.USVirginIslands ,
                USStates.Utah,
                USStates.Vermont ,
                USStates.Virginia ,
                USStates.Washington,
                USStates.WestVirginia ,
                USStates.Wisconsin ,
                USStates.Wyoming
        ];
        #endregion

        #region Static Constructor
        /// <summary>
        /// Initializes the <see cref="USStatesFactory"/> class.
        /// </summary>
        static USStatesFactory()
        {
            _states = [];
            CreateStateList();
        }
        #endregion

        #region Static Properties
        /// <summary>
        /// Gets the reference to the list of US States.
        /// </summary>
        /// <value>
        /// A <see cref="USStateCollection"/> instance containing the list of States.
        /// </value>
        public static USStateCollection States => _states;
        #endregion

        #region Public Static Methods / Functions
        /// <summary>
        /// Gets the name of the specified State.
        /// </summary>
        /// <param name="state">
        /// A <see cref="USStates"/> enumerated value indicating the US State or Territory.
        /// </param>
        /// <returns>
        /// A string containing the name of the State or territory.
        /// </returns>
        public static string? GetStateName(USStates state)
        {
            USState? foundState = _states.FirstOrDefault(stateItem => stateItem.StateCode == state);
            return foundState != null ? foundState.Name : string.Empty;
        }
        /// <summary>
        /// Gets the name of the specified State.
        /// </summary>
        /// <param name="state">
        /// A <see cref="USStates"/> enumerated value indicating the US State or Territory.
        /// </param>
        /// <returns>
        /// A string containing the name of the State or territory.
        /// </returns>
        public static string? GetStateAbbreviation(USStates state)
        {
            USState? foundState = _states.FirstOrDefault(stateItem => stateItem.StateCode == state);
            return foundState != null ? foundState.Abbreviation : string.Empty;
        }
        /// <summary>
        /// Gets the state enumeration for the specified State name or abbreviation.
        /// </summary>
        /// <param name="stateNameOrAbbrev">
        /// The State name or 2-character postal abbreviation.</param>
        /// <returns>
        /// The matching <see cref="USStates"/> enumerated value.
        /// </returns>
        public static USStates GetState(string stateNameOrAbbrev)
        {
            USStates state = USStates.UnknownOrNotSpecified;
            if (!string.IsNullOrEmpty(stateNameOrAbbrev))
            {
                USState? foundState = _states.FirstOrDefault(stateItem => (stateItem.Name == stateNameOrAbbrev) ||
                                                                           (stateItem.Abbreviation == stateNameOrAbbrev));
                if (foundState == null)
                {
                    stateNameOrAbbrev = stateNameOrAbbrev.ToLowerInvariant();
                    foundState = _states.FirstOrDefault(stateItem => (stateItem.Name?.ToLowerInvariant() == stateNameOrAbbrev) ||
                                                                          (stateItem.Abbreviation?.ToLowerInvariant() == stateNameOrAbbrev));
                    if (foundState == null)
                    {
                        stateNameOrAbbrev = stateNameOrAbbrev.ToUpperInvariant();
                        foundState = _states.FirstOrDefault(stateItem => (stateItem.Name?.ToUpperInvariant() == stateNameOrAbbrev) ||
                                                                                  (stateItem.Abbreviation?.ToUpperInvariant() == stateNameOrAbbrev));
                    }
                }
                if (foundState != null)
                {
                    state = foundState.StateCode;
                }
            }
            return state;
        }
        /// <summary>
        /// Gets the state enumeration for the specified State name or abbreviation.
        /// </summary>
        /// <param name="stateNameOrAbbrev">
        /// The State name or 2-character postal abbreviation.</param>
        /// <returns>
        /// The matching <see cref="USStates"/> enumerated value.
        /// </returns>
        public static USState? GetStateInstance(string stateNameOrAbbrev)
        {
            USState? foundState = null;
            if (!string.IsNullOrEmpty(stateNameOrAbbrev))
            {
                foundState = _states.FirstOrDefault(stateItem => (stateItem.Name == stateNameOrAbbrev) ||
                                                                  (stateItem.Abbreviation == stateNameOrAbbrev));
                if (foundState == null)
                {
                    stateNameOrAbbrev = stateNameOrAbbrev.ToLowerInvariant();
                    foundState = _states.FirstOrDefault(stateItem => (stateItem.Name?.ToLowerInvariant() == stateNameOrAbbrev) ||
                                                                      (stateItem.Abbreviation?.ToLowerInvariant() == stateNameOrAbbrev));
                    if (foundState == null)
                    {
                        stateNameOrAbbrev = stateNameOrAbbrev.ToUpperInvariant();
                        foundState = _states.FirstOrDefault(stateItem => (stateItem.Name?.ToUpperInvariant() == stateNameOrAbbrev) ||
                                                                          (stateItem.Abbreviation?.ToUpperInvariant() == stateNameOrAbbrev));
                    }
                }
            }
            return foundState;
        }
        #endregion

        #region Private Static Methods / Functions
        private static void CreateStateList()
        {

            int length = _names.Length;

            for (int count = 0; count < length; count++)
            {
                _states.Add(new USState
                {
                    Abbreviation = _abbreviations[count],
                    Name = _names[count],
                    StateCode = _codes[count],
                    IsState = true,
                    IsSupported = false
                });
            }

            // Set territory flags.
            _states.First(state => state.Abbreviation == StateAbbrevAmericanSamoa).IsState = false;
            _states.First(state => state.Abbreviation == StateAbbrevGuam).IsState = false;
            _states.First(state => state.Abbreviation == StateAbbrevUSVirginIslands).IsState = false;
        }
        #endregion
    }
}