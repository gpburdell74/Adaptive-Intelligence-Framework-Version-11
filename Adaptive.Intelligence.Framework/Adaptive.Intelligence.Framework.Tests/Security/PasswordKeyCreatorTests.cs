using Adaptive.Intelligence.Extensions;
using Adaptive.Intelligence.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adaptive.Intelligence.Framework.Tests.Security
{
    public class PasswordKeyCreatorTests
    {
        private const string StandardUserId = "aleph_beta_17433";
        private const string StandardPwd = "872.11.X.Q";

        [Fact]
        public void CanCreateTest()
        {
            byte[]? pkey = PasswordKeyCreator.CreatePrimaryKeyData(StandardUserId, StandardPwd);
            Assert.NotNull(pkey);
            Assert.Equal(48, pkey.Length);

            byte[]? key2 = PasswordKeyCreator.CreateKeyVariant(pkey);
            Assert.NotNull(key2);
            Assert.Equal(48, key2.Length);

            byte[]? key3 = PasswordKeyCreator.CreateKeyVariant(key2);
            Assert.NotNull(key3);
            Assert.Equal(48, key3.Length);

        }
        [Fact]
        public void ConsistencyTest()
        {
            byte[]? pkey = PasswordKeyCreator.CreatePrimaryKeyData(StandardUserId, StandardPwd);
            Assert.NotNull(pkey);
            Assert.Equal(48, pkey.Length);

            byte[]? skey = PasswordKeyCreator.CreatePrimaryKeyData(StandardUserId, StandardPwd);
            Assert.NotNull(skey);
            Assert.Equal(48, skey.Length);

            byte[]? tkey = PasswordKeyCreator.CreatePrimaryKeyData(StandardUserId, StandardPwd);
            Assert.NotNull(tkey);
            Assert.Equal(48, tkey.Length);

            Assert.Equal(0, pkey.Compare(skey));
            Assert.Equal(0, skey.Compare(tkey));
            Assert.Equal(0, tkey.Compare(pkey));

        }

    }
}
