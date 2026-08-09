namespace Adaptive.Intelligence.IO
{
    /// <summary>
    /// Provides static methods and functions for rendering file and path names with
    /// regard to the underlying operating system.
    /// </summary>
    public static class FileNameRenderer
    {
        #region Private Static Members

        /// <summary>
        /// Is Windows Flag.
        /// </summary>
        private static readonly bool _windows;

        /// <summary>
        /// Is Not Windows Flag.
        /// </summary>
        private static readonly bool _linuxOrMac;

        #endregion

        #region Static Constructor
        /// <summary>
        /// Performs static initialization.
        /// </summary>
        static FileNameRenderer()
        {
            _windows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
                 System.Runtime.InteropServices.OSPlatform.Windows);

            _linuxOrMac =
                System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
                    System.Runtime.InteropServices.OSPlatform.Linux) ||
                System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
                    System.Runtime.InteropServices.OSPlatform.OSX);
        }
        #endregion

        #region Public Static Methods
        /// <summary>
        /// Rewrites the path and file name to the appropriate OS format.
        /// </summary>
        /// <param name="path">
        /// A string containing the fully qualified path to modify.
        /// </param>
        /// <param name="fileName">
        /// A string containing the file name to modify,
        /// </param>
        /// <returns>
        /// A string containing the fully qualified path and file name in the
        /// proper format for the current operating system.
        /// </returns>
        public static string RenderFileName(string path, string fileName)
        {
            if (_linuxOrMac)
            {
                path = path.Replace('\\', '/');
                fileName = fileName.Replace('\\', '/');
            }

            return Path.Combine(path, fileName);
        }

        /// <summary>
        /// Rewrites the path and file name to the appropriate OS format using the
        /// default path of the local user.
        /// </summary>
        /// <param name="fileName">
        /// A string containing the file name to modify,
        /// </param>
        /// <returns>
        /// A string containing the fully qualified path and file name in the
        /// proper format for the current operating system.
        /// </returns>
        public static string RenderFileNameInUserPath(string fileName)
        {
            string path = System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return RenderFileName(path, fileName);
        }

        /// <summary>
        /// Renders the temporary file path with the additional
        /// subdirectory.
        /// </summary>
        /// <param name="path">
        /// A string containing the sub-directory name to attach
        /// to the temp file path.
        /// </param>
        /// <returns>
        /// A string containing the rendered path value.
        /// </returns>
        public static string RenderInTempPath(string path)
        {
            if (_windows)
            {
                return Path.Combine(Path.GetTempPath(),
                    path);
            }
            else
            {
                return Path.Combine(Path.GetTempPath(),
                    path);
            }
        }
        #endregion
    }
}
