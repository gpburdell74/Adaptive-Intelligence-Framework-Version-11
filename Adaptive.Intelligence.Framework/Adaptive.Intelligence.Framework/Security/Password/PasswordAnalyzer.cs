using System.Globalization;

namespace Adaptive.Intelligence.Security.Password
{
    /// <summary>
    /// Provides a static methods for analyzing a password for validation and calculating the
    /// strength of the password.
    /// </summary>
    public static class PasswordAnalyzer
    {
        #region Private Member Declarations
        /// <summary>
        /// The minimum password length rule.
        /// </summary>
        private const int MinimumLength = 8;
        /// <summary>
        /// The maximum password score value that indicates a very weak password.
        /// </summary>
        private const int RangeMaxVeryWeak = 10;
        /// <summary>
        /// The maximum password score value that indicates a weak password.
        /// </summary>
        private const int RangeMaxWeak = 45;
        /// <summary>
        /// The maximum password score value that indicates a fair-to-middlin' password.
        /// </summary>
        private const int RangeMaxFair = 75;
        /// <summary>
        /// The maximum password score value that indicates a strong password.
        /// </summary>
        private const int RangeMaxStrong = 90;
        /// <summary>
        /// The list of special characters.
        /// </summary>
        private const string SpecialChars = "@!#$%^&*.,';:{}[]|~`_-+=";
        /// <summary>
        /// The "top 25 worst passwords of 2016".  Count off the score if this is in the password.
        /// </summary>
        private static readonly string[] BadList =
        [
            "123456",
            "password",
            "12345",
            "12345678",
            "qwerty",
            "123456789",
            "1234",
            "baseball",
            "dragon",
            "football",
            "1234567",
            "monkey",
            "letmein",
            "abc123",
            "111111",
            "mustang",
            "access",
            "shadow",
            "master",
            "michael",
            "superman",
            "696969",
            "123123",
            "batman",
            "trustno1",
            "pazzwurd",
            "iloveyou",
            "nothing",
            "secret",
            "admin",
            "password1",
            "987654321",
            "qwertyuiop",
            "mynoob",
            "123321",
            "666666",
            "18atcskd2w",
            "7777777",
            "1q2w3e4r",
            "654321",
            "555555",
            "3rjs1la7qe",
            "google",
            "1q2w3e4r5t",
            "123qwe",
            "zxcvbnm",
            "1q2w3e",
            "abc123",
            "monkey",
            "football",
            "login",
            "sunshine",
            "master",
            "superman",
            "hello",
            "azure",
            "microsoft",
            "apple",
            "Aa123456",
            "dubsmash",
            "test",
            "princess",
            "BvtTest123",
            "q1w2e3r4t5y6",
            "Chegg123",
            "0987654321",
            "flower",
            "babygirl"
        ];

        #endregion

        #region Public Static Methods / Functions
        /// <summary>
        /// Verifies the specified password is valid due to basic password rules, and if so,
        /// calculates the strength of the password value.
        /// </summary>
        /// <param name="passwordValue">
        /// A string containing the password value to be tested.
        /// </param>
        /// <returns>
        /// A <see cref="PasswordAnalysisResults"/> instance containing the results of
        /// analyzing the password.  If the <see cref="PasswordAnalysisResults.IsValid"/> property
        /// is <b>false</b>, the password failed one or more rules; otherwise, the score is
        /// calculated and returned in the <see cref="PasswordAnalysisResults.Score"/> property.
        /// </returns>
        public static PasswordAnalysisResults VerifyPasswordAndCalculateStrength(string? passwordValue)
        {
            PasswordAnalysisResults result = new();

            if (!string.IsNullOrEmpty(passwordValue))
            {
                if (passwordValue.Length >= MinimumLength)
                {
                    result.HasRequiredLength = true;
                    AnalyzePasswordValue(result, passwordValue);
                }
                else
                {
                    result.Score = 0;
                    result.ScoreCategory = PasswordScoreRange.Invalid;
                    result.IsValid = false;
                }
            }
            return result;

        }
        /// <summary>
        /// Verifies the specified password is valid due to basic password rules, and if so,
        /// calculates the strength of the password value.
        /// </summary>
        /// <param name="passwordValue">
        /// A string containing the password value to be tested.
        /// </param>
        /// <returns>
        /// A <see cref="PasswordAnalysisResults"/> instance containing the results of
        /// analyzing the password.  If the <see cref="PasswordAnalysisResults.IsValid"/> property
        /// is <b>false</b>, the password failed one or more rules; otherwise, the score is
        /// calculated and returned in the <see cref="PasswordAnalysisResults.Score"/> property.
        /// </returns>
        public static PasswordAnalysisResults CalculateStrength(string? passwordValue)
        {
            PasswordAnalysisResults result = new();

            if (!string.IsNullOrEmpty(passwordValue))
            {
                if (passwordValue.Length >= MinimumLength)
                {
                    result.HasRequiredLength = true;
                }

                AnalyzePasswordValue(result, passwordValue);
            }

            return result;
        }
        #endregion

