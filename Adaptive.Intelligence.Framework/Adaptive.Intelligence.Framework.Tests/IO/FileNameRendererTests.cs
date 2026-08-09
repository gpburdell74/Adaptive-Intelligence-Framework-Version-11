using System.IO;
using System.Runtime.InteropServices;
using Adaptive.Intelligence.IO;

namespace Adaptive.Intelligence.Framework.Tests.IO
{
    /// <summary>
    /// Provides tests for the <see cref="FileNameRenderer"/> class.
    /// </summary>
    public class FileNameRendererTests
    {
        [Fact]
        public void RenderFileName_LinuxOrMacPathAndFileNameContainBackslashes_ReplacesSeparatorsBeforeCombining()
        {
            // Arrange
            string path = "root\\child";
            string fileName = "folder\\file.txt";

            // Act
            string result = FileNameRenderer.RenderFileName(path, fileName);

            // Assert
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Assert.Equal(Path.Combine("root/child", "folder/file.txt"), result);
            }
            else
            {
                Assert.Equal(Path.Combine(path, fileName), result);
            }
        }

        [Fact]
        public void RenderFileName_PathAndFileNameAlreadyNormalized_ReturnsPathCombineResult()
        {
            // Arrange
            string path = "root/child";
            string fileName = "file.txt";

            // Act
            string result = FileNameRenderer.RenderFileName(path, fileName);

            // Assert
            Assert.Equal(Path.Combine(path, fileName), result);
        }

        [Fact]
        public void RenderFileNameInUserPath_FileNameProvided_ReturnsFileNameInUserProfilePath()
        {
            // Arrange
            string fileName = "documents\\report.txt";
            string userProfilePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            bool isLinuxOrMac = RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            string expectedFileName = isLinuxOrMac ? fileName.Replace('\\', '/') : fileName;

            // Act
            string result = FileNameRenderer.RenderFileNameInUserPath(fileName);

            // Assert
            Assert.Equal(Path.Combine(userProfilePath, expectedFileName), result);
        }

        [Fact]
        public void RenderInTempPath_SubDirectoryProvided_ReturnsCombinedTempPath()
        {
            // Arrange
            string additionalPath = "logs";

            // Act
            string result = FileNameRenderer.RenderInTempPath(additionalPath);

            // Assert
            Assert.Equal(Path.Combine(Path.GetTempPath(), additionalPath), result);
        }
    }
}
