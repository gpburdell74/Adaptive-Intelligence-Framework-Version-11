using Adaptive.Intelligence.Security.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adaptive.Intelligence.Framework.Tests.Security.Memory
{
    public class InMemoryCredentialsTests
    {
        [Fact]
        public void InMemoryCredentialsCreateTest()
        {
            InMemoryCredentials credentials = new InMemoryCredentials();
            Assert.NotNull(credentials);
            credentials.Dispose();
        }
        public void InMemoryCredentialsSetUserIdTest()
        {
            InMemoryCredentials credentials = new InMemoryCredentials();
            Assert.NotNull(credentials);

            string data = "abcdef@123231.com";

            credentials.UserId = data;

            string result = credentials.UserId;

            Assert.Equal(data, result);

            credentials.Dispose();
        }
        public void InMemoryCredentialsSetPasswordTest()
        {
            InMemoryCredentials credentials = new InMemoryCredentials();
            Assert.NotNull(credentials);

            string data = "abcdef@123231.com";

            credentials.Password = data;

            string result = credentials.Password;

            Assert.Equal(data, result);

            credentials.Dispose();
        }
        public void InMemoryCredentialsSetUserIDPasswordTest()
        {
            InMemoryCredentials credentials = new InMemoryCredentials();
            Assert.NotNull(credentials);

            string origUid = "abcdef@123231.com";
            string origPwd = "1233.212jsjsxj23k1";

            credentials.UserId = origUid;
            credentials.Password = origPwd;

            string resultUid = credentials.UserId;
            string resultPwd = credentials.Password;

            Assert.Equal(origUid, resultUid);
            Assert.Equal(origPwd, resultPwd);

            credentials.Dispose();
        }

        [Theory]
        [InlineData("abc", "123")]
        [InlineData("0876543212,qawukeghwiuehf", "sdlasdckj3q09eui123d")]
        [InlineData("xg@cc.com", "")]
        [InlineData("xg@cc.com", null)]
        [InlineData(null, "asdkjaskdjlklajsd")]
        public void CreateTests(string? userName, string? password)
        {
            InMemoryCredentials credentials = new InMemoryCredentials();
            Assert.NotNull(credentials);

            string? origUid = userName;
            string? origPwd = null;
            if (!string.IsNullOrEmpty(password))
                origPwd = password;

            credentials.UserId = origUid;
            credentials.Password = origPwd;

            string? resultUid = credentials.UserId;
            string? resultPwd = credentials.Password;

            Assert.Equal(origUid, resultUid);
            Assert.Equal(origPwd, resultPwd);

            credentials.Dispose();
        }
    }
}