        #region Private Static Methods / Functions
        /// <summary>
        /// Analyzes the provided password value...
        /// </summary>
        /// <param name="result">
        /// The <see cref="PasswordAnalysisResults"/> instance containing the password analysis data.
        /// </param>
        /// <param name="passwordValue">
        /// A string containing the password value to be analyzed.
        /// </param>
        private static void AnalyzePasswordValue(PasswordAnalysisResults result, string passwordValue)
        {
            List<char> lastEncountered = [];

            int lowerCount = 0;
            int upperCount = 0;
            int numberCount = 0;
            int symbolCount = 0;

            int totalConsecutiveUpperCase = 0;
            int totalConsecutiveLowerCase = 0;

            int consecutiveUpperCase = 0;
            int consecutiveLowerCase = 0;
            int middleNumberOrSymbols = 0;

            int sequentialLetterCount = 0;
            int sequentialNumberCount = 0;
            int sequentialSymbolCount = 0;

            char prevChar = (char)0;
            int repeatCount = 0;

            // Analyze each character.
            foreach (char character in passwordValue)
            {
                // Count if the previous character is repeated.
                if (character == prevChar)
                {
                    repeatCount++;
                }

                prevChar = character;

                // If a lower case letter is present...
                if (char.IsLower(character))
                {
                    result.HasLowerCaseCharacter = true;
                    lowerCount++;
                    consecutiveLowerCase++;

                    // The consecutive upper case letters are counted.  If there
                    // is more than one, store this information.  This is discounted
                    // against the password strength.
                    if (consecutiveUpperCase > 1)
                    {
                        totalConsecutiveUpperCase += consecutiveUpperCase;
                    }

                    // Reset the consecutive upper case letter count since we encountered a lower
                    // case character.
                    consecutiveUpperCase = 0;
                }

                // If an upper case letter is present...
                if (char.IsUpper(character))
                {
                    result.HasUpperCaseCharacter = true;
                    upperCount++;
                    consecutiveUpperCase++;

                    // The consecutive lower case letters are counted.  If there
                    // is more than one, store this information.  This is discounted
                    // against the password strength.
                    if (consecutiveLowerCase > 1)
                    {
                        totalConsecutiveUpperCase += consecutiveLowerCase;
                    }

                    // Reset the consecutive lower case letter count since we encountered an upper
                    // case character.
                    consecutiveLowerCase = 0;
                }

                // If a special character is encountered...
                if (SpecialChars.Contains(character.ToString()))
                {
                    result.HasSpecialCharacter = true;
                    symbolCount++;

                    // Determine the index of the character.  If the index is in the "middle" of the
                    // list of characters, count this as a positive toward password strength.
                    int index = SpecialChars.IndexOf(character);
                    if (index is >= 3 and <= 19)
                    {
                        middleNumberOrSymbols++;
                    }

                    // The consecutive upper case letters are counted.  If there
                    // is more than one, store this information.  This is discounted
                    // against the password strength.
                    if (consecutiveUpperCase > 1)
                    {
                        totalConsecutiveUpperCase += consecutiveUpperCase;
                    }

                    // The consecutive lower case letters are counted.  If there
                    // is more than one, store this information.  This is discounted
                    // against the password strength.
                    if (consecutiveLowerCase > 1)
                    {
                        totalConsecutiveLowerCase += consecutiveLowerCase;
                    }

                    // Reset the consecutive lower case letter count since we encountered a special
                    // character.
                    consecutiveUpperCase = 0;
                    consecutiveLowerCase = 0;
                }

                // If a number is encountered...
                if (char.IsNumber(character))
                {
                    result.HasNumber = true;
                    numberCount++;
                    int number = Convert.ToInt32(character);

                    // Determine the index of the character.  If the index is in the "middle" of the
                    // list of numbers, count this as a positive toward password strength.
                    if (number is >= 3 and < 8)
                    {
                        middleNumberOrSymbols++;
                    }

                    // The consecutive upper case letters are counted.  If there
                    // is more than one, store this information.  This is discounted
                    // against the password strength.
                    if (consecutiveUpperCase > 1)
                    {
                        totalConsecutiveUpperCase += consecutiveUpperCase;
                    }

                    // The consecutive lower case letters are counted.  If there
                    // is more than one, store this information.  This is discounted
                    // against the password strength.
                    if (consecutiveLowerCase > 1)
                    {
                        totalConsecutiveLowerCase += consecutiveLowerCase;
                    }

                    // Reset the consecutive lower case letter count since we encountered a special
                    // character.
                    consecutiveUpperCase = 0;
                    consecutiveLowerCase = 0;

                }

                // Store the last character encountered, so we can count sequential entries.
                // (Such as "1234" or "ABCDE").
                // No processing required if this is the first character.
                if (lastEncountered.Count == 0)
                {
                    lastEncountered.Add(character);
                }
                else
                {

                    // If this is a sequential duplicate, store it.
                    char lastChar = char.ToLower(lastEncountered[^1], CultureInfo.InvariantCulture);

                    // Calculate the next "sequential" character.  Test to ensure we don't go over
                    // the ASCII char limit (255).
                    char nextChar = (char)255;
                    if (lastChar < 254)
                    {
                        nextChar = (char)(lastChar + 1);
                    }

                    // If the password character matches, store it.
                    if (nextChar == character)
                    {
                        lastEncountered.Add(character);
                    }
                    else if (char.IsSymbol(character) && char.IsSymbol(lastChar))
                    {
                        // Separate test is required for "symbol" characters.
                        lastEncountered.Add(character);
                    }
                    else
                    {
                        // If not, store the number of sequential duplicates, and clear the list to start
                        // over again.
                        int len = lastEncountered.Count;
                        lastEncountered.Clear();
                        if (len > 1)
                        {
                            if (char.IsUpper(character) || char.IsLower(character))
                            {
                                sequentialLetterCount += len;
                            }
                            else if (char.IsNumber(character))
                            {
                                sequentialNumberCount += len;
                            }
                            else if (SpecialChars.Contains(character.ToString()))
                            {
                                sequentialSymbolCount += len;
                            }
                        }
                    }
                }
            }

            // Ensure these last bits are added.
            if (consecutiveUpperCase > 1)
            {
                totalConsecutiveUpperCase += consecutiveUpperCase;
            }

            if (consecutiveLowerCase > 1)
            {
                totalConsecutiveLowerCase += consecutiveLowerCase;
            }

            // Calculate the raw score.
            int passwordScore = CalculateScore(passwordValue.Length, lowerCount, upperCount, numberCount, symbolCount,
                repeatCount, totalConsecutiveUpperCase, totalConsecutiveLowerCase, middleNumberOrSymbols, sequentialLetterCount,
                sequentialNumberCount, sequentialSymbolCount);

            // Deduct points if the password is in, or contains an entry from, the "Worst Passwords of 2016"
            // list.
            string lower = passwordValue.ToLower(CultureInfo.InvariantCulture);
            foreach (string bad in BadList)
            {
                if (lower.Contains(bad))
                {
                    passwordScore -= 30;
                }
            }

            // Store the final results.
            result.IsValid =
                result.HasLowerCaseCharacter && result.HasRequiredLength &&
                result.HasSpecialCharacter && result.HasUpperCaseCharacter &&
                result.HasNumber;

            result.Score = passwordScore;
            result.ScoreCategory = GetScoreCategory(result.Score);


        }
        /// <summary>
        /// Calculates the score of the password generally on a scale of 0 - 100.
        /// </summary>
        /// <remarks>
        /// For extremely good passwords, score may exceed 100.
        /// </remarks>
        /// <param name="passwordLength">
        /// The number of characters in the password.
        /// </param>
        /// <param name="lowerCount">
        /// The number of lower-case characters in the password.
        /// </param>
        /// <param name="upperCount">
        /// The number of upper-case characters in the password.
        /// </param>
        /// <param name="numberCount">
        /// The number of number characters in the password.
        /// </param>
        /// <param name="repeatCount">
        /// The number of times any character was repeated in the password.
        /// </param>
        /// <param name="symbolCount">
        /// The number of special characters in the password.
        /// </param>
        /// <param name="consecutiveUpperCase">
        /// The number of upper-case characters in the password that are repeated (i.e. "AAAAA");
        /// </param>
        /// <param name="consecutiveLowerCase">
        /// The number of lower-case characters in the password that are repeated (i.e. "aaaaa");
        /// </param>
        /// <param name="middleNumberOrSymbols">
        /// The number of middle number of symbol character selections.
        /// </param>
        /// <param name="sequentialLetterCount">
        /// The number of letter characters in the password that are sequential (i.e. "AbCDEf");
        /// </param>
        /// <param name="sequentialNumberCount">
        /// The number of number characters in the password that are sequential (i.e. "1234");
        /// </param>
        /// <param name="sequentialSymbolCount">
        /// The number of number characters in the password that are sequential (i.e. "@!#$%^&amp;*()");
        /// </param>
        /// <returns></returns>
        private static int CalculateScore(int passwordLength, int lowerCount, int upperCount,
            int numberCount, int symbolCount, int repeatCount, int consecutiveUpperCase, int consecutiveLowerCase, int middleNumberOrSymbols,
            int sequentialLetterCount, int sequentialNumberCount, int sequentialSymbolCount)
        {
            int positive = CalculatePositives(passwordLength, upperCount, lowerCount, numberCount,
                symbolCount, middleNumberOrSymbols);

            int negative = CalculateNegatives(repeatCount, consecutiveUpperCase, consecutiveLowerCase,
                sequentialLetterCount, sequentialNumberCount, sequentialSymbolCount);

            int result = positive + negative;
            return result;
        }
        /// <summary>
        /// Calculates the positive values for the password score.
        /// </summary>
        /// <returns>
        /// An integer containing the sum of the result.
        /// </returns>
        private static int CalculatePositives(int passwordLength, int upperCaseLetterCount, int lowerCaseLetterCount,
            int numbersCount, int symbolsCount, int middleNumbersOrSymbolsCount)
        {
            return
                CalcLengthBonus(passwordLength) +
                CalcUpperCaseLettersBonus(passwordLength, upperCaseLetterCount) +
                CalcLowerCaseLettersBonus(passwordLength, lowerCaseLetterCount) +
                CalcNumbersBonus(numbersCount) +
                CalcSymbolsBonus(symbolsCount) +
                CalcMiddleNumbersOrSymbolsBonus(middleNumbersOrSymbolsCount);
        }
        /// <summary>
        /// Calculates the positive values for the password score.
        /// </summary>
        /// <returns>
        /// An integer containing the sum of the result.
        /// </returns>
        private static int CalculateNegatives(int repeatingCharCount, int consecutiveUpperCaseLetterCount,
            int consecutiveLowerCaseLetterCount, int sequentialLetterCount, int sequentialNumberCount,
            int sequentialSymbolCount)
        {
            return
                RepeatingCharacterCountBonus(repeatingCharCount) +
                CalcConsecutiveUpperCaseLetterCountBonus(consecutiveUpperCaseLetterCount) +
                CalcConsecutiveLowerCaseLetterCountBOnus(consecutiveLowerCaseLetterCount) +
                CalcSequentialLetterCountBonus(sequentialLetterCount) +
                CalcSequentialNumberCountBonus(sequentialNumberCount) +
                CalcSequentialSymbolCountBonus(sequentialSymbolCount);
        }

