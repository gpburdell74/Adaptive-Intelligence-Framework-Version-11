using Adaptive.Intelligence.Security;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Adaptive.Intelligence.Framework.Tests.Security
{
    public class SuperCryptTests
    {
        [Fact]
        public void CreateTest()
        {
            // Arrange & Act
            SuperCrypt crypto = new SuperCrypt("TestKey", "TestIV", 1234);

            // Assert
            Assert.NotNull(crypto);

            // Alleviate.
            crypto.Dispose();

        }

        [Theory]
        [InlineData("TestCodePhrase", "TestPassword", 1234)]
        [InlineData("", "TestPassword", 1234)]
        [InlineData("TestCodePhrase", "", 1234)]
        [InlineData("", "", 1234)]
        [InlineData("", "", 0)]
        [InlineData("ABCDEFG", "ABC", int.MaxValue)]
        public void Create_Tests_With_Various_Data(string codePhrase, string password, int pin)
        {
            // Arrange & Act
            SuperCrypt crypto = new SuperCrypt(codePhrase, password, pin);

            // Assert
            Assert.NotNull(crypto);

            // Alleviate.
            crypto.Dispose();
        }

        [Theory]
        [InlineData("TestKey", "TestIV", 1234, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+[]{}|;:,.<>?/~`")]
        [InlineData("", "", 0, "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+[]{}|;:,.<>?/~`")]
        public void Same_Object_Encrypt_Decrypt_Test(string passPhrase, string password, int pin, string testString)
        {
            // Arrange.
            SuperCrypt crypto = new SuperCrypt(passPhrase, password, pin);

            // Act.
            byte[]? encryptedData = crypto.Encrypt(testString);
            byte[]? decryptedData = crypto.Decrypt(encryptedData);
            string decryptedString = System.Text.Encoding.UTF8.GetString(decryptedData!);

            // Assert 1.
            Assert.NotNull(encryptedData);
            Assert.NotNull(decryptedData);
            Assert.NotNull(decryptedString);

            Assert.Equal(testString, decryptedString);

            // Alleviate.
            crypto.Dispose();


        }
        [Fact]
        public void Same_Object_Encrypt_Decrypt_RandomData_Test()
        {
            for (int index = 0; index < 99; index++)
            {
                // Arrange.
                SuperCrypt crypto = new SuperCrypt(RandomString(16), RandomString(32), RandomInt());
                string testString = RandomString(1024);
                // Act.
                byte[]? encryptedData = crypto.Encrypt(testString);
                byte[]? decryptedData = crypto.Decrypt(encryptedData);
                string decryptedString = System.Text.Encoding.UTF8.GetString(decryptedData!);

                // Assert 1.
                Assert.NotNull(encryptedData);
                Assert.NotNull(decryptedData);
                Assert.NotNull(decryptedString);

                Assert.Equal(testString, decryptedString);

                // Alleviate.
                crypto.Dispose();
            }
        }

        [Fact]
        public void Different_Object_Encrypt_Decrypt_RandomData_Test()
        {
            for (int index = 0; index < 99; index++)
            {
                // Arrange.
                string testString = RandomString(1024);
                string phrase = RandomString(16);
                string pwd = RandomString(32);
                int pin = RandomInt();
                SuperCrypt firstCrypto = new SuperCrypt(phrase, pwd, pin);
                SuperCrypt secondCrypto = new SuperCrypt(phrase, pwd, pin);

                // Act.
                byte[]? encryptedData = firstCrypto.Encrypt(testString);
                byte[]? decryptedData = secondCrypto.Decrypt(encryptedData);
                string decryptedString = System.Text.Encoding.UTF8.GetString(decryptedData!);

                // Assert 1.
                Assert.NotNull(encryptedData);
                Assert.NotNull(decryptedData);
                Assert.NotNull(decryptedString);
                Assert.Equal(testString, decryptedString);

                // Alleviate.
                firstCrypto.Dispose();
                secondCrypto.Dispose();
            }
        }

        private static int RandomInt()
        {
            byte[] data = new byte[4];
            RandomNumberGenerator.Fill(data);
            return BitConverter.ToInt32(data, 0);
        }

        private static string RandomString(int length)
        {
            byte[] data = RandomNumberGenerator.GetBytes(length);
            return System.Text.Encoding.ASCII.GetString(data);

        }
    }
}
