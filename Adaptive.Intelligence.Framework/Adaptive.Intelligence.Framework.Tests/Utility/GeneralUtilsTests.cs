using Adaptive.Intelligence.Constants;
using Adaptive.Intelligence.Utility;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;


namespace Adaptive.Intelligence.Framework.Tests.Utility
{
    /// <summary>
    /// Contains unit tests for <see cref="GeneralUtils"/>.
    /// </summary>
    public class GeneralUtilsTests
    {
        [Fact]
        public void CreateListBlocks_OriginalListIsNull_ReturnsEmptyList()
        {
            // Arrange
            List<int>? originalList = null;

            // Act
            List<List<int>> result = GeneralUtils.CreateListBlocks(originalList, 3);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void CreateListBlocks_OriginalListIsEmpty_ReturnsEmptyList()
        {
            // Arrange
            List<int> originalList = [];

            // Act
            List<List<int>> result = GeneralUtils.CreateListBlocks(originalList, 3);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void CreateListBlocks_BlockSizeGreaterThanLength_ReturnsSingleBlock()
        {
            // Arrange
            List<int> originalList = [1, 2, 3];

            // Act
            List<List<int>> result = GeneralUtils.CreateListBlocks(originalList, 10);

            // Assert
            Assert.Single(result);
            Assert.Equal(originalList, result[0]);
        }

        [Fact]
        public void CreateListBlocks_BlockSizeSmallerThanLength_ReturnsSplitBlocks()
        {
            // Arrange
            List<int> originalList = [1, 2, 3, 4, 5];

            // Act
            List<List<int>> result = GeneralUtils.CreateListBlocks(originalList, 2);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal([1, 2], result[0]);
            Assert.Equal([3, 4], result[1]);
            Assert.Equal([5], result[2]);
        }

        [Fact]
        public void CreateIterationListsFromIdList_ListIsNull_ReturnsEmptyList()
        {
            // Arrange
            List<string>? listOfIdValues = null;

            // Act
            List<string> result = GeneralUtils.CreateIterationListsFromIdList(listOfIdValues);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void CreateIterationListsFromIdList_ListIsEmpty_ReturnsEmptyList()
        {
            // Arrange
            List<string> listOfIdValues = [];

            // Act
            List<string> result = GeneralUtils.CreateIterationListsFromIdList(listOfIdValues);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void CreateIterationListsFromIdList_ItemCountIs100_ReturnsOneQuotedCommaDelimitedEntry()
        {
            // Arrange
            List<string> listOfIdValues = [.. Enumerable.Range(1, 100).Select(index => index.ToString(CultureInfo.InvariantCulture))];

            // Act
            List<string> result = GeneralUtils.CreateIterationListsFromIdList(listOfIdValues);

            // Assert
            Assert.Single(result);
            Assert.Equal($"'{string.Join(",", listOfIdValues)}'", result[0]);
        }

        [Fact]
        public void CreateIterationListsFromIdList_ItemCountIs101_ReturnsOneBatchFromLoopAndNoRemainder()
        {
            // Arrange
            List<string> listOfIdValues = [.. Enumerable.Range(1, 101).Select(index => index.ToString(CultureInfo.InvariantCulture))];

            // Act
            List<string> result = GeneralUtils.CreateIterationListsFromIdList(listOfIdValues);

            // Assert
            Assert.Single(result);
            Assert.Equal($"'{string.Join(",", listOfIdValues)}'", result[0]);
        }

        [Fact]
        public void CreateIterationListsFromIdList_ItemCountGreaterThan101_ReturnsLoopBatchAndRemainderBatch()
        {
            // Arrange
            List<string> listOfIdValues = [.. Enumerable.Range(1, 102).Select(index => index.ToString(CultureInfo.InvariantCulture))];

            // Act
            List<string> result = GeneralUtils.CreateIterationListsFromIdList(listOfIdValues);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal($"'{string.Join(",", listOfIdValues.Take(101))}'", result[0]);
            Assert.Equal("'102'", result[1]);
        }

        [Fact]
        public void GetPluralEnglishForm_WordIsNull_ReturnsEmptyString()
        {
            // Arrange
            string? word = null;

            // Act
            string result = GeneralUtils.GetPluralEnglishForm(word);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetPluralEnglishForm_WordIsEmpty_ReturnsEmptyString()
        {
            // Arrange
            string word = string.Empty;

            // Act
            string result = GeneralUtils.GetPluralEnglishForm(word);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetPluralEnglishForm_WordHasValue_ReturnsPluralizedWord()
        {
            // Arrange
            string word = "cat";

            // Act
            string result = GeneralUtils.GetPluralEnglishForm(word);

            // Assert
            Assert.Equal("cats", result);
        }

        [Fact]
        public void GetSingleEnglishForm_WordIsNull_ReturnsEmptyString()
        {
            // Arrange
            string? word = null;

            // Act
            string result = GeneralUtils.GetSingleEnglishForm(word);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetSingleEnglishForm_WordIsEmpty_ReturnsEmptyString()
        {
            // Arrange
            string word = string.Empty;

            // Act
            string result = GeneralUtils.GetSingleEnglishForm(word);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void GetSingleEnglishForm_WordHasValue_ReturnsSingularizedWord()
        {
            // Arrange
            string word = "cars";

            // Act
            string result = GeneralUtils.GetSingleEnglishForm(word);

            // Assert
            Assert.Equal("car", result);
        }

        [Fact]
        public void EnglishPlural_ValueIsOne_ReturnsUnitTextWithoutPluralText()
        {
            // Arrange
            int value = 1;

            // Act
            string result = GeneralUtils.EnglishPlural(value, "day", "s");

            // Assert
            Assert.Equal("1 day", result);
        }

        [Fact]
        public void EnglishPlural_ValueIsNotOne_ReturnsUnitTextWithPluralText()
        {
            // Arrange
            int value = 2;

            // Act
            string result = GeneralUtils.EnglishPlural(value, "day", "s");

            // Assert
            Assert.Equal("2 days", result);
        }

        [Fact]
        public void EnglishStringAppend_ItemsContainsThreeValues_ReturnsCommaDelimitedWithAnd()
        {
            // Arrange
            string[] items = ["alpha", "beta", "gamma"];

            // Act
            string result = GeneralUtils.EnglishStringAppend(items);

            // Assert
            Assert.Equal("alpha, beta and gamma", result);
        }

        [Fact]
        public void EnglishStringAppend_ItemsIsNull_ReturnsEmptyString()
        {
            // Arrange
            string[]? items = null;

            // Act
            string result = GeneralUtils.EnglishStringAppend(items);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void EnglishStringAppend_ItemsIsEmpty_ReturnsEmptyString()
        {
            // Arrange
            string[] items = Array.Empty<string>();

            // Act
            string result = GeneralUtils.EnglishStringAppend(items);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void EnglishStringAppend_ItemsContainsOnlyWhitespace_ReturnsEmptyString()
        {
            // Arrange
            string[] items = [" ", "\t", ""];

            // Act
            string result = GeneralUtils.EnglishStringAppend(items);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void EnglishStringAppend_ItemsContainsOneNonEmptyValue_ReturnsTrimmedValue()
        {
            // Arrange
            string[] items = ["  alpha  ", " ", string.Empty];

            // Act
            string result = GeneralUtils.EnglishStringAppend(items);

            // Assert
            Assert.Equal("alpha", result);
        }

        [Fact]
        public void EnglishStringAppend_ItemsContainsTwoValues_ReturnsValuesWithAnd()
        {
            // Arrange
            string[] items = [" alpha ", "beta"];

            // Act
            string result = GeneralUtils.EnglishStringAppend(items);

            // Assert
            Assert.Equal("alpha and beta", result);
        }

        [Fact]
        public void IsListNullOrEmpty_ListIsNull_ReturnsTrue()
        {
            // Arrange
            List<int>? listInstance = null;

            // Act
            bool result = GeneralUtils.IsListNullOrEmpty(listInstance);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsListNullOrEmpty_ListIsEmpty_ReturnsTrue()
        {
            // Arrange
            List<int> listInstance = [];

            // Act
            bool result = GeneralUtils.IsListNullOrEmpty(listInstance);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsListNullOrEmpty_ListHasItems_ReturnsFalse()
        {
            // Arrange
            List<int> listInstance = [1];

            // Act
            bool result = GeneralUtils.IsListNullOrEmpty(listInstance);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CreateGuidIdString_WhenCalled_ReturnsDashlessGuidString()
        {
            // Arrange

            // Act
            string result = GeneralUtils.CreateGuidIdString();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.DoesNotContain("-", result, StringComparison.Ordinal);
            Assert.Equal(32, result.Length);
            Assert.True(Guid.TryParseExact(result, "N", out _));
        }

        [Fact]
        public void FindTimeZoneForOffset_StringCannotBeParsed_ReturnsLocalTimeZone()
        {
            // Arrange
            string hoursOffsetStr = "not-a-number";

            // Act
            TimeZoneInfo result = GeneralUtils.FindTimeZoneForOffset(hoursOffsetStr);

            // Assert
            Assert.Equal(TimeZoneInfo.Local.Id, result.Id);
        }

        [Fact]
        public void FindTimeZoneForOffset_StringCanBeParsed_ReturnsSameAsIntegerOverload()
        {
            // Arrange
            int localOffset = TimeZoneInfo.Local.BaseUtcOffset.Hours;
            string hoursOffsetStr = localOffset.ToString(CultureInfo.InvariantCulture);

            // Act
            TimeZoneInfo fromString = GeneralUtils.FindTimeZoneForOffset(hoursOffsetStr);
            TimeZoneInfo fromInt = GeneralUtils.FindTimeZoneForOffset(localOffset);

            // Assert
            Assert.Equal(fromInt.Id, fromString.Id);
        }

        [Fact]
        public void FindTimeZoneForOffset_NoCandidatesFound_ReturnsEasternStandardTime()
        {
            // Arrange
            int hoursOffset = int.MaxValue;

            // Act
            TimeZoneInfo result = GeneralUtils.FindTimeZoneForOffset(hoursOffset, false);

            // Assert
            Assert.Equal("Eastern Standard Time", result.Id);
        }

        [Fact]
        public void FindTimeZoneForOffset_MatchLocalTrue_ReturnsTimeZoneWithMatchingLocalRules()
        {
            // Arrange
            int hoursOffset = TimeZoneInfo.Local.BaseUtcOffset.Hours;

            // Act
            TimeZoneInfo result = GeneralUtils.FindTimeZoneForOffset(hoursOffset, true);

            // Assert
            Assert.Equal(hoursOffset, result.BaseUtcOffset.Hours);
            Assert.True(result.HasSameRules(TimeZoneInfo.Local));
        }

        [Fact]
        public void FindTimeZoneForOffset_MatchLocalFalseWithSingleCandidate_ReturnsThatCandidate()
        {
            // Arrange
            ReadOnlyCollection<TimeZoneInfo> systemList = TimeZoneInfo.GetSystemTimeZones(true);
            IGrouping<int, TimeZoneInfo>? singleGroup = systemList.GroupBy(zone => zone.BaseUtcOffset.Hours)
                .FirstOrDefault(group => group.Count() == 1);

            if (singleGroup == null)
            {
                return;
            }

            int hoursOffset = singleGroup!.Key;
            TimeZoneInfo expected = singleGroup.Single();

            // Act
            TimeZoneInfo result = GeneralUtils.FindTimeZoneForOffset(hoursOffset, false);

            // Assert
            Assert.Equal(expected.Id, result.Id);
        }

        [Fact]
        public void FindTimeZoneForOffset_MatchLocalFalseWithMultipleCandidatesAndUsEntry_ReturnsUsEntry()
        {
            // Arrange
            ReadOnlyCollection<TimeZoneInfo> systemList = TimeZoneInfo.GetSystemTimeZones(true);
            IGrouping<int, TimeZoneInfo>? usGroup = systemList.GroupBy(zone => zone.BaseUtcOffset.Hours)
                .FirstOrDefault(group => group.Count() > 1 && group.Any(zone => zone.DisplayName.Contains("US", StringComparison.Ordinal)));

            if (usGroup == null)
            {
                return;
            }

            int hoursOffset = usGroup!.Key;

            // Act
            TimeZoneInfo result = GeneralUtils.FindTimeZoneForOffset(hoursOffset, false);

            // Assert
            Assert.Contains("US", result.DisplayName, StringComparison.Ordinal);
        }

        [Fact]
        public void FindTimeZoneForOffset_MatchLocalFalseWithMultipleNonUsCandidates_ReturnsFirstCandidate()
        {
            // Arrange
            ReadOnlyCollection<TimeZoneInfo> systemList = TimeZoneInfo.GetSystemTimeZones(true);
            IGrouping<int, TimeZoneInfo>? nonUsGroup = systemList.GroupBy(zone => zone.BaseUtcOffset.Hours)
                .FirstOrDefault(group => group.Count() > 1 && group.All(zone => !zone.DisplayName.Contains("US", StringComparison.Ordinal)));

            if (nonUsGroup == null)
            {
                return;
            }

            int hoursOffset = nonUsGroup!.Key;
            TimeZoneInfo expected = nonUsGroup.First();

            // Act
            TimeZoneInfo result = GeneralUtils.FindTimeZoneForOffset(hoursOffset, false);

            // Assert
            Assert.Equal(expected.Id, result.Id);
        }

        [Fact]
        public void CleanUpPhoneNumber_ValueIsNull_ReturnsEmptyString()
        {
            // Arrange
            string phoneNumber = null!;

            // Act
            string result = phoneNumber.CleanUpPhoneNumber();

            // Assert
            Assert.Equal(string.Empty, result);
        }


        [Fact]
        public void BlankDate_WhenCalled_ReturnsExpectedConstantValue()
        {
            // Arrange

            // Act
            DateTimeOffset result = GeneralUtils.BlankDate();

            // Assert
            Assert.Equal(new DateTimeOffset(1900, 1, 1, 0, 0, 0, TimeSpan.Zero), result);
        }

        [Fact]
        public void BlankDate_WhenCalledMultipleTimes_ReturnsSameValue()
        {
            // Arrange

            // Act
            DateTimeOffset first = GeneralUtils.BlankDate();
            DateTimeOffset second = GeneralUtils.BlankDate();

            // Assert
            Assert.Equal(first, second);
        }

        [Fact]
        public void CleanUpPhoneNumber_ValueIsWhiteSpace_ReturnsEmptyString()
        {
            // Arrange
            string phoneNumber = " \t ";

            // Act
            string result = phoneNumber.CleanUpPhoneNumber();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void CleanUpPhoneNumber_ValueHasNoDigits_ReturnsEmptyString()
        {
            // Arrange
            string phoneNumber = "(abc)-xyz";

            // Act
            string result = phoneNumber.CleanUpPhoneNumber();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void CleanUpPhoneNumber_ValueContainsDigitsAndLeadingOne_RemovesNonDigitsAndLeadingOne()
        {
            // Arrange
            string phoneNumber = "+1 (234) 567-8900";

            // Act
            string result = phoneNumber.CleanUpPhoneNumber();

            // Assert
            Assert.Equal("2345678900", result);
        }

        [Fact]
        public void CleanUpPhoneNumber_ValueContainsDigitsWithoutLeadingOne_RemovesOnlyNonDigits()
        {
            // Arrange
            string phoneNumber = "(234) 567-8900";

            // Act
            string result = phoneNumber.CleanUpPhoneNumber();

            // Assert
            Assert.Equal("2345678900", result);
        }

        [Fact]
        public void ConvertToSQLMoney_ValueIsNull_ReturnsZero()
        {
            // Arrange
            string moneyText = null!;

            // Act
            float result = moneyText.ConvertToSQLMoney();

            // Assert
            Assert.Equal(0F, result);
        }

        [Fact]
        public void ConvertToSQLMoney_ValueIsEmpty_ReturnsZero()
        {
            // Arrange
            string moneyText = string.Empty;

            // Act
            float result = moneyText.ConvertToSQLMoney();

            // Assert
            Assert.Equal(0F, result);
        }

        [Fact]
        public void ConvertToSQLMoney_ValueIsValidWithDollarSign_ReturnsParsedValue()
        {
            // Arrange
            string moneyText = "$123.45";

            // Act
            float result = moneyText.ConvertToSQLMoney();

            // Assert
            Assert.Equal(123.45F, result);
        }

        [Fact]
        public void ConvertToSQLMoney_ValueIsInvalid_ReturnsZero()
        {
            // Arrange
            string moneyText = "$abc";

            // Act
            float result = moneyText.ConvertToSQLMoney();

            // Assert
            Assert.Equal(0F, result);
        }

        [Fact]
        public void ConvertToDouble_ValueIsNull_ReturnsZero()
        {
            // Arrange
            string doubleText = null!;

            // Act
            double result = doubleText.ConvertToDouble();

            // Assert
            Assert.Equal(0D, result);
        }

        [Fact]
        public void ConvertToDouble_ValueIsEmpty_ReturnsZero()
        {
            // Arrange
            string doubleText = string.Empty;

            // Act
            double result = doubleText.ConvertToDouble();

            // Assert
            Assert.Equal(0D, result);
        }

        [Fact]
        public void ConvertToDouble_ValueIsValidWithDollarSign_ReturnsParsedValue()
        {
            // Arrange
            string doubleText = "$123.45";

            // Act
            double result = doubleText.ConvertToDouble();

            // Assert
            Assert.Equal(123.45D, result);
        }

        [Fact]
        public void ConvertToDouble_ValueIsInvalid_ReturnsZero()
        {
            // Arrange
            string doubleText = "$abc";

            // Act
            double result = doubleText.ConvertToDouble();

            // Assert
            Assert.Equal(0D, result);
        }

        [Fact]
        public void ConvertToDecimal_ValueIsNull_ReturnsZero()
        {
            // Arrange
            string decimalText = null!;

            // Act
            decimal result = decimalText.ConvertToDecimal();

            // Assert
            Assert.Equal(0M, result);
        }

        [Fact]
        public void ConvertToDecimal_ValueIsEmpty_ReturnsZero()
        {
            // Arrange
            string decimalText = string.Empty;

            // Act
            decimal result = decimalText.ConvertToDecimal();

            // Assert
            Assert.Equal(0M, result);
        }

        [Fact]
        public void ConvertToDecimal_ValueIsValidWithDollarSign_ReturnsParsedDecimal()
        {
            // Arrange
            string decimalText = "$123.45";

            // Act
            decimal result = decimalText.ConvertToDecimal();

            // Assert
            Assert.Equal(123.45M, result);
        }

        [Fact]
        public void ConvertToDecimal_ValueIsInvalid_ReturnsZero()
        {
            // Arrange
            string decimalText = "$abc";

            // Act
            decimal result = decimalText.ConvertToDecimal();

            // Assert
            Assert.Equal(0M, result);
        }

        [Fact]
        public void FromArgbString_AllComponentsProvided_ReturnsExpectedColor()
        {
            // Arrange
            string a = "80";
            string r = "01";
            string g = "A0";
            string b = "ff";

            // Act
            Color result = GeneralUtils.FromArgbString(a, r, g, b);

            // Assert
            Assert.Equal(128, result.A);
            Assert.Equal(1, result.R);
            Assert.Equal(160, result.G);
            Assert.Equal(255, result.B);
        }

        [Fact]
        public void FromArgbString_SomeComponentsAreEmpty_UsesDefault255ForEmptyComponents()
        {
            // Arrange
            string a = string.Empty;
            string r = "10";
            string g = null!;
            string b = "0A";

            // Act
            Color result = GeneralUtils.FromArgbString(a, r, g, b);

            // Assert
            Assert.Equal(255, result.A);
            Assert.Equal(16, result.R);
            Assert.Equal(255, result.G);
            Assert.Equal(10, result.B);
        }

        [Fact]
        public void HashString_ContentIsNull_ReturnsNull()
        {
            // Arrange
            string originalContent = null!;

            // Act
            string? result = GeneralUtils.HashString(originalContent);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void HashString_ContentIsEmpty_ReturnsNull()
        {
            // Arrange
            string originalContent = string.Empty;

            // Act
            string? result = GeneralUtils.HashString(originalContent);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void HashString_ContentHasValue_ReturnsSha512Base64Hash()
        {
            // Arrange
            string originalContent = "hello world";
            byte[] input = Encoding.UTF8.GetBytes(originalContent);
            byte[] output = SHA512.HashData(input);
            string expected = Convert.ToBase64String(output);

            // Act
            string? result = GeneralUtils.HashString(originalContent);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void LaunchProcess_FileNameIsInvalid_DoesNotThrow()
        {
            // Arrange
            string fileName = "::invalid-path::";

            // Act
            Exception? exception = Record.Exception(() => GeneralUtils.LaunchProcess(fileName));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void LaunchProcess_FileNameIsNull_DoesNotThrow()
        {
            // Arrange
            string fileName = null!;

            // Act
            Exception? exception = Record.Exception(() => GeneralUtils.LaunchProcess(fileName));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void MarkupPhoneNumber_ValueIsNull_ReturnsEmptyString()
        {
            // Arrange
            string phone = null!;

            // Act
            string result = phone.MarkupPhoneNumber();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void MarkupPhoneNumber_ValueIsEmpty_ReturnsEmptyString()
        {
            // Arrange
            string phone = string.Empty;

            // Act
            string result = phone.MarkupPhoneNumber();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void MarkupPhoneNumber_ValueLengthIs10_ReturnsFormattedPhoneNumber()
        {
            // Arrange
            string phone = "2345678900";
            string expected = string.Format(CultureInfo.InvariantCulture, AiConstants.PhoneNumberFormat, "234", "567", "8900");

            // Act
            string result = phone.MarkupPhoneNumber();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void MarkupPhoneNumber_ValueLengthIsNot10_ReturnsOriginalValue()
        {
            // Arrange
            string phone = "2345678";

            // Act
            string result = phone.MarkupPhoneNumber();

            // Assert
            Assert.Equal(phone, result);
        }







        [Fact]
        public void ParseColor_ColorNameStartsWithHashAndHasSixDigits_ReturnsOpaqueColor()
        {
            // Arrange
            string colorName = "#00FF00";

            // Act
            Color result = GeneralUtils.ParseColor(colorName);

            // Assert
            Assert.Equal(255, result.A);
            Assert.Equal(0, result.R);
            Assert.Equal(255, result.G);
            Assert.Equal(0, result.B);
        }

        [Fact]
        public void ParseColor_ColorNameIsEightDigitHex_ReturnsArgbColor()
        {
            // Arrange
            string colorName = "80010203";

            // Act
            Color result = GeneralUtils.ParseColor(colorName);

            // Assert
            Assert.Equal(128, result.A);
            Assert.Equal(1, result.R);
            Assert.Equal(2, result.G);
            Assert.Equal(3, result.B);
        }

        [Fact]
        public void ParseColor_ColorNameIsNotHexLength_ReturnsNamedColor()
        {
            // Arrange
            string colorName = "Blue";

            // Act
            Color result = GeneralUtils.ParseColor(colorName);

            // Assert
            Assert.Equal(Color.Blue.ToArgb(), result.ToArgb());
        }

        [Fact]
        public void SmartTrim_DataIsNull_ReturnsEmptyString()
        {
            // Arrange
            string data = null!;

            // Act
            string result = data.SmartTrim(5);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void SmartTrim_DataIsWhitespace_ReturnsEmptyString()
        {
            // Arrange
            string data = "   \t   ";

            // Act
            string result = data.SmartTrim(5);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void SmartTrim_TrimmedDataExceedsMaxLength_ReturnsTrimmedSubstring()
        {
            // Arrange
            string data = "  abcdef  ";

            // Act
            string result = data.SmartTrim(4);

            // Assert
            Assert.Equal("abcd", result);
        }

        [Fact]
        public void SmartTrim_TrimmedDataWithinMaxLength_ReturnsTrimmedData()
        {
            // Arrange
            string data = "  abc  ";

            // Act
            string result = data.SmartTrim(5);

            // Assert
            Assert.Equal("abc", result);
        }

        [Fact]
        public void ToCustomTextIfNull_YearIsGreaterThan1901_ReturnsFormattedUtcDate()
        {
            // Arrange
            DateTimeOffset value = new(2024, 2, 3, 22, 15, 0, TimeSpan.FromHours(-5));
            DateTimeOffset utcValue = value.ToUniversalTime();
            string expected = string.Format(CultureInfo.InvariantCulture, AiConstants.DateFormat, utcValue);

            // Act
            string result = value.ToCustomTextIfNull("no date");

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToCustomTextIfNull_YearIs1901_ReturnsNoDateText()
        {
            // Arrange
            DateTimeOffset value = new(1901, 12, 31, 0, 0, 0, TimeSpan.Zero);
            string noDateText = "no date";

            // Act
            string result = value.ToCustomTextIfNull(noDateText);

            // Assert
            Assert.Equal(noDateText, result);
        }

        [Fact]
        public void ToCustomTextIfNullWithTime_YearIsGreaterThan1900_ReturnsFormattedUtcDateTime()
        {
            // Arrange
            DateTimeOffset value = new(2024, 2, 3, 22, 15, 0, TimeSpan.FromHours(-5));
            DateTimeOffset utcValue = value.ToUniversalTime();
            string expected = string.Format(CultureInfo.InvariantCulture, AiConstants.DateTimeFormat, utcValue);

            // Act
            string result = value.ToCustomTextIfNullWithTime("no date");

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToCustomTextIfNullWithTime_YearIs1900_ReturnsNoDateText()
        {
            // Arrange
            DateTimeOffset value = new(1900, 12, 31, 0, 0, 0, TimeSpan.Zero);
            string noDateText = "no date";

            // Act
            string result = value.ToCustomTextIfNullWithTime(noDateText);

            // Assert
            Assert.Equal(noDateText, result);
        }

        [Fact]
        public void ToDefaultStringIfNull_ValueIsNull_ReturnsDefaultValue()
        {
            // Arrange
            string value = null!;
            string defaultValue = "fallback";

            // Act
            string result = value.ToDefaultStringIfNull(defaultValue);

            // Assert
            Assert.Equal(defaultValue, result);
        }

        [Fact]
        public void ToDefaultStringIfNull_ValueIsWhitespace_ReturnsDefaultValue()
        {
            // Arrange
            string value = "   ";
            string defaultValue = "fallback";

            // Act
            string result = value.ToDefaultStringIfNull(defaultValue);

            // Assert
            Assert.Equal(defaultValue, result);
        }

        [Fact]
        public void ToDefaultStringIfNull_ValueHasText_ReturnsOriginalValue()
        {
            // Arrange
            string value = " actual ";
            string defaultValue = "fallback";

            // Act
            string result = value.ToDefaultStringIfNull(defaultValue);

            // Assert
            Assert.Equal(value, result);
        }


        [Fact]
        public void ParseColor_ColorNameIsSixDigitHex_ReturnsOpaqueColor()
        {
            // Arrange
            string colorName = "FF0000";

            // Act
            Color result = GeneralUtils.ParseColor(colorName);

            // Assert
            Assert.Equal(255, result.A);
            Assert.Equal(255, result.R);
            Assert.Equal(0, result.G);
            Assert.Equal(0, result.B);
        }

        [Fact]
        public void ToEmptyStringIfNull_ValueIsNull_ReturnsEmptyString()
        {
            // Arrange
            string value = null!;

            // Act
            string result = value.ToEmptyStringIfNull();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ToEmptyStringIfNull_ValueIsWhitespace_ReturnsEmptyString()
        {
            // Arrange
            string value = "   \t";

            // Act
            string result = value.ToEmptyStringIfNull();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ToEmptyStringIfNull_ValueHasText_ReturnsOriginalValue()
        {
            // Arrange
            string value = "text";

            // Act
            string result = value.ToEmptyStringIfNull();

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void ToNoDateTextIfNull_DateTimeOffsetUtcYearIs1900OrEarlier_ReturnsEmptyString()
        {
            // Arrange
            DateTimeOffset value = new(1901, 1, 1, 0, 30, 0, TimeSpan.FromHours(2));

            // Act
            string result = value.ToNoDateTextIfNull();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ToNoDateTextIfNull_DateTimeOffsetUtcYearIsGreaterThan1900_ReturnsFormattedDate()
        {
            // Arrange
            DateTimeOffset value = new(2024, 2, 3, 22, 15, 0, TimeSpan.FromHours(-5));
            DateTimeOffset utcValue = value.ToUniversalTime();
            string expected = string.Format(CultureInfo.InvariantCulture, AiConstants.DateFormat, utcValue);

            // Act
            string result = value.ToNoDateTextIfNull();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToNoDateTextIfNull_DateTimeUtcYearIs1900_ReturnsEmptyString()
        {
            // Arrange
            DateTime value = new(1900, 12, 31, 0, 0, 0, DateTimeKind.Utc);

            // Act
            string result = value.ToNoDateTextIfNull();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void ToNoDateTextIfNull_DateTimeUtcYearIsNot1900_ReturnsFormattedDateTime()
        {
            // Arrange
            DateTime value = new(1899, 12, 31, 11, 5, 0, DateTimeKind.Utc);
            DateTime utcValue = value.ToUniversalTime();
            string expected = string.Format(CultureInfo.InvariantCulture, AiConstants.DateTimeFormat, utcValue);

            // Act
            string result = value.ToNoDateTextIfNull();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToUserTimeZone_ValueOffsetHoursIsZero_AddsLocalUtcOffsetHours()
        {
            // Arrange
            DateTimeOffset value = new(2024, 2, 3, 12, 0, 0, TimeSpan.Zero);
            int localOffsetHours = TimeZoneInfo.Utc.GetUtcOffset(DateTime.Now).Hours;
            DateTimeOffset expected = value.AddHours(localOffsetHours);

            // Act
            DateTimeOffset result = value.ToUserTimeZone();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToUserTimeZone_ValueOffsetHoursIsNonZero_AdjustsByNegativeOffsetHours()
        {
            // Arrange
            DateTimeOffset value = new(2024, 2, 3, 12, 0, 0, TimeSpan.FromHours(3));
            DateTimeOffset expected = value.AddHours(-3);

            // Act
            DateTimeOffset result = value.ToUserTimeZone();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToUserTimeZone_WithOffsetParameter_AdjustsByNegativeOffsetHours()
        {
            // Arrange
            DateTimeOffset value = new(2024, 2, 3, 12, 0, 0, TimeSpan.FromHours(-4));
            int offset = -4;
            DateTimeOffset expected = value.AddHours(4);

            // Act
            DateTimeOffset result = value.ToUserTimeZone(offset);

            // Assert
            Assert.Equal(expected, result);
        }




        [Fact]
        public void ToSqlListString_ValueListIsNull_ReturnsNullTextConstant()
        {
            // Arrange
            List<string>? valueList = null;

            // Act
            string result = GeneralUtils.ToSqlListString(valueList);

            // Assert
            Assert.Equal(AiConstants.NullText, result);
        }


        [Fact]
        public void ToUserTimeZone_StringOffsetIsNumeric_AddsParsedHourOffset()
        {
            // Arrange
            DateTimeOffset value = new(2024, 6, 1, 10, 30, 0, TimeSpan.Zero);
            string offset = "-5";
            DateTimeOffset expected = value.AddHours(-5);

            // Act
            DateTimeOffset result = value.ToUserTimeZone(offset);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToUserTimeZone_StringOffsetIsInvalid_ThrowsFormatException()
        {
            // Arrange
            DateTimeOffset value = new(2024, 6, 1, 10, 30, 0, TimeSpan.Zero);

            // Act
            void action() => value.ToUserTimeZone("invalid");

            // Assert
            Assert.Throws<FormatException>(action);
        }

        [Fact]
        public void ToSqlListString_ValueListIsEmpty_ReturnsNullTextConstant()
        {
            // Arrange
            List<string> valueList = [];

            // Act
            string result = GeneralUtils.ToSqlListString(valueList);

            // Assert
            Assert.Equal(AiConstants.NullText, result);
        }

        [Fact]
        public void ToSqlListString_ValueListHasOneItem_ReturnsItemWithoutComma()
        {
            // Arrange
            List<string> valueList = ["Alpha"];

            // Act
            string result = GeneralUtils.ToSqlListString(valueList);

            // Assert
            Assert.Equal("Alpha", result);
        }

        [Fact]
        public void ToSqlListString_ValueListHasMultipleItems_ReturnsCommaDelimitedString()
        {
            // Arrange
            List<string> valueList = ["Alpha", "Beta", "Gamma"];

            // Act
            string result = GeneralUtils.ToSqlListString(valueList);

            // Assert
            Assert.Equal("Alpha,Beta,Gamma", result);
        }

        [Fact]
        public void SplitList_OriginalListIsEmpty_ReturnsEmptyList()
        {
            // Arrange
            List<int> originalList = [];

            // Act
            List<List<int>> result = GeneralUtils.SplitList(originalList, 2);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void SplitList_ItemCountIsMultipleOfSubListSize_ReturnsEqualSizedLists()
        {
            // Arrange
            List<int> originalList = [1, 2, 3, 4, 5, 6];

            // Act
            List<List<int>> result = GeneralUtils.SplitList(originalList, 3);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal([1, 2, 3], result[0]);
            Assert.Equal([4, 5, 6], result[1]);
        }

        [Fact]
        public void SplitList_ItemCountIsNotMultipleOfSubListSize_ReturnsFinalPartialList()
        {
            // Arrange
            List<int> originalList = [1, 2, 3, 4, 5];

            // Act
            List<List<int>> result = GeneralUtils.SplitList(originalList, 2);

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal([1, 2], result[0]);
            Assert.Equal([3, 4], result[1]);
            Assert.Equal([5], result[2]);
        }

        [Fact]
        public void SplitList_SubListSizeIsGreaterThanItemCount_ReturnsSingleSubList()
        {
            // Arrange
            List<int> originalList = [1, 2, 3];

            // Act
            List<List<int>> result = GeneralUtils.SplitList(originalList, 10);

            // Assert
            Assert.Single(result);
            Assert.Equal(originalList, result[0]);
        }

        [Fact]
        public void SplitList_SubListSizeIsZero_ReturnsSingleListContainingAllItems()
        {
            // Arrange
            List<int> originalList = [1, 2, 3];

            // Act
            List<List<int>> result = GeneralUtils.SplitList(originalList, 0);

            // Assert
            Assert.Single(result);
            Assert.Equal(originalList, result[0]);
        }

        [Fact]
        public void ToStream_ByteArrayProvided_ReturnsStreamContainingSameBytes()
        {
            // Arrange
            byte[] originalData = [1, 2, 3, 4];

            // Act
            MemoryStream result = GeneralUtils.ToStream(originalData);

            // Assert
            Assert.Equal(originalData, result.ToArray());
            Assert.Equal(0, result.Position);
        }

        [Fact]
        public void ToStream_StringProvided_ReturnsReadableStreamAtBeginning()
        {
            // Arrange
            string originalData = "Hello stream";

            // Act
            MemoryStream result = GeneralUtils.ToStream(originalData);
            string text = new StreamReader(result).ReadToEnd();

            // Assert
            Assert.Equal(originalData, text);
            Assert.Equal(result.Length, result.Position);
        }

        [Fact]
        public void ToStream_StringIsNull_ReturnsEmptyStream()
        {
            // Arrange
            string originalData = null!;

            // Act
            MemoryStream result = GeneralUtils.ToStream(originalData);

            // Assert
            Assert.Equal(0, result.Length);
            Assert.Equal(0, result.Position);
        }

    }
}
