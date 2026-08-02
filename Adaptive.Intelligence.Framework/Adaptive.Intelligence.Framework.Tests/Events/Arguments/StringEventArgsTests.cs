using Adaptive.Intelligence.Events.Arguments;

namespace Adaptive.Intelligence.Framework.Tests.Events.Arguments
{
    /// <summary>
    /// Provides tests for the <see cref="StringEventArgs"/> class.
    /// </summary>
    public class StringEventArgsTests
    {
        [Fact]
        /// <summary>
        /// Gets the definition for Default_Constructor_Initializes_Content_To_Null.
        /// </summary>
        public void Default_Constructor_Initializes_Content_To_Null()
        {
            StringEventArgs args = new();

            Assert.Null(args.Content);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Constructor_With_Content_Sets_Content_Property.
        /// </summary>
        public void Constructor_With_Content_Sets_Content_Property()
        {
            StringEventArgs args = new("hello world");

            Assert.Equal("hello world", args.Content);
        }

        [Fact]
        /// <summary>
        /// Gets the definition for Content_Property_Allows_Read_And_Write.
        /// </summary>
        public void Content_Property_Allows_Read_And_Write()
        {
            StringEventArgs args = new();

            args.Content = "first";
            Assert.Equal("first", args.Content);

            args.Content = "second";
            Assert.Equal("second", args.Content);

            args.Content = null;
            Assert.Null(args.Content);
        }
    }
}