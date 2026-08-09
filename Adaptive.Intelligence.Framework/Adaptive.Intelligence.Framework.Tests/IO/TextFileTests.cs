using Adaptive.Intelligence.IO;

namespace Adaptive.Intelligence.Framework.Tests;

/// <summary>
/// Provides unit tests for the <see cref="TextFile"/> class.
/// </summary>
public class TextFileTests
{
    /// <summary>
    /// Verifies the default constructor initializes an unopened file instance.
    /// </summary>
    [Fact]
    public void Constructor_Default_Initializes_Unopened_Instance()
    {
        // Arrange
        using TextFile target = new();

        // Act
        string? fileName = target.FileName;
        bool canRead = target.CanRead;
        bool canWrite = target.CanWrite;

        // Assert
        Assert.Null(fileName);
        Assert.False(canRead);
        Assert.False(canWrite);
    }

    /// <summary>
    /// Verifies the file-name constructor stores the provided file name.
    /// </summary>
    [Fact]
    public void Constructor_WithFileName_Sets_FileName()
    {
        // Arrange
        string expected = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");

        // Act
        using TextFile target = new(expected);

        // Assert
        Assert.Equal(expected, target.FileName);
    }

    /// <summary>
    /// Verifies setting <see cref="TextFile.FileName"/> updates the value when the file is not open.
    /// </summary>
    [Fact]
    public void FileName_Set_WhenFileIsNotOpen_Updates_Value()
    {
        // Arrange
        using TextFile target = new();
        string expected = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");

        // Act
        target.FileName = expected;

        // Assert
        Assert.Equal(expected, target.FileName);
    }

    /// <summary>
    /// Verifies setting <see cref="TextFile.FileName"/> does not update the value while the file is open.
    /// </summary>
    [Fact]
    public void FileName_Set_WhenFileIsOpen_DoesNotChange_Value()
    {
        // Arrange
        string originalPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");
        string newPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");
        File.WriteAllText(originalPath, "content");

        try
        {
            using TextFile target = new();
            bool openSuccess = target.OpenForRead(originalPath);

            // Act
            target.FileName = newPath;

            // Assert
            Assert.True(openSuccess);
            Assert.Equal(originalPath, target.FileName);
            target.Close();
        }
        finally
        {
            if (File.Exists(originalPath))
            {
                File.Delete(originalPath);
            }
        }
    }

    /// <summary>
    /// Verifies <see cref="TextFile.CanRead"/> returns true after opening an existing file for reading.
    /// </summary>
    [Fact]
    public void CanRead_AfterOpenForRead_Returns_True()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");
        File.WriteAllText(filePath, "readable");

