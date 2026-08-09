using System;
using System.Collections.Generic;
using System.Text;
using Adaptive.Intelligence.Security.Memory;

namespace Adaptive.Intelligence.Framework.Tests.Security.Memory
{
    /// <summary>
    /// Unit Tests for the <see cref="SecureString"/> class.
    /// </summary>
    public class SecureStringTests
    {
        [Fact]
        public void SecureStringCreateTest()
        {
            SecureString s = new SecureString();

            Assert.NotNull(s);

            s.Dispose();
        }

        [Fact]
        public void SecureStringStoreTest()
        {
            SecureString s = new SecureString("ABCDEFG");

            Assert.NotNull(s);
            Assert.Equal("ABCDEFG", s.Value);

            s.Dispose();
        }

        [Fact]
        public void SetValueTest()
        {
            SecureString s = new SecureString();

            s.Value = "ABCDE";
            string v = s.Value;

            Assert.Equal("ABCDE", v);
        }
        [Fact]
        public void SetValueNullTest()
        {
            SecureString? s = new SecureString();

            s.Value = null;
            string? v = s.Value;

            Assert.Null(v);

        }

        [Fact]
        public void ConstructorWithIterationsTest()
        {
            SecureString s = new SecureString(256);

            string expected = "ABCDEFGHIJKLMNOP";
            s.Value = expected;
            string? v = s.Value;

            Assert.Equal(expected, v);
        }
        [Fact]
        public void ConstructorWithIterationsAndValueTest()
        {
            SecureString s = new SecureString(256, "ABCDEFGHIJKLMNOP");

            string expected = "ABCDEFGHIJKLMNOP";
            string? v = s.Value;

            Assert.Equal(expected, v);
        }

        [Fact]
        public void IsAsciiTest()
        {
            SecureString s = new SecureString(256, "ABCDEFGHIJKLMNOP");
            s.IsAscii = true;

            string expected = "ABCDEFGHIJKLMNOP";
            string? v = s.Value;

            Assert.Equal(expected, v);
        }
        [Fact]
        public void IsUnicodeTest()
        {
            SecureString s = new SecureString(256, "ABCDEFGHIJKLMNOP");
            s.IsAscii = false;

            string expected = "ABCDEFGHIJKLMNOP";
            string? v = s.Value;

            Assert.Equal(expected, v);
        }
        [Fact]
        public void IsAsciiWithNullTest()
        {
            SecureString s = new SecureString(256, null);
            s.IsAscii = true;

            string? expected = null;
            string? v = s.Value;

            Assert.Equal(expected, v);
        }
        [Fact]
        public void IsUnicodeWithNullTest()
        {
            SecureString s = new SecureString(256, null);
            s.IsAscii = false;

            string? expected = null;
            string? v = s.Value;

            Assert.Equal(expected, v);
        }
    }
}
