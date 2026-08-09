using Adaptive.Intelligence.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adaptive.Intelligence.Framework.Tests.Security
{
    public class AesKeyStoreTests
    {
        [Fact]
        public void CreateTest()
        {
            AesKeyStore store = new AesKeyStore();
            store.Dispose();
        }
        [Fact]
        public void CreateTestKeyIVData()
        {
            byte[] key = GetRandomBytes(32);
            byte[] iv = GetRandomBytes(16);

            AesKeyStore? store = new AesKeyStore(key, iv);
            Assert.NotNull(store);

            Assert.Equal(key, store.Key);
            Assert.Equal(iv, store.IV);

            store.Dispose();
        }
        [Fact]
        public void CreateTestCombinedKeyIVData()
        {
            byte[] key = GetRandomBytes(32);
            byte[] iv = GetRandomBytes(16);
            byte[] combined = new byte[48];

            Array.Copy(key, 0, combined, 0, 32);
            Array.Copy(iv, 0, combined, 32, 16);

            AesKeyStore? store = new AesKeyStore(combined);
            Assert.NotNull(store);

            Assert.Equal(key, store.Key);
            Assert.Equal(iv, store.IV);

            store.Dispose();
        }
        [Fact]
        public void DisposeSafetyTest()
        {
            AesKeyStore store = new AesKeyStore();
            store.Dispose();
            store.Dispose();
            store.Dispose();
            store.Dispose();
            store.Dispose();

            store.ClearKeyMemory();
            store.Dispose();

        }

        [Fact]
        public void SetIVTest()
        {
            AesKeyStore store = new AesKeyStore();

            byte[] iv = GetRandomBytes(16);
            store.IV = iv;

            store.Dispose();
        }
        [Fact]
        public void SetIVBadDataMinTest()
        {
            AesKeyStore store = new AesKeyStore();

            byte[] iv = GetRandomBytes(15);
            store.IV = iv;

            iv = GetRandomBytes(0);
            store.IV = iv;

            store.Dispose();
        }
        [Fact]
        public void SetIVBadDataMaxTest()
        {
            AesKeyStore store = new AesKeyStore();

            byte[] iv = GetRandomBytes(17);
            store.IV = iv;

            iv = GetRandomBytes(256);
            store.IV = iv;

            store.Dispose();
        }
        [Fact]
        public void SetKeyNullTest()
        {
            AesKeyStore store = new AesKeyStore();

            store.Key = null;

            store.Dispose();
        }


        [Fact]
        public void SetKeyTest()
        {
            AesKeyStore store = new AesKeyStore();

            byte[] key = GetRandomBytes(32);
            store.Key = key;

            store.Dispose();
        }
        [Fact]
        public void SetKeyBadDataMinTest()
        {
            AesKeyStore store = new AesKeyStore();

            byte[] key = GetRandomBytes(15);
            store.IV = key;

            key = GetRandomBytes(0);
            store.IV = key;

            store.Dispose();
        }
        [Fact]
        public void SetKeyBadDataMaxTest()
        {
            AesKeyStore store = new AesKeyStore();

            byte[] key = GetRandomBytes(17);
            store.IV = key;

            key = GetRandomBytes(256);
            store.IV = key;

            store.Dispose();
        }
        [Fact]
        public void SetIVNullTest()
        {
            AesKeyStore store = new AesKeyStore();

            store.IV = null;

            store.Dispose();
        }

        [Fact]
        public void GetIVTest()
        {
            AesKeyStore store = new AesKeyStore();
            store.IV = GetRandomBytes(16);

            byte[]? iv = store.IV;

            Assert.NotNull(iv);
            Assert.Equal(16, iv.Length);
            store.Dispose();
        }

        [Fact]
        public void GetKeyTest()
        {
            AesKeyStore store = new AesKeyStore();
            store.Key = GetRandomBytes(32);

            byte[]? key = store.Key;

            Assert.NotNull(key);
            Assert.Equal(32, key.Length);
            store.Dispose();
        }
        [Fact]
        public void GetKeyNullTest()
        {
            AesKeyStore store = new AesKeyStore();
            store.Key = null;

            byte[]? key = store.Key;

            Assert.Null(key);
            store.Dispose();
        }
        [Fact]
        public void GetIVNullTest()
        {
            AesKeyStore store = new AesKeyStore();
            store.IV = null;

            byte[]? iv = store.IV;

            Assert.Null(iv);
            store.Dispose();
        }

        [Fact]
        public void CloneTest()
        {
            byte[] key = GetRandomBytes(32);
            byte[] iv = GetRandomBytes(16);
            AesKeyStore? storeA = new AesKeyStore(key, iv);

            AesKeyStore? storeB = storeA.Clone();
            Assert.NotNull(storeB);

            storeA.Dispose();
            storeA = null;

            Assert.Equal(key, storeB.Key);
            Assert.Equal(iv, storeB.IV);


            storeB.Dispose();
        }
        [Fact]
        public void CloneTestNoKeyData()
        {
            AesKeyStore? storeA = new AesKeyStore();
            AesKeyStore? storeB = storeA.Clone();
            Assert.NotNull(storeB);

            Assert.Equal(storeA.Key, storeB.Key);
            Assert.Equal(storeA.IV, storeB.IV);

            storeA.Dispose();
            storeB.Dispose();
        }

        [Fact]
        public void GetKeyDataTest()
        {
            AesKeyStore? store = new AesKeyStore();
            store.Key = GetRandomBytes(32);
            store.IV = GetRandomBytes(16);

            byte[]? data = store.GetKeyIVData();
            Assert.NotNull(data);
            Assert.Equal(48, data.Length);
            store.Dispose();
        }
        private static byte[] GetRandomBytes(int length)
        {
            byte[] data = new byte[length];
            Random.Shared.NextBytes(data);
            return data;
        }

    }
}
