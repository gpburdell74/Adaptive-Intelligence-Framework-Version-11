namespace Adaptive.Intelligence.Enumerations
{
    /// <summary>
    /// Lists the types of file formats that are known, used or supported in the shared library.
    /// </summary>
    public enum FileFormats
    {
        /// <summary>
        /// Indicates the file format is not specified.
        /// </summary>
        NotSpecified = 0,

        #region MS Office Document Types		
        /// <summary>
        /// Indicates a legacy MS Excel spreadsheet (.XLS).
        /// </summary>
        ExcelLegacy,
        /// <summary>
        /// Indicates an MS Excel spreadsheet (.XLSX).
        /// </summary>
        Excel,
        /// <summary>
        /// Indicates an MS Excel macro file (.XLSM).
        /// </summary>
        ExcelMacro,
        /// <summary>
        /// Indicates a legacy MS Excel macro file (.XLM).
        /// </summary>
        ExcelMacroLegacy,
        /// <summary>
        /// Indicates an MS Excel template (.XLST).
        /// </summary>
        ExcelTemplate,
        /// <summary>
        /// Indicates a legacy MS Excel template (.XLT).
        /// </summary>
        ExcelTemplateLegacy,
        /// <summary>
        /// Indicates a legacy MS Access database (.MDB).
        /// </summary>
        MSAccessLegacyDatabase,
        /// <summary>
        /// Indicates an MS Access database (.ACCDB).
        /// </summary>
        MSAccessDatabase,
        /// <summary>
        /// Indicates a OneNote package file. (.ONEPKG)
        /// </summary>
        OneNote,
        /// <summary>
        /// Indicates a OneNote export file. (.ONE)
        /// </summary>
        OneNoteExport,
        /// <summary>
        /// Indicates a legacy MS PowerPoint file. (.PPT).
        /// </summary>
        PowerPointLegacy,
        /// <summary>
        /// Indicates an MS PowerPoint file. (.PPTX).
        /// </summary>
        PowerPoint,
        /// <summary>
        /// Indicates an MS Publisher file. (.PUB).
        /// </summary>
        Publisher,
        /// <summary>
        /// Indicates a legacy MS Word document (.DOC).
        /// </summary>
        WordDocumentLegacy,
        /// <summary>
        /// Indicates an MS WOrd document (.DOCX).
        /// </summary>
        WordDocument,
        /// <summary>
        /// Indicates an MS Word macro file (.DOCM).
        /// </summary>
        WordMacro,
        /// <summary>
        /// Indicates a legacy MS Word macro file (.DOM).
        /// </summary>
        WordLegacyMacro,
        /// <summary>
        /// Indicates an MS Word template (.DOCT).
        /// </summary>
        WordTemplate,
        /// <summary>
        /// Indicates a legacy MS Word template (.DOT).
        /// </summary>
        WordTemplateLegacy,
        /// <summary>
        /// Indicates an MS XML Paper Specification file (.XPS),
        /// </summary>
        Xps,
        #endregion

        #region Text File Types		
        /// <summary>
        /// Indicates a comma-separated value text file (.CSV).
        /// </summary>
        CommaSeparatedValues,
        /// <summary>
        /// Indicates a JSON file (.JSON).
        /// </summary>
        JavascriptObjectNotation,
        /// <summary>
        /// Indicates a rich text file. (.RTF).
        /// </summary>
        RichTextFile,
        /// <summary>
        /// Indicates a standard text file (.TXT).
        /// </summary>
        TextFile,
        #endregion

        #region Executable and OS File Types		
        /// <summary>
        /// Indicates a batch file (.BAT).
        /// </summary>
        BatchFile,
        /// <summary>
        /// Indicates a Microsoft Command Console file (.MSC).
        /// </summary>
        CommandConsoleFile,
        /// <summary>
        /// Indicates a command file (.CMD).
        /// </summary>
        CommandFile,
        /// <summary>
        /// Indicates a control panel definition file (.CPL).
        /// </summary>
        ControlPanel,
        /// <summary>
        /// Indicates a legacy DOS command/executable file (.COM).
        /// </summary>
        DOSCommandFile,
        /// <summary>
        /// Indicates a Windows dynamically-linked library file (.DLL).
        /// </summary>
        DynamicLinkedLibrary,
        /// <summary>
        /// Indicates a Windows executable file (.EXE).
        /// </summary>
        ExecutableFile,
        /// <summary>
        /// Indicates a Windows image file (.ISO).
        /// </summary>
        ImageFile,
        /// <summary>
        /// Indicates a national language service / font mapping file for Windows (.NLS).
        /// </summary>
        NationalLanguageService,
        /// <summary>
        /// Indicates a Windows PowerShell file (.PS1).
        /// </summary>
        PowerShellFile,
        /// <summary>
        /// Indicates a Windows registry file. (.REG).
        /// </summary>
        RegistryFile,
        /// <summary>
        /// Indicates a general system file or driver (.SYS).
        /// </summary>
        SystemFile,
        /// <summary>
        /// Indicates a Windows/Microsoft Installer file (.MSI).
        /// </summary>
        WindowsInstallerFile,
        #endregion

        #region Common Image Files		
        /// <summary>
        /// Indicates a bitmap image file (.BMP).
        /// </summary>
        Bitmap,
        /// <summary>
        /// Indicates a generic image file (.IMG).
        /// </summary>
        GenericImage,
        /// <summary>
        /// Indicates a GIF image file (.GIF).
        /// </summary>
        GraphicsInterchangeFormat,
        /// <summary>
        /// Indicates a JPEG image file (.JPG).
        /// </summary>
        JointPhotographicExpertsGroup,
        /// <summary>
        /// Indicates a JPEG image file (.JPEG).
        /// </summary>
        JointPhotographicExpertsGroupLong,
        /// <summary>
        /// Indicates a PhotoShop image file (.PSD).
        /// </summary>
        PhotoShop,
        /// <summary>
        /// Indicates a PNG image file (.PNG).
        /// </summary>
        PortableNetworkGraphics,
        /// <summary>
        /// Indicates a raw image file (.RAW).
        /// </summary>
        RawImage,
        /// <summary>
        /// Indicates a TIFF image file (.TIFF).
        /// </summary>
        TaggedImageFileFormat,
        /// <summary>
        /// Indicates a Windows Icon image file (.ICO).
        /// </summary>
        WindowsIcon,
        #endregion

        #region Common Video Files		
        /// <summary>
        /// Indicates a video file (.MOV).
        /// </summary>
        MovieFile,
        /// <summary>
        /// Indicates an MPEG (v1) video file (.MPG).
        /// </summary>
        MPeg,
        /// <summary>
        /// Indicates an MPEG (v1) video file (.MPEG).
        /// </summary>
        MPegLong,
        /// <summary>
        /// Indicates an MPEG (v2) video file (.MP2).
        /// </summary>
        MPegV2,
        /// <summary>
        /// Indicates an MPEG (v3) video file (.MP3).
        /// </summary>
        MPegV3,
        /// <summary>
        /// Indicates an MPEG (v4) video file (.MP4).
        /// </summary>
        MPegV4,
        /// <summary>
        /// Indicates a Windows Movie file (.WMV).
        /// </summary>
        WindowsMovieFile,
        #endregion

        #region Common Compressed Data File Types		
        /// <summary>
        /// Indicates a cabinet compression archive file (.CAB).
        /// </summary>
        Cabinet,
        /// <summary>
        /// Indicates a G-ZIP compression archive file (.GZIP).
        /// </summary>
        GZip,
        /// <summary>
        /// Indicates a PK-ZIP compression archive file (.ZIP).
        /// </summary>
        PkZip,
        /// <summary>
        /// Indicates a TAR compression archive file (.TAR).
        /// </summary>
        Tar,
        /// <summary>
        /// Indicates a TAR+GZIP compression archive file (.TAR.GZ).
        /// </summary>
        TarGZip,
        #endregion

        #region Common Coding File Types		
        /// <summary>
        /// Indicates an ASP .NET page definition. (.ASPX).
        /// </summary>
        AspNetFile,
        /// <summary>
        /// Indicates a C++ project file. (.CPPPROJ).
        /// </summary>
        CPlusPlusProject,
        /// <summary>
        /// Indicates a C++ source code file. (.CPP).
        /// </summary>
        CPlusPlusFile,
        /// <summary>
        /// Indicates a C source code file. (.C).
        /// </summary>
        CFile,
        /// <summary>
        /// Indicates a C# project file. (.CS).
        /// </summary>
        CSharpProject,
        /// <summary>
        /// Indicates a memory dump file. (.DMP).
        /// </summary>
        DumpFile,
        /// <summary>
        /// Indicates a GIT file. (.GIT).
        /// </summary>
        GitFile,
        /// <summary>
        /// Indicates a C or C++ header file. (.H).
        /// </summary>
        HeaderFile,
        /// <summary>
        /// Indicates an HTML file. (.HTML).
        /// </summary>
        HtmlFile,
        /// <summary>
        /// Indicates a JavaScript source file. (.JS).
        /// </summary>
        JavascriptFile,
        /// <summary>
        /// Indicates a program database file. (.PDB).
        /// </summary>
        ProgramDatabase,
        /// <summary>
        /// Indicates a precompiled header file. (.PCH).
        /// </summary>
        PrecompiledHeader,
        /// <summary>
        /// Indicates a Visual Studio solution file. (.SLN).
        /// </summary>
        Solution,
        /// <summary>
        /// Indicates a source control data file. (.SCC).
        /// </summary>
        SourceControlFile,
        /// <summary>
        /// Indicates a SQL source code file. (.SQL).
        /// </summary>
        SqlFile,
        /// <summary>
        /// Indicates a SQL server database file (.MDF).
        /// </summary>
        SqlServerDatabase,
        /// <summary>
        /// Indicates a SQL server database log file (.LDF).
        /// </summary>
        SqlServerDatabaseLog,
        /// <summary>
        /// Indicates a TypeScript source file. (.TS).
        /// </summary>
        TypeScriptFile,
        /// <summary>
        /// Indicates a Visual Basic source code file. (.VB).
        /// </summary>
        VisualBasicFile,
        /// <summary>
        /// Indicates a Visual Basic Project file. (.VBPROJ).
        /// </summary>
        VisualBasicProject,
        /// <summary>
        /// Indicates an Extensible Application Markup Language source definition file. (.XAML).
        /// </summary>
        XamlFile,
        /// <summary>
        /// Indicates an Extensible Markup Language file. (.XML).
        /// </summary>
        XmlFile,
        #endregion

        #region Common Generic File Types		
        /// <summary>
        /// Indicates a binary data file (.BIN).
        /// </summary>
        BinaryFile,
        /// <summary>
        /// Indicates a calendar file (.CAL).
        /// </summary>
        CalendarFile,
        /// <summary>
        /// Indicates a compiled HTML file (.CHM).
        /// </summary>
        CompiledHtml,
        /// <summary>
        /// Indicates a C or C++ resource file (.RC).
        /// </summary>
        CResourceFile,
        /// <summary>
        /// Indicates a generic data file (.DAT).
        /// </summary>
        DataFile,
        /// <summary>
        /// Indicates a generic database file (.DB).
        /// </summary>
        GenericDatabase,
        /// <summary>
        /// Indicates a general code library file (.LIB).
        /// </summary>
        Library,
        /// <summary>
        /// Indicates a general log file (.LOG).
        /// </summary>
        LogFile,
        /// <summary>
        /// Indicates a .NET resource file (.RESX).
        /// </summary>
        NETResourceFile, //.resx
        /// <summary>
        /// Indicates a PostScript file (.PS).
        /// </summary>
        PostScript,
        /// <summary>
        /// Indicates a general temporary file (.TMP).
        /// </summary>
        TemporaryFile
        #endregion
    }
}