        #region Positive Score Functions
        private static int CalcLengthBonus(int passwordLength)
        {
            // +(n * 3)
            return passwordLength * 3;
        }
        private static int CalcUpperCaseLettersBonus(int passwordLength, int upperCaseLetters)
        {
            // +((len-n)*2
            return (passwordLength - upperCaseLetters) * 2;
        }
        private static int CalcLowerCaseLettersBonus(int passwordLength, int lowerCaseLetters)
        {
            // +((len-n)*2
            return (passwordLength - lowerCaseLetters) * 2;
        }
        private static int CalcNumbersBonus(int numberOfNumbers)
        {
            // +(n*4
            return numberOfNumbers * 4;
        }
        private static int CalcSymbolsBonus(int symbolsCount)
        {
            // +(n*6)
            return symbolsCount * 6;
        }
        private static int CalcMiddleNumbersOrSymbolsBonus(int middleItemsCount)
        {
            // +(n*2)
            return middleItemsCount * 2;
        }
        #endregion

        #region Negative Score Functions
        private static int RepeatingCharacterCountBonus(int repeatingCharCount)
        {
            // -n
            return -repeatingCharCount;
        }
        private static int CalcConsecutiveUpperCaseLetterCountBonus(int consecutiveUpperCaseLetterCount)
        {
            // -(n * 2)
            return consecutiveUpperCaseLetterCount * -2;
        }
        private static int CalcConsecutiveLowerCaseLetterCountBOnus(int consecutiveLowerCaseLetterCoint)
        {
            // -(n * 2)
            return consecutiveLowerCaseLetterCoint * -2;
        }
        private static int CalcSequentialLetterCountBonus(int sequentialLetterCount)
        {
            // -(n * 3)
            return sequentialLetterCount * -3;
        }
        private static int CalcSequentialNumberCountBonus(int sequentialNumberCount)
        {
            // -(n * 3)
            return sequentialNumberCount * -3;
        }
        private static int CalcSequentialSymbolCountBonus(int sequentialSymbolCount)
        {
            // -(n * 3)
            return sequentialSymbolCount * -3;
        }
        #endregion

        /// <summary>
        /// Gets the score category base on the integer value.
        /// </summary>
        /// <param name="score">The password score value.</param>
        /// <returns>
        /// A <see cref="PasswordScoreRange"/> enumerated value indicating the strength of the password
        /// based on its score.
        /// </returns>
        private static PasswordScoreRange GetScoreCategory(int score)
        {
            PasswordScoreRange range = PasswordScoreRange.NotAnalyzed;

            if (score >= 0)
            {
                if (score <= RangeMaxVeryWeak)
                {
                    range = PasswordScoreRange.VeryWeak;
                }
                else if (score < RangeMaxWeak)
                {
                    range = PasswordScoreRange.Weak;
                }
                else if (score < RangeMaxFair)
                {
                    range = PasswordScoreRange.Fair;
                }
                else if (score < RangeMaxStrong)
                {
                    range = PasswordScoreRange.Strong;
                }
                else
                {
                    range = PasswordScoreRange.VeryStrong;
                }
            }
            return range;
        }
        #endregion
    }

}


