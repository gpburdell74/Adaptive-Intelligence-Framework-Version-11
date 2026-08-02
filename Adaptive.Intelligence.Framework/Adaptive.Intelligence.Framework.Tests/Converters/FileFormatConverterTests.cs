using Adaptive.Intelligence.Constants;
using Adaptive.Intelligence.Converters;
using Adaptive.Intelligence.Enumerations;

namespace Adaptive.Intelligence.Framework.Tests.Converters
{
    /// <summary>
    /// Gets the definition for FileFormatConverterTests.
    /// </summary>
    public class FileFormatConverterTests
    {
        /// <summary>
        /// Gets the definition for _converter.
        /// </summary>
        private readonly FileFormatConverter _converter;

        public FileFormatConverterTests()
        {
            _converter = new FileFormatConverter();
        }

        [Theory]
        [InlineData(FileFormats.Excel, FileExtensionConstants.ExtExcel)]
        [InlineData(FileFormats.WordDocument, FileExtensionConstants.ExtWordDocument)]
        [InlineData(FileFormats.NotSpecified, "")]
        /// <summary>
        /// Gets the definition for Convert_ShouldReturnCorrectExtension.
        /// </summary>
        public void Convert_ShouldReturnCorrectExtension(FileFormats format, string expectedExtension)
        {
            var result = _converter.Convert(format);
            Assert.Equal(expectedExtension, result);
        }

        [Theory]
        [InlineData(FileExtensionConstants.ExtExcel, FileFormats.Excel)]
        [InlineData(FileExtensionConstants.ExtWordDocument, FileFormats.WordDocument)]
        [InlineData("", FileFormats.NotSpecified)]
        /// <summary>
        /// Gets the definition for ConvertBack_ShouldReturnCorrectFileFormat.
        /// </summary>
        public void ConvertBack_ShouldReturnCorrectFileFormat(string extension, FileFormats expectedFormat)
        {
            var result = _converter.ConvertBack(extension);
            Assert.Equal(expectedFormat, result);
        }
        [Fact]
        /// <summary>
        /// Gets the definition for ConvertOddDataTest.
        /// </summary>
        public void ConvertOddDataTest()
        {
            string fileType = _converter.Convert((FileFormats)1003);
            Assert.Equal(string.Empty, fileType);

            string fileType2 = _converter.Convert((FileFormats)(-3));
            Assert.Equal(string.Empty, fileType2);
        }
    }
}