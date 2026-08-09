using Adaptive.Intelligence.Security.Password;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adaptive.Intelligence.Framework.Tests.Security.Password
{
    /// <summary>
    /// Unit Tests for the <see cref="PasswordAnalyzer"/> class.
    /// </summary>
    public class PasswordAnalyzerTests
    {
        [Fact]
        public void VerifyPasswordAndCalculateStrengthExecutes()
        {
            PasswordAnalysisResults results = PasswordAnalyzer.VerifyPasswordAndCalculateStrength("44332211.Xgghuid");

            Assert.NotNull(results);
        }
        [Fact]
        public void VerifyPasswordAndCalculateStrengthExecutesWithEmpty()
        {
            PasswordAnalysisResults results = PasswordAnalyzer.VerifyPasswordAndCalculateStrength(string.Empty);

            Assert.NotNull(results);
        }
        [Fact]
        public void VerifyPasswordAndCalculateStrengthExecutesWithNull()
        {
            PasswordAnalysisResults results = PasswordAnalyzer.VerifyPasswordAndCalculateStrength(null);

            Assert.NotNull(results);
        }
        [Fact]
        public void VerifyPasswordAndCalculateStrengthExecutesShortPwd()
        {
            PasswordAnalysisResults results = PasswordAnalyzer.VerifyPasswordAndCalculateStrength("34");

            Assert.NotNull(results);
            Assert.Equal(0, results.Score);
            Assert.Equal(PasswordScoreRange.Invalid, results.ScoreCategory);
            Assert.False(results.IsValid);
        }
        [Fact]
        public void VerifyPasswordAndCalculateStrengthExecutesWeakPwd()
        {
            PasswordAnalysisResults results = PasswordAnalyzer.VerifyPasswordAndCalculateStrength("34ac.77w1");

            Assert.NotNull(results);
            Assert.True(results.Score >= 70);
            Assert.Equal(PasswordScoreRange.Fair, results.ScoreCategory);
            Assert.False(results.IsValid);
        }
        [Fact]
        public void CalculateStrengthExecutes()
        {
            PasswordAnalysisResults results = PasswordAnalyzer.CalculateStrength("abcde");
            Assert.NotNull(results);
        }
        [Fact]
        public void CalculateStrengthExecutesNull()
        {
            PasswordAnalysisResults results = PasswordAnalyzer.CalculateStrength(null);
            Assert.NotNull(results);
        }
        [Fact]
        public void CalculateStrengthExecutesEmpty()
        {
            PasswordAnalysisResults results = PasswordAnalyzer.CalculateStrength(string.Empty);
            Assert.NotNull(results);
        }

        [InlineData("a", 5)]
        [InlineData("ab", 6)]
        [InlineData("abc", 9)]
        [InlineData("abcd", 12)]
        [InlineData("a.1.2.3.4", 109)]
        [InlineData("a1234", 19)]
        [InlineData("1234", 14)]
        [InlineData("A!1didstuff", 43)]
        [InlineData("password", -7)]
        [InlineData("password1", -26)]
        [InlineData("PAssword1", -26)]
        [InlineData("passskkWWord1", 43)]
        [InlineData("1.X$33211299882.abcxciee", 189)]
        [InlineData("1.X$33211299882.abQ!!@@##$$.xciee", 310)]
        [InlineData("I am bu$y te5ting stuff rignt now", 129)]
        [Theory]
        public void CalculateStrengthTests(string password, int expectedStrength)
        {
            PasswordAnalysisResults results = PasswordAnalyzer.CalculateStrength(password);

            Assert.Equal(expectedStrength, results.Score);
        }
    }
}
