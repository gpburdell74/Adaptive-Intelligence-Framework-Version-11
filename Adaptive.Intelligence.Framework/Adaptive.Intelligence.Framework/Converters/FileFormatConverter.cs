using Adaptive.Intelligence.Constants;
using System.Collections.Frozen;
using Adaptive.Intelligence.Abstractions;
using Adaptive.Intelligence.Enumerations;

namespace Adaptive.Intelligence.Converters
{
    /// <summary>
    /// Provides a class for translating the <see cref="FileFormats"/> enumerated value and file extensions to
    /// and from each representation.
    /// </summary>
    /// <seealso cref="FileFormats" />
    /// <seealso cref="IValueConverter{F, T}" />
    public sealed class FileFormatConverter : IValueConverter<FileFormats, string>
    {
        #region Private Member Declarations
        /// <summary>
        /// The conversion lookup table.
        /// </summary>
        private static readonly FrozenDictionary<FileFormats, string> _convertTable;
        /// <summary>
        /// The re-conversion lookup table.
        /// </summary>
        private static readonly FrozenDictionary<string, FileFormats> _reConvertTable;
        #endregion

        #region Static Constructor 
        /// <summary>
        /// Performs static data initialization for the <see cref="FileFormatConverter"/> class.
        /// </summary>
        /// <remarks>
        /// This is the static constructor.
        /// </remarks>
        static FileFormatConverter()
        {
            _convertTable = CreateConversionTable();
            _reConvertTable = CreateReconversionTable();
        }
        #endregion

        #region Public Methods / Functions
        /// <summary>
        /// Converts the original enumerated file format value to its equivalent file extension string.
        /// </summary>
        /// <param name="originalValue">
        /// The <see cref="FileFormats"/> enumerated value to be converted.
        /// </param>
        /// <returns>
        /// A string containing the file extension value for the specified type.
        /// </returns>
        public string Convert(FileFormats originalValue)
        {
            _convertTable.TryGetValue(originalValue, out string? extension);
            return extension ?? string.Empty;
        }

        /// <summary>
        /// Converts the file extension string to the matching <see cref="FileFormats"/> enumerated value.
        /// </summary>
        /// <param name="convertedValue">
        /// A string containing the original file extension value to be converted.
        /// </param>
        /// <returns>
        /// The matching <see cref="FileFormats"/> enumerated value.
        /// </returns>
        /// <remarks>
        /// The implementation of this method is be the inverse of the <see cref="Convert" /> method.
        /// </remarks>
        public FileFormats ConvertBack(string convertedValue)
        {
            FileFormats value = FileFormats.NotSpecified;
            if (!string.IsNullOrEmpty(convertedValue))
            {
                // Remove the leading ".", if present... ensure the whole thing is lowercase for comparison.
                if (convertedValue[0] == '.')
                {
                    convertedValue = convertedValue[1..];
                }

                _reConvertTable.TryGetValue(convertedValue.ToLowerInvariant(), out value);
            }

            return value;
        }
        #endregion