        try
        {
            using TextFile target = new();

            // Act
            bool success = target.OpenForRead(filePath);
            bool canRead = target.CanRead;

            // Assert
            Assert.True(success);
            Assert.True(canRead);
            target.Close();
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    /// <summary>
    /// Verifies <see cref="TextFile.CanWrite"/> returns true after creating a file for writing.
    /// </summary>
    [Fact]
    public void CanWrite_AfterCreate_Returns_True()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");

        try
        {
            using TextFile target = new(filePath);

            // Act
            bool success = target.Create();
            bool canWrite = target.CanWrite;

            // Assert
            Assert.True(success);
            Assert.True(canWrite);
            target.Close();
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
    /// <summary>
    /// Verifies <see cref="TextFile.Close"/> clears the open state and file name after opening a file.
    /// </summary>
    [Fact]
    public void Close_WhenFileIsOpen_Resets_State()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");
        File.WriteAllText(filePath, "close me");

        try
        {
            using TextFile target = new();
            bool openSuccess = target.OpenForRead(filePath);

            // Act
            target.Close();

            // Assert
            Assert.True(openSuccess);
            Assert.False(target.IsOpen);
            Assert.Null(target.FileName);
            Assert.False(target.CanRead);
            Assert.False(target.CanWrite);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    /// <summary>
    /// Verifies <see cref="TextFile.IsOpen"/> returns false for a new instance.
    /// </summary>
    [Fact]
    public void IsOpen_WhenNewInstance_ReturnsFalse()
    {
        // Arrange
        using TextFile target = new();

        // Act
        bool isOpen = target.IsOpen;

        // Assert
        Assert.False(isOpen);
    }

    /// <summary>
    /// Verifies <see cref="TextFile.IsOpen"/> reflects state changes when creating and closing a file.
    /// </summary>
    [Fact]
    public void IsOpen_AfterCreateAndClose_ReflectsStateChanges()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");

        try
        {
            using TextFile target = new();

            // Act
            bool createSuccess = target.Create(filePath);
            bool isOpenAfterCreate = target.IsOpen;
            target.Close();
            bool isOpenAfterClose = target.IsOpen;

            // Assert
            Assert.True(createSuccess);
            Assert.True(isOpenAfterCreate);
            Assert.False(isOpenAfterClose);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    /// <summary>
    /// Verifies <see cref="TextFile.Close"/> clears the file name when no stream is open.
    /// </summary>
    [Fact]
    public void Close_WhenFileIsNotOpen_ClearsFileName()
    {
        // Arrange
        using TextFile target = new();
        target.FileName = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");

        // Act
        target.Close();

        // Assert
        Assert.Null(target.FileName);
        Assert.False(target.IsOpen);
    }

    /// <summary>
    /// Verifies <see cref="TextFile.Create()"/> returns false when no file name is set.
    /// </summary>
    [Fact]
    public void Create_WithoutFileName_ReturnsFalse()
    {
        // Arrange
        using TextFile target = new();

        // Act
        bool success = target.Create();

        // Assert
        Assert.False(success);
        Assert.False(target.IsOpen);
    }

    /// <summary>
    /// Verifies <see cref="TextFile.Create()"/> creates a file using the stored file name.
    /// </summary>
    [Fact]
    public void Create_WithStoredFileName_CreatesFileAndReturnsTrue()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");

        try
        {
            using TextFile target = new();
            target.FileName = filePath;

            // Act
            bool success = target.Create();

            // Assert
            Assert.True(success);
            Assert.True(File.Exists(filePath));
            Assert.True(target.IsOpen);
            Assert.Null(target.FileName);
            target.Close();
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    /// <summary>
    /// Verifies <see cref="TextFile.Create(string)"/> returns false when the provided path is invalid.
    /// </summary>
    [Fact]
    public void Create_WithInvalidPath_ReturnsFalse()
    {
        // Arrange
        using TextFile target = new();
        string invalidPath = $"invalid{'\0'}path.txt";

        // Act
        bool success = target.Create(invalidPath);

        // Assert
        Assert.False(success);
        Assert.Null(target.FileName);
        Assert.False(target.IsOpen);
    }

    /// <summary>
    /// Verifies <see cref="TextFile.Delete()"/> returns false when no file name is set.
    /// </summary>
    [Fact]
    public void Delete_WithoutFileName_ReturnsFalse()
    {
        // Arrange
        using TextFile target = new();

        // Act
        bool success = target.Delete();

        // Assert
        Assert.False(success);
    }

    /// <summary>
    /// Verifies <see cref="TextFile.Delete()"/> deletes the file referenced by the stored file name.
    /// </summary>
    [Fact]
    public void Delete_WithStoredFileName_DeletesFileAndReturnsTrue()
    {
        // Arrange
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");
        File.WriteAllText(filePath, "delete me");

        using TextFile target = new(filePath);

        // Act
        bool success = target.Delete();

        // Assert
        Assert.True(success);
        Assert.False(File.Exists(filePath));
        Assert.Null(target.FileName);
        Assert.False(target.IsOpen);
    }


    /// <summary>
    /// Verifies <see cref="TextFile.OpenForRead()"/> returns false when no file name is set.
    /// </summary>
    [Fact]
    public void OpenForRead_WithoutFileName_ReturnsFalse()
    {
        // Arrange
        using TextFile target = new();

        // Act
        bool success = target.OpenForRead();

        // Assert
        Assert.False(success);
        Assert.False(target.IsOpen);
    }


    /// <summary>
    /// Verifies <see cref="TextFile.Delete(string)"/> returns true when the target file does not exist.
    /// </summary>
    [Fact]
    public void Delete_WithMissingFile_ReturnsTrue()
    {
        // Arrange
        using TextFile target = new();
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");

        // Act
        bool success = target.Delete(filePath);

        // Assert
        Assert.True(success);
        Assert.False(File.Exists(filePath));
    }

    /// <summary>
    /// Verifies <see cref="TextFile.Delete(string)"/> returns false when delete throws an <see cref="IOException"/>.
    /// </summary>
    [Fact]
    public void Delete_WhenFileIsLocked_ReturnsFalse()
    {
        // Arrange
        using TextFile target = new();
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");
        File.WriteAllText(filePath, "locked");

        FileStream? lockStream = null;
        try
        {
            lockStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            // Act
            bool success = target.Delete(filePath);

            // Assert
            Assert.False(success);
            Assert.True(File.Exists(filePath));
        }
        finally
        {
            lockStream?.Dispose();
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    /// <summary>
    /// Verifies <see cref="TextFile.OpenForRead()"/> opens the file referenced by <see cref="TextFile.FileName"/>.
    /// </summary>
    [Fact]
    public void OpenForRead_WithStoredFileName_OpensFileAndReturnsTrue()
    {
        // Arrange
        using TextFile target = new();
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");
        File.WriteAllText(filePath, "read content");

        try
        {
            target.FileName = filePath;

            // Act
            bool success = target.OpenForRead();

            // Assert
            Assert.True(success);
            Assert.True(target.CanRead);
            Assert.True(target.IsOpen);
            target.Close();
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    /// <summary>
    /// Verifies <see cref="TextFile.OpenForRead(string)"/> returns false when the file does not exist.
    /// </summary>
    [Fact]
    public void OpenForRead_WithMissingFile_ReturnsFalse()
    {
        // Arrange
        using TextFile target = new();
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");

        // Act
        bool success = target.OpenForRead(filePath);

        // Assert
        Assert.False(success);
        Assert.False(target.IsOpen);
    }

    /// <summary>
    /// Verifies <see cref="TextFile.OpenForRead(string)"/> returns false when called while another file is already open.
    /// </summary>
    [Fact]
    public void OpenForRead_WhenAnotherFileIsAlreadyOpen_ReturnsFalse()
    {
        // Arrange
        using TextFile target = new();
        string firstFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");
        string secondFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");
        File.WriteAllText(firstFilePath, "first");
        File.WriteAllText(secondFilePath, "second");

        try
        {
            bool firstOpenSuccess = target.OpenForRead(firstFilePath);

            // Act
            bool secondOpenSuccess = target.OpenForRead(secondFilePath);

            // Assert
            Assert.True(firstOpenSuccess);
            Assert.False(secondOpenSuccess);
            Assert.True(target.IsOpen);
            target.Close();
        }
        finally
        {
            if (File.Exists(firstFilePath))
            {
                File.Delete(firstFilePath);
            }

            if (File.Exists(secondFilePath))
            {
                File.Delete(secondFilePath);
            }
        }
    }

    /// <summary>
    /// Verifies <see cref="TextFile.OpenForWrite()"/> returns false when no file name is set.
    /// </summary>
    [Fact]
    public void OpenForWrite_WithoutFileName_ReturnsFalse()
    {
        // Arrange
        using TextFile target = new();

        // Act
        bool success = target.OpenForWrite();

        // Assert
        Assert.False(success);
        Assert.False(target.IsOpen);
    }

    /// <summary>
    /// Verifies <see cref="TextFile.OpenForWrite()"/> opens the file referenced by <see cref="TextFile.FileName"/> for append.
    /// </summary>
    [Fact]
    public void OpenForWrite_WithStoredFileName_OpensFileAndReturnsTrue()
    {
        // Arrange
        using TextFile target = new();
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");
        File.WriteAllText(filePath, "existing");

        try
        {
            target.FileName = filePath;

            // Act
            bool success = target.OpenForWrite();

            // Assert
            Assert.True(success);
            Assert.True(target.CanWrite);
            Assert.True(target.IsOpen);
            target.Close();
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    /// <summary>
    /// Verifies <see cref="TextFile.OpenForWrite(string)"/> appends text to an existing file.
    /// </summary>
    [Fact]
    public void OpenForWrite_WithExistingFile_AppendsTextAndReturnsTrue()
    {
        // Arrange
        using TextFile target = new();
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");
        File.WriteAllText(filePath, "line1");

        try
        {
            // Act
            bool openSuccess = target.OpenForWrite(filePath);
            bool writeSuccess = target.WriteLine("line2");
            target.Close();
            string content = File.ReadAllText(filePath);

            // Assert
            Assert.True(openSuccess);
            Assert.True(writeSuccess);
            Assert.Equal($"line1line2{Environment.NewLine}", content);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    /// <summary>
    /// Verifies <see cref="TextFile.OpenForWrite(string)"/> returns false when the path is invalid.
    /// </summary>
    [Fact]
    public void OpenForWrite_WithInvalidPath_ReturnsFalse()
    {
        // Arrange
        using TextFile target = new();
        string invalidPath = $"invalid{'\0'}path.txt";

        // Act
        bool success = target.OpenForWrite(invalidPath);

        // Assert
        Assert.False(success);
        Assert.False(target.IsOpen);
        Assert.Equal(invalidPath, target.FileName);
    }

    /// <summary>
    /// Verifies <see cref="TextFile.OpenForWrite(string)"/> returns false when called while another file is already open.
    /// </summary>
    [Fact]
    public void OpenForWrite_WhenAnotherFileIsAlreadyOpen_ReturnsFalse()
    {
        // Arrange
        using TextFile target = new();
        string firstFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");
        string secondFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");
        File.WriteAllText(firstFilePath, "first");
        File.WriteAllText(secondFilePath, "second");

        try
        {
            bool firstOpenSuccess = target.OpenForWrite(firstFilePath);

            // Act
            bool secondOpenSuccess = target.OpenForWrite(secondFilePath);

            // Assert
            Assert.True(firstOpenSuccess);
            Assert.False(secondOpenSuccess);
            Assert.True(target.IsOpen);
            target.Close();
        }
        finally
        {
            if (File.Exists(firstFilePath))
            {
                File.Delete(firstFilePath);
            }

            if (File.Exists(secondFilePath))
            {
                File.Delete(secondFilePath);
            }
        }
    }

    /// <summary>
    /// Verifies <see cref="TextFile.ReadLine"/> returns null when the file is not open.
    /// </summary>
    [Fact]
    public void ReadLine_WhenFileIsNotOpen_ReturnsNull()
    {
        // Arrange
        using TextFile target = new();

        // Act
        string? line = target.ReadLine();

        // Assert
        Assert.Null(line);
    }

    /// <summary>
    /// Verifies <see cref="TextFile.ReadLine"/> returns the next line when the file is open for reading.
    /// </summary>
    [Fact]
    public void ReadLine_WhenFileIsOpen_ReturnsNextLine()
    {
        // Arrange
        using TextFile target = new();
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");
        File.WriteAllText(filePath, $"first{Environment.NewLine}second");

        try
        {
            bool openSuccess = target.OpenForRead(filePath);

            // Act
            string? line = target.ReadLine();

            // Assert
            Assert.True(openSuccess);
            Assert.Equal("first", line);
            target.Close();
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    /// <summary>
    /// Verifies <see cref="TextFile.ReadAll"/> returns null when the file is not open.
    /// </summary>
    [Fact]
    public void ReadAll_WhenFileIsNotOpen_ReturnsNull()
    {
        // Arrange
        using TextFile target = new();

        // Act
        string? text = target.ReadAll();

        // Assert
        Assert.Null(text);
    }

    /// <summary>
    /// Verifies <see cref="TextFile.ReadAll"/> returns all text when the file is open for reading.
    /// </summary>
    [Fact]
    public void ReadAll_WhenFileIsOpen_ReturnsAllText()
    {
        // Arrange
        using TextFile target = new();
        string expected = $"line1{Environment.NewLine}line2";
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");
        File.WriteAllText(filePath, expected);

        try
        {
            bool openSuccess = target.OpenForRead(filePath);

            // Act
            string? text = target.ReadAll();

            // Assert
            Assert.True(openSuccess);
            Assert.Equal(expected, text);
            target.Close();
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    /// <summary>
    /// Verifies <see cref="TextFile.Write"/> returns false when the file is not open.
    /// </summary>
    [Fact]
    public void Write_WhenFileIsNotOpen_ReturnsFalse()
    {
        // Arrange
        using TextFile target = new();

        // Act
        bool success = target.Write("text");

        // Assert
        Assert.False(success);
    }

    /// <summary>
    /// Verifies <see cref="TextFile.Write"/> writes text and returns true when the file is open.
    /// </summary>
    [Fact]
    public void Write_WhenFileIsOpen_WritesTextAndReturnsTrue()
    {
        // Arrange
        using TextFile target = new();
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");

        try
        {
            bool createSuccess = target.Create(filePath);

            // Act
            bool writeSuccess = target.Write("abc");
            target.Close();
            string content = File.ReadAllText(filePath);

            // Assert
            Assert.True(createSuccess);
            Assert.True(writeSuccess);
            Assert.Equal("abc", content);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    /// <summary>
    /// Verifies <see cref="TextFile.WriteLine"/> returns false when the file is not open.
    /// </summary>
    [Fact]
    public void WriteLine_WhenFileIsNotOpen_ReturnsFalse()
    {
        // Arrange
        using TextFile target = new();

        // Act
        bool success = target.WriteLine("text");

        // Assert
        Assert.False(success);
    }

    /// <summary>
    /// Verifies <see cref="TextFile.WriteLine"/> writes a line and returns true when the file is open.
    /// </summary>
    [Fact]
    public void WriteLine_WhenFileIsOpen_WritesLineAndReturnsTrue()
    {
        // Arrange
        using TextFile target = new();
        string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.txt");

        try
        {
            bool createSuccess = target.Create(filePath);

            // Act
            bool writeSuccess = target.WriteLine("line");
            target.Close();
            string content = File.ReadAllText(filePath);

            // Assert
            Assert.True(createSuccess);
            Assert.True(writeSuccess);
            Assert.Equal($"line{Environment.NewLine}", content);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }




}
