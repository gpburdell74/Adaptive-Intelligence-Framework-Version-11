using Adaptive.Intelligence.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adaptive.Intelligence.Framework.Tests.Security
{
    public class RsaProviderTests
    {
        [Fact]
        public void CreateTest()
        {
            RsaProvider provider = new RsaProvider();
            Assert.NotNull(provider);
            provider.Dispose();

        }

        [Fact]
        public void DisposeSafetyTest()
        {
            RsaProvider provider = new RsaProvider();
            Assert.NotNull(provider);

            provider.Dispose();
            provider.Dispose();
            provider.Dispose();
            provider.GetKeyValueForExport();
            provider.Dispose();
            provider.GetPrivateKeyValueForStorage();
            provider.Dispose();
            provider.Dispose();
            provider.Dispose();
            provider.Dispose();

        }
        [Fact]
        public void DecryptTest()
        {
            RsaProvider provider = new RsaProvider();

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
            RsaProvider provider = new RsaProvider();

            byte[] originalData = new byte[] { 111, 112, 123, 4, 5, 6, 7 };

            byte[]? encrypted = provider.Encrypt(originalData);

            Assert.NotNull(encrypted);

        }
        [Fact]
        public void EncryptFullDataTest()
        {
            RsaProvider provider = new RsaProvider();

            byte[] originalData = new byte[64];
            for (int count = 0; count < 64; count++)
                originalData[count] = 255;

            byte[]? encrypted = provider.Encrypt(originalData);

            Assert.NotNull(encrypted);

        }
        [Fact]
        public void EncryptMinDataTest()
        {
            RsaProvider provider = new RsaProvider();

            byte[] originalData = new byte[64];
            Array.Clear(originalData, 0, 0);

            byte[]? encrypted = provider.Encrypt(originalData);

            Assert.NotNull(encrypted);

        }

        [Fact]
        public void DecryptFromBase64StringTest()
        {
            byte[] content = GetRandomBytes(64);

            RsaProvider provider = new RsaProvider();
            byte[]? encrypted = provider.Encrypt(content);
            Assert.NotNull(encrypted);

            string encrypted64 = Convert.ToBase64String(encrypted);

            byte[]? results = provider.DecryptFromBase64String(encrypted64);

            Assert.NotNull(results);
            for (int count = 0; count < content.Length; count++)
            {
                Assert.Equal(content[count], results[count]);
            }
            provider.Dispose();
        }

        [Fact]
        public void GetKeyTest()
        {
            using RsaProvider provider = new RsaProvider();
            string? key = provider.GetKeyValueForExport();

            Assert.NotNull(key);

            byte[] raw = Convert.FromBase64String(key);
            Assert.True(raw.Length > 0);
        }

        [Fact]
        public void GetKeyValueForExportTest()
        {
            using RsaProvider provider = new RsaProvider();
            string? keyContent = provider.GetKeyValueForExport();

            Assert.NotNull(keyContent);

            byte[] raw = Convert.FromBase64String(keyContent);
            Assert.True(raw.Length > 0);
        }

        [Fact()]
        public void SetKeyDataFromBase64Test()
        {
            RsaProvider? provider = new RsaProvider();
            Assert.NotNull(provider);

            string? privateKey = null;

            privateKey = provider.GetPrivateKeyValueForStorage();
            provider.Dispose();

            if (privateKey != null)
            {
                provider = new RsaProvider();
                provider.SetPrivateKeyFromBase64String(privateKey);
                provider.Dispose();
            }
        }

        [Fact]
        public void DecryptNullTest()
        {
            RsaProvider? provider = new RsaProvider();

            byte[]? result = provider.Decrypt(null);

            provider.Dispose();
        }

        [Fact]
        public void DecryptBadDataTest()
        {
            RsaProvider? provider = new RsaProvider();

            byte[]? result = provider.Decrypt(new byte[3]);

            provider.Dispose();
        }
        [Fact]
        public void DecryptFromBase64StringNullTest()
        {
            RsaProvider? provider = new RsaProvider();

            byte[]? result = provider.DecryptFromBase64String(null);

            provider.Dispose();
        }
        [Fact]
        public void DecryptFromBase64StringEmptyTest()
        {
            RsaProvider? provider = new RsaProvider();

            byte[]? result = provider.DecryptFromBase64String("");

            provider.Dispose();
        }

        [Fact]
        public void DecryptFromBase64StringBadTest()
        {
            RsaProvider? provider = new RsaProvider();

            byte[]? result = provider.DecryptFromBase64String("XQ");

            provider.Dispose();
        }

        [Fact]
        public void ImportFromBase64StringTest()
        {
            RsaProvider? provider = new RsaProvider();
            string? keyInfo = provider.GetKeyValueForExport();
            provider.Dispose();
            provider = null;

            provider = new RsaProvider();
            provider.ImportPublicKeyFromBase64String(keyInfo);
            provider.Dispose();

        }

        [Fact]
        public void ImportFromBase64StringEmptyTest()
        {
            RsaProvider? provider = new RsaProvider();
            string? keyInfo = provider.GetKeyValueForExport();
            provider.Dispose();
            provider = null;

            provider = new RsaProvider();
            provider.ImportPublicKeyFromBase64String("");
            provider.Dispose();

        }

        [Fact]
        public void ImportFromBase64StringNullTest()
        {
            RsaProvider? provider = new RsaProvider();
            string? keyInfo = provider.GetKeyValueForExport();
            provider.Dispose();
            provider = null;

            provider = new RsaProvider();
            provider.ImportPublicKeyFromBase64String(null);
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