        #region Private Static Methods / Functions
        /// <summary>
        /// Creates the static look up table for the data conversion.
        /// </summary>
        private static FrozenDictionary<FileFormats, string> CreateConversionTable()
        {
            var dictionary = new Dictionary<FileFormats, string>
            {
                { FileFormats.NotSpecified, string.Empty },
                { FileFormats.ExcelLegacy, FileExtensionConstants.ExtExcelLegacy},
                { FileFormats.Excel, FileExtensionConstants.ExtExcel},
                { FileFormats.ExcelMacro, FileExtensionConstants.ExtExcelMacro},
                { FileFormats.ExcelMacroLegacy, FileExtensionConstants.ExtExcelMacroLegacy},
                { FileFormats.ExcelTemplate, FileExtensionConstants.ExtExcelTemplate},
                { FileFormats.ExcelTemplateLegacy, FileExtensionConstants.ExtExcelTemplateLegacy},
                { FileFormats.MSAccessLegacyDatabase, FileExtensionConstants.ExtMSAccessLegacyDatabase},
                { FileFormats.MSAccessDatabase, FileExtensionConstants.ExtMSAccessDatabase},
                { FileFormats.OneNote, FileExtensionConstants.ExtOneNote},
                { FileFormats.OneNoteExport, FileExtensionConstants.ExtOneNoteExport},
                { FileFormats.PowerPointLegacy, FileExtensionConstants.ExtPowerPointLegacy},
                { FileFormats.PowerPoint, FileExtensionConstants.ExtPowerPoint},
                { FileFormats.Publisher, FileExtensionConstants.ExtPublisher},
                { FileFormats.WordDocumentLegacy, FileExtensionConstants.ExtWordDocumentLegacy},
                { FileFormats.WordDocument, FileExtensionConstants.ExtWordDocument},
                { FileFormats.WordMacro, FileExtensionConstants.ExtWordMacro},
                { FileFormats.WordLegacyMacro, FileExtensionConstants.ExtWordLegacyMacro},
                { FileFormats.WordTemplate, FileExtensionConstants.ExtWordTemplate},
                { FileFormats.WordTemplateLegacy, FileExtensionConstants.ExtWordTemplateLegacy},
                { FileFormats.Xps, FileExtensionConstants.ExtXps},
                { FileFormats.CommaSeparatedValues, FileExtensionConstants.ExtCommaSeparatedValues},
                { FileFormats.JavascriptObjectNotation, FileExtensionConstants.ExtJavascriptObjectNotation},
                { FileFormats.RichTextFile, FileExtensionConstants.ExtRichTextFile},
                { FileFormats.TextFile, FileExtensionConstants.ExtTextFile},
                { FileFormats.BatchFile, FileExtensionConstants.ExtBatchFile},
                { FileFormats.CommandConsoleFile, FileExtensionConstants.ExtCommandConsoleFile},
                { FileFormats.CommandFile, FileExtensionConstants.ExtCommandFile},
                { FileFormats.ControlPanel, FileExtensionConstants.ExtControlPanel},
                { FileFormats.DOSCommandFile, FileExtensionConstants.ExtDOSCommandFile},
                { FileFormats.DynamicLinkedLibrary, FileExtensionConstants.ExtDynamicLinkedLibrary},
                { FileFormats.ExecutableFile, FileExtensionConstants.ExtExecutableFile},
                { FileFormats.ImageFile, FileExtensionConstants.ExtImageFile},
                { FileFormats.NationalLanguageService, FileExtensionConstants.ExtNationalLanguageService},
                { FileFormats.PowerShellFile, FileExtensionConstants.ExtPowerShellFile},
                { FileFormats.RegistryFile, FileExtensionConstants.ExtRegistryFile},
                { FileFormats.SystemFile, FileExtensionConstants.ExtSystemFile},
                { FileFormats.WindowsInstallerFile, FileExtensionConstants.ExtWindowsInstallerFile},
                { FileFormats.Bitmap, FileExtensionConstants.ExtBitmap},
                { FileFormats.GenericImage, FileExtensionConstants.ExtGenericImage},
                { FileFormats.GraphicsInterchangeFormat, FileExtensionConstants.ExtGraphicsInterchangeFormat},
                { FileFormats.JointPhotographicExpertsGroup, FileExtensionConstants.ExtJointPhotographicExpertsGroup},
                { FileFormats.JointPhotographicExpertsGroupLong, FileExtensionConstants.ExtJointPhotographicExpertsGroupLong},
                { FileFormats.PhotoShop, FileExtensionConstants.ExtPhotoShop},
                { FileFormats.PortableNetworkGraphics, FileExtensionConstants.ExtPortableNetworkGraphics},
                { FileFormats.RawImage, FileExtensionConstants.ExtRawImage},
                { FileFormats.TaggedImageFileFormat, FileExtensionConstants.ExtTaggedImageFileFormat},
                { FileFormats.WindowsIcon, FileExtensionConstants.ExtWindowsIcon},
                { FileFormats.MovieFile, FileExtensionConstants.ExtMovieFile},
                { FileFormats.MPeg, FileExtensionConstants.ExtMPeg},
                { FileFormats.MPegLong, FileExtensionConstants.ExtMPegLong},
                { FileFormats.MPegV2, FileExtensionConstants.ExtMPegV2},
                { FileFormats.MPegV3, FileExtensionConstants.ExtMPegV3},
                { FileFormats.MPegV4, FileExtensionConstants.ExtMPegV4},
                { FileFormats.WindowsMovieFile, FileExtensionConstants.ExtWindowsMovieFile},
                { FileFormats.Cabinet, FileExtensionConstants.ExtCabinet},
                { FileFormats.GZip, FileExtensionConstants.ExtGZip},
                { FileFormats.PkZip, FileExtensionConstants.ExtPkZip},
                { FileFormats.Tar, FileExtensionConstants.ExtTar},
                { FileFormats.TarGZip, FileExtensionConstants.ExtTarGZip},
                { FileFormats.AspNetFile, FileExtensionConstants.ExtAspNetFile},
                { FileFormats.CPlusPlusProject, FileExtensionConstants.ExtCPlusPlusProject},
                { FileFormats.CPlusPlusFile, FileExtensionConstants.ExtCPlusPlusFile},
                { FileFormats.CFile, FileExtensionConstants.ExtCFile},
                { FileFormats.CSharpProject, FileExtensionConstants.ExtCSharpProject},
                { FileFormats.DumpFile, FileExtensionConstants.ExtDumpFile},
                { FileFormats.GitFile, FileExtensionConstants.ExtGitFile},
                { FileFormats.HeaderFile, FileExtensionConstants.ExtHeaderFile},
                { FileFormats.HtmlFile, FileExtensionConstants.ExtHtmlFile},
                { FileFormats.JavascriptFile, FileExtensionConstants.ExtJavascriptFile},
                { FileFormats.ProgramDatabase, FileExtensionConstants.ExtProgramDatabase},
                { FileFormats.PrecompiledHeader, FileExtensionConstants.ExtPrecompiledHeader},
                { FileFormats.Solution, FileExtensionConstants.ExtSolution},
                { FileFormats.SourceControlFile, FileExtensionConstants.ExtSourceControlFile},
                { FileFormats.SqlFile, FileExtensionConstants.ExtSqlFile},
                { FileFormats.SqlServerDatabase, FileExtensionConstants.ExtSqlServerDatabase},
                { FileFormats.SqlServerDatabaseLog, FileExtensionConstants.ExtSqlServerDatabaseLog},
                { FileFormats.TypeScriptFile, FileExtensionConstants.ExtTypeScriptFile},
                { FileFormats.VisualBasicFile, FileExtensionConstants.ExtVisualBasicFile},
                { FileFormats.VisualBasicProject, FileExtensionConstants.ExtVisualBasicProject},
                { FileFormats.XamlFile, FileExtensionConstants.ExtXamlFile},
                { FileFormats.XmlFile, FileExtensionConstants.ExtXmlFile},
                { FileFormats.BinaryFile, FileExtensionConstants.ExtBinaryFile},
                { FileFormats.CalendarFile, FileExtensionConstants.ExtCalendarFile},
                { FileFormats.CompiledHtml, FileExtensionConstants.ExtCompiledHtml},
                { FileFormats.CResourceFile, FileExtensionConstants.ExtCResourceFile},
                { FileFormats.DataFile, FileExtensionConstants.ExtDataFile},
                { FileFormats.GenericDatabase, FileExtensionConstants.ExtGenericDatabase},
                { FileFormats.Library, FileExtensionConstants.ExtLibrary},
                { FileFormats.LogFile, FileExtensionConstants.ExtLogFile},
                { FileFormats.NETResourceFile, FileExtensionConstants.ExtNETResourceFile},
                { FileFormats.PostScript, FileExtensionConstants.ExtPostScript},
                { FileFormats.TemporaryFile, FileExtensionConstants.ExtTemporaryFile}
            };

            return dictionary.ToFrozenDictionary();
        }

