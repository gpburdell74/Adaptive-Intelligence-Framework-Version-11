using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Adaptive.Intelligence.IO;

namespace Adaptive.Intelligence.Framework.Tests.IO
{
    /// <summary>
    /// Provides the tests for the static <see cref="FileUtility"/> class.
    /// </summary>
    public class FileUtilityTests
    {
        [Fact]
        public void Touch_Works()
        {
            string path = Path.GetTempPath();
            string fileName = Guid.NewGuid().ToString().Replace("-", "");
            string fileName2 = fileName + "_2.txt"; 
            string fileName3 = fileName + "_3.txt";
            string fileName4 = fileName + "_4.txt";
            fileName += ".txt";

            FileUtility.Touch(path + fileName);
            FileUtility.Touch(path + fileName2);
            FileUtility.Touch(path + fileName3);
            FileUtility.Touch(path + fileName4);

            Assert.True(File.Exists(path + fileName));
            Assert.True(File.Exists(path + fileName2));
            Assert.True(File.Exists(path + fileName3));
            Assert.True(File.Exists(path + fileName4));

            FileInfo info = new FileInfo(path + fileName);
            Assert.NotNull(info);
            Assert.Equal(0, info.Length);

             info = new FileInfo(path + fileName2);
            Assert.NotNull(info);
            Assert.Equal(0, info.Length);

            info = new FileInfo(path + fileName3);
            Assert.NotNull(info);
            Assert.Equal(0, info.Length);

            info = new FileInfo(path + fileName4);
            Assert.NotNull(info);
            Assert.Equal(0, info.Length);

            File.Delete(path + fileName);
            File.Delete(path + fileName2);
            File.Delete(path + fileName3);
            File.Delete(path + fileName4);
        }

        [Fact]
        public void Ensure_Unique_FileName_Works()
        {
            string path = Path.GetTempPath();
            string baseFileName = "MyFile";
            string ext = ".log";
            
                string[] fileNames = { "MyFile.log", "MyFile1.log", "MyFile2.log" };

            FileUtility.Touch(path + fileNames[0]);
            FileUtility.Touch(path + fileNames[1]);
            FileUtility.Touch(path + fileNames[2]);

            string newFileName = FileUtility.EnsureUniqueFileName(path + baseFileName + ext);
            Assert.NotNull(newFileName);
            Assert.False(File.Exists(newFileName));

            File.Delete(path + fileNames[0]);
            File.Delete(path + fileNames[1]);
            File.Delete(path + fileNames[2]);
        }
    }
}
