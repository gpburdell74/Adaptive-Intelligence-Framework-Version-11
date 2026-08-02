using System.Text;

namespace Adaptive.Intelligence.Constants
{
    /// <summary>
    /// Provides constants definitions for the content in the library and general use.
    /// </summary>
    public static class AiConstants
    {

        #region Data Type Names		
        /// <summary>
        /// The <see cref="DateTimeOffset"/> data type name in lower case.
        /// </summary>
        public const string DataTypeDateTimeOffsetLower = "datetimeoffset";
        /// <summary>
        /// The <see cref="Guid"/> data type name in lower case.
        /// </summary>
        public const string DataTypeGuidLower = "guid";
        #endregion

        #region Value Constants
        /// <summary>
        /// The "NULL" text.
        /// </summary>
        public const string NullText = "NULL";
        /// <summary>
        /// The numeric zero value.
        /// </summary>
        public const string Zero = "0";
        #endregion

        #region Gender Conversion Constants
        /// <summary>
        /// Specifies a string code for male.
        /// </summary>
        public const string GenderCodeMale = "M";
        /// <summary>
        /// Specifies a string code for female.
        /// </summary>
        public const string GenderCodeFemale = "F";
        /// <summary>
        /// Specifies a string code for male.
        /// </summary>
        public const char GenderCodeCharMale = 'M';
        /// <summary>
        /// Specifies the string text for male.
        /// </summary>
        public const string GenderMale = "Male";
        /// <summary>
        /// Specifies the string text for female.
        /// </summary>
        public const string GenderFemale = "Female";
        #endregion

        #region Formatting Constants		
        /// <summary>
        /// The hexadecimal format string.
        /// </summary>
        public static readonly CompositeFormat HexFormat = CompositeFormat.Parse("x2");
        /// <summary>
        /// The hexadecimal format string for a single character.
        /// </summary>
        public static readonly CompositeFormat HexFormatSingle = CompositeFormat.Parse("x");
        /// <summary>
        /// The phone number format string.
        /// </summary>
        public static readonly CompositeFormat PhoneNumberFormat = CompositeFormat.Parse("({0}) {1}-{2}");
        /// <summary>
        /// A general date format string.
        /// </summary>
        public static readonly CompositeFormat DateFormat = CompositeFormat.Parse("{0:MM/dd/yyyy}");
        /// <summary>
        /// A general date/time format string.
        /// </summary>
        public static readonly CompositeFormat DateTimeFormat = CompositeFormat.Parse("{0:MM/dd/yyyy hh:mm tt}");
        #endregion

        #region File Description Constants
        /// <summary>
        /// The description for a generic file.
        /// </summary>
        public const string FileDescGeneric = "File";
        #endregion

        #region Registry Constants
        /// <summary>
        /// The registry sub-key value for editing a file.
        /// </summary>
        public const string RegSubKeyNameEdit = "\\shell\\edit\\command";
        /// <summary>
        /// The registry sub-key value for opening a file.
        /// </summary>
        public const string RegSubKeyNameOpen = "\\shell\\Open\\command";
        /// <summary>
        /// The registry sub-key value for the location of the default icon.
        /// </summary>
        public const string RegSubKeyNameDefaultIcon = "\\DefaultIcon";
        #endregion

        #region XML Constants        
        /// <summary>
        /// The XML open bracket string.
        /// </summary>
        public const string XmlBracketOpen = "<";
        /// <summary>
        /// The XML end bracket string.
        /// </summary>
        public const string XmlBracketEnd = ">";
        /// <summary>
        /// The XML closing bracket start string.
        /// </summary>
        public const string XmlClosingBracketStart = "</";
        /// <summary>
        /// The XML end tag bracket string for self-closing values.
        /// </summary>
        public const string XmlBracketShortTagEnd = "/>";
        #endregion
    }
}