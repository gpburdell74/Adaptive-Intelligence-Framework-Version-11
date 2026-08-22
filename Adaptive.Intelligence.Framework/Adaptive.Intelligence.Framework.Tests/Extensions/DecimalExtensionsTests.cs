using Adaptive.Intelligence.Extensions;

namespace Adaptive.Intelligence.Framework.Tests.Extensions
{
    /// <summary>
    /// Contains tests for <see cref="DecimalExtensions"/>.
    /// </summary>
    public class DecimalExtensionsTests
    {
        [Fact]
        public void GetBytes_Produces_RoundTrip_Value()
        {
            decimal value = 1234567890.123456789m;

            byte[] data = value.GetBytes();

            using MemoryStream stream = new(data);
            using BinaryReader reader = new(stream);
            decimal result = reader.ReadDecimal();

            Assert.Equal(value, result);
            Assert.NotEmpty(data);
        }
    }
}