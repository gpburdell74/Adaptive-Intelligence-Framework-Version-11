using Adaptive.Intelligence.Security;
using Adaptive.Intelligence.Utility;
using Adaptive.Intelligence.Extensions;
using System.Security.Cryptography;

namespace Adaptive.Intelligence.Framework.Tests.Security
{
    public class AesKeyTableTests
    {
        [Fact]
        public void CreateTest()
        {
            // Arrange
            byte[] data = RandomNumberGenerator.GetBytes(288);

            // Act
            AesKeyTable table = new AesKeyTable(data);

            // Assert 
            Assert.NotNull(table);

            // Alleviate.
            table.Dispose();

        }

        [Theory]
        [InlineData(0)]
        [InlineData(32)]
        [InlineData(64)]
        [InlineData(48)]
        [InlineData(286)]
        [InlineData(287)]
        [InlineData(289)]
        public void CreateTest_BadKeySize(int keySize)
        {
            AesKeyTable? table = null;

            // Arrange
            byte[] data = RandomNumberGenerator.GetBytes(keySize);

            // Act
            ArgumentException expectedException = null;
            try
            {
                table = new AesKeyTable(data);
            }
            catch (ArgumentException ex)
            {
                expectedException = ex;
            }
            // Assert 
            Assert.NotNull(expectedException);
            Assert.Null(table);
        }

        [Fact]
        public void CreateTest_With_Valid_Arrays()
        {
            // Arrange.

            byte[] primary = RandomNumberGenerator.GetBytes(48);
            byte[] secondary = RandomNumberGenerator.GetBytes(48);
            byte[] tertiary = RandomNumberGenerator.GetBytes(48);
            byte[] quarternary = RandomNumberGenerator.GetBytes(48);
            byte[] quinary = RandomNumberGenerator.GetBytes(48);
            byte[] senary = RandomNumberGenerator.GetBytes(48);

            // Act.
            AesKeyTable table = new AesKeyTable(primary, secondary, tertiary, quarternary, quinary, senary);

            // Assert.
            Assert.NotNull(table);
            Assert.NotNull(table.Primary);
            Assert.NotNull(table.Secondary);
            Assert.NotNull(table.Tertiary);
            Assert.NotNull(table.Quaternary);
            Assert.NotNull(table.Quinary);
            Assert.NotNull(table.Senary);

            Assert.True(table.Primary.Compare(primary) == 0);
            Assert.True(table.Secondary.Compare(secondary) == 0);
            Assert.True(table.Tertiary.Compare(tertiary) == 0);
            Assert.True(table.Quaternary.Compare(quarternary) == 0);
            Assert.True(table.Quinary.Compare(quinary) == 0);
            Assert.True(table.Senary.Compare(senary) == 0);

            // Alleviate.
            table.Dispose();
            ByteArrayUtil.Clear(primary);
            ByteArrayUtil.Clear(secondary);
            ByteArrayUtil.Clear(tertiary);
            ByteArrayUtil.Clear(quarternary);
            ByteArrayUtil.Clear(quinary);
            ByteArrayUtil.Clear(senary);

        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(47)]
        [InlineData(49)]
        [InlineData(64)]
        public void CreateTest_With_Invalid_Arrays(int arraySize)
        {
            // Arrange.

            byte[] primary = RandomNumberGenerator.GetBytes(arraySize);
            byte[] secondary = RandomNumberGenerator.GetBytes(arraySize);
            byte[] tertiary = RandomNumberGenerator.GetBytes(arraySize);
            byte[] quarternary = RandomNumberGenerator.GetBytes(arraySize);
            byte[] quinary = RandomNumberGenerator.GetBytes(arraySize);
            byte[] senary = RandomNumberGenerator.GetBytes(arraySize);

            // Act.
            AesKeyTable? table = null;
            ArgumentException? arrayException = null;

            try
            {
                table = new AesKeyTable(primary, secondary, tertiary, quarternary, quinary, senary);
            }
            catch (ArgumentException ex)
            {
                arrayException = ex;
            }

            // Assert.
            Assert.Null(table);
            Assert.NotNull(arrayException);

            // Alleviate.
            ByteArrayUtil.Clear(primary);
            ByteArrayUtil.Clear(secondary);
            ByteArrayUtil.Clear(tertiary);
            ByteArrayUtil.Clear(quarternary);
            ByteArrayUtil.Clear(quinary);
            ByteArrayUtil.Clear(senary);

        }

        [Fact]
        public void DisposeTest()
        {
            // Arrange
            byte[] data = RandomNumberGenerator.GetBytes(288);
            AesKeyTable table = new AesKeyTable(data);

            // Act.
            table.Dispose();
            table.Dispose();
            table.Dispose();
            table.Dispose();
            table.Dispose();
            table.Dispose();

        }

        [Fact]
        public void Properties_AreCleared_AfterDispose()
        {
            // Arrange
            byte[] data = RandomNumberGenerator.GetBytes(288);
            AesKeyTable table = new AesKeyTable(data);

            // Act
            table.Dispose();

            // Assert
            Assert.All(new[] { table.Primary, table.Secondary, table.Tertiary, table.Quaternary, table.Quinary, table.Senary },
                arr => Assert.True(arr.All(b => b == 0)));
        }

        [Fact]
        public void Dispose_IsIdempotent()
        {
            // Arrange
            byte[] data = RandomNumberGenerator.GetBytes(288);
            AesKeyTable table = new AesKeyTable(data);

            // Act & Assert
            table.Dispose();
            var ex = Record.Exception(() => table.Dispose());
            Assert.Null(ex);
        }

        [Fact]
        public void Constructor_Throws_OnNullArray()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new AesKeyTable(null!));
        }

        [Fact]
        public void Constructor_Throws_OnNullAnyKeyArray()
        {
            // Arrange
            byte[] valid = RandomNumberGenerator.GetBytes(48);
            byte[] invalid = new byte[0];

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new AesKeyTable(invalid, valid, valid, valid, valid, valid));
            Assert.Throws<ArgumentException>(() => new AesKeyTable(valid, invalid, valid, valid, valid, valid));
            Assert.Throws<ArgumentException>(() => new AesKeyTable(valid, valid, invalid, valid, valid, valid));
            Assert.Throws<ArgumentException>(() => new AesKeyTable(valid, valid, valid, invalid, valid, valid));
            Assert.Throws<ArgumentException>(() => new AesKeyTable(valid, valid, valid, valid, invalid, valid));
            Assert.Throws<ArgumentException>(() => new AesKeyTable(valid, valid, valid, valid, valid, invalid));
        }
        [Fact]
        public void Properties_ReturnDefensiveCopy()
        {
            // Arrange
            byte[] data = RandomNumberGenerator.GetBytes(288);
            AesKeyTable table = new AesKeyTable(data);

            // Act
            var original = table.Primary;
            original[0] ^= 0xFF; // Mutate the returned array

            // Assert
            Assert.NotEqual(original[0], table.Primary[0]);
            table.Dispose();
        }

        [Fact]
        public void CanBeUsedWithUsingStatement()
        {
            // Arrange
            byte[] data = RandomNumberGenerator.GetBytes(288);

            // Act & Assert
            using (AesKeyTable table = new AesKeyTable(data))
            {
                Assert.NotNull(table.Primary);
            }
        }
    }
}