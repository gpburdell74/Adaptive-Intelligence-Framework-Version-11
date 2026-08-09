using Adaptive.Intelligence.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adaptive.Intelligence.Framework.Tests.Security
{
    public class AesProviderTests
    {
        [Fact]
        public void CreateTest()
        {
            AesProvider provider = new AesProvider();
            Assert.NotNull(provider);
            provider.Dispose();

        }

        [Fact]
        public void DisposeSafetyTest()
        {
            AesProvider provider = new AesProvider();
            Assert.NotNull(provider);

            provider.Dispose();
            provider.Dispose();
            provider.Dispose();
            provider.GenerateNewKey();
            provider.Dispose();
            provider.GenerateNewKey();
            provider.Dispose();
            provider.Dispose();
            provider.Dispose();
            provider.Dispose();

        }
        [Fact]
        public void DecryptTest()
        {
            AesProvider provider = new AesProvider();

            byte[] originalData = new byte[] { 1, 2, 3, 4, 5, 6, 7 };

            byte[]? encrypted = provider.Encrypt(originalData);

            Assert.NotNull(encrypted);
            byte[]? decrypted = provider.Decrypt(encrypted);

            Assert.NotNull(decrypted);
            Assert.Equal(originalData.Length, decrypted.Length);

            for (int count = 0; count < originalData.Length; count++)
            {
                Assert.Equal(originalData[count], decrypted[count]);
            }
            provider.Dispose();
        }
        [Fact]
        public void EncryptTest()
        {
            AesProvider provider = new AesProvider();

            byte[] originalData = new byte[] { 111, 112, 123, 4, 5, 6, 7 };

            byte[]? encrypted = provider.Encrypt(originalData);

            Assert.NotNull(encrypted);

        }
        [Fact]
        public void EncryptFullDataTest()
        {
            AesProvider provider = new AesProvider();

            byte[] originalData = new byte[256];
            for (int count = 0; count < 256; count++)
                originalData[count] = 255;

            byte[]? encrypted = provider.Encrypt(originalData);

            Assert.NotNull(encrypted);

        }
        [Fact]
        public void EncryptMinDataTest()
        {
            AesProvider provider = new AesProvider();

            byte[] originalData = new byte[256];
            Array.Clear(originalData, 0, 0);

            byte[]? encrypted = provider.Encrypt(originalData);

            Assert.NotNull(encrypted);

        }

        [Fact]
        public void GetKeyTest()
        {
            AesProvider provider = new AesProvider();
            string? key = provider.GetKey();
            Assert.NotNull(key);
            Assert.Equal(44, key.Length);

            byte[]? keyData = Convert.FromBase64String(key);
            provider.Dispose();
        }

        [Fact]
        public void GetIVTest()
        {
            AesProvider provider = new AesProvider();
            string? iv = provider.GetIV();
            Assert.NotNull(iv);
            Assert.Equal(24, iv.Length);

            byte[]? ivData = Convert.FromBase64String(iv);
            provider.Dispose();
        }

        [Fact]
        public void GetKeyDataTest()
        {
            AesProvider provider = new AesProvider();

            byte[]? data = provider.GetKeyData();
            Assert.NotNull(data);
            Assert.Equal(32, data.Length);

            provider.Dispose();
        }

        [Fact]
        public void GetIVDataTest()
        {
            AesProvider provider = new AesProvider();

            byte[]? data = provider.GetIVData();
            Assert.NotNull(data);
            Assert.Equal(16, data.Length);

            provider.Dispose();
        }

        [Fact]
        public void SetKeyDataFromBase64Test()
        {
            AesProvider provider = new AesProvider();

            byte[] data = GetRandomBytes(48);
            string keyData = Convert.ToBase64String(data);
            provider.SetKeyIVFromBase64String(keyData);

            provider.Dispose();

        }

        private static byte[] GetRandomBytes(int length)
        {
            byte[] data = new byte[length];
            Random.Shared.NextBytes(data);
            return data;
        }


    }
}