        /// <summary>
        /// Creates the static look up table for the data de-conversion.
        /// </summary>
        private static FrozenDictionary<string, FileFormats> CreateReconversionTable()
        {
            var dictionary = new Dictionary<string, FileFormats>
            {

            {string.Empty, FileFormats.NotSpecified},
            {FileExtensionConstants.ExtExcelLegacy, FileFormats.ExcelLegacy},
            {FileExtensionConstants.ExtExcel, FileFormats.Excel},
            {FileExtensionConstants.ExtExcelMacro, FileFormats.ExcelMacro},
            {FileExtensionConstants.ExtExcelMacroLegacy, FileFormats.ExcelMacroLegacy},
            {FileExtensionConstants.ExtExcelTemplate, FileFormats.ExcelTemplate},
            {FileExtensionConstants.ExtExcelTemplateLegacy, FileFormats.ExcelTemplateLegacy},
            {FileExtensionConstants.ExtMSAccessLegacyDatabase, FileFormats.MSAccessLegacyDatabase},
            {FileExtensionConstants.ExtMSAccessDatabase, FileFormats.MSAccessDatabase},
            {FileExtensionConstants.ExtOneNote, FileFormats.OneNote},
            {FileExtensionConstants.ExtOneNoteExport, FileFormats.OneNoteExport},
            {FileExtensionConstants.ExtPowerPointLegacy, FileFormats.PowerPointLegacy},
            {FileExtensionConstants.ExtPowerPoint, FileFormats.PowerPoint},
            {FileExtensionConstants.ExtPublisher, FileFormats.Publisher},
            {FileExtensionConstants.ExtWordDocumentLegacy, FileFormats.WordDocumentLegacy},
            {FileExtensionConstants.ExtWordDocument, FileFormats.WordDocument},
            {FileExtensionConstants.ExtWordMacro, FileFormats.WordMacro},
            {FileExtensionConstants.ExtWordLegacyMacro, FileFormats.WordLegacyMacro},
            {FileExtensionConstants.ExtWordTemplate, FileFormats.WordTemplate},
            {FileExtensionConstants.ExtWordTemplateLegacy, FileFormats.WordTemplateLegacy},
            {FileExtensionConstants.ExtXps, FileFormats.Xps},
            {FileExtensionConstants.ExtCommaSeparatedValues, FileFormats.CommaSeparatedValues},
            {FileExtensionConstants.ExtJavascriptObjectNotation, FileFormats.JavascriptObjectNotation},
            {FileExtensionConstants.ExtRichTextFile, FileFormats.RichTextFile},
            {FileExtensionConstants.ExtTextFile, FileFormats.TextFile},
            {FileExtensionConstants.ExtBatchFile, FileFormats.BatchFile},
            {FileExtensionConstants.ExtCommandConsoleFile, FileFormats.CommandConsoleFile},
            {FileExtensionConstants.ExtCommandFile, FileFormats.CommandFile},
            {FileExtensionConstants.ExtControlPanel, FileFormats.ControlPanel},
            {FileExtensionConstants.ExtDOSCommandFile, FileFormats.DOSCommandFile},
            {FileExtensionConstants.ExtDynamicLinkedLibrary, FileFormats.DynamicLinkedLibrary},
            {FileExtensionConstants.ExtExecutableFile, FileFormats.ExecutableFile},
            {FileExtensionConstants.ExtImageFile, FileFormats.ImageFile},
            {FileExtensionConstants.ExtNationalLanguageService, FileFormats.NationalLanguageService},
            {FileExtensionConstants.ExtPowerShellFile, FileFormats.PowerShellFile},
            {FileExtensionConstants.ExtRegistryFile, FileFormats.RegistryFile},
            {FileExtensionConstants.ExtSystemFile, FileFormats.SystemFile},
            {FileExtensionConstants.ExtWindowsInstallerFile, FileFormats.WindowsInstallerFile},
            {FileExtensionConstants.ExtBitmap, FileFormats.Bitmap},
            {FileExtensionConstants.ExtGenericImage, FileFormats.GenericImage},
            {FileExtensionConstants.ExtGraphicsInterchangeFormat, FileFormats.GraphicsInterchangeFormat},
            {FileExtensionConstants.ExtJointPhotographicExpertsGroup, FileFormats.JointPhotographicExpertsGroup},
            {FileExtensionConstants.ExtJointPhotographicExpertsGroupLong, FileFormats.JointPhotographicExpertsGroupLong},
            {FileExtensionConstants.ExtPhotoShop, FileFormats.PhotoShop},
            {FileExtensionConstants.ExtPortableNetworkGraphics, FileFormats.PortableNetworkGraphics},
            {FileExtensionConstants.ExtRawImage, FileFormats.RawImage},
            {FileExtensionConstants.ExtTaggedImageFileFormat, FileFormats.TaggedImageFileFormat},
            {FileExtensionConstants.ExtWindowsIcon, FileFormats.WindowsIcon},
            {FileExtensionConstants.ExtMovieFile, FileFormats.MovieFile},
            {FileExtensionConstants.ExtMPeg, FileFormats.MPeg},
            {FileExtensionConstants.ExtMPegLong, FileFormats.MPegLong},
            {FileExtensionConstants.ExtMPegV2, FileFormats.MPegV2},
            {FileExtensionConstants.ExtMPegV3, FileFormats.MPegV3},
            {FileExtensionConstants.ExtMPegV4, FileFormats.MPegV4},
            {FileExtensionConstants.ExtWindowsMovieFile, FileFormats.WindowsMovieFile},
            {FileExtensionConstants.ExtCabinet, FileFormats.Cabinet},
            {FileExtensionConstants.ExtGZip, FileFormats.GZip},
            {FileExtensionConstants.ExtPkZip, FileFormats.PkZip},
            {FileExtensionConstants.ExtTar, FileFormats.Tar},
            {FileExtensionConstants.ExtTarGZip, FileFormats.TarGZip},
            {FileExtensionConstants.ExtAspNetFile, FileFormats.AspNetFile},
            {FileExtensionConstants.ExtCPlusPlusProject, FileFormats.CPlusPlusProject},
            {FileExtensionConstants.ExtCPlusPlusFile, FileFormats.CPlusPlusFile},
            {FileExtensionConstants.ExtCFile, FileFormats.CFile},
            {FileExtensionConstants.ExtCSharpProject, FileFormats.CSharpProject},
            {FileExtensionConstants.ExtDumpFile, FileFormats.DumpFile},
            {FileExtensionConstants.ExtGitFile, FileFormats.GitFile},
            {FileExtensionConstants.ExtHeaderFile, FileFormats.HeaderFile},
            {FileExtensionConstants.ExtHtmlFile, FileFormats.HtmlFile},
            {FileExtensionConstants.ExtJavascriptFile, FileFormats.JavascriptFile},
            {FileExtensionConstants.ExtProgramDatabase, FileFormats.ProgramDatabase},
            {FileExtensionConstants.ExtPrecompiledHeader, FileFormats.PrecompiledHeader},
            {FileExtensionConstants.ExtSolution, FileFormats.Solution},
            {FileExtensionConstants.ExtSourceControlFile, FileFormats.SourceControlFile},
            {FileExtensionConstants.ExtSqlFile, FileFormats.SqlFile},
            {FileExtensionConstants.ExtSqlServerDatabase, FileFormats.SqlServerDatabase},
            {FileExtensionConstants.ExtSqlServerDatabaseLog, FileFormats.SqlServerDatabaseLog},
            {FileExtensionConstants.ExtTypeScriptFile, FileFormats.TypeScriptFile},
            {FileExtensionConstants.ExtVisualBasicFile, FileFormats.VisualBasicFile},
            {FileExtensionConstants.ExtVisualBasicProject, FileFormats.VisualBasicProject},
            {FileExtensionConstants.ExtXamlFile, FileFormats.XamlFile},
            {FileExtensionConstants.ExtXmlFile, FileFormats.XmlFile},
            {FileExtensionConstants.ExtBinaryFile, FileFormats.BinaryFile},
            {FileExtensionConstants.ExtCalendarFile, FileFormats.CalendarFile},
            {FileExtensionConstants.ExtCompiledHtml, FileFormats.CompiledHtml},
            {FileExtensionConstants.ExtCResourceFile, FileFormats.CResourceFile},
            {FileExtensionConstants.ExtDataFile, FileFormats.DataFile},
            {FileExtensionConstants.ExtGenericDatabase, FileFormats.GenericDatabase},
            {FileExtensionConstants.ExtLibrary, FileFormats.Library},
            {FileExtensionConstants.ExtLogFile, FileFormats.LogFile},
            {FileExtensionConstants.ExtNETResourceFile, FileFormats.NETResourceFile},
            {FileExtensionConstants.ExtPostScript, FileFormats.PostScript},
                { FileExtensionConstants.ExtTemporaryFile, FileFormats.TemporaryFile},
            };

            return dictionary.ToFrozenDictionary<string, FileFormats>();
        }
        #endregion
    }
}