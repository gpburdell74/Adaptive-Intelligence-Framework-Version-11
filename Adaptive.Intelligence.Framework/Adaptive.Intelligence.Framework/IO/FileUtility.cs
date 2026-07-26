using System.Text.Json.Serialization;

namespace Adaptive.Intelligence.IO
{
    /// <summary>
    /// Provides general utility methods for file operations and file system operations.
    /// </summary>
    public static class FileUtility
    {
        /// <summary>
        /// Ensures the file name is for a file that does not currently exist in the path.
        /// </summary>
        /// <param name="originalFileName">
        /// The original file name to check for.
        /// </param>
        /// <returns>
        /// A unique file name based on the original file name.
        /// </returns>
        public static string EnsureUniqueFileName(string originalFileName)
        {
            string finalName = originalFileName;

            if (File.Exists(originalFileName))
            {
                int counter = 1;
                string? path = Path.GetDirectoryName(originalFileName);
                if (path != null)
                {
                    string name = Path.GetFileNameWithoutExtension(originalFileName);
                    string ext = Path.GetExtension(originalFileName);
                    string test = Path.Combine(path, name + counter + ext);

                    while (File.Exists(test))
                    {
                        counter++;
                        test = Path.Combine( path, name + counter + ext);
                    }
                    finalName = Path.Combine(path, name + counter + ext);
                }
            }
            return finalName;
        }

        /// <summary>
        /// Emulates the linux "touch" command by creating a new, empty file with the specified name.
        /// </summary>
        /// <param name="fileName">
        /// A string containing the path and name of the file.
        /// </param>
        public static void Touch(string fileName)
        {
            if (!File.Exists(fileName))
            {
                try
                {
                    FileStream stream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write);
                    stream.Close();
                    stream.Dispose();
                }
                catch
                {

                }
            }
        }
    }
}
