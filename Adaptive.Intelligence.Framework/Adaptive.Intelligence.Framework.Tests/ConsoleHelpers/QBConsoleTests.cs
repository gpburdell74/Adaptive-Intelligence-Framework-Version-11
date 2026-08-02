using Adaptive.Intelligence.ConsoleHelpers;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Adaptive.Intelligence.Framework.Tests.ConsoleHelpers
{
    /// <summary>
    /// Gets the definition for QBConsoleTests.
    /// </summary>
    public class QBConsoleTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for CenterText_WhenContentLongerThanWidth_TruncatesText.
        /// </summary>
        public void CenterText_WhenContentLongerThanWidth_TruncatesText()
        {
            string value = QBConsole.CenterText(5, "123456789");

            Assert.Equal("12345", value);
            Assert.Equal(5, value.Length);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for CenterText_WhenContentShorterThanWidth_PadsLeftAndRight.
        /// </summary>
        public void CenterText_WhenContentShorterThanWidth_PadsLeftAndRight()
        {
            string value = QBConsole.CenterText(10, "abc");

            Assert.Equal("   abc    ", value);
            Assert.Equal(10, value.Length);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for CenterText_WhenContentMatchesWidth_ReturnsOriginalText.
        /// </summary>
        public void CenterText_WhenContentMatchesWidth_ReturnsOriginalText()
        {
            string value = QBConsole.CenterText(4, "test");

            Assert.Equal("test", value);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Print_WhenContentProvided_WritesTextToConsole.
        /// </summary>
        public void Print_WhenContentProvided_WritesTextToConsole()
        {
            string value = CaptureConsoleOutput(() => QBConsole.Print("Hello"));

            Assert.Equal("Hello", value);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Print_WhenContentNullOrEmpty_WritesNothing.
        /// </summary>
        public void Print_WhenContentNullOrEmpty_WritesNothing()
        {
            string value = CaptureConsoleOutput(() =>
            {
                QBConsole.Print(string.Empty);
                QBConsole.Print(null!);
            });

            Assert.Equal(string.Empty, value);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for InitializeCharacterSet_AsciiMode_PopulatesCharacterSet.
        /// </summary>
        public void InitializeCharacterSet_AsciiMode_PopulatesCharacterSet()
        {
            QBConsole console = CreateConsoleWithoutConstructor();

            InvokePrivateMethod<object?>(console, "InitializeCharacterSet", false);
            char[]? characterSet = GetPrivateField<char[]>(console, "_characterSet");

            Assert.NotNull(characterSet);
            Assert.Equal(256, characterSet.Length);
            Assert.Equal('A', characterSet[65]);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for InitializeCharacterSet_AdjustedMode_HandlesEncodingAvailability.
        /// </summary>
        public void InitializeCharacterSet_AdjustedMode_HandlesEncodingAvailability()
        {
            QBConsole console = CreateConsoleWithoutConstructor();

            InvokePrivateMethod<object?>(console, "InitializeCharacterSet", true);
            char[]? characterSet = GetPrivateField<char[]>(console, "_characterSet");

            if (characterSet != null)
            {
                Assert.Equal(256, characterSet.Length);
            }
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Dispose_ClearsCharacterSetReference.
        /// </summary>
        public void Dispose_ClearsCharacterSetReference()
        {
            QBConsole console = CreateConsoleWithoutConstructor();
            InvokePrivateMethod<object?>(console, "InitializeCharacterSet", false);

            console.Dispose();
            char[]? characterSet = GetPrivateField<char[]>(console, "_characterSet");

            Assert.Null(characterSet);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for GetAsciiCharacter_WhenCharacterSetUninitialized_ReturnsNullCharacter.
        /// </summary>
        public void GetAsciiCharacter_WhenCharacterSetUninitialized_ReturnsNullCharacter()
        {
            QBConsole console = CreateConsoleWithoutConstructor();

            char character = InvokePrivateMethod<char>(console, "GetAsciiCharacter", 65);

            Assert.Equal('\0', character);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for GetAsciiCharacter_WhenCharacterSetInitialized_ReturnsAsciiCharacter.
        /// </summary>
        public void GetAsciiCharacter_WhenCharacterSetInitialized_ReturnsAsciiCharacter()
        {
            QBConsole console = CreateConsoleWithoutConstructor();
            InvokePrivateMethod<object?>(console, "InitializeCharacterSet", false);

            char character = InvokePrivateMethod<char>(console, "GetAsciiCharacter", 65);

            Assert.Equal('A', character);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for PrintDoubleBoxHorizontal_WhenWidthSpecified_WritesCharacters.
        /// </summary>
        public void PrintDoubleBoxHorizontal_WhenWidthSpecified_WritesCharacters()
        {
            QBConsole console = CreateConsoleWithoutConstructor();
            InvokePrivateMethod<object?>(console, "InitializeCharacterSet", false);

            string output = CaptureConsoleOutput(() => InvokePrivateMethod<object?>(console, "PrintDoubleBoxHorizontal", 3));

            Assert.Equal(3, output.Length);
            Assert.All(output, c => Assert.NotEqual('\0', c));
            Assert.True(output.Distinct().Count() == 1);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for PrintDoubleBoxWindowRow_WhenWidthTwoOrLess_WritesNothing.
        /// </summary>
        public void PrintDoubleBoxWindowRow_WhenWidthTwoOrLess_WritesNothing()
        {
            QBConsole console = CreateConsoleWithoutConstructor();
            InvokePrivateMethod<object?>(console, "InitializeCharacterSet", false);

            string output = CaptureConsoleOutput(() => InvokePrivateMethod<object?>(console, "PrintDoubleBoxWindowRow", 2));

            Assert.Equal(string.Empty, output);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for PrintDoubleBoxWindowRow_WhenWidthGreaterThanTwo_WritesBordersAndSpaces.
        /// </summary>
        public void PrintDoubleBoxWindowRow_WhenWidthGreaterThanTwo_WritesBordersAndSpaces()
        {
            QBConsole console = CreateConsoleWithoutConstructor();
            InvokePrivateMethod<object?>(console, "InitializeCharacterSet", false);

            string output = CaptureConsoleOutput(() => InvokePrivateMethod<object?>(console, "PrintDoubleBoxWindowRow", 6));

            Assert.Equal(6, output.Length);
            Assert.Equal(output[0], output[^1]);
            Assert.Equal("    ", output[1..^1]);
        }

        /// <summary>
        /// Gets the definition for CreateConsoleWithoutConstructor.
        /// </summary>
        private static QBConsole CreateConsoleWithoutConstructor()
        {
            return (QBConsole)RuntimeHelpers.GetUninitializedObject(typeof(QBConsole));
        }

        /// <summary>
        /// Gets the definition for CaptureConsoleOutput.
        /// </summary>
        private static string CaptureConsoleOutput(Action action)
        {
            TextWriter originalOut = Console.Out;
            using StringWriter writer = new();

            try
            {
                Console.SetOut(writer);
                action();
                return writer.ToString();
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }

        private static T GetPrivateField<T>(object instance, string fieldName)
        {
            FieldInfo? field = instance.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(field);
            return (T)field.GetValue(instance)!;
        }

        private static T InvokePrivateMethod<T>(object instance, string methodName, params object[] parameters)
        {
            MethodInfo? method = instance.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.NotNull(method);
            return (T)method.Invoke(instance, parameters)!;
        }
    }
}