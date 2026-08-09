using Adaptive.Intelligence.Security.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adaptive.Intelligence.Framework.Tests.Security.Memory
{
    /// <summary>
    /// Unit Tests for the <see cref="SecureInt32"/> class.
    /// </summary>
    public class SecureInt32Tests
    {
        [Fact]
        public void SecureInt32CreateTest()
        {
            SecureInt32 s = new SecureInt32();

            Assert.NotNull(s);

            s.Dispose();
        }

        [Fact]
        public void SecureInt32StoreTest()
        {
            SecureInt32 s = new SecureInt32(42);

            Assert.NotNull(s);
            Assert.Equal(42, s.Value);

            s.Dispose();
        }

        [Fact]
        public void SetValueTest()
        {
            SecureInt32 s = new SecureInt32();

            s.Value = 99;
            int v = s.Value;

            Assert.Equal(99, v);
        }
        [Fact]
        public void SetValueZeroTest()
        {
            SecureInt32 s = new SecureInt32();

            s.Value = 0;
            int v = s.Value;

            Assert.Equal(0, v);
        }
        [Fact]
        public void SetValueMaxTest()
        {
            SecureInt32 s = new SecureInt32();

            s.Value = Int32.MaxValue;
            int v = s.Value;

            Assert.Equal(Int32.MaxValue, v);
        }
        [Fact]
        public void SetValueMinTest()
        {
            SecureInt32 s = new SecureInt32();

            s.Value = Int32.MinValue;
            int v = s.Value;

            Assert.Equal(Int32.MinValue, v);
        }
    }
}
