using Adaptive.Intelligence.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Adaptive.Intelligence.Framework.Tests.Security
{
    public class BitSplicerTests
    {
        [Fact]
        public void SpliceBitsTest()
        {
            byte[] data = new byte[256];
            for (int count = 0; count < 256; count++)
            {
                data[count] = (byte)count;
            }

            byte[]? content = BitSplicer.SpliceBits(data);
            Assert.NotNull(content);
            Assert.Equal(260, content.Length);

        }
        [Fact]
        public void UnSpliceBitsTest()
        {
            byte[] data = new byte[256];
            for (int count = 0; count < 256; count++)
            {
                data[count] = (byte)count;
            }


            byte[]? content = BitSplicer.SpliceBits(data);

            byte[]? original = BitSplicer.UnSpliceBits(content);

            Assert.NotNull(original);
            Assert.Equal(data.Length, original.Length);
            for (int index = 0; index < 256; index++)
            {
                Assert.Equal(data[index], original[index]);
            }
        }

        [Fact]
        public void SpliceBitsNullTest()
        {
            byte[]? result = BitSplicer.SpliceBits(null);
            Assert.Null(result);
        }
        [Fact]
        public void UnSpliceBitsNullTest()
        {
            byte[]? result = BitSplicer.UnSpliceBits(null);
            Assert.Null(result);
        }
    }
}
